using CommunityToolkit.Mvvm.ComponentModel;
using DopravniPodnik.Data.Models;
using DopravniPodnik.Data.service;

namespace DopravniPodnik.ViewModels.Forms;

public partial class LogyFormViewModel : ViewModelBase
{
    private readonly DatabaseService _databaseService = new();
    
    [ObservableProperty]
    private string? cas;
    [ObservableProperty]
    private string? tabulka;
    [ObservableProperty]
    private string? operace;
    [ObservableProperty]
    private string? staraHodnota;
    [ObservableProperty]
    private string? novaHodnota;
    
    private int? Id;

    public LogyFormViewModel(object selectedItem)
    {
        if (selectedItem != null)
        {
            Cas = ((Logy)selectedItem).Cas.ToString();
            Tabulka = ((Logy)selectedItem).Tabulka;
            Operace = ((Logy)selectedItem).Operace;
            StaraHodnota = ((Logy)selectedItem).StaraHodnota;
            NovaHodnota = ((Logy)selectedItem).NovaHodnota;
            Id = ((Logy)selectedItem).IdLogu;
        }
    }
}