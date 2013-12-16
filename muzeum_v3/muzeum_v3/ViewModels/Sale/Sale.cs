using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;


namespace muzeum_v3.ViewModels.Sale
{
    public class Sale
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, e);
        }
        //Id eksponatu używane jest tylko w bazie danych
        private int saleId;
        private string expositionName;
        private int numberOfTickets;
        private decimal priceOfTicket;
        private string nameOfTicket;

        public string NameOfTicket
        {
            get { return nameOfTicket; }
            set
            {
                nameOfTicket = value; OnPropertyChanged(new PropertyChangedEventArgs("NameOfTicket"));
            }
        }

        public int SaleId { get { return saleId; } }
        public string ExpositionName
        {
            get { return expositionName; }
            set
            {
                expositionName = value; OnPropertyChanged(new PropertyChangedEventArgs("ExpositionName"));
            }
        }

        public int NumberOfTickets
        {
            get { return numberOfTickets; }
            set
            {
                numberOfTickets = value; OnPropertyChanged(new PropertyChangedEventArgs("NumberOfTickets"));
            }
        }



        public decimal PriceOfTicket
        {
            get { return priceOfTicket; }
            set
            {
                priceOfTicket = value; OnPropertyChanged(new PropertyChangedEventArgs("PriceOfTicket"));
            }
        }

        //Konstruktory  eksponatu:
        public Sale()
        {
        }

        public Sale(int saleId, string expositionName, int numberOfTickets, decimal priceOfTicket, string nameOfTicket)
        {
            this.saleId = saleId;
            ExpositionName = expositionName;
            NumberOfTickets = numberOfTickets;
            PriceOfTicket = priceOfTicket;
            NameOfTicket = nameOfTicket;
        }

        //funkcja dodająca produkt do bazy dancyh
        public void toDataBase(SqlSale sqlSale)
        {
            this.saleId = sqlSale.SaleId;
        }

        public void CopySale(Sale e)
        {
            this.saleId = e.SaleId;
            ExpositionName = e.ExpositionName;
            NumberOfTickets = e.NumberOfTickets;
            PriceOfTicket = e.PriceOfTicket;
            NameOfTicket = e.NameOfTicket;
        }
    }
    public class SqlSale
    {
        public int SaleId { set; get; }
        public string ExpositionName { set; get; }
        public int NumberOfTickets { set; get; }
        public decimal PriceOfTicket { set; get; }
        public string NameOfTicket { set; get; }
        //Konstruktory  eksponatu sql:
        public SqlSale()
        {
        }
        public SqlSale(int saleId, string expositionName, int numberOfTickets, decimal priceOfTicket, string nameOfTicket)
        {
            SaleId = saleId;
            ExpositionName = expositionName;
            NumberOfTickets = numberOfTickets;
            PriceOfTicket = priceOfTicket;
            NameOfTicket = nameOfTicket;
        }
        public SqlSale(Sale e)
        {
            SaleId = e.SaleId;
            ExpositionName = e.ExpositionName;
            NumberOfTickets = e.NumberOfTickets;
            PriceOfTicket = e.PriceOfTicket;
            NameOfTicket = e.NameOfTicket;
        }

        public Sale SqlSale2Sale()
        {
            return new Sale(SaleId, ExpositionName, NumberOfTickets, PriceOfTicket, NameOfTicket);
        }
    }
}
