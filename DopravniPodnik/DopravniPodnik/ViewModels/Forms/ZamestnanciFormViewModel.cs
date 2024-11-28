using System.Collections;
using System.ComponentModel;
using System.Data;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DopravniPodnik.Data.Models;
using DopravniPodnik.Data.service;
using DopravniPodnik.Utils;
using Oracle.ManagedDataAccess.Client;


namespace DopravniPodnik.ViewModels.Forms;

public partial class ZamestnanciFormViewModel: ViewModelBase , INotifyDataErrorInfo
{
    private readonly ErrorsViewModel _errorsViewModel;
    public bool HasErrors => _errorsViewModel.HasErrors;
    public bool CanCreate => !HasErrors;

    public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;
    private readonly DatabaseService _databaseService = new();

    private int? IdZamestnance { get; set; }
    private int? IdUzivatele { get; set; }

    [ObservableProperty]
    public string plat;
    [ObservableProperty]
    public DateTime? platnostUvazkuDo;


    public ZamestnanciFormViewModel(object selectedItem)
    {
        _errorsViewModel = new ErrorsViewModel();
        _errorsViewModel.ErrorsChanged += ErrorsViewModel_ErrorsChanged;

        if (selectedItem != null)
        {
            if (selectedItem.GetType() == typeof(Int32))
            {
                IdUzivatele = (int)selectedItem;

                var item =
                    _databaseService.FetchData<Zamestnanci>(
                        $"SELECT * FROM ST67028.ZAMESTNANCI WHERE id_uzivatele = {IdUzivatele}")[0];
                if (item != null)
                {
                    Plat = item.Plat;
                    PlatnostUvazkuDo = item.PlatnostUvazkuDo;
                    IdZamestnance = item.IdZamestnance;
                }
                return;
            }
            if (selectedItem.GetType() == typeof(Zamestnanci))
            {
                var zamestannec = (Zamestnanci)selectedItem;
                Plat = zamestannec.Plat;
                PlatnostUvazkuDo = zamestannec.PlatnostUvazkuDo;
                IdUzivatele = zamestannec.IdUzivatele;
                IdZamestnance = zamestannec.IdZamestnance;
            }
        }
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
        
        var parameters = new List<OracleParameter>
        {
            new OracleParameter("p_id_zamestnance", OracleDbType.Decimal)
                { Value = IdZamestnance, Direction = ParameterDirection.Input },
            new OracleParameter("p_plat", OracleDbType.Decimal) 
                { Value = Plat, Direction = ParameterDirection.Input },
            new OracleParameter("p_platnost_uvazku_do", OracleDbType.Date)
                { Value = PlatnostUvazkuDo, Direction = ParameterDirection.Input },
            new OracleParameter("p_id_nadrizeneho", OracleDbType.Decimal)
                { Value = null, Direction = ParameterDirection.Input },
            new OracleParameter("p_id_uzivatele", OracleDbType.Decimal)
                { Value = IdUzivatele, Direction = ParameterDirection.Input }
        };

        var procedureCallWrapper = new ProcedureCallWrapper(query, parameters);
        _databaseService.CallDbProcedure(procedureCallWrapper, out var error);

        Console.WriteLine(error);
        Exit();
    }
    
    

    private void ValidateInput(string propertyName)
    {
        _errorsViewModel.ClearErrors(propertyName);
        
        switch (propertyName)
        {
            case nameof(Plat):
                //validate hee
                break;
            case nameof(PlatnostUvazkuDo):
                //and here
                break;
        }
    }

    private void ValidateAllInputs()
    {
        ValidateInput(nameof(Plat));
        ValidateInput(nameof(PlatnostUvazkuDo));
    }
    
    public IEnumerable GetErrors(string? propertyName)
    {
        return _errorsViewModel.GetErrors(propertyName);
    }

    private void ErrorsViewModel_ErrorsChanged(object? sender, DataErrorsChangedEventArgs e)
    {
        ErrorsChanged?.Invoke(this,e);
        OnPropertyChanged(nameof(CanCreate));
    }

    protected override void OnPropertyChanged(PropertyChangedEventArgs propertyChangedEventArgs)
    {
        ValidateInput(propertyChangedEventArgs.PropertyName);
    }
}