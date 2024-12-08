using System.Data;
using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DopravniPodnik.Data.Models;
using DopravniPodnik.Data.service;
using DopravniPodnik.Utils;
using Oracle.ManagedDataAccess.Client;

namespace DopravniPodnik.ViewModels.Forms;

public partial class LinkyFormViewModel : ViewModelBase
{
    private readonly DatabaseService _databaseService = new();
    [ObservableProperty] private string _cisloLinky = "";
    [ObservableProperty] private string? _jmeno ="";
    private object _idLinky;

    public LinkyFormViewModel(Linky? linka)
    {
        if (linka == null)
            _idLinky = DBNull.Value;
        else
        {
            _idLinky = linka.IdLinky;
            CisloLinky = linka.CisloLinky.ToString();
            Jmeno = linka.Jmeno;
        }
    }

    [RelayCommand]
    private void Submit()
    {
        int cislo;
        if (!Int32.TryParse(CisloLinky, out cislo))
            return;
        string query = @"
            BEGIN
                ST67028.INSERT_UPDATE.edit_linky(
                    :p_id_linky,
                    :p_cislo_linky,
                    :p_jmeno
                );
            END;
        ";
        
        var parameters = new List<OracleParameter>
        {
            new OracleParameter("p_id_linky", OracleDbType.Decimal)
                { Value = _idLinky, Direction = ParameterDirection.Input },
            new OracleParameter("p_cislo_linky", OracleDbType.Decimal) 
                { Value = cislo, Direction = ParameterDirection.Input },
            new OracleParameter("p_jmeno", OracleDbType.Varchar2)
                { Value = Jmeno, Direction = ParameterDirection.Input }
        };

        var procedureCallWrapper = new ProcedureCallWrapper(query, parameters);
        _databaseService.ExecuteDbCall(procedureCallWrapper, out var error);
        if (!string.IsNullOrEmpty(error))
        {
            MessageBox.Show($"Při ukládání data do databáze došlo k chybě", "Chyba pri ukladani", 
                MessageBoxButton.OK, MessageBoxImage.Error);
            return;
        }
        Exit();
    }
}