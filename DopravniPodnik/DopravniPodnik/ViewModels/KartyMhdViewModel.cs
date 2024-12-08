using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DopravniPodnik.Data.DTO;
using DopravniPodnik.Data.service;
using DopravniPodnik.ViewModels.Forms;

namespace DopravniPodnik.ViewModels;

public partial class KartyMhdViewModel : ViewModelBase
{
    private readonly DatabaseService _databaseService = new();
    [ObservableProperty]
    private ObservableCollection<KartyMhdViewDTO> _items;
    [ObservableProperty]
    private KartyMhdViewDTO? _selectedItem;


    public KartyMhdViewModel()
    {
        Items = new();
        LoadKarty();
    }

    private void LoadKarty()
    {

        var data = _databaseService.FetchData<KartyMhdViewDTO>(
            $"SELECT * FROM KARTY_MHD_VIEW");
        foreach (var entry in data)
        {
            Items.Add(entry);
        }
    }
    [RelayCommand]
    public void Detail()
    {
        if(SelectedItem!=null)
            WindowManager.SetContentView(typeof(KartaDetailViewModel), new object[] { SelectedItem.IdKarty });
    }
    public override void Update()
    {
        base.Update();
        WindowManager.ReturnToSelectedContentView();
    }
    [RelayCommand]
    public void Dobit()
    {
        if(SelectedItem!=null)
            WindowManager.SetContentView(typeof(DobitKartuViewModel), new object[] { SelectedItem });
    }
}