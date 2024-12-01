using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DopravniPodnik.Data.Models;
using DopravniPodnik.Data.service;

namespace DopravniPodnik.ViewModels;

public partial class KartyViewModel : ViewModelBase
{
    private readonly DatabaseService _databaseService = new();
    public ObservableCollection<KartyMhd> Items;
    [ObservableProperty]
    public KartyMhd selectedItem;

    private int id;

    // to be finished when the photos can be uploaded
    public KartyViewModel()
    {
        LoadKarty();
    }

    private void LoadKarty()
    {
        // _databaseService.FetchData<KartyMhd>($"SELECT * FROM ST67028.KARTY_MHD WHERE ID_ZAKAZNIKA = {id}");
    }
    [RelayCommand]
    public void Zalozit()
    {
        Console.WriteLine("zalozit");
    }
    [RelayCommand]
    public void Informace()
    {
        Console.WriteLine("informace");

    }
    [RelayCommand]
    public void Zaplatit()
    {
        Console.WriteLine("zaplatit");

    }
}