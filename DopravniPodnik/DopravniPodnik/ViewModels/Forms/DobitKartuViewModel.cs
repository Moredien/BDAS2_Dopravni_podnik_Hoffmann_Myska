﻿using System.Collections.ObjectModel;
using System.Data;
using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DopravniPodnik.Data.DTO;
using DopravniPodnik.Data.Models;
using DopravniPodnik.Data.service;
using DopravniPodnik.Utils;
using Oracle.ManagedDataAccess.Client;

namespace DopravniPodnik.ViewModels.Forms;

public partial class DobitKartuViewModel : ViewModelBase
{
    private readonly DatabaseService _databaseService = new();
    [ObservableProperty] private string _castka;

    [ObservableProperty] private ObservableCollection<MetodyPlatby> _metodyPlatby;
    [ObservableProperty] private MetodyPlatby _selectedMetodaPlatby;

    [ObservableProperty] private string _cisloKarty;
    [ObservableProperty] private string _jmenoMajitele;
    [ObservableProperty] private string _cisloUctu;

    [ObservableProperty] private Visibility _kartouFormVisible;
    [ObservableProperty] private Visibility _prevodemFormVisible;

    private KartyMhd? _karta;
    private int _idZakaznika;

    public DobitKartuViewModel(object selectedItem)
    {
        if (selectedItem.GetType() == typeof(KartyMhd))
        {
            var karta = (KartyMhd)selectedItem;
            this._karta = karta;
        }
        else if (selectedItem.GetType() == typeof(KartyMhdViewDTO))
        {
            var kartaDTO = (KartyMhdViewDTO)selectedItem;
            int idKarty = kartaDTO.IdKarty;
            _karta = _databaseService.FetchData<KartyMhd>($"SELECT * FROM KARTY_MHD WHERE ID_KARTY = {idKarty}")
                .FirstOrDefault();
        }
        else
            return;

        _idZakaznika = _karta.IdZakaznika;

        MetodyPlatby = new()
        {
            new MetodyPlatby() { Jmeno = "Platební kartou", value = 0 },
            new MetodyPlatby() { Jmeno = "Převodem", value = 1 },
        };
        SelectedMetodaPlatby = MetodyPlatby[0];
    }

    partial void OnSelectedMetodaPlatbyChanged(MetodyPlatby value)
    {
        switch (value.value)
        {
            case 0:
                KartouFormVisible = Visibility.Visible;
                PrevodemFormVisible = Visibility.Collapsed;
                break;
            case 1:
                KartouFormVisible = Visibility.Collapsed;
                PrevodemFormVisible = Visibility.Visible;
                break;
            default:
                return;
        }
    }

    [RelayCommand]
    private void Zaplatit()
    {
        if (Int32.TryParse(Castka, out int castkaInt) && castkaInt > 0)
        {
            // ProvestPlatbu(castkaInt);
            ZmenitZustatek(castkaInt);
            Exit();
        }
    }

    private void ZmenitZustatek(int castka)
    {
        string query = @"
                    BEGIN
                        ST67028.INSERT_UPDATE.edit_karty_mhd(
                            :p_id_karty,
                            :p_zustatek,
                            :p_platnost_od, 
                            :p_platnost_do,
                            :p_id_zakaznika,
                            :p_id_foto
                        );
                    END;
                ";
        int novaCastka = _karta.Zustatek + castka;

        var parameters = new List<OracleParameter>
        {
            new OracleParameter("p_id_karty", OracleDbType.Decimal)
                { Value = _karta.IdKarty, Direction = ParameterDirection.Input },
            new OracleParameter("p_zustatek", OracleDbType.Decimal)
                { Value = novaCastka, Direction = ParameterDirection.Input },
            new OracleParameter("p_platnost_od", OracleDbType.Date)
                { Value = _karta.PlatnostOd, Direction = ParameterDirection.Input },
            new OracleParameter("p_platnost_do", OracleDbType.Date)
                { Value = _karta.PlatnostDo, Direction = ParameterDirection.Input },
            new OracleParameter("p_id_zakaznika", OracleDbType.Decimal)
                { Value = _karta.IdZakaznika, Direction = ParameterDirection.Input },
            new OracleParameter("p_id_foto", OracleDbType.Decimal)
                { Value = _karta.IdFoto, Direction = ParameterDirection.Input }
        };

        var procedureCallWrapper = new ProcedureCallWrapper(query, parameters);
        _databaseService.ExecuteDbCall(procedureCallWrapper, out var error);
    }

