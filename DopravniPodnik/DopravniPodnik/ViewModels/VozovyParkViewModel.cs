using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DopravniPodnik.Data.DTO;
using DopravniPodnik.Data.service;
using DopravniPodnik.Utils;
using DopravniPodnik.ViewModels.Forms;

namespace DopravniPodnik.ViewModels;

public partial class VozovyParkViewModel : ViewModelBase
{
    [ObservableProperty] 
    private ObservableCollection<VozovyParkDTO> _items = new ();
    [ObservableProperty]
    private VozovyParkDTO? _selectedItem;
    
    private readonly DatabaseService _databaseService = new();

    public VozovyParkViewModel()
    {
        LoadItems();
    }


    private void LoadItems()
    {
        Items.Clear();

        var data = _databaseService.FetchData<VozovyParkDTO>("SELECT * FROM ST67028.DOSTUPNA_VOZIDLA_VIEW");
        
        foreach (var obj in data)
        {
            Items.Add(obj);
        }
        
    }

    [RelayCommand]
    public void Edit()
    {
        WindowManager.SetContentView(typeof(DostupnaVozidlaFormViewModel), new object[] { SelectedItem });
    }
    [RelayCommand]
    public void Create()
    {
        WindowManager.SetContentView(typeof(DostupnaVozidlaFormViewModel), new object[] { null });
    }
    [RelayCommand]
    public void Delete()
    {
        if (SelectedItem == null)
            return;
        string query = $"DELETE FROM VOZIDLA WHERE ID_VOZIDLA = {SelectedItem.IdVozidla}";
            
        var procedureCallWrapper = new ProcedureCallWrapper(query, new());
        _databaseService.ExecuteDbCall(procedureCallWrapper, out var error);

        Items.Remove(SelectedItem);
    }
}