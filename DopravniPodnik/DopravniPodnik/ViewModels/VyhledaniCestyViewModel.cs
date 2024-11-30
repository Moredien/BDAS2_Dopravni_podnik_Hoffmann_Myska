using System.Collections.ObjectModel;
using System.Data;
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
    [ObservableProperty] public ObservableCollection<Zastavky> zastavky = new ObservableCollection<Zastavky>();
    [ObservableProperty] public Zastavky zastavkaOdkud;
    [ObservableProperty] public Zastavky zastavkaKam;
    
    public ObservableCollection<LinkaDTO> Items { get; set; }
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
        if (ZastavkaOdkud == null || ZastavkaKam == null)
            return;
        if(ZastavkaOdkud == ZastavkaKam)
            return;

        var temp = _databaseService.FetchDataParam<LinkaDTO>(GetProcedureCallWrapper());

        foreach (var linka in temp)
        {
            Items.Add(linka);
        }

    }
    private void LoadZastavky()
    {
        zastavky.Clear();

        var data = _databaseService.FetchData<Zastavky>("SELECT * FROM ST67028.ZASTAVKY");
        
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