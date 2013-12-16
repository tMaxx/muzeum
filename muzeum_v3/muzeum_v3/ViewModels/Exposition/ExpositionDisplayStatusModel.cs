using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Windows.Media;
//Klasa odpowiedzialna za sprawdzanie poprawności pól przy dodawaniu eksponatu.
//Dodatkowo koloruje odpowiednio pola na czerwono gdy dane są niepoprawne przy dodawaniu/zmianie
namespace muzeum_v3.ViewModels.Exposition
{
    class ExpositionDisplayStatusModel : INotifyPropertyChanged
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
        private SolidColorBrush expositionName = ok;
        private SolidColorBrush organizerName = ok;
        private SolidColorBrush locationName = ok;
        private SolidColorBrush description = ok;
        //odpowiednie properites
        public string Status
        {
            get { return status; }
            set { status = value; OnPropertyChanged(new PropertyChangedEventArgs("Status")); }
        }

        public SolidColorBrush ExpositionName
        {
            get { return expositionName; }
            set { expositionName = value; OnPropertyChanged(new PropertyChangedEventArgs("ExpositionName")); }
        }

        public SolidColorBrush Description
        {
            get { return description; }
            set { description = value; OnPropertyChanged(new PropertyChangedEventArgs("Description")); }
        }

        public SolidColorBrush OrganizerName
        {
            get { return organizerName; }
            set { organizerName = value; OnPropertyChanged(new PropertyChangedEventArgs("OrganizerName")); }
        }
        public SolidColorBrush LocationName
        {
            get { return locationName; }
            set { locationName = value; OnPropertyChanged(new PropertyChangedEventArgs("LocationName")); }
        }
        public void clearStatus()
        {
            ExpositionName = description = locationName = organizerName = ok;
            Status = "OK";
        }

        //konstruktor
        public ExpositionDisplayStatusModel()
        {
            clearStatus();
        }
        //Można dodać dowolne funkcje sprawdzające poprawność danych
        //Ta tutaj po prostu sprawdza, czy pola są puste, czy nie.

        public bool CheckExpositionForAdd(Exposition p)
        {
            int errorCount = 0;
            if (String.IsNullOrEmpty(p.ExpositionName))
            { errorCount++; ExpositionName = error; }
            else ExpositionName = ok;
            if (String.IsNullOrEmpty(p.Description))
            { errorCount++; Description = error; }
            else Description = ok;
            if (String.IsNullOrEmpty(p.LocationName))
            { errorCount++; LocationName = error; }
            else LocationName = ok;
            if (String.IsNullOrEmpty(p.OrganizerName))
            { errorCount++; OrganizerName = error; }
            else OrganizerName = ok;
            if (errorCount == 0) { Status = "OK"; return true; }
            else { Status = "Niestety nie wypełniłeś wszystkich pól: "; return false; }
        }
    }
}
