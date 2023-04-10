using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Time_Tracking;

namespace Time_Tracking
{
    // Implementierung des Interface INotifyPropertyChanged zum updaten von UI beim ändern der Daten.
    public class Projekt : INotifyPropertyChanged
    {

        // Erstellung der Projekt-Eigenschaften.
        private String projektName;

        public String ProjektName
        {
            get { return projektName; }
            set { projektName = value; OnPropertyChanged(ProjektName); }
        }

        private String projektBeschreibung;

        public String ProjektBeschreibung
        {
            get { return projektBeschreibung; }
            set { projektBeschreibung = value; OnPropertyChanged(ProjektBeschreibung); }
        }

        private DateTime? anfangsDatum;

        public DateTime? AnfangsDatum
        {
            get { return anfangsDatum; }
            set { anfangsDatum = value; OnPropertyChanged("AnfangsDatum"); }
        }

        private DateTime? endDatum;

        public DateTime? EndDatum
        {
            get { return endDatum; }
            set { endDatum = value; OnPropertyChanged("EndDatum"); }
        }

        private TimeSpan? gebrauchteZeit;

        public TimeSpan? GebrauchteZeit
        {
            get { return gebrauchteZeit; }
            set { gebrauchteZeit = value; OnPropertyChanged("GebrauchteZeit"); }
        }
        // Eventhandler zum UI Überwachung.
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyname)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyname));
            }
        }

    }
}
