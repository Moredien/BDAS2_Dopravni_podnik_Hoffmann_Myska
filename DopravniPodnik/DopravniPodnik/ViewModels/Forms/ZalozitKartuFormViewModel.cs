using System.Data;
using System.IO;
using System.Runtime.InteropServices.JavaScript;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DopravniPodnik.Data.Models;
using DopravniPodnik.Data.service;
using DopravniPodnik.Utils;
using Microsoft.Win32;
using Oracle.ManagedDataAccess.Client;

namespace DopravniPodnik.ViewModels.Forms;

public partial class ZalozitKartuFormViewModel : ViewModelBase
{
    private readonly DatabaseService _databaseService = new();

    [ObservableProperty] public string jmeno;
    [ObservableProperty] public string prijmeni;
    [ObservableProperty] public DateTime platnostOd;
    [ObservableProperty] public DateTime platnostDo;

    [ObservableProperty] public string typPredplatneho = "Roční";
    [ObservableProperty] public DateTime konecPredplatneho = DateTime.Today.AddYears(1);
    
    [ObservableProperty]
    public ImageSource fotoSource;

    private byte[] fotoData;

    private int id_zakaznika;
    
    // add typy predplatneho

    public ZalozitKartuFormViewModel()
    {
        GetUserData();
    }

    private void GetUserData()
    {
        var uzivatel = _databaseService.FetchData<Uzivatele>($"SELECT * FROM UZIVATELE WHERE UZIVATELSKE_JMENO = '{UserSession.Instance.UserName}'").FirstOrDefault();
        var zakaznik = _databaseService.FetchData<Zakaznici>(
            $"SELECT * FROM ZAKAZNICI WHERE ID_UZIVATELE = {uzivatel.IdUzivatele}").FirstOrDefault();
        Jmeno = uzivatel.Jmeno;
        Prijmeni = uzivatel.Prijmeni;
        PlatnostOd= DateTime.Today;
        PlatnostDo = DateTime.Today.AddYears(5);
        id_zakaznika = zakaznik.IdZakaznika;
    }

    [RelayCommand]
    public void NewPhoto()
    {
        OpenFileDialog openFileDialog = new OpenFileDialog
        {
            Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp;*.gif",
            Title = "Select an Image"
        };

        if (openFileDialog.ShowDialog() == true)
        {
            string filePath = openFileDialog.FileName;
            fotoData = File.ReadAllBytes(filePath);

            FotoSource = CreateImageSourceFromBytes(fotoData);
        }
    }

    [RelayCommand]
    public void Zalozit()
    {
        if (fotoData == null)
            return;
        string query = @"
            BEGIN
                ST67028.ZALOZIT_KARTU(
                    :p_zustatek,
                    :p_platnost_od,
                    :p_platnost_do, 
                    :p_id_zakaznika,
                    :p_foto,
                    :p_typ_predplatneho,
                    :p_konec_predplatneho
                );
            END;
        ";
        
        var parameters = new List<OracleParameter>
        {
            new OracleParameter("p_zustatek", OracleDbType.Decimal)
                { Value = 0, Direction = ParameterDirection.Input },
            new OracleParameter("p_platnost_od", OracleDbType.Date) 
                { Value = PlatnostOd, Direction = ParameterDirection.Input },
            new OracleParameter("p_platnost_do", OracleDbType.Date)
                { Value = PlatnostDo, Direction = ParameterDirection.Input },
            new OracleParameter("p_id_zakaznika", OracleDbType.Decimal)
                { Value = id_zakaznika, Direction = ParameterDirection.Input },
            new OracleParameter("p_foto", OracleDbType.Blob)
                { Value = fotoData, Direction = ParameterDirection.Input },
            new OracleParameter("p_typ_predplatneho", OracleDbType.Varchar2)
                { Value = typPredplatneho, Direction = ParameterDirection.Input },
            new OracleParameter("p_konec_predplatneho", OracleDbType.Date)
                { Value = KonecPredplatneho, Direction = ParameterDirection.Input }
        };

        var procedureCallWrapper = new ProcedureCallWrapper(query, parameters);
        _databaseService.ExecuteDbCall(procedureCallWrapper, out var error);

    }
    public ImageSource CreateImageSourceFromBytes(byte[] imageData)
    {
        if (imageData == null || imageData.Length == 0)
        {
            throw new ArgumentException("Image data is null or empty.");
        }

        using (var memoryStream = new MemoryStream(imageData))
        {
            BitmapImage bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.CacheOption = BitmapCacheOption.OnLoad; // Load the image into memory immediately
            bitmap.StreamSource = memoryStream;
            bitmap.EndInit();
            bitmap.Freeze(); // Make the BitmapImage thread-safe
            return bitmap;
        }
    }
}