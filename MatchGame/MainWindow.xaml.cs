using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MatchGame
{
    using System.Windows.Threading;
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        DispatcherTimer timer = new DispatcherTimer();
        int tenthsOfSecondsElapsed;
        int matchesFound;
        int highScore = 0;

        public MainWindow()
        {
            InitializeComponent();
            timer.Interval = TimeSpan.FromSeconds(.1);
            timer.Tick += Timer_Tick;
            SetUpGame();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            tenthsOfSecondsElapsed--;
            timeTextBlock.Text = (tenthsOfSecondsElapsed / 10F).ToString("0.0s");
            if (matchesFound == 8)
            {
                if(highScore <= tenthsOfSecondsElapsed)
                {
                    highScore = tenthsOfSecondsElapsed;
                }
                timer.Stop();
                timeTextBlock.Text = timeTextBlock.Text + " - Play again?";
                gameEndTextBlock.Visibility = Visibility.Visible;
                highScoreTextBlock.Visibility = Visibility.Visible;
                gameEndTextBlock.Text = "You won the game!";
                highScoreTextBlock.Text = (highScore / 10F).ToString("0.0");
                highScoreTextBlock.Text = "Best time: " + highScoreTextBlock.Text + "s left";
            }

            if (tenthsOfSecondsElapsed <= 0)
            {
                timer.Stop();
                gameEndTextBlock.Visibility = Visibility.Visible;
                gameEndTextBlock.Text = "Uh oh, timer run out!";
                timeTextBlock.Text = timeTextBlock.Text + " - Play again?";
                highScoreTextBlock.Visibility = Visibility.Visible;
                highScoreTextBlock.Text = (highScore / 10F).ToString("0.0");
                highScoreTextBlock.Text = "Best time: " + highScoreTextBlock.Text + "s left";

                foreach (TextBlock textBlock in mainGrid.Children.OfType<TextBlock>())
                {
                    if (textBlock.Name != "timeTextBlock" && textBlock.Name != "gameEndTextBlock" && textBlock.Name != "highScoreTextBlock")
                    {
                        textBlock.Visibility = Visibility.Hidden;

                    }
                }
            }
        }

        private void SetUpGame()
        {
            List<string> animalEmoji = new List<string>()
            {
                "🐙", "🐙",
                "🐡", "🐡",
                "🐘", "🐘",
                "🐳", "🐳",
                "🐪", "🐪",
                "🦕", "🦕",
                "🦘", "🦘",
                "🦔", "🦔",

            };

            Random random = new Random();

            foreach (TextBlock textBlock in mainGrid.Children.OfType<TextBlock>())
            {
                if (textBlock.Name != "timeTextBlock" && textBlock.Name != "gameEndTextBlock" && textBlock.Name != "highScoreTextBlock")
                {
                    textBlock.Visibility = Visibility.Visible;
                    textBlock.Foreground = Brushes.Black;
                    int index = random.Next(animalEmoji.Count);
                    string nextEmoji = animalEmoji[index];
                    textBlock.Text = nextEmoji;
                    animalEmoji.RemoveAt(index);
                }
            }
            gameEndTextBlock.Visibility = Visibility.Hidden;
            highScoreTextBlock.Visibility = Visibility.Hidden;
            timer.Start();
            tenthsOfSecondsElapsed = 300;
            matchesFound = 0;
            highScoreTextBlock.Text = "";
        }

        TextBlock lastTextBlockClicked;
        bool findingMatch = false;

        private async void TextBlock_MouseDown(object sender, MouseButtonEventArgs e)
        {
            TextBlock textBlock = sender as TextBlock;
            if(findingMatch == false)
            {
                textBlock.Foreground = Brushes.Green;
               await HighlightWait();
                textBlock.Visibility = Visibility.Hidden;
                lastTextBlockClicked = textBlock;
                findingMatch = true;
            }
            else if (textBlock.Text == lastTextBlockClicked.Text)
            {
                textBlock.Foreground = Brushes.Green;
                await HighlightWait();
                matchesFound++;
                textBlock.Visibility = Visibility.Hidden;
                findingMatch = false;
            }
            else
            {
                textBlock.Foreground = Brushes.Red;
                await HighlightWait();
                lastTextBlockClicked.Visibility = Visibility.Visible;
                textBlock.Foreground = Brushes.Black;
                lastTextBlockClicked.Foreground = Brushes.Black;
                findingMatch = false;
            }
        }

        private void timeTextBlock_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (matchesFound == 8 || tenthsOfSecondsElapsed <= 0)
            {
                SetUpGame();
            }
        }

        public System.Windows.Media.Brush Foreground { get; set; }

        async Task HighlightWait()
        {
            await Task.Delay(150);
        }

    }
}
