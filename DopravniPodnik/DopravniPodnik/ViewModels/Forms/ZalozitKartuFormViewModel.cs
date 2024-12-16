using System.Collections.ObjectModel;
using System.Data;
using System.IO;
using System.Runtime.InteropServices.JavaScript;
using System.Windows;
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

public class MetodyPlatby
{
    public string Jmeno { get; set; }
    public int value { get; set; }
}
public partial class ZalozitKartuFormViewModel : ViewModelBase
{
    private readonly DatabaseService _databaseService = new();

    [ObservableProperty] private string _jmeno = "";
    [ObservableProperty] private string _prijmeni = "";
    [ObservableProperty] private DateTime _platnostOd;
    [ObservableProperty] private DateTime _platnostDo;

    [ObservableProperty] private string _typPredplatneho = "Roční";
    [ObservableProperty] private DateTime _konecPredplatneho = DateTime.Today.AddYears(1);
    
    [ObservableProperty]
    private ImageSource _fotoSource;

    private byte[] _fotoData;
    private string JmenoSouboru { get; set; }

    private int _idZakaznika;

    [ObservableProperty] private ObservableCollection<TypyPredplatneho> _typyPredplatneho;
    [ObservableProperty] private TypyPredplatneho? _selectedTypPredplatneho;
    [ObservableProperty] private string _cenaPredplatneho;
    [ObservableProperty] private ObservableCollection<MetodyPlatby> _metodyPlatby;
    [ObservableProperty] private MetodyPlatby? _selectedMetodaPlatby;

    [ObservableProperty] private string _cisloKarty;
    [ObservableProperty] private string _jmenoMajitele;
    [ObservableProperty] private string _cisloUctu;

    [ObservableProperty] private Visibility _kartouFormVisible;
    [ObservableProperty] private Visibility _prevodemFormVisible;
    
    public ZalozitKartuFormViewModel()
    {
        TypyPredplatneho = new();
        MetodyPlatby = new()
        {
            new MetodyPlatby(){Jmeno = "Platební kartou", value = 0},
            new MetodyPlatby(){Jmeno = "Převodem", value = 1},
        };
        SelectedMetodaPlatby = MetodyPlatby[0];
        GetTypyPredplatneho();
        GetUserData();
    }

    partial void OnSelectedMetodaPlatbyChanged(MetodyPlatby value)
    {
        switch (value.value)
        {
            case 0:
                KartouFormVisible = Visibility.Visible;
                PrevodemFormVisible = Visibility.Collapsed;
                break;
            case 1:
                KartouFormVisible = Visibility.Collapsed;
                PrevodemFormVisible = Visibility.Visible;
                break;
            default:
                return;
        }
    }

    partial void OnSelectedTypPredplatnehoChanged(TypyPredplatneho value)
    {
        CenaPredplatneho = $"{value.Cena} Kč";
    }
    private void GetUserData()
    {
        var uzivatel = _databaseService.FetchData<Uzivatele>($"SELECT * FROM UZIVATELE WHERE UZIVATELSKE_JMENO = '{UserSession.Instance.UserName}'").FirstOrDefault();
        if (uzivatel != null)
        {
            var zakaznik = _databaseService.FetchData<Zakaznici>(
                $"SELECT * FROM ZAKAZNICI WHERE ID_UZIVATELE = {uzivatel.IdUzivatele}").FirstOrDefault();
            Jmeno = uzivatel.Jmeno;
            Prijmeni = uzivatel.Prijmeni;
            PlatnostOd= DateTime.Today;
            PlatnostDo = DateTime.Today.AddYears(5);
            _idZakaznika = zakaznik.IdZakaznika;
        }
    }

    private void GetTypyPredplatneho()
    {
        var data = _databaseService.FetchData<TypyPredplatneho>($"SELECT * FROM TYPY_PREDPLATNEHO");
        foreach (var typ in data)
        {
            TypyPredplatneho.Add(typ);
        }
        TypyPredplatneho.Add(new TypyPredplatneho());
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
            _fotoData = File.ReadAllBytes(filePath);
            JmenoSouboru = Path.GetFileName(filePath);

            FotoSource = CreateImageSourceFromBytes(_fotoData);
        }
    }

    [RelayCommand]
    public void Zalozit()
    {
        if (_fotoData == null||
            SelectedMetodaPlatby.value == 0 && (String.IsNullOrEmpty(CisloKarty) || String.IsNullOrEmpty(JmenoMajitele)) ||
            SelectedMetodaPlatby.value == 1 && String.IsNullOrEmpty(CisloUctu))
            return;

        int vysePlatby = SelectedTypPredplatneho == null ? 0 : SelectedTypPredplatneho.Cena;
        
        string query = @"
            BEGIN
                ST67028.ZALOZIT_KARTU(
                    :p_zustatek,
                    :p_platnost_od,
                    :p_platnost_do, 
                    :p_id_zakaznika,
                    :p_foto,
                    :p_jmeno_souboru,
                    :p_typ_predplatneho,
                    :p_konec_predplatneho,
                    :p_vyse_platby,
                    :p_cislo_karty,
                    :p_jmeno_majitele,
                    :p_cislo_uctu
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
                { Value = _idZakaznika, Direction = ParameterDirection.Input },
            new OracleParameter("p_foto", OracleDbType.Blob)
                { Value = _fotoData, Direction = ParameterDirection.Input },
            new OracleParameter("p_jmeno_souboru", OracleDbType.Varchar2)
                { Value = JmenoSouboru, Direction = ParameterDirection.Input },
            new OracleParameter("p_typ_predplatneho", OracleDbType.Varchar2)
                { Value = SelectedTypPredplatneho.Jmeno, Direction = ParameterDirection.Input },
            new OracleParameter("p_konec_predplatneho", OracleDbType.Date)
                { Value = KonecPredplatneho, Direction = ParameterDirection.Input },
            new OracleParameter("p_vyse_platby", OracleDbType.Decimal)
                { Value = vysePlatby, Direction = ParameterDirection.Input },
            new OracleParameter("p_cislo_karty", OracleDbType.Varchar2)
                { Value = CisloKarty, Direction = ParameterDirection.Input },
            new OracleParameter("p_jmeno_majitele", OracleDbType.Varchar2)
                { Value = JmenoMajitele, Direction = ParameterDirection.Input },
            new OracleParameter("p_cislo_uctu", OracleDbType.Varchar2)
                { Value = CisloUctu, Direction = ParameterDirection.Input },
        };

        var procedureCallWrapper = new ProcedureCallWrapper(query, parameters);
        _databaseService.ExecuteDbCall(procedureCallWrapper, out var error);
        if (!string.IsNullOrEmpty(error))
        {
            MessageBox.Show($"Při ukládání data do databáze došlo k chybě", "Chyba pri ukladani", 
                MessageBoxButton.OK, MessageBoxImage.Error);
            return;
        }
        
        Exit();
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