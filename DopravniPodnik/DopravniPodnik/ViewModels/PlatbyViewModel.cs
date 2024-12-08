using System.Collections.ObjectModel;
using System.Windows;
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
    private ObservableCollection<PlatbyDTO> _items;
    [ObservableProperty]
    private PlatbyDTO? _selectedItem;

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
        if(SelectedItem != null)
            WindowManager.SetContentView(typeof(PlatbaFormViewModel), new object[] { SelectedItem });
    }
    [RelayCommand]
    private void Odstranit()
    {
        if (SelectedItem == null)
        {
            MessageBox.Show($"Nebyl vybrán žádný záznam", "", 
                MessageBoxButton.OK, MessageBoxImage.Error);
            return;
        }
        
        var confirmation = MessageBox.Show(
            "Opravdu chcete záznam o platbě odebrat ?", 
            "Potvrzeni", 
            MessageBoxButton.YesNo);
        
        if(confirmation == MessageBoxResult.No) return;
        
        string query;
        //decide from which child table to delete
        if (SelectedItem.TypPlatby == 0)
            query = $"DELETE FROM PLATBY_KARTOU WHERE ID_PLATBY = {SelectedItem.IdPlatby}";
        else if(SelectedItem.TypPlatby ==1){}
            query = $"DELETE FROM PLATBY_PREVODEM WHERE ID_PLATBY = {SelectedItem.IdPlatby}";
            
        var procedureCallWrapper = new ProcedureCallWrapper(query, new());
        _databaseService.ExecuteDbCall(procedureCallWrapper, out var error);
        if (!string.IsNullOrEmpty(error))
        {
            MessageBox.Show($"Při mazání data z databáze došlo k chybě", "Chyba pri mazani", 
                MessageBoxButton.OK, MessageBoxImage.Error);
            return;
        }
        
        //delete from parent table
        query = $"DELETE FROM PLATBY WHERE ID_PLATBY = {SelectedItem.IdPlatby}";
        procedureCallWrapper = new ProcedureCallWrapper(query, new());
        _databaseService.ExecuteDbCall(procedureCallWrapper, out var errorPlatby);
        if (!string.IsNullOrEmpty(errorPlatby))
        {
            MessageBox.Show($"Při mazání data z databáze došlo k chybě", "Chyba pri mazani", 
                MessageBoxButton.OK, MessageBoxImage.Error);
            return;
        }
            
        Items.Remove(SelectedItem);
    }
    public override void Update()
    {
        base.Update();
        WindowManager.ReturnToSelectedContentView();
    }
}