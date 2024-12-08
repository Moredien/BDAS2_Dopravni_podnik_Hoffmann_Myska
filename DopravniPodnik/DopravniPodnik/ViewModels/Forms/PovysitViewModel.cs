using System.Collections.ObjectModel;
using System.Data;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DopravniPodnik.Data.DTO;
using DopravniPodnik.Data.Models;
using DopravniPodnik.Data.service;
using DopravniPodnik.Utils;
using DopravniPodnik.Views;
using Oracle.ManagedDataAccess.Client;

namespace DopravniPodnik.ViewModels.Forms;

public partial class PovysitViewModel : ViewModelBase
{
    private readonly DatabaseService _databaseService = new();

    [ObservableProperty] private int _aktualniPlat;
    [ObservableProperty] private string _novyPlat = "";
    [ObservableProperty] private ObservableCollection<ZamestnanecViewDTO> _zamestnanci = new();
    [ObservableProperty] private ObservableCollection<ZamestnanecViewDTO> _podrizeni = new();
    [ObservableProperty] private ZamestnanecViewDTO? _selectedNadrizeny;
    [ObservableProperty] private ZamestnanecViewDTO? _selectedPodrizeny;

    private ZamestnanecViewDTO? _editedZamestnanec;

    public PovysitViewModel(ZamestnanecViewDTO? zamestanenc)
    {
        _editedZamestnanec = zamestanenc;
        if (zamestanenc != null)
        {
            AktualniPlat = zamestanenc.Plat;
            NovyPlat = zamestanenc.Plat.ToString();
        }

        LoadZamestnanci();
        
        
    }

    [RelayCommand]
    private void PridatPodrizeneho()
    {
        if (SelectedPodrizeny != null && SelectedPodrizeny.IdZamestnance != null &&
            _editedZamestnanec!=null&&
            SelectedPodrizeny.IdZamestnance != _editedZamestnanec.IdZamestnance)
        {
            if(!Podrizeni.Contains(SelectedPodrizeny) && SelectedPodrizeny!= SelectedNadrizeny)
                Podrizeni.Add(SelectedPodrizeny);
        }
    }

    [RelayCommand]
    private void Submit()
    {
        if (SelectedNadrizeny.IdZamestnance == _editedZamestnanec.IdZamestnance ||
            !Int32.TryParse(NovyPlat, out int plat)) 
            return;
        string podrizeniId = string.Join(",",Podrizeni.Select(p => p.IdZamestnance));
        
        string query = @"
            BEGIN
                ST67028.POVYSIT_ZAMESTNANCE(
                    :p_id_zamestnance,
                    :p_novy_plat,
                    :p_id_noveho_nadrizeneho, 
                    :p_list_podrizenych
                );
            END;
        ";
        
        var parameters = new List<OracleParameter>
        {
            new OracleParameter("p_id_zamestnance", OracleDbType.Decimal)
                { Value = _editedZamestnanec.IdZamestnance, Direction = ParameterDirection.Input },
            new OracleParameter("p_novy_plat", OracleDbType.Decimal) 
                { Value = plat, Direction = ParameterDirection.Input },
            new OracleParameter("p_id_noveho_nadrizeneho", OracleDbType.Decimal)
                { Value = SelectedNadrizeny.IdZamestnance, Direction = ParameterDirection.Input },
            new OracleParameter("p_list_podrizenych", OracleDbType.Varchar2)
                { Value = podrizeniId, Direction = ParameterDirection.Input }
        };

        var procedureCallWrapper = new ProcedureCallWrapper(query, parameters);
        _databaseService.ExecuteDbCall(procedureCallWrapper, out var error);
        Exit();
    }
    private void LoadZamestnanci()
    {
        var data = _databaseService.FetchData<ZamestnanecViewDTO>($"SELECT * FROM ZAMESTNANCI_VIEW");
        foreach (var entry in data)
        {
            if (entry.IdZamestnance == _editedZamestnanec.IdZamestnance)
                continue;
            Zamestnanci.Add(entry);
        }
        Zamestnanci.Add(new ZamestnanecViewDTO());
    }
}