using System.Collections;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DopravniPodnik.Data.DTO;
using DopravniPodnik.Data.Models;
using DopravniPodnik.Data.service;
using DopravniPodnik.Utils;
using Oracle.ManagedDataAccess.Client;


namespace DopravniPodnik.ViewModels.Forms;

public partial class ZamestnanciFormViewModel : ViewModelBase, INotifyDataErrorInfo
{
    private readonly ErrorsViewModel _errorsViewModel;
    public bool HasErrors => _errorsViewModel.HasErrors;
    public bool CanCreate => !HasErrors;

    public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;
    private readonly DatabaseService _databaseService = new();
    private ZamestnanecService _zamestnanecService = new();


    // private int? IdZamestnance { get; set; }
    private int? IdUzivatele { get; set; }
    // private int IdZamestnance { get; set; }
    [ObservableProperty] public string uzivatelskeJmeno;
    [ObservableProperty] public string jmeno;
    [ObservableProperty] public string prijmeni;
    [ObservableProperty] public string plat;
    [ObservableProperty] public DateTime? platnostUvazkuDo = DateTime.Today;

    [ObservableProperty] public ZamestnanecViewDTO? editedZamestanenc;
    [ObservableProperty] public ObservableCollection<ZamestnanecViewDTO> zamestnanci = new();

    [ObservableProperty] public ZamestnanecViewDTO selectedNadrizeny;
    private UzivatelDTO EditedUzivatel { get; set; }
    
    public ZamestnanciFormViewModel(int idUzivatele)
    {
        _errorsViewModel = new ErrorsViewModel();
        _errorsViewModel.ErrorsChanged += ErrorsViewModel_ErrorsChanged;
        IdUzivatele = idUzivatele;

        editedZamestanenc =
            _databaseService.FetchData<ZamestnanecViewDTO>(
                $"SELECT * FROM ZAMESTNANCI_VIEW WHERE ID_UZIVATELE = {idUzivatele}")
                .FirstOrDefault();
        if (editedZamestanenc != null)
        {
            // IdZamestnance = editedZamestanenc.IdZamestnance;
            UzivatelskeJmeno = editedZamestanenc.UzivatelskeJmeno;
            Jmeno = editedZamestanenc.Jmeno;
            Prijmeni = editedZamestanenc.Prijmeni;
            Plat = editedZamestanenc.Plat.ToString();
            PlatnostUvazkuDo = editedZamestanenc.PlatnostUvazkuDo;
        }
        else
        {
            editedZamestanenc = new ZamestnanecViewDTO(); 
        }

        EditedUzivatel = LoadUserDetails(idUzivatele);
        
        LoadZamestnanci();

    }

    public ZamestnanciFormViewModel(ZamestnanecViewDTO selectedItem)
    {
        _errorsViewModel = new ErrorsViewModel();
        _errorsViewModel.ErrorsChanged += ErrorsViewModel_ErrorsChanged;

        if (selectedItem != null)
        {
            editedZamestanenc = (ZamestnanecViewDTO?)selectedItem;
            UzivatelskeJmeno = editedZamestanenc.UzivatelskeJmeno;
            Jmeno = editedZamestanenc.Jmeno;
            Prijmeni = editedZamestanenc.Prijmeni;
            Plat = editedZamestanenc.Plat.ToString();
            PlatnostUvazkuDo = editedZamestanenc.PlatnostUvazkuDo;
            IdUzivatele = editedZamestanenc.IdUzivatele;

            LoadZamestnanci();
        }
        else
            Exit();
    }

