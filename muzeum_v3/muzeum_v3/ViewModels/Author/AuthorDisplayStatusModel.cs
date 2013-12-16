using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Windows.Media;

namespace muzeum_v3.ViewModels.Author
{
    class AuthorDisplayStatusModel : INotifyPropertyChanged
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
        private SolidColorBrush authorName= ok;
        private SolidColorBrush description = ok;
        private SolidColorBrush dateOfBirth = ok;
        private SolidColorBrush dateOfDeath = ok;
        //odpowiednie properites
        public string Status
        {
            get { return status; }
            set { status = value; OnPropertyChanged(new PropertyChangedEventArgs("Status")); }
        }

        public SolidColorBrush AuthorName
        {
            get { return authorName; }
            set { authorName = value; OnPropertyChanged(new PropertyChangedEventArgs("AuthorName")); }
        }

        public SolidColorBrush Description
        {
            get { return description; }
            set { description = value; OnPropertyChanged(new PropertyChangedEventArgs("Description")); }
        }

        public SolidColorBrush DateOfBirth
        {
            get { return dateOfBirth; }
            set { dateOfBirth = value; OnPropertyChanged(new PropertyChangedEventArgs("DateOfBirth")); }
        }
        public SolidColorBrush DateOfDeath
        {
            get { return dateOfDeath; }
            set { dateOfDeath = value; OnPropertyChanged(new PropertyChangedEventArgs("DateOfDeath")); }
        }
        public void clearStatus()
        {
           dateOfDeath = description = dateOfBirth = authorName = ok;
           Status = "OK";
        } 

//konstruktor
        public AuthorDisplayStatusModel()
        {
            clearStatus();
        } 
//Można dodać dowolne funkcje sprawdzające poprawność danych
//Ta tutaj po prostu sprawdza, czy pola są puste, czy nie.
       
        public bool CheckAuthorForAdd(Author p)
        {
            int errorCount = 0;
            if (String.IsNullOrEmpty(p.AuthorName))
            { errorCount++; AuthorName = error; }
            else AuthorName = ok;
            if (String.IsNullOrEmpty(p.Description))
            { errorCount++; Description = error; }
            else Description = ok;
            if (String.IsNullOrEmpty(p.DateOfBirth))
            { errorCount++; DateOfBirth = error; }
            else DateOfBirth = ok;
            if (String.IsNullOrEmpty(p.DateOfDeath))
            { errorCount++; DateOfDeath = error; }
            else DateOfDeath = ok;
            if (errorCount == 0) { Status = "OK"; return true; }
            else { Status = "Niestety nie wypełniłeś wszystkich pól: "; return false; }
        } 
    }
}
