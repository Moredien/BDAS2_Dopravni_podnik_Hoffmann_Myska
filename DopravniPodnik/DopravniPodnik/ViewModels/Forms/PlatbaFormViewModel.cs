using System.Data;
using System.Globalization;
using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DopravniPodnik.Data.DTO;
using DopravniPodnik.Data.service;
using DopravniPodnik.Utils;
using Oracle.ManagedDataAccess.Client;

namespace DopravniPodnik.ViewModels.Forms;

public partial class PlatbaFormViewModel : ViewModelBase
{
    private readonly DatabaseService _databaseService = new();
    [ObservableProperty] private DateTime _casPlatby;
    [ObservableProperty] private string _typPlatbyText;
    [ObservableProperty] private string _vysePlatby;
    private int _idPlatby;
    private int _idZakaznika;
    private int _idTypPlatby;

    public PlatbaFormViewModel(PlatbyDTO? platba)
    {
        if(platba== null)
            Exit();
        CasPlatby = platba!.CasPlatby;
        TypPlatbyText = platba.TypPlatbyText;
        VysePlatby = platba.VysePlatby.ToString(CultureInfo.InvariantCulture);
        _idPlatby = platba.IdPlatby;
        _idZakaznika = platba.IdZakaznika;
        _idTypPlatby = platba.TypPlatbyText == "Platba převodem" ? 1 : 0;
    }

    [RelayCommand]
    private void Save()
    {
        double vyse;
        if (!Double.TryParse(VysePlatby, out vyse))
            return;
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
                { Value = _idPlatby, Direction = ParameterDirection.Input },
            new OracleParameter("p_cas_platby", OracleDbType.Date) 
                { Value = CasPlatby, Direction = ParameterDirection.Input },
            new OracleParameter("p_vyse_platby", OracleDbType.Decimal)
                { Value = vyse, Direction = ParameterDirection.Input },
            new OracleParameter("p_id_zakaznika", OracleDbType.Decimal)
                { Value = _idZakaznika, Direction = ParameterDirection.Input },
            new OracleParameter("p_typ_platby", OracleDbType.Decimal)
                { Value = _idTypPlatby, Direction = ParameterDirection.Input }
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