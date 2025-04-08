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

namespace MatchGame;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
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
}