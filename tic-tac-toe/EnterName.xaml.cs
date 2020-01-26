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
using System.Windows.Shapes;

namespace tic_tac_toe
{
    /// <summary>
    /// Логика взаимодействия для EnterName.xaml
    /// </summary>
    public partial class EnterName : Window
    {
        public string Name;
        public EnterName()
        {
            InitializeComponent();
        }

        public string ShowDialog(bool isFirst)
        {
            if (isFirst)
                tbMessage.Text = "Введите имя первого игрока";
            else
                tbMessage.Text = "Введите имя второго игрока";
            ShowDialog();
            return Name;
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            Name = tbName.Text;
        }

        private void BtOK_Click(object sender, RoutedEventArgs e)
        {
            Name = tbName.Text;
            this.Close();
        }
    }
}
