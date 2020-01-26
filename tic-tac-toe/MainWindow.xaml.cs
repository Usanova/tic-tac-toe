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
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Forms;

namespace tic_tac_toe
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        const int CountCells = 15;
        string NameFirstPlayer = "Шуня";
        string NameSecondPlayer = "Пашуня";

        enum cell
        {
            cross, zero, nothing
        }
        enum player
        {
            firstPlayer, secondPlayer
        }
        enum TypeWin
        {
            row, column, diagonalLeft, diagonalRight
        }
        cell[][] PlayingField = new cell[CountCells][];
        Border[][] BorderField = new Border[CountCells][];
        static Timer TimerForEnd = new Timer();
        player WhoseMove;
        bool EndOfGame;
        int CountBusyCells;


        public MainWindow()
        {
            InitializeComponent();
            TimerForEnd.Tick += new EventHandler(Update);
            TimerForEnd.Interval = 1000;
            for(int i = 0; i < CountCells; i++)
            {
                PlayingField[i] = new cell[CountCells];
                BorderField[i] = new Border[CountCells];
            }
            EnterName enterName = new EnterName();
            NameSecondPlayer = enterName.ShowDialog(true);
            enterName = new EnterName();
            NameFirstPlayer = enterName.ShowDialog(false);
            Start();
        }



        void Start()
        {
            TimerForEnd.Stop();
            grPlayingField.Children.Clear();
            for (int i = 0; i < CountCells; i++)
            {
                for (int j = 0; j < CountCells; j++)
                {
                    Cell NewCell = new Cell(i, j);
                    NewCell.makeMove += ClickOnCell;
                    grPlayingField.Children.Add(NewCell.l);
                    PlayingField[i][j] = cell.nothing;
                    BorderField[i][j] = null;
                }
            }
            Swap(ref NameFirstPlayer, ref NameSecondPlayer);
            WhoseMove = player.firstPlayer;
            EndOfGame = false;
            CountBusyCells = 0;
            lbWhoseMove.Content = $"Сейчас ходит {NameFirstPlayer}";
        }

        void Swap(ref string a, ref string b)
        {
            string temp = a;
            a = b;
            b = temp;
        }
        private void Update(object sender, EventArgs e)
        {
            Start();
        }
        void ClickOnCell(int i, int j)
        {
            if (!EndOfGame)
            {
                if (WhoseMove == player.firstPlayer)
                {
                    PlayingField[i][j] = cell.cross;
                    BorderField[i][j] = move(i, j, cell.cross);
                    ChekResult(i, j, cell.cross);
                    WhoseMove = player.secondPlayer;
                    lbWhoseMove.Content = $"Сейчас ходит {NameSecondPlayer}";
                }
                else
                {
                    PlayingField[i][j] = cell.zero;
                    BorderField[i][j] = move(i, j, cell.zero);
                    ChekResult(i, j, cell.zero);
                    WhoseMove = player.firstPlayer;
                    lbWhoseMove.Content = $"Сейчас ходит {NameFirstPlayer}";
                }
                CountBusyCells++;
                if (CountBusyCells == CountCells*CountCells)
                    TimerForEnd.Start();
            }
        }
        void ChekResult(int i, int j, cell value)
        {
            int StartIndex = 0;
            if (CheckWinRow(i, j, ref StartIndex, value))
                Win(TypeWin.row, i, StartIndex);
            if (CheckWinColumn(i, j, ref StartIndex, value))
                Win(TypeWin.column, j, StartIndex);
            if (CheckWinDiagonalL(i, j, ref StartIndex, value))
                Win(TypeWin.diagonalLeft, i, StartIndex, j + (StartIndex - i));
            if (CheckWinDiagonalR(i, j, ref StartIndex, value))
                Win(TypeWin.diagonalRight, i, StartIndex, j - (StartIndex - i));

        }
        bool CheckWinRow(int i, int j, ref int StartIndex, cell value)
        {
            for(int start = j - 4; start <= j; start++)
            {
                int CountValue = 0;
                for(int point = start; start >= 0 && point < start + 5; point++)
                {
                    if (point < CountCells && PlayingField[i][point] == value)
                        CountValue++;  
                }
                if(CountValue == 5)
                {
                    StartIndex = start;
                    return true;
                }
            }
            return false;
        }
        bool CheckWinColumn(int i, int j, ref int StartIndex, cell value)
        {
            for (int start = i - 4; start <= i; start++)
            {
                int CountValue = 0;
                for (int point = start; start >= 0 && point < start + 5; point++)
                {
                    if (point < CountCells && PlayingField[point][j] == value)
                        CountValue++;
                }
                if (CountValue == 5)
                {
                    StartIndex = start;
                    return true;
                }
            }
            return false;
        }
        bool CheckWinDiagonalL(int i, int j, ref int StartIndex, cell value)
        {
            int dif = j - i;
            for (int start = i - 4; start <= i; start++)
            {
                int CountValue = 0;
                for (int point = start; start >= 0 && start + dif >=0 && point < start + 5; point++)
                {
                    if (point < CountCells && point + dif < CountCells && PlayingField[point][point + dif] == value)
                        CountValue++;
                }
                if (CountValue == 5)
                {
                    StartIndex = start;
                    return true;
                }
            }
            return false;
        }
        bool CheckWinDiagonalR(int i, int j, ref int StartIndex, cell value)
        {
            for (int start1 = i - 4, start2 = j + 4; start1 <= i; start1++, start2--)
            {
                int CountValue = 0;
                for (int point1 = start1, point2 = start2;
                    start1 >= 0 && start2 < CountCells && point1 < start1 + 5; point1++, point2--)
                {
                    if (point1 < CountCells && point2 >= 0 && PlayingField[point1][point2] == value)
                        CountValue++;
                }
                if (CountValue == 5)
                {
                    StartIndex = start1;
                    return true;
                }
            }
            return false;
        }
        Border move(int i, int j, cell value)
        {
            var handel = Properties.Resources.zero.GetHbitmap(); ;
            if (value == cell.cross)
                handel = Properties.Resources.cross.GetHbitmap();
            Image img = new Image()
            {
                Source = Imaging.CreateBitmapSourceFromHBitmap(handel, IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions()),
                Margin = new Thickness(5)
            };
            Border b = new Border() { Child = img };
            Grid.SetRow(b, i);
            Grid.SetColumn(b, j);
            grPlayingField.Children.Add(b);
            return b;
        }
        void Win(TypeWin typeWin, int index, int StartIndex, int StartIndex1 = 0)
        {
            switch (typeWin)
            {
                case TypeWin.row:
                    for (int i = StartIndex; i < StartIndex + 5; i++)
                        BorderField[index][i].Background = new SolidColorBrush(Colors.Red);
                    break;
                case TypeWin.column:
                    for (int i = StartIndex; i < StartIndex + 5; i++)
                        BorderField[i][index].Background = new SolidColorBrush(Colors.Red);
                    break;
                case TypeWin.diagonalLeft:
                    for (int i = StartIndex, j = StartIndex1; i < StartIndex + 5 && j < StartIndex1 + 5; i++, j++)
                        BorderField[i][j].Background = new SolidColorBrush(Colors.Red);
                    break;
                case TypeWin.diagonalRight:
                    for (int i = StartIndex, j = StartIndex1; i < StartIndex + 5 &&  j > StartIndex1 - 5; i++, j--)
                        BorderField[i][j].Background = new SolidColorBrush(Colors.Red);
                    break;
            }
            EndOfGame = true;
            TimerForEnd.Start();
        }
    }
}
