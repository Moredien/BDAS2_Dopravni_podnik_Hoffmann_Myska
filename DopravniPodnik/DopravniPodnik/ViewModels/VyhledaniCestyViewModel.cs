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

namespace DopravniPodnik.ViewModels;

public partial class VyhledaniCestyViewModel : ViewModelBase
{
    [ObservableProperty] private ObservableCollection<Zastavky> _zastavky = [];
    [ObservableProperty] private Zastavky? _zastavkaOdkud;
    [ObservableProperty] private Zastavky? _zastavkaKam;

    [ObservableProperty] private ObservableCollection<LinkaDTO> _items;
    private readonly DatabaseService _databaseService = new();

    public VyhledaniCestyViewModel()
    {
        LoadZastavky();
        Items = new ObservableCollection<LinkaDTO>();
    }

    [RelayCommand]
    public void Vyhledat()
    {
        Items.Clear();
        if (ZastavkaOdkud is null)
        {
            MessageBox.Show($"Nebyla vybrána počáteční zastávka", "Prazdna zastavka", 
                MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        if (ZastavkaKam is null)
        {
            MessageBox.Show("Nebyla vybrána konečná zastávka", "Prazdna zastavka", 
                MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        if (ZastavkaOdkud == ZastavkaKam)
        {
            MessageBox.Show("Nemůžete zvolit stejnou zastávku pro začátek a konec", "Chyba", 
                MessageBoxButton.OK, MessageBoxImage.Error);
            return;
        }

        var temp = _databaseService.FetchDataParam<LinkaDTO>(GetProcedureCallWrapper());

        foreach (var linka in temp)
        {
            Items.Add(linka);
        }

    }
    private async void LoadZastavky()
    {
        Zastavky.Clear();

        var data = await Task.Run(() =>_databaseService.FetchData<Zastavky>("SELECT * FROM ST67028.ZASTAVKY"));
        
        foreach (var obj in data)
        {
            Zastavky.Add(obj);
        }
    }

    private ProcedureCallWrapper GetProcedureCallWrapper()
    {
        return new ProcedureCallWrapper(
            "BEGIN :result := FIND_LINKA_FOR_STOPS(:p_start_zastavka, :p_end_zastavka); END;",
            [
                new OracleParameter
                {
                    ParameterName = "result",
                    OracleDbType = OracleDbType.RefCursor,
                    Direction = ParameterDirection.Output
                },

                new OracleParameter
                {
                    ParameterName = "p_start_zastavka",
                    OracleDbType = OracleDbType.Varchar2,
                    Value = ZastavkaOdkud.Jmeno
                },

                new OracleParameter
                {
                    ParameterName = "p_end_zastavka",
                    OracleDbType = OracleDbType.Varchar2,
                    Value = ZastavkaKam.Jmeno
                }
            ]
        );
    }
}