using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace muzeum_v3.ViewModels.Hall
{
    public class Hall
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, e);
        }
        //Id eksponatu używane jest tylko w bazie danych
        private int hallId;
        private string locationName;
        private string hallName;
        private string description;

        public int HallId { get { return hallId; } }
        public string HallName
        {
            get { return hallName; }
            set
            {
                hallName = value; OnPropertyChanged(new PropertyChangedEventArgs("HallName"));
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
        public string LocationName
        {
            get { return locationName; }
            set
            {
                locationName = value; OnPropertyChanged(new PropertyChangedEventArgs("LocationName"));
            }
        }

        public Hall()
        {
        }

        public Hall(int hallId, string locationName, string hallName, string description)
        {
            this.hallId = hallId;
            LocationName = locationName;
            Description = description;
            HallName = hallName;
        }

        //funkcja dodająca do bazy dancyh
        public void toDataBase(SqlHall sqlHall)
        {
            this.hallId = sqlHall.HallId;
        }

        public void CopyHall(Hall p)
        {
            this.hallId = p.HallId;
            LocationName = p.LocationName;
            Description = p.Description;
            HallName = p.HallName;
        }
    }
    public class SqlHall
    {
        public int HallId { set; get; }
        public string LocationName { set; get; }
        public string HallName { set; get; }
        public string Description { set; get; }
        //Konstruktory  eksponatu sql:
        public SqlHall()
        {
        }
        public SqlHall(int hallId, string locationName, string hallName, string description)
        {
            HallId = hallId;
            LocationName = locationName;
            Description = description;
            HallName = hallName;
        }
        public SqlHall(Hall e)
        {

            HallId = e.HallId;
            LocationName = e.LocationName;
            Description = e.Description;
            HallName = e.HallName;
        }

        public Hall SqlHall2Hall()
        {
            return new Hall(HallId, LocationName, HallName, Description);
        }
    }
}
