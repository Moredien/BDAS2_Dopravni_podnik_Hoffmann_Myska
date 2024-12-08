using System.Collections.ObjectModel;
using System.Data;
using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DopravniPodnik.Data.DTO;
using DopravniPodnik.Data.Models;
using DopravniPodnik.Data.service;
using DopravniPodnik.Utils;
using Oracle.ManagedDataAccess.Client;

namespace DopravniPodnik.ViewModels.Forms;

public partial class ZastavkyLinkyViewFormViewModel : ViewModelBase
{
    private readonly DatabaseService _databaseService = new();

    [ObservableProperty] private DateTime _odjezdDate = DateTime.Today;
    [ObservableProperty] private DateTime _odjezdTime = DateTime.Now;
    [ObservableProperty] private ObservableCollection<Zastavky> _zastavky = new();
    [ObservableProperty] private ObservableCollection<Linky> _linky = new();
    [ObservableProperty] private Zastavky _selectedZastavka;
    [ObservableProperty] private Linky _selectedLinka;
    [ObservableProperty] private string _iterace = "";
    [ObservableProperty] private ObservableCollection<int> _smery = new() { -1,1};
    [ObservableProperty] private int _selectedSmer;

    private ZastavkyLinkyViewDTO _editedZastavkyLinkyDto;

    public ZastavkyLinkyViewFormViewModel(ZastavkyLinkyViewDTO selectedItem)
    {
        _editedZastavkyLinkyDto = selectedItem;
        LoadData(selectedItem);
    }
    
    [RelayCommand]
    private void Submit()
    {
        if(!Int32.TryParse(Iterace,out int iter))
            return;
        string query = @"
            BEGIN
                ST67028.INSERT_UPDATE.edit_zastaveni(
                    :p_id_zastaveni,
                    :p_odjezd,
                    :p_id_linky, 
                    :p_id_zastavky,
                    :p_iterace,
                    :p_smer
                );
            END;
        ";
        object id;
        if (_editedZastavkyLinkyDto == null)
            id = DBNull.Value;
        else
            id = _editedZastavkyLinkyDto.IdZastaveni;
        
        var parameters = new List<OracleParameter>
        {
            new OracleParameter("p_id_zastaveni", OracleDbType.Decimal)
                { Value = id, Direction = ParameterDirection.Input },
            new OracleParameter("p_odjezd", OracleDbType.Date) 
                { Value = OdjezdDate.Date+OdjezdTime.TimeOfDay, Direction = ParameterDirection.Input },
            new OracleParameter("p_id_linky", OracleDbType.Decimal)
                { Value = SelectedLinka.IdLinky, Direction = ParameterDirection.Input },
            new OracleParameter("p_id_zastavky", OracleDbType.Decimal)
                { Value = SelectedZastavka.IdZastavky, Direction = ParameterDirection.Input },
            new OracleParameter("p_iterace", OracleDbType.Decimal)
                { Value = iter, Direction = ParameterDirection.Input },
            new OracleParameter("p_smer", OracleDbType.Decimal)
                { Value = SelectedSmer, Direction = ParameterDirection.Input }
        };
        
        var procedureCallWrapper = new ProcedureCallWrapper(query, parameters);
        _databaseService.ExecuteDbCall(procedureCallWrapper, out var error);
        if (!string.IsNullOrEmpty(error))
        {
            MessageBox.Show($"Při ukládání data do databáze došlo k chybě", "Chyba pri ukladani", 
                MessageBoxButton.OK, MessageBoxImage.Error);
            return;
        }
        
        Exit();
    }

    private void LoadData(ZastavkyLinkyViewDTO? jizdyViewDto)
    {
        if (jizdyViewDto != null)
        {
            OdjezdDate = jizdyViewDto.Odjezd;
            OdjezdTime = jizdyViewDto.Odjezd;
            SelectedSmer = jizdyViewDto.Smer;
            Iterace = jizdyViewDto.Iterace.ToString();
        }
        LoadComboBoxes(jizdyViewDto);
    }

    private void LoadComboBoxes(ZastavkyLinkyViewDTO? jizdyViewDto)
    {
        var zastavkyData = _databaseService.FetchData<Zastavky>($"SELECT * FROM ZASTAVKY");
        foreach (var entry in zastavkyData)
            Zastavky.Add(entry);
        var linkyData = _databaseService.FetchData<Linky>($"SELECT * FROM LINKY");
        foreach (var entry in linkyData)
            Linky.Add(entry);

        if (jizdyViewDto == null)
            return;
        
        SelectedZastavka = Zastavky.FirstOrDefault(v => v.IdZastavky == jizdyViewDto.IdZastavky)!;
        SelectedLinka = Linky.FirstOrDefault(l => l.IdLinky == jizdyViewDto.IdLinky)!;
    }

    private void LoadSmerAndIterace(ZastavkyLinkyViewDTO zastavkyLinkyDto)
    {
        var zastaveni = _databaseService
            .FetchData<Zastaveni>($"SELECT * FROM ZASTAVENI WHERE ID_ZASTAVENI = {zastavkyLinkyDto.IdZastaveni}")
            .FirstOrDefault();
        
    }
}