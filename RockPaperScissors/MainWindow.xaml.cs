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
using System.IO;

namespace RockPaperScissors
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //Set Variables
        public int playerValue = 0;
        public int botValue = 0;
        public int playerScore = 0;
        public int previousHighScore;
        public Random rnd = new Random();
        public string username;
        public string previousUsername;
        public string highscoreFilePath = "Highscore.txt";
        public bool ifSave = false;
        public List<Player> players = new List<Player>();

        public MainWindow()
        {
            InitializeComponent();
            BootUpOptions();
        }
        //Player Choices
        private void btnRock_Click(object sender, RoutedEventArgs e)
        {
            playerValue = 1;
            imgPlayer.Source = new BitmapImage(new Uri("/images/Rock.png", UriKind.Relative));
            Game();
        }
        private void btnScissors_Click(object sender, RoutedEventArgs e)
        {
            playerValue = 2;
            imgPlayer.Source = new BitmapImage(new Uri("/images/Scissors.png", UriKind.Relative));
            Game();
        }

        private void btnPaper_Click(object sender, RoutedEventArgs e)
        {
            playerValue = 3;
            imgPlayer.Source = new BitmapImage(new Uri("/images/Paper.png", UriKind.Relative));
            Game();
        }
        //Bot Chooses
        public void BotChoice()
        {
            botValue = rnd.Next(1, 4);
            switch (botValue)
            {
                case 1:
                    imgBot.Source = new BitmapImage(new Uri("/images/Rock.png", UriKind.Relative));
                    break;
                case 2:
                    imgBot.Source = new BitmapImage(new Uri("/images/Scissors.png", UriKind.Relative));
                    break;
                case 3:
                    imgBot.Source = new BitmapImage(new Uri("/images/Paper.png", UriKind.Relative));
                    break;
                default:
                    break;
            }
        }
        //Waits because user needs to be able to see bot choice
        //Runs Score()
        async void Wait()
        {
            await Task.Delay(2000);
            Score();
        }
        //Calculates who wins
        //If bot wins save playerscore then reset player score
        //Run HighScore()
        //Reset Images to thinking
        public void Score()
        {
            //Tie
            if (playerValue == botValue)
            {
                
            }
            //Player Wins
            else if ((playerValue - botValue == -1 ) || (playerValue - botValue == 2))
            {
                playerScore++;
                lblScore.Content = playerScore;
            }
            //Bot Wins
            else if ((playerValue - botValue == 1) || (playerValue - botValue == -2))
            {
                IfHighscoreBeatSave();
                playerScore = 0;
                lblScore.Content = playerScore;
            }
            HighScore();
            imgPlayer.Source = new BitmapImage(new Uri("/images/Thinking.png", UriKind.Relative));
            imgBot.Source = new BitmapImage(new Uri("/images/Thinking.png", UriKind.Relative));
        }
        //Save Previous highscore
        //Run BotChoice Method
        //Run Wait() which runs Score() After Waiting
        public void Game()
        {
            TempSavePreviousHighscore();
            BotChoice();
            Wait();
        }
        //If highscore is beat then change lblHighscore to playerscore and  lblName to "You"
        public void HighScore()
        {
            if (playerScore > Convert.ToInt32(lblHighscore.Content))
            {
                lblHighscore.Content = playerScore;
                lblName.Content = "Dig";
            }
        }
        //Asks for a username and checks if the name is empty then reset to previous highscore
        //Checks if playerScore is larger than previousHighScore 
        //Else just runs PlayerList()
        public void Username()
        {
            WriteName secondWindow = new WriteName(this);
            secondWindow.ShowDialog();
            if (username == "")
            {
                lblName.Content = previousUsername;
                lblHighscore.Content = previousHighScore;
            }
            else if (playerScore > previousHighScore)
            {
                lblName.Content = username;
                PlayerList();
            }
            else
            {
                PlayerList();
            }
            
        }
        //Runs BootUpOptions 
        public void BootUpOptions()
        {
            Load();
            SetSaveOption();
        }
        //Asks if you want to load highscore and then depending on result loads highscores from a file
        public void Load()
        {
            if (MessageBox.Show("Vil du indlæse programmets highscore fra en fil?", "Indlæs", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                using (StreamReader sr = new StreamReader(highscoreFilePath))
                {
                    /*
                    string line = "";
                    if (sr.EndOfStream != true)
                    {
                        line = sr.ReadLine().Trim();
                    }
                    string[] info = line.Split(":");
                    if (info.Length > 1)
                    {
                        lblHighscore.Content = info[0];
                        lblName.Content = info[1];

                    }*/
                    
                    string line2 = "";
                    while (sr.EndOfStream != true)
                    {
                        line2 = sr.ReadLine().Trim();
                        string[] info2 = line2.Split(":");
                        if (info2.Length > 1)
                        {
                            Player newPlayer = new Player(info2[1], int.Parse(info2[0]));
                            players.Add(newPlayer);
                            lblHighscore.Content = players[0].Score;
                            lblName.Content = players[0].Username;
                        }
                    }
                }
            }
        }
        //Sets if you want to save or not
        public void SetSaveOption()
        {
            if (MessageBox.Show("Vil du gemme programmets highscore i en fil hvis den bliver slået?" + Environment.NewLine + "Bemærk: Dette vil overskrive filens highscore", "Gem", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                ifSave = true;
            }
        }
        //Saves all players in players list to file
        public void Save()
        {
            using (StreamWriter sw = new StreamWriter($"{highscoreFilePath}"))
            {
                foreach (Player player in players)
                {
                    string seperatedHighscore = $"{player.Score}:{player.Username}";
                    sw.WriteLine(seperatedHighscore);
                }
            }
        }
        //Saves previous highscore when you start a game
        public void TempSavePreviousHighscore()
        {
            if (playerScore == 0)
            {
                previousHighScore = Convert.ToInt32(lblHighscore.Content);
                previousUsername = (string)lblName.Content;
            }
        }
        // Checks if there is less than 10 players in playerslist and if there is more
        // checks if the current score is less than the lowest score in playerslist
        public void IfHighscoreBeatSave()
        {
            if ((players.Count < 10) || (players[9].Score < playerScore))
            {
                Username();
                if (ifSave == true)
                {
                    Save();
                }
            }
        }
        //Creates a player based on username and playerScore
        //Adds player to playerslist
        //Sorts player list by highest score
        //Removes the last player if there is more than 10 players
        public void PlayerList()
        {
            Player newPlayer = new Player(username, playerScore);
            players.Add(newPlayer);
            players.Sort((x, y) => y.Score.CompareTo(x.Score));
            if (players.Count > 10)
            {
                players.RemoveAt(10);
            }
        }
        
    }
}
