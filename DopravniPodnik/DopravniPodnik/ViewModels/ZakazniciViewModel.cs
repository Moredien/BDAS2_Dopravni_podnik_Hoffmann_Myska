using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DopravniPodnik.Data.DTO;
using DopravniPodnik.Data.service;

namespace DopravniPodnik.ViewModels;

public partial class ZakazniciViewModel : ViewModelBase
{
    [ObservableProperty] 
    private ObservableCollection<UzivatelDTO> _items;
    [ObservableProperty] 
    private ObservableCollection<UzivatelDTO> _filteredItems;
    [ObservableProperty] 
    private UzivatelDTO? _selectedItem;

    [ObservableProperty]
    private string _searchedText = "";

    partial void OnSearchedTextChanged(string value)
    {
        
        FilterList(value);
    }
    
    private readonly DatabaseService _databaseService = new();

    public ZakazniciViewModel()
    {
        Items = new();
        FilteredItems = new();
        LoadData();
        FilterList(null);
    }

    [RelayCommand]
    public void Informace()
    {
        if(SelectedItem!=null)
            WindowManager.SetContentView(typeof(ProfilViewModel), new object[] { SelectedItem });
    }

    private void LoadData()
    {
        var data = _databaseService.FetchData<UzivatelDTO>($"SELECT * FROM UZIVATEL_VIEW WHERE TYP_UZIVATELE = 'Zákazník'");
        foreach (var uzivatel in data)
        {
            Items.Add(uzivatel);
        }
    }

    private void FilterList(string? keyword)
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
            if(string.Concat(entry.uzivatelske_jmeno,entry.jmeno,entry.prijmeni).ToLower().Contains(SearchedText.ToLower()))
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