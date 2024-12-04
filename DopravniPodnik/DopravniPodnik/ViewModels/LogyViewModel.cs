using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DopravniPodnik.Data.Models;
using DopravniPodnik.Data.service;
using DopravniPodnik.ViewModels.Forms;

namespace DopravniPodnik.ViewModels;

public partial class LogyViewModel : ViewModelBase
{
    private DatabaseService _databaseService = new();
    [ObservableProperty] private ObservableCollection<Logy> items;

    [ObservableProperty]
    private Logy selectedItem;

    public LogyViewModel()
    {
        Items = new();
        var data = _databaseService.FetchData<Logy>($"SELECT * FROM LOGY");
        foreach (var entry in data)
        {
            Items.Add(entry);
        }
    }

    [RelayCommand]
    private void Detail()
    {
        if (selectedItem == null)
            return;
        WindowManager.SetContentView(typeof(LogyFormViewModel), new object[] { selectedItem });
    }
}