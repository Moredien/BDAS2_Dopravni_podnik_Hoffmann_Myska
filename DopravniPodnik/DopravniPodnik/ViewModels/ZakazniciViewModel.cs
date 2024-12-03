using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DopravniPodnik.Data.DTO;
using DopravniPodnik.Data.service;

namespace DopravniPodnik.ViewModels;

public partial class ZakazniciViewModel : ViewModelBase
{
    public ObservableCollection<UzivatelDTO> Items { get; set; }
    public ObservableCollection<UzivatelDTO> FilteredItems { get; set; }
    [ObservableProperty] 
    public UzivatelDTO selectedItem;

    [ObservableProperty]
    private string searchedText;

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
        WindowManager.SetContentView(typeof(ProfilViewModel), new[] { selectedItem });
    }

    private void LoadData()
    {
        var data = _databaseService.FetchData<UzivatelDTO>($"SELECT * FROM UZIVATEL_VIEW WHERE TYP_UZIVATELE = 'Zákazník'");
        foreach (var uzivatel in data)
        {
            Items.Add(uzivatel);
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
            if (entry.uzivatelske_jmeno.ToLower().Contains(keyword.ToLower()) ||
                entry.jmeno.ToLower().Contains(keyword.ToLower()) ||
                entry.prijmeni.ToLower().Contains(keyword.ToLower()))
            {
                FilteredItems.Add(entry);
            }
        }
    }
}