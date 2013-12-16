using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Windows.Media;

namespace muzeum_v3.ViewModels.Owner
{
    class OwnerDisplayStatusModel : INotifyPropertyChanged
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
        private SolidColorBrush ownerName;
        private SolidColorBrush city;
        private SolidColorBrush country;
        private SolidColorBrush email;
        private SolidColorBrush phoneNumber;
     
        //odpowiednie properites
        public string Status
        {
            get { return status; }
            set { status = value; OnPropertyChanged(new PropertyChangedEventArgs("Status")); }
        }

        public SolidColorBrush OwnerName
        {
            get { return ownerName; }
            set { ownerName = value; OnPropertyChanged(new PropertyChangedEventArgs("OwnerName")); }
        }

        public SolidColorBrush City
        {
            get { return city; }
            set { city = value; OnPropertyChanged(new PropertyChangedEventArgs("City")); }
        }

        public SolidColorBrush Country
        {
            get { return country; }
            set { country = value; OnPropertyChanged(new PropertyChangedEventArgs("Country")); }
        }

        public SolidColorBrush Email
        {
            get { return email; }
            set { email = value; OnPropertyChanged(new PropertyChangedEventArgs("Email")); }
        }

        public SolidColorBrush PhoneNumber
        {
            get { return phoneNumber; }
            set { phoneNumber = value; OnPropertyChanged(new PropertyChangedEventArgs("PhoneNumber")); }
        }
        public void clearStatus()
        {
            ownerName = city = country = email = phoneNumber = ok;
            Status = "OK";
        } 

//konstruktor
        public OwnerDisplayStatusModel()
        {
            clearStatus();
        } 
//Można dodać dowolne funkcje sprawdzające poprawność danych
//Ta tutaj po prostu sprawdza, czy pola są puste, czy nie.

        public bool CheckOwnerForAdd(Owner p)
        {
            int errorCount = 0;
            if (String.IsNullOrEmpty(p.OwnerName))
            { errorCount++; OwnerName = error; }
            else OwnerName = ok;
            if (String.IsNullOrEmpty(p.City))
            { errorCount++; City = error; }
            else City = ok;
            if (String.IsNullOrEmpty(p.Country))
            { errorCount++; Country = error; }
            else Country = ok;
            if (String.IsNullOrEmpty(p.Email))
            { errorCount++; Email = error; }
            else Email = ok;
            if (String.IsNullOrEmpty(p.PhoneNumber))
            { errorCount++; PhoneNumber = error; }
            else PhoneNumber = ok;
           
            if (errorCount == 0) { Status = "OK"; return true; }
            else { Status = "Niestety nie wypełniłeś wszystkich pól: "; return false; }
        } 
    }
}
