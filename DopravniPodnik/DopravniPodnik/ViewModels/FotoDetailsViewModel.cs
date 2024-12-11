using System.IO;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using CommunityToolkit.Mvvm.ComponentModel;
using DopravniPodnik.Data.Models;
using DopravniPodnik.Data.service;

namespace DopravniPodnik.ViewModels;

public partial class FotoDetailsViewModel : ViewModelBase
{
    private readonly DatabaseService _databaseService = new();
    [ObservableProperty]
    private Foto? _foto;
    [ObservableProperty]
    private ImageSource? _fotoSource;



    public FotoDetailsViewModel(int fotoId)
    {
        Foto = FetchFoto(fotoId).Result;
        if (Foto != null)
            FotoSource = CreateImageSourceFromBytes(Foto.Data);
    }

    private Task<Foto> FetchFoto(int id)
    {
        return Task.FromResult(_databaseService.FetchData<Foto>($"SELECT * FROM FOTO WHERE ID_FOTO = {id}")[0]);
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