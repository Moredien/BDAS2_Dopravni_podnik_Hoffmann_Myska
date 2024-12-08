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

public partial class ZamestnanciViewModel : ViewModelBase
{
    private ZamestnanecService _zamestnanecService = new();
    [ObservableProperty] private ObservableCollection<ZamestnanecViewDTO> _items;
    [ObservableProperty] private ObservableCollection<ZamestnanecViewDTO> _filteredItems;
    [ObservableProperty] private ZamestnanecViewDTO? _selectedItem;

    // [ObservableProperty] private ZamestnanecViewDTO? selectedZamestnanec;
    [ObservableProperty] private string _searchedText= "";

    [ObservableProperty] private bool _isMasked = true;

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
        if (SelectedItem == null)
            return;
        WindowManager.SetContentView(typeof(ZamestnanciFormViewModel), new object[] { SelectedItem });
    }

    [RelayCommand]
    public void Odebrat()
    {
        if (SelectedItem == null)
            return;
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
        _databaseService.ExecuteDbCall(procedureCallWrapper, out var _);
        Items.Remove(SelectedItem);
        FilteredItems.Remove(SelectedItem);
    }

    [RelayCommand]
    private void Povysit()
    {
        if (SelectedItem != null)
        {
            WindowManager.SetContentView(typeof(PovysitViewModel), new object[] { SelectedItem });
        }
    }

    private void LoadData()
    {
        var data = _zamestnanecService.GetAllZamestnanci();
        foreach (var zamestnanec in data)
        {
            Items.Add(zamestnanec);
        }
    }

    private void FilterList(string? keyword)
    {
        if (string.IsNullOrEmpty(keyword))
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
            if (string.Concat(entry.UzivatelskeJmeno, entry.Jmeno, entry.Prijmeni).ToLower()
                .Contains(SearchedText.ToLower()))
            {
                FilteredItems.Add(entry);
            }
        }
    }

    public override void Update()
    {
        base.Update();
        WindowManager.ReturnToSelectedContentView();
    }
}