﻿using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DopravniPodnik.Data.DTO;
using DopravniPodnik.Data.Models;
using DopravniPodnik.Data.service;
using DopravniPodnik.Utils;
using DopravniPodnik.ViewModels.Forms;

namespace DopravniPodnik.ViewModels;

public partial class KartyViewModel : ViewModelBase
{
    private readonly DatabaseService _databaseService = new();
    [ObservableProperty]
    private ObservableCollection<KartyMhd> _items;
    [ObservableProperty]
    private KartyMhd? _selectedItem;

    private Zakaznici? _zakaznik;

    public KartyViewModel()
    {
        Items = new();
        LoadKarty();
    }

    private async void LoadKarty()
    {
        var idUzivatele = _databaseService
            .FetchData<Uzivatele>($"SELECT * FROM UZIVATELE WHERE UZIVATELSKE_JMENO = '{UserSession.Instance.UserName}'")
            .FirstOrDefault()?.IdUzivatele;
        _zakaznik = _databaseService
            .FetchData<Zakaznici>($"SELECT * FROM ZAKAZNICI WHERE ID_UZIVATELE = {idUzivatele}")
            .FirstOrDefault();
        var data = await Task.Run(() =>_databaseService.FetchData<KartyMhd>(
            $"SELECT * FROM KARTY_MHD WHERE ID_ZAKAZNIKA = {_zakaznik?.IdZakaznika}"));
        foreach (var entry in data)
        {
            Items.Add(entry);
        }
    }
    [RelayCommand]
    public void Zalozit()
    {
        WindowManager.SetContentView(typeof(ZalozitKartuFormViewModel), new object[] { });
    }
    [RelayCommand]
    public void Informace()
    {
        if (SelectedItem != null)
            WindowManager.SetContentView(typeof(KartaDetailViewModel), new object[] { SelectedItem });

    }
    [RelayCommand]
    public void Dobit()
    {
        if (SelectedItem != null) 
            WindowManager.SetContentView(typeof(DobitKartuViewModel), new object[] { SelectedItem });
    }
    public override void Update()
    {
        base.Update();
        WindowManager.ReturnToSelectedContentView();
    }
}