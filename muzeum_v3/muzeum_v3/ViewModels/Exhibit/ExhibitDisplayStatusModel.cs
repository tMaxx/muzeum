using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Windows.Media;
//Klasa odpowiedzialna za sprawdzanie poprawności pól przy dodawaniu eksponatu.
//Dodatkowo koloruje odpowiednio pola na czerwono gdy dane są niepoprawne przy dodawaniu/zmianie
namespace muzeum_v3.ViewModels.Exhibit
{
    public class ExhibitDisplayStatusModel : INotifyPropertyChanged
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
        private SolidColorBrush exhibitNameBrush = ok;
        private SolidColorBrush descriptionBrush = ok;
        private SolidColorBrush authorBrush = ok;
        private SolidColorBrush ownerBrush = ok;
        //odpowiednie properites
        public string Status
        {
            get { return status; }
            set { status = value; OnPropertyChanged(new PropertyChangedEventArgs("Status")); }
        }

        public SolidColorBrush ExhibitNameBrush
        {
            get { return exhibitNameBrush; }
            set { exhibitNameBrush = value; OnPropertyChanged(new PropertyChangedEventArgs("ExhibitNameBrush")); }
        }

        public SolidColorBrush DescriptionBrush
        {
            get { return descriptionBrush; }
            set { descriptionBrush = value; OnPropertyChanged(new PropertyChangedEventArgs("DescriptionBrush")); }
        }

        public SolidColorBrush AuthorBrush
        {
            get { return authorBrush; }
            set { authorBrush = value; OnPropertyChanged(new PropertyChangedEventArgs("AuthorBrush")); }
        }
        public SolidColorBrush OwnerBrush
        {
            get { return ownerBrush; }
            set { ownerBrush = value; OnPropertyChanged(new PropertyChangedEventArgs("OwnerBrush")); }
        }
        public void clearStatus()
        {
           exhibitNameBrush = descriptionBrush = ownerBrush = authorBrush = ok;
           Status = "OK";
        } 

//konstruktor
        public ExhibitDisplayStatusModel()
        {
            clearStatus();
        } 
//Można dodać dowolne funkcje sprawdzające poprawność danych
//Ta tutaj po prostu sprawdza, czy pola są puste, czy nie.
       
        public bool CheckExhibitForAdd(Exhibit p)
        {
            int errorCount = 0;
            if (String.IsNullOrEmpty(p.ExhibitName))
            { errorCount++; ExhibitNameBrush = error; }
            else ExhibitNameBrush = ok;
            if (String.IsNullOrEmpty(p.Description))
            { errorCount++; DescriptionBrush = error; }
            else DescriptionBrush = ok;
            if (String.IsNullOrEmpty(p.Author))
            { errorCount++; AuthorBrush = error; }
            else AuthorBrush = ok;
            if (String.IsNullOrEmpty(p.Owner))
            { errorCount++; OwnerBrush = error; }
            else OwnerBrush = ok;
            if (errorCount == 0) { Status = "OK"; return true; }
            else { Status = "Niestety nie wypełniłeś wszystkich pól: "; return false; }
        } 
    }
}
