using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Drawing;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Win32;
using System.IO;
using System.Windows.Media.Imaging;
using System.Windows.Controls.Primitives;

namespace ImageProcessing;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private List<Bitmap> _bitmaps = new List<Bitmap>();
    private Random random = new Random();

    public MainWindow()
    {
        InitializeComponent();
    }

    private async void MenuItem_Click(object sender, RoutedEventArgs e)
    {
        _bitmaps.Clear();
        menuFile.IsEnabled = scrollBar.IsEnabled = false;
        var dialog = new OpenFileDialog();
        dialog.FileName = "Image"; // Default file name
        dialog.DefaultExt = ".png"; // Default file extension
        dialog.Filter = "Image file|*.png; *.jpg; *.bmp"; // Filter files by extension

        bool? result = dialog.ShowDialog();

        if (result == true)
        {
            var bitmap = new Bitmap(dialog.FileName);
            await Task.Run(() => { ImageProcessing(bitmap); });
        }
        menuFile.IsEnabled = scrollBar.IsEnabled = true;
    }

    private void ImageProcessing(Bitmap bitmap)
    {
        var pixelsOfImages = GetPixels(bitmap);
        var pixelsInStep = pixelsOfImages.Count / 100;
        var currentPixelsSet = new List<Pixel>();
        var newBitmap = new Bitmap(bitmap.Width, bitmap.Height);
        _bitmaps.Add(newBitmap);

        for (int i = 1; i <100; i++)
        {
            for (int j = 0; j < pixelsInStep; j++)
            {
                var index = random.Next(pixelsOfImages.Count);
                currentPixelsSet.Add(pixelsOfImages[index]);
                pixelsOfImages.RemoveAt(index);
            }

            newBitmap = new Bitmap(bitmap.Width, bitmap.Height);

            foreach (var pixel in currentPixelsSet)
            {
                newBitmap.SetPixel(pixel.Point.X, pixel.Point.Y, pixel.Color);
            }
            _bitmaps.Add(newBitmap);
            this.Dispatcher.Invoke(() =>
            {
                Title = i + "%";
            });
        }
        _bitmaps.Add(bitmap);
    }

    private List<Pixel> GetPixels(Bitmap bitmap)
    {
        var pixelsOfImages = new List<Pixel>(bitmap.Height * bitmap.Width);

        for (int y = 0; y < bitmap.Height; y++)
        {
            for (int x = 0; x < bitmap.Width; x++)
            {
                pixelsOfImages.Add(new Pixel
                {
                    Color = bitmap.GetPixel(x, y),
                    Point = new System.Drawing.Point(x, y)
                });
            }
        }
        return pixelsOfImages;
    }

    private void ScrollBar_Scroll(object sender, System.Windows.Controls.Primitives.ScrollEventArgs e)
    {
        if (_bitmaps == null || _bitmaps.Count == 0)
            return;

        var scrollValue = (int)(scrollBar.Value);
        Title = scrollValue.ToString();
        imageBox.Source = BitmapToImageSource(_bitmaps[scrollValue]);
    }

    BitmapImage BitmapToImageSource(Bitmap bitmap)
    {
        using (MemoryStream memory = new MemoryStream())
        {
            bitmap.Save(memory, System.Drawing.Imaging.ImageFormat.Bmp);
            memory.Position = 0;
            BitmapImage bitmapimage = new BitmapImage();
            bitmapimage.BeginInit();
            bitmapimage.StreamSource = memory;
            bitmapimage.CacheOption = BitmapCacheOption.OnLoad;
            bitmapimage.EndInit();

            return bitmapimage;
        }
    }
}