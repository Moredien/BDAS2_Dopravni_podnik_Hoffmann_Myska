using System.Data;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DopravniPodnik.Data.DTO;
using DopravniPodnik.Data.service;
using DopravniPodnik.Utils;
using DopravniPodnik.ViewModels.Forms;
using System.Drawing;
using System.IO;
using System.Windows.Media;


namespace DopravniPodnik.ViewModels;

public partial class ProfilViewModel : ViewModelBase
{
    private readonly DatabaseService _databaseService = new();
    private readonly OracleDbContext _context = OracleDbContext.Instance;
    
    public Image Foto;
    [ObservableProperty] public BitmapSource displayedImage;
    [ObservableProperty] public UzivatelDTO uzivatel;

    [ObservableProperty]
    public ImageSource fotoSource;

    public object ImageSource
    {
        get
        {
            var bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.UriSource = new Uri("..\\..\\..\\Images\\defaultUser.png", UriKind.Relative);
            bitmap.EndInit();
            return bitmap;
        }
    }

    public ProfilViewModel()
    {
        FetchUzivatel();
        LoadPhoto();

    }

    public ProfilViewModel(UzivatelDTO uzivatel)
    {
        this.uzivatel = uzivatel;
    }

    private void FetchUzivatel()
    {
        var username = UserSession.Instance.UserName;
        uzivatel =
            _databaseService.FetchData<UzivatelDTO>(
                $"SELECT * FROM ST67028.UZIVATEL_VIEW WHERE UZIVATELSKE_JMENO = '{username}'")[0];
        
    }

    [RelayCommand]
    public void NewPhoto()
    {
        Console.WriteLine("open new photo form");

    }

    [RelayCommand]
    public void PhotoDetails()
    {
        Console.WriteLine("photo details here");
    }

    [RelayCommand]
    public void EditUzivatel()
    {
        WindowManager.SetContentView(typeof(UzivatelFormViewModel), new object[]{uzivatel});
    }


    private void LoadPhoto()
    {
        // fotoSource = new BitmapImage();
        var bitmap = new BitmapImage();
        if (uzivatel.foto_data == null)
        {
            bitmap.BeginInit();
            bitmap.UriSource = new Uri(Path.GetFullPath("../../../Images/defaultUser.png"), UriKind.Absolute);
            bitmap.EndInit();
            fotoSource = bitmap;
            
        }
    }
    
}