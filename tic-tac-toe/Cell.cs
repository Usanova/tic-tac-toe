using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace tic_tac_toe
{
    class Cell
    {
        int i;
        int j;
        public Label l;
        public delegate void MakeMove(int i, int j);
        public event MakeMove makeMove;
        public Cell(int i, int j)
        {
            this.i = i;
            this.j = j;
            l = new Label() { Content = $"" };
            l.MouseDown += ClickOnCell;
            Grid.SetRow(l, i);
            Grid.SetColumn(l, j);
        }
        void ClickOnCell(object sender, EventArgs e)
        {
            makeMove(i, j);
        }
    }
}
