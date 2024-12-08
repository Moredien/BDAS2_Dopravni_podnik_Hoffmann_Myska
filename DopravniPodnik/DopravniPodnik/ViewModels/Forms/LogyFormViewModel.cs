using System.Globalization;
using CommunityToolkit.Mvvm.ComponentModel;
using DopravniPodnik.Data.Models;

namespace DopravniPodnik.ViewModels.Forms;

public partial class LogyFormViewModel : ViewModelBase
{
    [ObservableProperty]
    private string? _cas;
    [ObservableProperty]
    private string? _tabulka;
    [ObservableProperty]
    private string? _operace;
    [ObservableProperty]
    private string? _staraHodnota;
    [ObservableProperty]
    private string? _novaHodnota;
    
    public LogyFormViewModel(object? selectedItem)
    {
        if (selectedItem != null)
        {
            Cas = ((Logy)selectedItem).Cas.ToString(CultureInfo.InvariantCulture);
            Tabulka = ((Logy)selectedItem).Tabulka;
            Operace = ((Logy)selectedItem).Operace;
            StaraHodnota = ((Logy)selectedItem).StaraHodnota;
            NovaHodnota = ((Logy)selectedItem).NovaHodnota;
        }
    }
}