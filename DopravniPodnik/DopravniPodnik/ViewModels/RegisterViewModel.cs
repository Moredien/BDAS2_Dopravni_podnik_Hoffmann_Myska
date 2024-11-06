using System.Security;
using System.Windows;
using System.Windows.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DopravniPodnik.Data.service;
using DopravniPodnik.Utils;

namespace DopravniPodnik.ViewModels;

public partial class RegisterViewModel : ViewModelBase
{
    [ObservableProperty]
    private string uzivatelske_jmeno;
    [ObservableProperty]
    private SecureString heslo_1;
    [ObservableProperty]
    private SecureString heslo_2;
    [ObservableProperty]
    private string jmeno;
    [ObservableProperty]
    private string prijemni;
    [ObservableProperty]
    private DateTime datum_narozeni = DateTime.Today;
    [ObservableProperty]
    private string mesto;
    [ObservableProperty]
    private string ulice;
    [ObservableProperty] 
    private string cislo_popisne;


    [RelayCommand]
    private void ExecuteRegister()
    {
        Console.WriteLine("Registering");
        Console.WriteLine($"pw1: {PasswordBoxHelper.ConvertToUnsecureString(Heslo_1)} pw2: {PasswordBoxHelper.ConvertToUnsecureString(Heslo_2)}");
        Console.WriteLine($"selected date: {Datum_narozeni.ToString()}");
        
    }

}