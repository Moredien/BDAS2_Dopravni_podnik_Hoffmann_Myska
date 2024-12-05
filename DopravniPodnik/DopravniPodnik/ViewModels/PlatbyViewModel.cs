using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DopravniPodnik.Data.DTO;
using DopravniPodnik.Data.service;
using DopravniPodnik.Utils;
using DopravniPodnik.ViewModels.Forms;

namespace DopravniPodnik.ViewModels;

public partial class PlatbyViewModel : ViewModelBase
{
    private readonly DatabaseService _databaseService = new();
    [ObservableProperty]
    public ObservableCollection<PlatbyDTO> items;
    [ObservableProperty]
    public PlatbyDTO selectedItem;

    public PlatbyViewModel()
    {
        Items = new();
        LoadItems();
    }

    private void LoadItems()
    {
        var data = _databaseService.FetchData<PlatbyDTO>($"SELECT * FROM PLATBY_VIEW");
        foreach (var entry in data)
        {
            Items.Add(entry);
        }
    }

    [RelayCommand]
    private void Zmenit()
    {
        WindowManager.SetContentView(typeof(PlatbaFormViewModel), new object[] { selectedItem });
    }
    [RelayCommand]
    private void Odstranit()
    {
        string query;
        //decide from which child table to delete
        if (SelectedItem.TypPlatby == 0)
            query = $"DELETE FROM PLATBY_KARTOU WHERE ID_PLATBY = {selectedItem.IdPlatby}";
        else if(SelectedItem.TypPlatby ==1){}
            query = $"DELETE FROM PLATBY_PREVODEM WHERE ID_PLATBY = {selectedItem.IdPlatby}";
            
        var procedureCallWrapper = new ProcedureCallWrapper(query, new());
        _databaseService.ExecuteDbCall(procedureCallWrapper, out var _);
        //delete from parent table
        query = $"DELETE FROM PLATBY WHERE ID_PLATBY = {selectedItem.IdPlatby}";
        procedureCallWrapper = new ProcedureCallWrapper(query, new());
        _databaseService.ExecuteDbCall(procedureCallWrapper, out var _);
            
        Items.Remove(SelectedItem);
    }
}