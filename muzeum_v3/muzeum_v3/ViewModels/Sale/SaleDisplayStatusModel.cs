using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Windows.Media;

namespace muzeum_v3.ViewModels.Sale
{
    class SaleDisplayStatusModel : INotifyPropertyChanged
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
        private SolidColorBrush expositionName;
        private SolidColorBrush numberOfTickets;
        private SolidColorBrush profit;
        private SolidColorBrush priceOfTicket;
        private SolidColorBrush nameofTicket;

        //odpowiednie properites
        public string Status
        {
            get { return status; }
            set { status = value; OnPropertyChanged(new PropertyChangedEventArgs("Status")); }
        }

        public SolidColorBrush NameOfTicket
        {
            get { return nameofTicket; }
            set { nameofTicket = value; OnPropertyChanged(new PropertyChangedEventArgs("NameOfTicket")); }
        }

        public SolidColorBrush ExpositionName
        {
            get { return expositionName; }
            set { expositionName = value; OnPropertyChanged(new PropertyChangedEventArgs("ExpositionName")); }
        }

        public SolidColorBrush NumberOfTickets
        {
            get { return numberOfTickets; }
            set { numberOfTickets = value; OnPropertyChanged(new PropertyChangedEventArgs("NumberOfTickets")); }
        }

        public SolidColorBrush Profit
        {
            get { return profit; }
            set { profit = value; OnPropertyChanged(new PropertyChangedEventArgs("Profit")); }
        }

        public SolidColorBrush PriceOfTicket
        {
            get { return priceOfTicket; }
            set { priceOfTicket = value; OnPropertyChanged(new PropertyChangedEventArgs("PriceOfTicket")); }
        }
        public void clearStatus()
        {
            expositionName = nameofTicket = numberOfTickets = profit = priceOfTicket = ok;
            Status = "OK";
        }

        //konstruktor
        //Można dodać dowolne funkcje sprawdzające poprawność danych
        //Ta tutaj po prostu sprawdza, czy pola są puste, czy nie.
        public SaleDisplayStatusModel()
        {
            clearStatus();
        }
        private bool isDecimal(string number)
        {
            if (String.IsNullOrEmpty(number))
                return false;
            else
            {
                decimal cost;
                try
                {
                    cost = Decimal.Parse(number);
                }
                catch
                {
                    return false;
                }
                if (cost < 0)
                    return false;
                else return true;
            }
        } 
       
        public bool CheckSaleForAdd(Sale p)
        {
            int errorCount = 0;
            if (String.IsNullOrEmpty(p.ExpositionName))
            { errorCount++; ExpositionName = error; }
            else ExpositionName = ok;
            if (!isDecimal(p.PriceOfTicket.ToString()))
            { errorCount++; PriceOfTicket = error; }
            else PriceOfTicket = ok;

            if (errorCount == 0) { Status = "OK"; return true; }
            else { Status = "Niestety nie wypełniłeś wszystkich pól: "; return false; }
        }
    }
}
