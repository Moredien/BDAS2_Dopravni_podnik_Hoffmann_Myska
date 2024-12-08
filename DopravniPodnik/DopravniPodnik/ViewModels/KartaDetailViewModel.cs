using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DopravniPodnik.Data.Models;
using DopravniPodnik.Data.service;

namespace DopravniPodnik.ViewModels;

public partial class KartaDetailViewModel : ViewModelBase
{
    private readonly DatabaseService _databaseService = new();

    [ObservableProperty] private BitmapSource _displayedImage;
    [ObservableProperty] private string _platnostOd;
    [ObservableProperty] private string _platnostDo;
    [ObservableProperty] private string _aktivniPredplatne;
    [ObservableProperty] private string _zustatek;

    private Foto? _foto;
    [ObservableProperty] private ImageSource _fotoSource;

    public KartaDetailViewModel(object? selectedItem)
    {
        if (selectedItem == null)
            return;
        KartyMhd? karta;
        if (selectedItem.GetType() == typeof(KartyMhd))
        {
            karta = (KartyMhd)selectedItem;
        }
        else if (selectedItem?.GetType() == typeof(Int32))
        {
            karta = _databaseService
                .FetchData<KartyMhd>($"SELECT * FROM KARTY_MHD WHERE ID_KARTY = {(int)selectedItem}").FirstOrDefault();
            LoadPhoto((Int32)selectedItem);
        }
        else
            return;

        if (karta != null)
        {
            PlatnostOd = karta.PlatnostOd.ToString("dd.MM. yyyy");
            PlatnostDo = karta.PlatnostDo.ToString("dd.MM. yyyy");
            AktivniPredplatne = GetAktivniPredplatne(karta.IdKarty);
            Zustatek = String.Concat(karta.Zustatek.ToString(), " Kč");
            LoadPhoto(karta.IdKarty);
        }
    }

    [RelayCommand]
    private void PhotoDetails()
    {
        if (_foto?.Data == null)
            return;
        WindowManager.SetContentView(typeof(FotoDetailsViewModel), new object[] { _foto.IdFoto });
    }

    private void LoadPhoto(int idKarty)
    {
        _foto = _databaseService.FetchData<Foto>($"SELECT * FROM FOTO WHERE ID_KARTY = {idKarty}").FirstOrDefault();

        var bitmap = new BitmapImage();

        using (var memoryStream = new MemoryStream(_foto.Data))
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

    private string GetAktivniPredplatne(int idKarty)
    {
        var predplatne =
            _databaseService.FetchData<Predplatne>(
                    $"SELECT * FROM PREDPLATNE WHERE ID_KARTY = {idKarty} AND OD < SYSDATE AND DO > SYSDATE")
                .FirstOrDefault();
        if (predplatne != null)
        {
            var typPredplatneho =
                _databaseService.FetchData<TypyPredplatneho>(
                        $"SELECT * FROM TYPY_PREDPLATNEHO WHERE ID_TYP_PREDPLATNEHO = {predplatne.IdTypPredplatneho}")
                    .FirstOrDefault();
            if (typPredplatneho != null) return typPredplatneho.Jmeno;
        }

        return String.Empty;
    }
}