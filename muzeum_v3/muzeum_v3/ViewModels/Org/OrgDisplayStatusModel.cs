using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Windows.Media;

namespace muzeum_v3.ViewModels.Org
{
    class OrgDisplayStatusModel : INotifyPropertyChanged
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
        private SolidColorBrush orgName;
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

        public SolidColorBrush OrgName
        {
            get { return orgName; }
            set { orgName = value; OnPropertyChanged(new PropertyChangedEventArgs("OrgName")); }
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
            orgName = city = country = email = phoneNumber = ok;
            Status = "OK";
        }

        //konstruktor
        public OrgDisplayStatusModel()
        {
            clearStatus();
        }
        //Można dodać dowolne funkcje sprawdzające poprawność danych
        //Ta tutaj po prostu sprawdza, czy pola są puste, czy nie.

        public bool CheckOrgForAdd(Org p)
        {
            int errorCount = 0;
            if (String.IsNullOrEmpty(p.OrgName))
            { errorCount++; OrgName = error; }
            else OrgName = ok;
            if (String.IsNullOrEmpty(p.City))
            { errorCount++; City = error; }
            else City = ok;
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
