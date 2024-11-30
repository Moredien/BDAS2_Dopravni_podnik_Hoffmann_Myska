﻿using System.Collections.ObjectModel;
using System.Data;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DopravniPodnik.Data.DTO;
using DopravniPodnik.Data.Models;
using DopravniPodnik.Data.service;
using DopravniPodnik.Utils;
using Oracle.ManagedDataAccess.Client;

namespace DopravniPodnik.ViewModels;

public partial class InfoOZastavceViewModel : ViewModelBase
{
    [ObservableProperty] public ObservableCollection<Zastavky> zastavky = new ObservableCollection<Zastavky>();
    [ObservableProperty] public Zastavky selectedZastavka;
    // [ObservableProperty] public DateTime datumOdjezdu;
    
    private DateTime? _selectedDate;
    public DateTime? SelectedDate
    {
        get => _selectedDate;
        set
        {
            if (_selectedDate != value)
            {
                _selectedDate = value;
                OnPropertyChanged(nameof(SelectedDate));
            }
        }
    }
    public ObservableCollection<InfoOZastavceDTO> Items { get; set; }
    private readonly DatabaseService _databaseService = new();

    public InfoOZastavceViewModel()
    {
        Items = new ObservableCollection<InfoOZastavceDTO>();
        LoadZastavky();
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

    [RelayCommand]
    public void Vyhledat()
    {
        Items.Clear();
        if (SelectedZastavka == null || SelectedZastavka == null)
            return;
        
        var temp = _databaseService.FetchDataParam<InfoOZastavceDTO>(GetProcedureCallWrapper());
        foreach (var item in temp)
        {
            Items.Add(item);
        }
    }
    
    private ProcedureCallWrapper GetProcedureCallWrapper()
    {
        return new ProcedureCallWrapper(
            "BEGIN :result := FIND_ODJEZDY_ZE_ZASTAVKY(:p_jmeno_zastavky, :p_cas_odjezdu); END;",
            [
                new OracleParameter
                {
                    ParameterName = "result",
                    OracleDbType = OracleDbType.RefCursor,
                    Direction = ParameterDirection.Output
                },

                new OracleParameter
                {
                    ParameterName = "p_jmeno_zastavky",
                    OracleDbType = OracleDbType.Varchar2,
                    Value = SelectedZastavka.Jmeno
                },

                new OracleParameter
                {
                    ParameterName = "p_cas_odjezdu",
                    OracleDbType = OracleDbType.Date,
                    Value = SelectedDate
                }
            ]
        );
    }
}