    private void ProvestPlatbu(int castka)
    {
        string query = @"
                    BEGIN
                        ST67028.INSERT_UPDATE.edit_platby(
                            :p_id_platby,
                            :p_cas_platby,
                            :p_vyse_platby, 
                            :p_id_zakaznika,
                            :p_typ_platby
                        );
                    END;
                ";
        var parameters = new List<OracleParameter>
        {
            new OracleParameter("p_id_platby", OracleDbType.Decimal)
                { Value = DBNull.Value, Direction = ParameterDirection.Input },
            new OracleParameter("p_cas_platby", OracleDbType.Date)
                { Value = DateTime.Now, Direction = ParameterDirection.Input },
            new OracleParameter("p_vyse_platby", OracleDbType.Decimal)
                { Value = castka, Direction = ParameterDirection.Input },
            new OracleParameter("p_id_zakaznika", OracleDbType.Decimal)
                { Value = _karta.IdZakaznika, Direction = ParameterDirection.Input },
            new OracleParameter("p_typ_platby", OracleDbType.Decimal)
                { Value = SelectedMetodaPlatby.value, Direction = ParameterDirection.Input }
        };

        var procedureCallWrapper = new ProcedureCallWrapper(query, parameters);
        _databaseService.ExecuteDbCall(procedureCallWrapper, out var error);
        
        //TODO zde by to chtelo vratit id platby a pouzit pro vytvoreni platby kartou/prevodem
        int idPlatby = 0; // id provedene platby priradit zde
        
        if (SelectedMetodaPlatby.value == 0)
        {
            query = @"
                    BEGIN
                        ST67028.INSERT_UPDATE.edit_platby_kartou(
                            :p_id_platby,
                            :p_cislo_karty,
                            :p_jmeno_majitele
                        );
                    END;
                ";
            parameters = new List<OracleParameter>
            {
                new OracleParameter("p_id_platby", OracleDbType.Decimal)
                    { Value = idPlatby, Direction = ParameterDirection.Input },
                new OracleParameter("p_cislo_karty", OracleDbType.Varchar2)
                    { Value = CisloKarty, Direction = ParameterDirection.Input },
                new OracleParameter("p_jmeno_majitele", OracleDbType.Varchar2)
                    { Value = JmenoMajitele, Direction = ParameterDirection.Input }
            };
            procedureCallWrapper = new ProcedureCallWrapper(query, parameters);
            _databaseService.ExecuteDbCall(procedureCallWrapper, out var _);
        }
        else if (SelectedMetodaPlatby.value == 1)
        {
            query = @"
                    BEGIN
                        ST67028.INSERT_UPDATE.edit_platby_prevodem(
                            :p_id_platby,
                            :p_cislo_uctu
                        );
                    END;
                ";
            parameters = new List<OracleParameter>
            {
                new OracleParameter("p_id_platby", OracleDbType.Decimal)
                    { Value = idPlatby, Direction = ParameterDirection.Input },
                new OracleParameter("p_cislo_uctu", OracleDbType.Varchar2)
                    { Value = CisloUctu, Direction = ParameterDirection.Input }
            };
            procedureCallWrapper = new ProcedureCallWrapper(query, parameters);
            _databaseService.ExecuteDbCall(procedureCallWrapper, out var _);
        }
    }
}