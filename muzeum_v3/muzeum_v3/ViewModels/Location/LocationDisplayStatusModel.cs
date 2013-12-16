using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Windows.Media;

namespace muzeum_v3.ViewModels.Location
{
    class LocationDisplayStatusModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, e);
        }
        //ustawia kolorowanie pól
        private static SolidColorBrush error = new SolidColorBrush(Colors.Red);
        private static SolidColorBrush ok = new SolidColorBrush(Colors.Black);
        //Status błędu
        private string status;
        //Kolor danrgo pola odpowiednio ustawiony na starcie:
        private SolidColorBrush locationName = ok;
        private SolidColorBrush description = ok;

        public string Status
        {
            get { return status; }
            set { status = value; OnPropertyChanged(new PropertyChangedEventArgs("Status")); }
        }

        public SolidColorBrush LocationName
        {
            get { return locationName; }
            set { locationName = value; OnPropertyChanged(new PropertyChangedEventArgs("LocationName")); }
        }

        public SolidColorBrush Description
        {
            get { return description; }
            set { description = value; OnPropertyChanged(new PropertyChangedEventArgs("Description")); }
        }
        public void clearStatus()
        {
            locationName = description = ok;
            Status = "OK";
        }

        //konstruktor
        public LocationDisplayStatusModel()
        {
            clearStatus();
        }
        //Można dodać dowolne funkcje sprawdzające poprawność danych
        //Ta tutaj po prostu sprawdza, czy pola są puste, czy nie.

        public bool CheckLocationForAdd(Location p)
        {
            int errorCount = 0;
            if (String.IsNullOrEmpty(p.LocationName))
            { errorCount++; LocationName = error; }
            else LocationName = ok;
            if (String.IsNullOrEmpty(p.Description))
            { errorCount++; Description = error; }
            else Description = ok;
            if (errorCount == 0) { Status = "OK"; return true; }
            else { Status = "Niestety nie wypełniłeś wszystkich pól: "; return false; }
        }
    }
}
