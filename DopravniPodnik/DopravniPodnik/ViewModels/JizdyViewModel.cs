using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using DopravniPodnik.Data.Models;

namespace DopravniPodnik.ViewModels;

public partial class JizdyViewModel : ViewModelBase
{
    public ObservableCollection<Jizdy> Jizdy { get; set; }

    public JizdyViewModel()
    {
        Jizdy = new ObservableCollection<Jizdy>()
        {
            new (){IdJizdy = 0,IdLinky = 1,IdVozidla = 8,Zacatek = new DateTime(2021,5,26,12,36,0),Konec = new DateTime(2021,5,26,12,56,8)},
            new (){IdJizdy = 1,IdLinky = 8,IdVozidla = 4,Zacatek = new DateTime(2021,6,5,6,40,30),Konec = new DateTime(2021,6,5,6,40,1)},
            new (){IdJizdy = 2,IdLinky = 3,IdVozidla = 6,Zacatek = new DateTime(2021,8,24,8,45,12),Konec = new DateTime(2021,8,24,8,55,12)},
            new (){IdJizdy = 3,IdLinky = 11,IdVozidla = 7,Zacatek = new DateTime(2021,1,2,9,14,02),Konec = new DateTime(2021,1,2,9,32,27)}
        };
    }
}