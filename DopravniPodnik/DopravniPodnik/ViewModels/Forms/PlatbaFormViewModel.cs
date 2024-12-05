using System.Data;
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
    [ObservableProperty] private DateTime casPlatby;
    [ObservableProperty] private string typPlatbyText;
    [ObservableProperty] private string vysePlatby;
    private int idPlatby;
    private int idZakaznika;
    private int idTypPlatby;

    public PlatbaFormViewModel(PlatbyDTO platba)
    {
        if(platba== null)
            Exit();
        CasPlatby = platba.CasPlatby;
        TypPlatbyText = platba.TypPlatbyText;
        VysePlatby = platba.VysePlatby.ToString();
        idPlatby = platba.IdPlatby;
        idZakaznika = platba.IdZakaznika;
        idTypPlatby = platba.TypPlatbyText == "Platba převodem" ? 1 : 0;
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
                { Value = idPlatby, Direction = ParameterDirection.Input },
            new OracleParameter("p_cas_platby", OracleDbType.Date) 
                { Value = CasPlatby, Direction = ParameterDirection.Input },
            new OracleParameter("p_vyse_platby", OracleDbType.Decimal)
                { Value = vyse, Direction = ParameterDirection.Input },
            new OracleParameter("p_id_zakaznika", OracleDbType.Decimal)
                { Value = idZakaznika, Direction = ParameterDirection.Input },
            new OracleParameter("p_typ_platby", OracleDbType.Decimal)
                { Value = idTypPlatby, Direction = ParameterDirection.Input }
        };

        var procedureCallWrapper = new ProcedureCallWrapper(query, parameters);
        _databaseService.ExecuteDbCall(procedureCallWrapper, out var error);
        Exit();
    }
}