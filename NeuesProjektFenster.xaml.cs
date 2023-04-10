using System.Windows;

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
