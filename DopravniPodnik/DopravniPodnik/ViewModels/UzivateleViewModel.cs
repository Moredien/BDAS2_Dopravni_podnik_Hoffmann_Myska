using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DopravniPodnik.Data.DTO;
using DopravniPodnik.Data.service;
using DopravniPodnik.ViewModels.Forms;

namespace DopravniPodnik.ViewModels;

public partial class UzivateleViewModel : ViewModelBase
{
    public ObservableCollection<UzivatelDTO> Uzivatele { get; set; }
    
    [ObservableProperty]
    private UzivatelDTO _selectedUzivatel;
    
    private readonly UserService _userService = new();

    public UzivateleViewModel()
    {
        SelectedUzivatel = _selectedUzivatel;
        Uzivatele = new ObservableCollection<UzivatelDTO>();

       var allUsers = _userService.FetchAllUsers();

       foreach (var user in allUsers)
       {
           Uzivatele.Add((UzivatelDTO)user);
       }

    }

    [RelayCommand]
    void Edit()
    {
        if (SelectedUzivatel == null)
        {
            Console.WriteLine("Neni vybran zadny uzivatel");
            return;
        }
        WindowManager.SetContentView(typeof(UzivatelFormViewModel),false,null,new object[]{_selectedUzivatel,Uzivatele});
    }
    [RelayCommand]
    void Create()
    {
        WindowManager.SetContentView(typeof(UzivatelFormViewModel),false,null,new object[]{Uzivatele});
    }
    [RelayCommand]
    void Delete()
    {
        if(SelectedUzivatel!=null)
            Uzivatele.Remove(SelectedUzivatel);
    }
}