using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;


namespace muzeum_v3.ViewModels.Ticket
{
    public class Ticket
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, e);
        }
        //Id eksponatu używane jest tylko w bazie danych
        private int ticketId;
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

        public int TicketId { get { return ticketId; } }

        public decimal PriceOfTicket
        {
            get { return priceOfTicket; }
            set
            {
                priceOfTicket = value; OnPropertyChanged(new PropertyChangedEventArgs("PriceOfTicket"));
            }
        }

        //Konstruktory  eksponatu:
        public Ticket()
        {
        }

        public Ticket(int TicketId, decimal priceOfTicket, string nameOfTicket)
        {
            this.ticketId = TicketId;
            PriceOfTicket = priceOfTicket;
            NameOfTicket = nameOfTicket;
        }

        //funkcja dodająca produkt do bazy dancyh
        public void toDataBase(SqlTicket sqlTicket)
        {
            this.ticketId = sqlTicket.TicketId;
        }

        public void CopyTicket(Ticket e)
        {
            this.ticketId = e.TicketId;
            PriceOfTicket = e.PriceOfTicket;
            NameOfTicket = e.NameOfTicket;
        }
    }
    public class SqlTicket
    {
        public int TicketId { set; get; }
        public decimal PriceOfTicket { set; get; }
        public string NameOfTicket { set; get; }
        //Konstruktory  eksponatu sql:
        public SqlTicket()
        {
        }
        public SqlTicket(int ticketId, decimal priceOfTicket, string nameOfTicket)
        {
            TicketId = TicketId;
            PriceOfTicket = priceOfTicket;
            NameOfTicket = nameOfTicket;
        }
        public SqlTicket(Ticket e)
        {
            TicketId = e.TicketId;
            PriceOfTicket = e.PriceOfTicket;
            NameOfTicket = e.NameOfTicket;
        }

        public Ticket SqlTicket2Ticket()
        {
            return new Ticket(TicketId, PriceOfTicket, NameOfTicket);
        }
    }
}
