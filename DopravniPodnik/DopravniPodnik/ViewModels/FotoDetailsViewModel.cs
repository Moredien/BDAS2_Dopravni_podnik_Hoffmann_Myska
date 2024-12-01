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
    public Foto foto;
    [ObservableProperty]
    public ImageSource fotoSource;



    public FotoDetailsViewModel(int foto_id)
    {
        Foto = FetchFoto(foto_id);
        if (Foto != null)
            fotoSource = CreateImageSourceFromBytes(Foto.Data);
    }

    private Foto FetchFoto(int id)
    {
        return _databaseService.FetchData<Foto>($"SELECT * FROM FOTO WHERE ID_FOTO = {id}")[0];
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