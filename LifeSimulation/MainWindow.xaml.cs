using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
//using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
//using System.Windows.Shapes;
using System;
using System.Windows.Threading;
using System.Drawing;
using System.IO;
using static System.Net.Mime.MediaTypeNames;

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
    private bool[,] _field;
    private int _rows;
    private int _cols;
    private int _countGeneration = 0;

    public MainWindow()
    {
        InitializeComponent();
    }

    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
        udcResolution.Value = 3;
        udcDensity.Value = 2;

        _dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
        _dispatcherTimer.Interval = new TimeSpan(100);
    }

    public void GameStart()
    {
        _bitmap = new Bitmap((int)border.ActualWidth, (int)border.ActualHeight);
        _graphics = Graphics.FromImage(_bitmap);

        _resolution = (int)udcResolution.Value;
        udcResolution.IsEnabled = udcDensity.IsEnabled = bStart.IsEnabled = false;

        _rows = (int)border.ActualHeight / _resolution;
        _cols = (int)border.ActualWidth / _resolution;

        _field = new bool[_cols, _rows];

        Random random = new Random();

        for (int x = 0; x < _cols; x++)
        {
            for (int y = 0; y < _rows; y++)
            {
                _field[x, y] = random.Next((int)udcDensity.Value) == 0;
            }
        }

        _countGeneration = 0;
        _dispatcherTimer.Start();
    }

    private void NextGeneration()
    {
        var newField = new bool[_cols, _rows];
        _graphics.Clear(Color.Black);

        for (int x = 0; x < _cols; x++)
        {
            for (int y = 0; y < _rows; y++)
            {
                var neighbours = CountNeighbours(x, y);
                var isLive = _field[x, y];

                if (!isLive && neighbours == 3)
                    newField[x, y] = true;
                else if (isLive && (neighbours > 3 || neighbours < 2))
                    newField[x, y] = false;
                else
                    newField[x, y] = _field[x, y];

                if (isLive)
                    _graphics.FillRectangle(Brushes.Crimson, x * _resolution, y * _resolution, _resolution, _resolution);
            }
        }

        _field = newField;
        myImage.Source = BitmapToImageSource(_bitmap);
        Title = $"Generation: {++_countGeneration}";
    }

    private int CountNeighbours(int x, int y)
    {
        int count = 0;

        for (int i = -1; i < 2; i++)
        {
            for (int j = -1; j < 2; j++)
            {
                var col = (x + i + _cols) % _cols;
                var row = (y + j + _rows) % _rows;

                var isSelfChecking = col == x && row == y;
                var isLive = _field[col, row];

                if (isLive && !isSelfChecking)
                    count++;
            }
        }

        return count;
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
}

