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
    private DispatcherTimer _timer = new DispatcherTimer();
    private int _tenthsOfSecondsElapsed;
    private int _matchesFound;
    private TextBlock _lastClickedTextBlock;
    private bool _findingMatch = false;

    public MainWindow()
    {
        InitializeComponent();
        _timer.Interval = TimeSpan.FromSeconds(1);
        _timer.Tick += _timer_Tick;
        SetUpGame();
    }

    private void _timer_Tick(object? sender, EventArgs e)
    {
        

        _tenthsOfSecondsElapsed++;
        TimeTextBlock.Text = (_tenthsOfSecondsElapsed / 10F).ToString("0.0s");
        if (_matchesFound == 8)
        {
            _timer.Stop();
            TimeTextBlock.Text = TimeTextBlock.Text + " - Play again?";
        }
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
            if (textBlock.Name != "TimeTextBlock")
            {
                var index = random.Next(animalEmoji.Count);
                textBlock.Text = animalEmoji[index];
                animalEmoji.RemoveAt(index);
            }
        }

        _timer.Start();
        _tenthsOfSecondsElapsed = 0;
        _matchesFound = 0;
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
            _matchesFound++;
            textBlock.Visibility = Visibility.Hidden;
            _findingMatch = false;
        }
        else
        {
            _lastClickedTextBlock.Visibility = Visibility.Visible;
            _findingMatch = false;
        }
    }

    private void TimeTextBlock_MouseDown(object sender, MouseButtonEventArgs e)
    {
        if (_matchesFound == 8)
        {
            SetUpGame();
        }
    }
}