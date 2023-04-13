using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace Time_Tracking
{
    /// Hochschule Hamm-Lippstadt / <summary>
    /// Hochschule Hamm-Lippstadt //
    /// </summary>Furoogh
    /// 3.Semester  12.01.2023
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        // Instanziierung eines Objekts von 2.Fenster 
        public NeuesProjektFenster NeuesProjektFensterObjkt;
        // Deklaration der Variablen für die Timer Funktion
        DispatcherTimer timer = new DispatcherTimer();
        Stopwatch stoppUhr = new Stopwatch();
        public String aktuelleZeit = String.Empty;
        // Erstellen der PropertyChanged Methode für UI Überwachung
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyname)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyname));
            }
        }
        // Deklaration eine Liste von Typ Projekt
        private ObservableCollection<Projekt> Projekte;
        // Deklaration der Eigenschaften von Typ Projekt
        private Projekt auswahlProjekt;
        public Projekt AuswahlProjekt
        {
            get { return auswahlProjekt; }
            set { auswahlProjekt = value; OnPropertyChanged("AuswahlProjekt"); }
        }
        // AlteProjekt ist zum speichern der Daten vom Abgewählten Projekt
        private Projekt alteProjekt;
        public Projekt AlteProjekt
        {
            get { return alteProjekt; }
            set { alteProjekt = value; OnPropertyChanged("AlteProjekt"); }
        }
        public MainWindow()
        {
            InitializeComponent();
            // Initialisierung von Objekt-Liste
            Projekte = new ObservableCollection<Projekt>();

            AuswahlProjekt = new Projekt();
            AlteProjekt = new Projekt();
            // Setzen des DataContext auf dieses Fenster
            this.DataContext = this;
            NeuesProjektFensterObjkt = new NeuesProjektFenster();
            // Initialisieren des Timers & zum aufrufen timer-Tick Event-Handler
            timer.Tick += new EventHandler(timer_Tick);
            timer.Interval = new TimeSpan(0, 0, 0, 0);
            LoadDataFromFile();
            // Füllen der Liste mit Beispiel-Projekte zum Start 
            /*Projekte.Add(new Projekt { ProjektName = "C# Programmieren", ProjektBeschreibung = "Weiterentwicklung von Projekte", AnfangsDatum = new DateTime(2023, 09, 05, 0, 0, 0), EndDatum = DateTime.Today, GebrauchteZeit = TimeSpan.Zero });
            Projekte.Add(new Projekt { ProjektName = "Netzwerke", ProjektBeschreibung = "Zusammenbauen von verschiedenen Vlans", AnfangsDatum = new DateTime(2022, 11, 03, 0, 0, 0), EndDatum = DateTime.Today, GebrauchteZeit = TimeSpan.Zero });
            Projekte.Add(new Projekt { ProjektName = "Meeting", ProjektBeschreibung = "Team-Besprechung mit Kollegen", AnfangsDatum = new DateTime(2022, 11, 05, 0, 0, 0), EndDatum = new DateTime(2022, 11, 05, 0, 0, 0), GebrauchteZeit = TimeSpan.Zero });
            Projekte.Add(new Projekt { ProjektName = "Hausbesuch", ProjektBeschreibung = "Vorbereiten Materialien", AnfangsDatum = new DateTime(2022, 10, 03, 0, 0, 0), EndDatum = new DateTime(2022, 10, 03, 0, 0, 0), GebrauchteZeit = TimeSpan.Zero });
            Projekte.Add(new Projekt { ProjektName = "Projektvorstellung", ProjektBeschreibung = "Vorbereiten des Projektes", AnfangsDatum = new DateTime(2022, 11, 05, 0, 0, 0), EndDatum = new DateTime(2022, 11, 05, 0, 0, 0), GebrauchteZeit = TimeSpan.Zero });
            Projekte.Add(new Projekt { ProjektName = "Website Raparatur", ProjektBeschreibung = "Beheben von Programm-fehler", AnfangsDatum = new DateTime(2020, 06, 01, 0, 0, 0), EndDatum = new DateTime(2020, 08, 04, 0, 0, 0), GebrauchteZeit = TimeSpan.Zero });*/
            // Zuweisung der Projekt-Liste auf Resource-Eigenschaft der DataGrid
            DatenTabelle.ItemsSource = Projekte;
        }
        // Erstellen der timer-Tick Methode als Eventhandler
        private void timer_Tick(object sender, EventArgs e)
        {
            if (stoppUhr.IsRunning)
            {
                TimeSpan zeitSpanne = stoppUhr.Elapsed;
                aktuelleZeit = String.Format("{0:00}:{1:00}:{2:00}", zeitSpanne.Hours, zeitSpanne.Minutes, zeitSpanne.Seconds);
                LblTimer.Content = aktuelleZeit;
            }
        }
        // Timer Start-Button Eventhandler
        private void Btn_Click_StartTimer(object sender, RoutedEventArgs e)
        {
            stoppUhr.Start();
            timer.Start();
        }
        // Timer Stopp-Button Eventhandler
        private void Btn_Click_Stopp(object sender, RoutedEventArgs e)
        {
            if (stoppUhr.IsRunning)
                stoppUhr.Stop();
        }
        // Button zum löschen des ausgewählten Projekt
        private void Btn_Click_Loeschen(object sender, RoutedEventArgs e)
        {
            LoescheItem(DatenTabelle.SelectedItem);
        }
        // Erstellen eines neuen Projekts
        private void Btn_Click_Projekthinzufuegen(object sender, RoutedEventArgs e)
        {
            // DatePicker Datum auf Default setzen
            NeuesProjektFensterObjkt.Anfangs_Datum.SelectedDate = DateTime.Today;
            NeuesProjektFensterObjkt.End_Datum.SelectedDate = DateTime.Today;
            // Alte Eingaben wieder löschen
            NeuesProjektFensterObjkt.Projekt_Name.Text = null;
            NeuesProjektFensterObjkt.Projekt_Beschreibung.Text = null;
            // Das Zweite Fenster anzeigen
            NeuesProjektFensterObjkt.ShowDialog();
        }
        // Methode zum Aufnahme der eingegebenen Daten aus 2.Fenster
        public void Hinzufuegen_Methode()
        {
            // Überprüfen, ob das Eingabe-feld von Projekt-Name nicht leer ist.
            if (NeuesProjektFensterObjkt.Projekt_Name.Text != string.Empty)
            {
                // Aufnahme der Daten aus 2.Fenster.
                Projekte.Add(new Projekt()
                {
                    ProjektName = NeuesProjektFensterObjkt.Projekt_Name.Text
                                                           ,
                    ProjektBeschreibung = NeuesProjektFensterObjkt.Projekt_Beschreibung.Text
                                                           ,
                    AnfangsDatum = Convert.ToDateTime(NeuesProjektFensterObjkt.Anfangs_Datum.Text)
                                                           ,
                    EndDatum = Convert.ToDateTime(NeuesProjektFensterObjkt.End_Datum.Text),
                    GebrauchteZeit = TimeSpan.Zero
                });
                NeuesProjektFensterObjkt.Hide();
                SaveDataToFile();
            }
            // Beim leeren Eingabe.
            else
            {
                MessageBox.Show("bitte Name eingeben", "Ok", MessageBoxButton.OK);
            }
        }
        // Eventhandler beim ändern des Auswahl-Projekts im ComboBox.
        private void Auswahl_Geaendert(object sender, SelectionChangedEventArgs e)
        {
            // Füllen von Labeln mit neuen Daten.
            Projekt_Beschreibung.Content = AuswahlProjekt.ProjektBeschreibung;
            AnfangsDatumLbl.Content = AuswahlProjekt.AnfangsDatum;
            EndDatumLbl.Content = AuswahlProjekt.EndDatum;
            // Stoppen des Timers & addieren von gebrauchten Zeit.
            if (stoppUhr.IsRunning)
            { stoppUhr.Stop(); }
            TimeSpan tt = stoppUhr.Elapsed;
            AlteProjekt.GebrauchteZeit = AlteProjekt.GebrauchteZeit + tt;
            // Zurücksetzen des Timers.
            stoppUhr.Reset();
            // Initialisierung des Alten-Projekts mit neuen Daten aus neuen Projekts.
            AlteProjekt = AuswahlProjekt;

            aktuelleZeit = null;
            aktuelleZeit = String.Format("00:00:00");
            LblTimer.Content = aktuelleZeit;

            SaveDataToFile();
        }
        // Laden von Daten im ComboBox.
        private void Combo_Geladen(object sender, RoutedEventArgs e)
        {
            var Combo = sender as ComboBox;
            Combo.ItemsSource = Projekte;
            Combo.SelectedIndex = 0;
        }
        //Methode zum löschen der Projekte aus DatenTabelle
        public void LoescheItem(object projektOBJ)
        {
            int index = Projekte.IndexOf(projektOBJ as Projekt);
            int auswahlProjektIndex = Projekte.IndexOf(AuswahlProjekt);

            if (index > -1 && index < Projekte.Count)
            {
                if (index == auswahlProjektIndex)
                {
                    MessageBox.Show("Das ausgewählte Projekt kann nicht gelöscht werden");
                }
                else
                {
                    Projekte.RemoveAt(index);
                    SaveDataToFile();
                }
            }
            else
            {
                MessageBox.Show("Nicht gelöscht !");
            }
        }
        // Methode zum speichern der Daten in Text-Datei
        private void SaveDataToFile()
        {
            // Create a new instance of StreamWriter to write to the file
            using (StreamWriter writer = new StreamWriter("data.txt"))
            {
                // Loop through the projects and write each one to the file
                foreach (Projekt project in Projekte)
                {
                    writer.WriteLine($"{project.ProjektName},{project.ProjektBeschreibung},{project.AnfangsDatum},{project.EndDatum},{project.GebrauchteZeit}");
                }
            }
        }
        // Methode zum laden der Projekte aus Text-Datei (data.txt).
        private void LoadDataFromFile()
        {
            // Überprüfen, ob die Datei vorhanden ist, andernfalls erstellen.
            if (!File.Exists("data.txt"))
            {
                // Create a new file with default data
                using (StreamWriter writer = new StreamWriter("data.txt"))
                {
                    writer.WriteLine("BeispielProjekt-Name1,BeispielProjekt-Beschreibung1,2023-04-13,2023-05-13,00:00:00");
                    writer.WriteLine("BeispielProjekt-Name2,BeispielProjekt-Beschreibung2,2023-04-13,2023-05-13,00:00:00");
                }
            }
            // Eine neue Instanz von StreamReader zum Lesen aus der Datei erstellen.
            using (StreamReader reader = new StreamReader("data.txt"))
            {

                // Die aktuelle Liste der Projekte löschen.
                Projekte.Clear();

                // Jede Zeile der Datei durchlaufen.
                while (!reader.EndOfStream)
                {
                    // Die Zeile lesen und in ein Array von Werten aufteilen.
                    string[] values = reader.ReadLine().Split(',');

                    // Ein neues Projekt erstellen und dessen Eigenschaften aus den Werten in der Datei setzen.
                    Projekt project = new Projekt();
                    project.ProjektName = values[0];
                    project.ProjektBeschreibung = values[1];
                    project.AnfangsDatum = DateTime.Parse(values[2]);
                    project.EndDatum = DateTime.Parse(values[3]);
                    project.GebrauchteZeit = TimeSpan.Parse(values[4]);

                    // Das Projekt zur Liste hinzufügen.
                    Projekte.Add(project);
                }
            }
        }


    }
}