    [RelayCommand]
    public void Submit()
    {
        string query = @"
            BEGIN
                ST67028.INSERT_UPDATE.edit_zamestnanci(
                    :p_id_zamestnance,
                    :p_plat, 
                    :p_platnost_uvazku_do, 
                    :p_id_nadrizeneho, 
                    :p_id_uzivatele
                );
            END;
        ";
        object idZamestnance;
        if (EditedZamestanenc.IdZamestnance == null)
            idZamestnance = DBNull.Value;
        else
            idZamestnance = EditedZamestanenc.IdZamestnance;

        if (SelectedNadrizeny.IdZamestnance == null)
            SelectedNadrizeny = null;
        var parameters = new List<OracleParameter>
        {
            new OracleParameter("p_id_zamestnance", OracleDbType.Decimal)
                { Value = idZamestnance, Direction = ParameterDirection.Input },
            new OracleParameter("p_plat", OracleDbType.Decimal)
                { Value = Plat, Direction = ParameterDirection.Input },
            new OracleParameter("p_platnost_uvazku_do", OracleDbType.Date)
                { Value = PlatnostUvazkuDo, Direction = ParameterDirection.Input },
            new OracleParameter("p_id_nadrizeneho", OracleDbType.Decimal)
                { Value = SelectedNadrizeny==null?DBNull.Value : SelectedNadrizeny.IdZamestnance, Direction = ParameterDirection.Input },
            new OracleParameter("p_id_uzivatele", OracleDbType.Decimal)
                { Value = IdUzivatele, Direction = ParameterDirection.Input }
        };
        var procedureCallWrapper = new ProcedureCallWrapper(query, parameters);
        _databaseService.ExecuteDbCall(procedureCallWrapper, out var error);

        Exit();
    }

    [RelayCommand]
    private void DetailUzivatele()
    {
        if(EditedUzivatel == null)
            WindowManager.SetContentView(typeof(ProfilViewModel), new object[] { EditedZamestanenc.IdUzivatele });
        else
            WindowManager.SetContentView(typeof(ProfilViewModel), new object[] { EditedUzivatel });
    }

    private UzivatelDTO LoadUserDetails(int id)
    {
        var uzivatel = _databaseService.FetchData<UzivatelDTO>($"SELECT * FROM UZIVATEL_VIEW WHERE ID_UZIVATELE = {id}")
            .FirstOrDefault();
        if (uzivatel != null)
        {
            Jmeno = uzivatel.jmeno;
            Prijmeni = uzivatel.prijmeni;
            IdUzivatele = uzivatel.id_uzivatele;
        }
        return uzivatel;
    }
    private void ValidateInput(string propertyName)
    {
        _errorsViewModel.ClearErrors(propertyName);

        // switch (propertyName)
        // {
        //     case nameof(Plat):
        //         //validate hee
        //         break;
        //     case nameof(PlatnostUvazkuDo):
        //         //and here
        //         break;
        // }
    }

    private void LoadZamestnanci()
    {
        var data = _zamestnanecService.GetAllZamestnanci();
        foreach (var zamestnanec in data)
        {
            if (zamestnanec.IdZamestnance == EditedZamestanenc.IdZamestnance)
                continue;
            Zamestnanci.Add(zamestnanec);
        }

        Zamestnanci.Add(new ZamestnanecViewDTO());
        
        if (EditedZamestanenc.IdNadrizeneho != null)
        {
            SelectedNadrizeny = zamestnanci.FirstOrDefault(z => z.IdZamestnance == EditedZamestanenc.IdNadrizeneho);
        }
    }

    private void ValidateAllInputs()
    {
        // ValidateInput(nameof(Plat));
        // ValidateInput(nameof(PlatnostUvazkuDo));
    }

    public IEnumerable GetErrors(string? propertyName)
    {
        return _errorsViewModel.GetErrors(propertyName);
    }

    private void ErrorsViewModel_ErrorsChanged(object? sender, DataErrorsChangedEventArgs e)
    {
        ErrorsChanged?.Invoke(this, e);
        OnPropertyChanged(nameof(CanCreate));
    }

    protected override void OnPropertyChanged(PropertyChangedEventArgs propertyChangedEventArgs)
    {
        ValidateInput(propertyChangedEventArgs.PropertyName);
    }
}