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
using Time_Tracking;

namespace Time_Tracking
{
    
    public partial class NeuesProjektFenster : Window
    {
        public NeuesProjektFenster()
        {
            InitializeComponent();
        }
        // Beim klicken auf Hinzufügen wird die Methode aus MainWindow aufgerufen.
        private void Btn_Click_Hinzufuegen(object sender, RoutedEventArgs e)
        {
            ((MainWindow)System.Windows.Application.Current.MainWindow).Hinzufuegen_Methode();
        }

    }
}
