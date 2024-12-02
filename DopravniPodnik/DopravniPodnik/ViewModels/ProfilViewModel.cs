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
using DopravniPodnik.Data.Models;
using Microsoft.Win32;
using Oracle.ManagedDataAccess.Client;


namespace DopravniPodnik.ViewModels;

public partial class ProfilViewModel : ViewModelBase
{
    private readonly DatabaseService _databaseService = new();
    private readonly OracleDbContext _context = OracleDbContext.Instance;
    
    // public Image Foto;
    [ObservableProperty] public BitmapSource displayedImage;
    [ObservableProperty] public UzivatelDTO uzivatel;

    [ObservableProperty]
    public ImageSource fotoSource;

    private Foto testFoto;
    
    public ProfilViewModel()
    {
        FetchUzivatel();
        LoadPhoto();

    }

    public ProfilViewModel(UzivatelDTO uzivatel)
    {
        this.uzivatel = uzivatel;
        LoadPhoto();
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

        var foto = LoadFotoFromFile();
        if(foto==null)
            return;
        testFoto = foto;
        string query = @"
            BEGIN
                ST67028.INSERT_UPDATE.edit_foto(
                    :p_id_foto,
                    :p_jmeno_souboru,
                    :p_data, 
                    :p_datum_pridani,
                    :p_id_karty,
                    :p_id_uzivatele
                );
            END;
        ";
        
        var parameters = new List<OracleParameter>
        {
            new OracleParameter("p_id_foto", OracleDbType.Decimal)
                { Value = DBNull.Value, Direction = ParameterDirection.Input },
            new OracleParameter("p_jmeno_souboru", OracleDbType.Varchar2) 
                { Value = foto.JmenoSouboru, Direction = ParameterDirection.Input },
            // this doesnt work with files over 4000 bytes
            new OracleParameter("p_data", OracleDbType.Blob)
                { Value = foto.Data, Direction = ParameterDirection.Input },
            new OracleParameter("p_datum_pridani", OracleDbType.Date)
                { Value = foto.DatumPridani, Direction = ParameterDirection.Input },
            new OracleParameter("p_id_karty", OracleDbType.Decimal)
            { Value = DBNull.Value, Direction = ParameterDirection.Input },
            new OracleParameter("p_id_uzivatele", OracleDbType.Decimal)
            { Value = foto.IdUzivatele, Direction = ParameterDirection.Input }
        };
        Console.WriteLine($"size: {foto.Data.Length}");
        var procedureCallWrapper = new ProcedureCallWrapper(query, parameters);
        _databaseService.ExecuteDbCall(procedureCallWrapper, out var error);
    }

    [RelayCommand]
    public void PhotoDetails()
    {
        WindowManager.SetContentView(typeof(FotoDetailsViewModel), new object[] { uzivatel.id_foto });
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
        if (uzivatel.foto_data == null || uzivatel.foto_data.Length==0)
        {
            bitmap.BeginInit();
            bitmap.UriSource = new Uri(Path.GetFullPath("../../../Images/defaultUser.png"), UriKind.Absolute);
            bitmap.EndInit();
            FotoSource = bitmap;
            
        }
        else
        {
            using (var memoryStream = new MemoryStream(uzivatel.foto_data))
            {
                BitmapImage bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad; // Load the image into memory
                bitmapImage.StreamSource = memoryStream;
                bitmapImage.EndInit();
                bitmapImage.Freeze(); // Make it thread-safe
                FotoSource = bitmapImage;
            }
        }
    }


    private Foto LoadFotoFromFile()
    {
        OpenFileDialog openFileDialog = new OpenFileDialog
        {
            Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp;*.gif",
            Title = "Select an Image"
        };

        if (openFileDialog.ShowDialog() == true)
        {
            string filePath = openFileDialog.FileName;
            var data = File.ReadAllBytes(filePath);
            return new Foto()
            {
                Data = data,
                DatumPridani = DateTime.Now,
                IdFoto = null,
                IdKarty = null,
                IdUzivatele = Uzivatel.id_uzivatele,
                JmenoSouboru = Path.GetFileName(filePath)
            };
        }

        return null;
    } 
    
    
}