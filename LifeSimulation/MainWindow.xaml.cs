using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using System.Drawing;
using System.IO;

namespace LifeSimulation;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private Bitmap _bitmap;
    private Graphics _graphics;
    private DispatcherTimer _dispatcherTimer = new DispatcherTimer();
    private int _resolution;

    private GameEngine _gameEngine;

    public MainWindow()
    {
        InitializeComponent();
    }

    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
        udcResolution.Value = 3;
        udcDensity.Value = 25;

        _dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
        _dispatcherTimer.Interval = new TimeSpan(100);
    }

    public void GameStart()
    {
        _resolution = (int)udcResolution.Value;
        udcResolution.IsEnabled = udcDensity.IsEnabled = bStart.IsEnabled = false;

        _bitmap = new Bitmap((int)border.ActualWidth, (int)border.ActualHeight);
        _graphics = Graphics.FromImage(_bitmap);

        _gameEngine = new GameEngine
        (
            rows: (int)border.ActualHeight / _resolution,
            cols: (int)border.ActualWidth / _resolution,
            density: (int)(udcDensity.Minimum + udcDensity.Maximum - udcDensity.Value)
        );

        _dispatcherTimer.Start();
    }

    private void NextGeneration()
    {
        _graphics.Clear(Color.Black);

        var field = _gameEngine.GetCurrentField();
        for (int x = 0; x < field.GetLength(0); x++)
        {
            for (int y = 0; y < field.GetLength(1); y++)
            {
                if (field[x,y])
                    _graphics.FillRectangle(Brushes.Crimson, x * _resolution, y * _resolution, _resolution - 1, _resolution - 1);
            }
        }

        myImage.Source = BitmapToImageSource(_bitmap);
        Title = $"Generation: {_gameEngine.CountGeneration}";

        _gameEngine.NextGeneration();
    }

    private void bStart_Click(object sender, RoutedEventArgs e)
    {
        GameStart();
    }

    private void bStop_Click(object sender, RoutedEventArgs e)
    {
        _dispatcherTimer.Stop();
        udcResolution.IsEnabled = udcDensity.IsEnabled = bStart.IsEnabled = true;
    }
    private void dispatcherTimer_Tick(object? sender, EventArgs e)
    {
        NextGeneration();
    }

    private BitmapImage BitmapToImageSource(Bitmap bitmap)
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

    private void myImage_MouseMove(object sender, MouseEventArgs e)
    {
        if (_dispatcherTimer.IsEnabled == false)
            return;

        var x = (int)e.GetPosition(myImage).X / _resolution;
        var y = (int)e.GetPosition(myImage).Y / _resolution;

        if (e.LeftButton == MouseButtonState.Pressed)
            _gameEngine.AddCell(x, y, true);

        if (e.RightButton == MouseButtonState.Pressed)
            _gameEngine.RemoveCell(x, y, false);
    }
}

