using System.Collections.ObjectModel;
using System.Data;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DopravniPodnik.Data.DTO;
using DopravniPodnik.Data.Models;
using DopravniPodnik.Data.service;
using DopravniPodnik.Utils;
using Oracle.ManagedDataAccess.Client;

namespace DopravniPodnik.ViewModels.Forms;

public partial class DostupnaVozidlaFormViewModel : ViewModelBase
{
    private readonly DatabaseService _databaseService = new();
    [ObservableProperty] private ObservableCollection<TypyVozidel> _typyVozidla = new();
    [ObservableProperty] 
    private TypyVozidel? _selectedTyp;
    [ObservableProperty]
    private string _znacka = "";

    private int? Id { get; set; }


    public DostupnaVozidlaFormViewModel(VozovyParkDTO? selectedItem)
    {
        var data = _databaseService.FetchData<TypyVozidel>("SELECT * FROM ST67028.TYPY_VOZIDEL");
        foreach (var obj in data)
        {
            TypyVozidla.Add(obj);
        }

        if (selectedItem != null)
        {
            Znacka = selectedItem.Znacka;
            Id = selectedItem.IdVozidla;
            SelectedTyp = TypyVozidla.FirstOrDefault(tv => tv.Nazev == selectedItem.TypVozidla);
        }
    }
    [RelayCommand]
    public void Submit()
    {
        if (SelectedTyp == null)
            return;
        string query = @"
            BEGIN
                ST67028.INSERT_UPDATE.edit_vozidla(
                    :p_id_vozidla,
                    :p_id_typ_vozidla,
                    :p_znacka
                );
            END;
        ";
            
        object id;
        if (Id == null)
            id = DBNull.Value;
        else
            id = Id;
        var parameters = new List<OracleParameter>
        {
            new OracleParameter("p_id_vozidla", OracleDbType.Decimal)
                { Value = id, Direction = ParameterDirection.Input },
            new OracleParameter("p_id_typ_vozidla", OracleDbType.Decimal)
                { Value = SelectedTyp.IdTypVozidla, Direction = ParameterDirection.Input },
            new OracleParameter("p_znacka", OracleDbType.Varchar2)
                { Value = Znacka, Direction = ParameterDirection.Input }
        };
        var procedureCallWrapper = new ProcedureCallWrapper(query, parameters);
        _databaseService.ExecuteDbCall(procedureCallWrapper, out var error);

        Console.WriteLine(error);
        Exit();
    }
}