using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace muzeum_v3.ViewModels.Exposition
{
    public class Exposition
    { 
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, e);
        }
        //Id eksponatu używane jest tylko w bazie danych
        private int expositionId;
        private string expositionName;
        private string organizerName;
        private string locationName;
        private string description;
        private int numberOfTickets;
        private decimal profit;
        
        public int ExpositionId { get { return expositionId; } }

        public int NumberOfTickets
        {
            get { return numberOfTickets; }
            set
            {
                numberOfTickets = value; OnPropertyChanged(new PropertyChangedEventArgs("NumberOfTickets"));
            }
        }

        public decimal Profit
        {
            get { return profit; }
            set
            {
                profit = value; OnPropertyChanged(new PropertyChangedEventArgs("Profit"));
            }
        }

        public string ExpositionName
        {
            get { return expositionName; }
            set
            {
                expositionName = value; OnPropertyChanged(new PropertyChangedEventArgs("ExpositionName"));
            }
        }
       
        public string Description
        {
            get { return description; }
            set
            {
                description = value; OnPropertyChanged(new PropertyChangedEventArgs("Description"));
            }
        }
        
        public string OrganizerName
        {
            get { return organizerName; }
            set
            {
                organizerName = value; OnPropertyChanged(new PropertyChangedEventArgs("OrganizerName"));
            }
        }

        public string LocationName
        {
            get { return locationName; }
            set
            {
                locationName = value; OnPropertyChanged(new PropertyChangedEventArgs("LocationName"));
            }
        }

        //Konstruktory  eksponatu:
         public Exposition()
        {
        }

         public Exposition(int expositionId, string expositionName, string organizerName, string locationName, string description, int numberOfTickets, decimal profit)
        {
            this.expositionId = expositionId;
            ExpositionName = expositionName;
            Description = description;
            OrganizerName = organizerName;
            LocationName = locationName;
            NumberOfTickets = numberOfTickets;
            Profit = profit;
        }

        //funkcja dodająca produkt do bazy dancyh
         public void toDataBase(SqlExposition sqlExposition)
         {
             this.expositionId = sqlExposition.ExpositionId;
         }

         public void CopyExposition(Exposition p)
         {
             this.expositionId = p.ExpositionId;
             ExpositionName = p.ExpositionName;
             Description = p.Description;
             OrganizerName = p.OrganizerName;
             LocationName = p.LocationName;
             NumberOfTickets = p.NumberOfTickets;
             Profit = p.Profit;
         }
    }
    public class SqlExposition
    {
        public int ExpositionId { set; get; }
        public string ExpositionName { set; get; }
        public string OrganizerName { set; get; }
        public string LocationName { set; get; }
        public string Description { set; get; }
        public int NumberOfTickets { set; get; }
        public decimal Profit { set; get; }
        //Konstruktory  eksponatu sql:
        public SqlExposition()
        {
        }

        public SqlExposition(int expositionId, string expositionName, string organizerName, string locationName, string description, int numberOfTickets, decimal profit)
        {
            ExpositionId = expositionId;
            ExpositionName = expositionName;
            Description = description;
            OrganizerName = organizerName;
            LocationName = locationName;
            NumberOfTickets = numberOfTickets;
            Profit = profit;
        }

        public SqlExposition(Exposition p)
        {
            ExpositionId = p.ExpositionId;
            ExpositionName = p.ExpositionName;
            Description = p.Description;
            OrganizerName = p.OrganizerName;
            LocationName = p.LocationName;
            NumberOfTickets = p.NumberOfTickets;
            Profit = p.Profit;
        }

        public Exposition SqlExposition2Exposition()
        {
            return new Exposition(ExpositionId, ExpositionName, OrganizerName, LocationName, Description, NumberOfTickets, Profit);
        }
    }
}
