using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ImageProcessing;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }

    private void MenuItem_Click(object sender, RoutedEventArgs e)
    {

        var dialog = new Microsoft.Win32.OpenFileDialog();
        dialog.FileName = "Image"; // Default file name
        dialog.DefaultExt = ".png"; // Default file extension
        dialog.Filter = "Image file|*.png; *.jpg; *.bmp"; // Filter files by extension

        bool? result = dialog.ShowDialog();

        if (result == true)
        {
            imageBox.Source = new BitmapImage(new Uri(dialog.FileName));
        }

    }

}