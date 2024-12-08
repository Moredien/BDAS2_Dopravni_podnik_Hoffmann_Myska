using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using DopravniPodnik.Data.DTO;
using DopravniPodnik.Data.service;

namespace DopravniPodnik.ViewModels;

public partial class DBObjektyViewModel : ViewModelBase
{
    private DatabaseService _databaseService = new();
    [ObservableProperty] private ObservableCollection<DBObjektyDTO> _items;

    public DBObjektyViewModel()
    {
        Items = new();
        var data = _databaseService.FetchData<DBObjektyDTO>($"SELECT * FROM DB_OBJEKTY_VIEW");
        foreach (var entry in data)
        {
            Items.Add(entry);
        }
    }
}