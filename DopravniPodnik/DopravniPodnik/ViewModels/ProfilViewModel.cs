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
using System.Windows;
using System.Windows.Media;
using DopravniPodnik.Data.Models;
using Microsoft.Win32;
using Oracle.ManagedDataAccess.Client;


namespace DopravniPodnik.ViewModels;

public partial class ProfilViewModel : ViewModelBase
{
    private readonly DatabaseService _databaseService = new();
    
    [ObservableProperty] private BitmapSource _displayedImage;
    [ObservableProperty] private UzivatelDTO? _uzivatel;

    [ObservableProperty]
    private ImageSource? _fotoSource;

    public ProfilViewModel()
    {
        FetchUzivatel();
        LoadPhoto();
    }

    public ProfilViewModel(int id_uzivatele)
    {
        Uzivatel = _databaseService
            .FetchData<UzivatelDTO>($"SELECT * FROM UZIVATEL_VIEW WHERE ID_UZIVATELE = {id_uzivatele}").FirstOrDefault();
        LoadPhoto();
    }
    public ProfilViewModel(UzivatelDTO uzivatel)
    {
        Uzivatel = uzivatel;
        LoadPhoto();
    }

    private void FetchUzivatel()
    {
        var username = UserSession.Instance.UserName;
        Uzivatel =
            _databaseService.FetchData<UzivatelDTO>(
                $"SELECT * FROM ST67028.UZIVATEL_VIEW WHERE UZIVATELSKE_JMENO = '{username}'")[0];
        
    }

    [RelayCommand]
    public void NewPhoto()
    {
        var foto = LoadFotoFromFile();
        if(foto==null)
            return;
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
        if (!string.IsNullOrEmpty(error))
        {
            MessageBox.Show("Při ukládání data do databáze došlo k chybě", "Chyba pri ukladani", 
                MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    [RelayCommand]
    public void PhotoDetails()
    {
        if (Uzivatel.id_foto == null)
            return;
        WindowManager.SetContentView(typeof(FotoDetailsViewModel), new object[] { Uzivatel.id_foto });
    }

    [RelayCommand]
    public void EditUzivatel()
    {
        WindowManager.SetContentView(typeof(UzivatelFormViewModel), new object[]{Uzivatel});
    }


    private void LoadPhoto()
    {
        
        var bitmap = new BitmapImage();
        if (Uzivatel.id_foto == null)
        {
            bitmap.BeginInit();
            bitmap.UriSource = new Uri(Path.GetFullPath("../../../Images/defaultUser.png"), UriKind.Absolute);
            bitmap.EndInit();
            FotoSource = bitmap;
            
        }
        else
        {
            var fotoData = _databaseService.FetchData<Foto>($"SELECT * FROM FOTO WHERE ID_FOTO = {Uzivatel.id_foto}").FirstOrDefault();
            
            using (var memoryStream = new MemoryStream(fotoData.Data))
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


    private Foto? LoadFotoFromFile()
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
    public override void Update()
    {
        base.Update();
        WindowManager.ReturnToSelectedContentView();
    }
}