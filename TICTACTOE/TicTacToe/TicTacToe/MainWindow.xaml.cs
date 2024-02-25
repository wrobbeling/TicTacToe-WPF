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


namespace TicTacToe_WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            InitializeButtonsArray();
        }



        private Button[,] btn = new Button[3, 3]; // 2D-Array für die Buttons im Spielfeld
        public bool isPlayer1Turn { get; set; } = true;   // Variable, um den Zug des aktuellen Spielers zu verfolgen
        public int player1Score { get; set; }
        public int player2Score { get; set; }

        // Deklaration einer Variablen, um den aktuellen Spielmodus zu verfolgen
        public bool isPlayerVsComputerMode { get; set; } = false;

        private Random random = new Random(); // für die simulierte KI


        // Methode zum Initialisieren des Buttons-Arrays
        private void InitializeButtonsArray()
        {
            btn[0, 0] = btn00;
            btn[0, 1] = btn01;
            btn[0, 2] = btn02;
            btn[1, 0] = btn03;
            btn[1, 1] = btn04;
            btn[1, 2] = btn05;
            btn[2, 0] = btn06;
            btn[2, 1] = btn07;
            btn[2, 2] = btn08;
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            // Logik für das Beenden der Anwendung
            Application.Current.Shutdown();
        }

        // Ereignishandler für das Klicken auf eine Schaltfläche im Spielfeld
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;

            // Überprüfen, ob die Schaltfläche bereits belegt ist
            if (button.Content == null)
            {
                // Setzen des Symbols basierend auf dem aktuellen Spieler
                button.Content = isPlayer1Turn ? "X" : "O";

                // Überprüfen auf einen Gewinner
                if (CheckForWinner())
                {
                    // Spiel ist vorbei, zurücksetzen oder neu starten
                    ResetGame();
                }
                else
                {
                    // Wechsel zum nächsten Spieler (Computer)
                    if (isPlayerVsComputerMode)
                    {
                        isPlayer1Turn = !isPlayer1Turn;
                        if (!isPlayer1Turn)
                        {
                            ComputerMove(); // Computer macht seinen Zug
                        }
                    }

                    else if (!isPlayerVsComputerMode) // Wechsel zum nächsten Spieler
                    {
                        isPlayer1Turn = !isPlayer1Turn;
                    }
                }
            }
        }

        private void ResetGame()
        {
            // Alle Buttons leeren
            btn00.Content = null;
            btn01.Content = null;
            btn02.Content = null;
            btn03.Content = null;
            btn04.Content = null;
            btn05.Content = null;
            btn06.Content = null;
            btn07.Content = null;
            btn08.Content = null;

            // Spieler X beginnt immer ein neues Spiel
            isPlayer1Turn = true;

        }

        // Methode zur Aktualisierung des Spielstands
        private void UpdateScore()
        {
            // Spieler 1: X
            textBlockPlayer1.Text = $"Spieler X: {player1Score}";

            // Spieler 2: O
            textBlockPlayer2.Text = $"Spieler O: {player2Score}";
        }



        private void Refresh_Click(object sender, RoutedEventArgs e)
        {
            // Logik für das Beenden der Anwendung
            ResetGame();


        }

        private bool CheckForWinner()
        {
            // Überprüfen auf horizontale Reihen
            for (int row = 0; row < 3; row++)
            {
                if (btn[row, 0].Content == btn[row, 1].Content &&
                    btn[row, 1].Content == btn[row, 2].Content &&
                    btn[row, 0].Content != null)
                {
                    // Gewinner gefunden, anzeigen
                    MessageBox.Show($"Spieler {btn[row, 0].Content} hat gewonnen!");
                    if (btn[row, 0].Content.ToString() == "X")
                        player1Score++;
                    else
                        player2Score++;
                    UpdateScore(); // Aktualisieren Sie den Spielstand
                    return true;
                }
            }

            // Überprüfen auf vertikale Reihen
            for (int col = 0; col < 3; col++)
            {
                if (btn[0, col].Content == btn[1, col].Content &&
                    btn[1, col].Content == btn[2, col].Content &&
                    btn[0, col].Content != null)
                {
                    // Gewinner gefunden, anzeigen
                    MessageBox.Show($"Spieler {btn[0, col].Content} hat gewonnen!");
                    if (btn[0, col].Content.ToString() == "X")
                        player1Score++;
                    else
                        player2Score++;
                    UpdateScore(); // Aktualisieren Sie den Spielstand
                    return true;
                }
            }

            // Überprüfen auf diagonale Reihen
            if (btn[0, 0].Content == btn[1, 1].Content &&
                btn[1, 1].Content == btn[2, 2].Content &&
                btn[0, 0].Content != null)
            {
                // Gewinner gefunden, anzeigen
                MessageBox.Show($"Spieler {btn[0, 0].Content} hat gewonnen!");
                if (btn[0, 0].Content.ToString() == "X")
                    player1Score++;
                else
                    player2Score++;
                UpdateScore(); // Aktualisieren Sie den Spielstand
                return true;
            }

            if (btn[0, 2].Content == btn[1, 1].Content &&
                btn[1, 1].Content == btn[2, 0].Content &&
                btn[0, 2].Content != null)
            {
                // Gewinner gefunden, anzeigen
                MessageBox.Show($"Spieler {btn[0, 2].Content} hat gewonnen!");
                if (btn[0, 2].Content.ToString() == "X")
                    player1Score++;
                else
                    player2Score++;
                UpdateScore(); // Aktualisieren Sie den Spielstand
                return true;
            }

            // Überprüfen auf Unentschieden
            bool isDraw = true;
            for (int row = 0; row < 3; row++)
            {
                for (int col = 0; col < 3; col++)
                {
                    if (btn[row, col].Content == null)
                    {
                        isDraw = false;
                        break;
                    }
                }
            }

            if (isDraw)
            {
                // Unentschieden, anzeigen
                MessageBox.Show("Unentschieden!");
                return true;
            }

            return false;
        }

        //KI
        private void ComputerMove()
        {
            // Suchen nach einem leeren Feld für den nächsten Zug des Computers
            List<Button> emptyButtons = new List<Button>();
            foreach (Button button in btn)
            {
                if (button.Content == null)
                {
                    emptyButtons.Add(button);
                }
            }

            // Wenn es leere Felder gibt, wählt die KI zufällig ein Feld aus und setzt ihr Symbol
            if (emptyButtons.Count > 0)
            {
                int index = random.Next(emptyButtons.Count);
                Button computerButton = emptyButtons[index];
                computerButton.Content = isPlayer1Turn ? "X" : "O";

                // Überprüfen auf Gewinner nach dem Zug des Computers
                if (CheckForWinner())
                {
                    // Spiel ist vorbei, zurücksetzen oder neu starten
                    ResetGame();
                }
                else
                {
                    // Wechsel zum nächsten Spieler (Spieler 1)
                    isPlayer1Turn = !isPlayer1Turn;
                }
            }
        }

        private void Info_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Simon Wroblewski\nFachinformatiker Anwendungsentwicklung\nEmail: fachinformatiker@kabelmail.de\n\n 2024", "Technischer Support:", MessageBoxButton.OK);
        }

        private void Howto_Click(object sender, RoutedEventArgs e)
        {

            MessageBox.Show("Hier ist eine grundlegende Spielanleitung:\n\n" +
                    "1. Das Spielfeld besteht aus einem 3x3-Raster.\n" +
                    "2. Ein Spieler beginnt das Spiel, indem er ein X oder O in ein leeres Feld setzt.\n" +
                    "3. Die Spieler wechseln sich ab, bis das Spielfeld voll ist oder einer der Spieler eine Reihe von drei gleichen Symbolen erreicht.\n" +
                    "4. Ein Spieler gewinnt, wenn er zuerst eine horizontale, vertikale oder diagonale Reihe von drei gleichen Symbolen bildet.\n" +
                    "5. Wenn das Spielfeld voll ist und keiner der Spieler eine Reihe von drei Symbolen hat, endet das Spiel unentschieden.\n" +
                    "6. Die Spieler können versuchen, die Spielzüge ihres Gegners zu blockieren und gleichzeitig eigene Gewinnchancen zu erhöhen.\n" +
                    "7. Über Radiobuttons erfolgt die Auswahl des Gegners, entweder der Computer(KI) oder ein Mensch.", "Über TicTacToe", MessageBoxButton.OK);
        }


        //Spieler gegen Spieler

        private void RadioButton_PlayerVsPlayer_Checked(object sender, RoutedEventArgs e)
        {
            // Setzen des Spielmodus auf Spieler vs. Spieler
            isPlayerVsComputerMode = false;
        }


        //Spieler gegen Computer (KI)

        private void RadioButton_PlayerVsComputer_Checked(object sender, RoutedEventArgs e)
        {
            // Setzen des Spielmodus auf Spieler vs. Computer
            isPlayerVsComputerMode = true;

            // Hier können Sie spezifische Logik für den Spielmodus "Spieler vs. Computer" implementieren..
        }
    }





}
