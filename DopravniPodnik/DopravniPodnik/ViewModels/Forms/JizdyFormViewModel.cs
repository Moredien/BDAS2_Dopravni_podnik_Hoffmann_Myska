using System.Collections.ObjectModel;
using System.Data;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DopravniPodnik.Data.DTO;
using DopravniPodnik.Data.Models;
using DopravniPodnik.Data.service;
using DopravniPodnik.Utils;
using Oracle.ManagedDataAccess.Client;

namespace DopravniPodnik.ViewModels.Forms;

public partial class JizdyFormViewModel : ViewModelBase
{
    private readonly DatabaseService _databaseService = new();

    [ObservableProperty] private DateTime _zacatekDate = DateTime.Now;
    [ObservableProperty] private DateTime _zacatekTime = DateTime.Now;
    [ObservableProperty] private DateTime _konecDate = DateTime.Now;
    [ObservableProperty] private DateTime _konecTime = DateTime.Now;

    [ObservableProperty] private ObservableCollection<VozovyParkDTO> _vozidla = new();
    [ObservableProperty] private ObservableCollection<Linky> _linky = new();
    [ObservableProperty] private ObservableCollection<Ridici> _ridici = new();

    [ObservableProperty] private VozovyParkDTO _selectedVozidlo;
    [ObservableProperty] private Linky _selectedLinka;
    [ObservableProperty] private Ridici _selectedRidic;

    private JizdyViewDTO? _editedJizdyViewDto;


    public JizdyFormViewModel(JizdyViewDTO? selectedItem)
    {
        _editedJizdyViewDto = selectedItem;
        LoadData(selectedItem);
    }

    [RelayCommand]
    private void Submit()
    {
        string query = @"
            BEGIN
                ST67028.INSERT_UPDATE.edit_jizdy(
                    :p_id_jizdy,
                    :p_zacatek,
                    :p_konec, 
                    :p_id_vozidla,
                    :p_id_linky,
                    :p_id_ridice
                );
            END;
        ";
        object id;
        if (_editedJizdyViewDto == null)
            id = DBNull.Value;
        else
            id = _editedJizdyViewDto.IdJizdy;

        var parameters = new List<OracleParameter>
        {
            new OracleParameter("p_id_jizdy", OracleDbType.Decimal)
                { Value = id, Direction = ParameterDirection.Input },
            new OracleParameter("p_zacatek", OracleDbType.Date) 
                { Value = ZacatekDate.Date+ZacatekTime.TimeOfDay, Direction = ParameterDirection.Input },
            new OracleParameter("p_konec", OracleDbType.Date)
                { Value = KonecDate.Date +KonecTime.TimeOfDay, Direction = ParameterDirection.Input },
            new OracleParameter("p_id_vozidla", OracleDbType.Decimal)
                { Value = SelectedVozidlo.IdVozidla, Direction = ParameterDirection.Input },
            new OracleParameter("p_id_linky", OracleDbType.Decimal)
                { Value = SelectedLinka.IdLinky, Direction = ParameterDirection.Input },
            new OracleParameter("p_id_ridice", OracleDbType.Decimal)
                { Value = SelectedRidic.IdRidice, Direction = ParameterDirection.Input }
        };

        var procedureCallWrapper = new ProcedureCallWrapper(query, parameters);
        _databaseService.ExecuteDbCall(procedureCallWrapper, out var error);
        Exit();
    }

    private void LoadData(JizdyViewDTO? jizdyViewDto)
    {
        if (jizdyViewDto != null)
        {
            ZacatekDate = jizdyViewDto.Zacatek;
            ZacatekTime = jizdyViewDto.Zacatek;
            if (jizdyViewDto.Konec != null)
            {
                KonecDate = jizdyViewDto.Konec.Value;
                KonecTime = jizdyViewDto.Konec.Value;
            }
        }
        

        LoadComboBoxes(jizdyViewDto);
    }

    private void LoadComboBoxes(JizdyViewDTO? jizdyViewDto)
    {
        var vozidlaData = _databaseService.FetchData<VozovyParkDTO>($"SELECT * FROM DOSTUPNA_VOZIDLA_VIEW");
        foreach (var entry in vozidlaData)
            if(!Vozidla.Any(v => v.IdVozidla == entry.IdVozidla))
                Vozidla.Add(entry);
        var linkyData = _databaseService.FetchData<Linky>($"SELECT * FROM LINKY");
        foreach (var entry in linkyData)
            Linky.Add(entry);
        var ridiciData = _databaseService.FetchData<Ridici>($"SELECT * FROM RIDICI");
        foreach (var entry in ridiciData)
            Ridici.Add(entry);

        if (jizdyViewDto == null)
            return;
        
        SelectedVozidlo = Vozidla.FirstOrDefault(v => v.IdVozidla == jizdyViewDto.IdVozidla)!;
        SelectedLinka = Linky.FirstOrDefault(l => l.IdLinky == jizdyViewDto.IdLinky)!;
        SelectedRidic = Ridici.FirstOrDefault(r => r.IdRidice == jizdyViewDto.IdRidice)!;
    }
}