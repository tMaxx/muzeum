using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Windows.Media;

namespace muzeum_v3.ViewModels.Hall
{
    class HallDisplayStatusModel : INotifyPropertyChanged
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
        private SolidColorBrush hallName = ok;
        private SolidColorBrush description = ok;
        private SolidColorBrush locationName = ok;
     
        //odpowiednie properites
        public string Status
        {
            get { return status; }
            set { status = value; OnPropertyChanged(new PropertyChangedEventArgs("Status")); }
        }

        public SolidColorBrush HallName
        {
            get { return hallName; }
            set { hallName = value; OnPropertyChanged(new PropertyChangedEventArgs("HallName")); }
        }

        public SolidColorBrush Description
        {
            get { return description; }
            set { description = value; OnPropertyChanged(new PropertyChangedEventArgs("Description")); }
        }

        public SolidColorBrush LocationName
        {
            get { return locationName; }
            set { locationName = value; OnPropertyChanged(new PropertyChangedEventArgs("LocationName")); }
        }
    
        public void clearStatus()
        {
           locationName = description = hallName = ok;
           Status = "OK";
        } 

//konstruktor
        public HallDisplayStatusModel()
        {
            clearStatus();
        } 
//Można dodać dowolne funkcje sprawdzające poprawność danych
//Ta tutaj po prostu sprawdza, czy pola są puste, czy nie.
       
        public bool CheckHallForAdd(Hall p)
        {
            int errorCount = 0;
            if (String.IsNullOrEmpty(p.HallName))
            { errorCount++; HallName = error; }
            else HallName = ok;
            if (String.IsNullOrEmpty(p.Description))
            { errorCount++; Description = error; }
            else Description = ok;
            if (String.IsNullOrEmpty(p.LocationName))
            { errorCount++; LocationName = error; }
            else LocationName = ok;
           
            if (errorCount == 0) { Status = "OK"; return true; }
            else { Status = "Niestety nie wypełniłeś wszystkich pól: "; return false; }
        } 
    }
}
