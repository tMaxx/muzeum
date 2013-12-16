using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Windows.Media;

namespace muzeum_v3.ViewModels.Presentation
{
    class PresentationDisplayStatusModel : INotifyPropertyChanged
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

        private SolidColorBrush dateOfBegin;
        private SolidColorBrush dateOfEnd;
        private SolidColorBrush presentedExhibit;
        private SolidColorBrush exposition;
        private SolidColorBrush location;
        private SolidColorBrush hall;
        //odpowiednie properites
        public string Status
        {
            get { return status; }
            set { status = value; OnPropertyChanged(new PropertyChangedEventArgs("Status")); }
        }

        public SolidColorBrush PresentedExhibit
        {
            get { return presentedExhibit; }
            set { presentedExhibit = value; OnPropertyChanged(new PropertyChangedEventArgs("presentedExhibit")); }
        }

        public SolidColorBrush Exposition
        {
            get { return exposition; }
            set { exposition = value; OnPropertyChanged(new PropertyChangedEventArgs("Exposition")); }
        }

        public SolidColorBrush Hall
        {
            get { return hall; }
            set { hall = value; OnPropertyChanged(new PropertyChangedEventArgs("Hall")); }
        }
        public SolidColorBrush Location
        {
            get { return location; }
            set { location = value; OnPropertyChanged(new PropertyChangedEventArgs("Location")); }
        }
        public SolidColorBrush DateOfBegin
        {
            get { return dateOfBegin; }
            set { dateOfBegin = value; OnPropertyChanged(new PropertyChangedEventArgs("DateOfBegin")); }
        }
        public SolidColorBrush DateOfEnd
        {
            get { return dateOfEnd; }
            set { dateOfEnd = value; OnPropertyChanged(new PropertyChangedEventArgs("DateOfEnd")); }
        }
        public void clearStatus()
        {
            dateOfEnd = dateOfBegin = location = hall = exposition = presentedExhibit= ok;
            Status = "OK";
        }

        //konstruktor
        public PresentationDisplayStatusModel()
        {
            clearStatus();
        }
        //Można dodać dowolne funkcje sprawdzające poprawność danych
        //Ta tutaj po prostu sprawdza, czy pola są puste, czy nie.

        public bool CheckPresentationForAdd(Presentation p)
        {
            int errorCount = 0;
            if (String.IsNullOrEmpty(p.PresentedExhibit))
            { errorCount++; PresentedExhibit = error; }
            else PresentedExhibit = ok;
            if (String.IsNullOrEmpty(p.Exposition))
            { errorCount++; Exposition = error; }
            else Exposition = ok;
            if (String.IsNullOrEmpty(p.Hall))
            { errorCount++; Hall = error; }
            else Hall = ok;
            if (String.IsNullOrEmpty(p.Location))
            { errorCount++; Location = error; }
            else Location = ok;
            if (String.IsNullOrEmpty(p.Location))
            { errorCount++; Location = error; }
            else Location = ok;
            if (String.IsNullOrEmpty(p.DateOfBegin))
            { errorCount++; DateOfBegin = error; }
            else DateOfBegin = ok;
            if (String.IsNullOrEmpty(p.DateOfEnd))
            { errorCount++; DateOfEnd = error; }
            else DateOfEnd = ok;
            if (errorCount == 0) { Status = "OK"; return true; }
            else { Status = "Niestety nie wypełniłeś wszystkich pól: "; return false; }
        }
    }
}
