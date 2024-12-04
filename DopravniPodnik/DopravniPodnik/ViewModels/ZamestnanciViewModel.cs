using System.Collections.ObjectModel;
using System.Data;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DopravniPodnik.Data.DTO;
using DopravniPodnik.Data.service;
using DopravniPodnik.Utils;
using DopravniPodnik.ViewModels.Forms;
using Oracle.ManagedDataAccess.Client;

namespace DopravniPodnik.ViewModels;

public partial  class ZamestnanciViewModel : ViewModelBase
{
    private ZamestnanecService _zamestnanecService = new();
    public ObservableCollection<ZamestnanecViewDTO> Items { get; set; }
    public ObservableCollection<ZamestnanecViewDTO> FilteredItems { get; set; }
    [ObservableProperty] 
    public ZamestnanecViewDTO selectedItem;

    [ObservableProperty] public ZamestnanecViewDTO selectedZamestnanec;
    [ObservableProperty]
    private string searchedText;

    partial void OnSearchedTextChanged(string value)
    {
        
        FilterList(value);
    }
    
    private readonly DatabaseService _databaseService = new();

    public ZamestnanciViewModel()
    {
        Items = new();
        FilteredItems = new();
        LoadData();
        FilterList(null);
    }

    [RelayCommand]
    public void Informace()
    {
        WindowManager.SetContentView(typeof(ZamestnanciFormViewModel), new[] { selectedItem });
    }

    [RelayCommand]
    public void Odebrat()
    {
        string query = @"
            BEGIN
                DELETE FROM ZAMESTNANCI WHERE ID_ZAMESTNANCE = :id_zamestnance;
                commit;
            END;
        ";
        
        var parameters = new List<OracleParameter>
        {
            new OracleParameter("id_zamestnance", OracleDbType.Decimal)
                { Value = SelectedItem.IdZamestnance, Direction = ParameterDirection.Input },
        };

        var procedureCallWrapper = new ProcedureCallWrapper(query, parameters);
        _databaseService.ExecuteDbCall(procedureCallWrapper, out var error);
        Items.Remove(selectedItem);
        FilteredItems.Remove(selectedItem);
    }

    private void LoadData()
    {
        var data = _zamestnanecService.GetAllZamestnanci();
        foreach (var zamestnanec in data)
        {
            Items.Add(zamestnanec);
        }
    }

    private void FilterList(string keyword)
    {
        if(string.IsNullOrEmpty(keyword))
        {
            foreach (var item in Items)
            {
                FilteredItems.Add(item);
            }
            return;
        }
        FilteredItems.Clear();
        foreach (var entry in Items)
        {
            if (string.Concat(entry.UzivatelskeJmeno,entry.Jmeno,entry.Prijmeni).ToLower().Contains(SearchedText.ToLower()))
            {
                FilteredItems.Add(entry);
            }
        }
    }
}