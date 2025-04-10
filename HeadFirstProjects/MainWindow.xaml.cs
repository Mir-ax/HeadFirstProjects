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
using System.Windows.Threading;

namespace MatchGame;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private TextBlock _lastClickedTextBlock;
    private bool _findingMatch = false;

    public MainWindow()
    {
        InitializeComponent();
        SetUpGame();
    }

    private void SetUpGame()
    {
        Random random = new Random();

        List<string> animalEmoji = new List<string>()
        {
            "🍕","🍕",
            "🍔","🍔",
            "🍟","🍟",
            "🌭","🌭",
            "🍿","🍿",
            "🥐","🥐",
            "🍳","🍳",
            "🥚","🥚"
        };

        foreach (var textBlock in mainGrid.Children.OfType<TextBlock>())
        {
            var index = random.Next(animalEmoji.Count);
            textBlock.Text = animalEmoji[index];
            animalEmoji.RemoveAt(index);

        }
    }

    private void TextBlock_MouseDown(object sender, MouseButtonEventArgs e)
    {
        var textBlock = sender as TextBlock;

        if (_findingMatch == false)
        {
            textBlock.Visibility = Visibility.Hidden;
            _lastClickedTextBlock = textBlock;
            _findingMatch = true;
        }
        else if (textBlock.Text == _lastClickedTextBlock.Text)
        {
            textBlock.Visibility = Visibility.Hidden;
            _findingMatch = false;
        }
        else
        {
            _lastClickedTextBlock.Visibility = Visibility.Visible;
            _findingMatch = false;
        }
    }
}