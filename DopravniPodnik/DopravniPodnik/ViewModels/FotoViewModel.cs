using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DopravniPodnik.Data.DTO;
using DopravniPodnik.Data.Models;
using DopravniPodnik.Data.service;

namespace DopravniPodnik.ViewModels;

public partial class FotoViewModel : ViewModelBase
{
    private readonly DatabaseService _databaseService = new();
    [ObservableProperty]
    private ObservableCollection<FotoDTO> _items;
    [ObservableProperty]
    private FotoDTO? _selectedItem;

    public FotoViewModel()
    {
        Items = new();
        LoadItems();
    }

    private void LoadItems()
    {
        var data = _databaseService.FetchData<FotoDTO>($"SELECT * FROM FOTO_VIEW");
        foreach (var entry in data)
        {
            Items.Add(entry);
        }
    }

    [RelayCommand]
    private void Detail()
    {
        if(SelectedItem!=null)
            WindowManager.SetContentView(typeof(FotoDetailsViewModel), new object[] { SelectedItem.IdFoto });
    }
}