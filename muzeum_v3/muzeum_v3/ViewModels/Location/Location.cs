using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;


namespace muzeum_v3.ViewModels.Location
{
    public class Location
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, e);
        }
        //Id eksponatu używane jest tylko w bazie danych
        private int locationId;
        private string locationName;
        private string description;

        public int LocationId { get { return locationId; } }
        public string LocationName
        {
            get { return locationName; }
            set
            {
                locationName = value; OnPropertyChanged(new PropertyChangedEventArgs("LocationName"));
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
        //Konstruktory  
        public Location()
        {
        }

        public Location(int locationId, string locationName, string description)
        {
            this.locationId = locationId;
            LocationName = locationName;
            Description = description;
        }

        //funkcja dodająca do bazy dancyh
        public void toDataBase(SqlLocation sqlLocation)
        {
            this.locationId = sqlLocation.LocationId;
        }

        public void CopyAuthor(Location p)
        {
            this.locationId = p.LocationId;
            LocationName = p.LocationName;
            Description = p.Description;
        }
    }
    public class SqlLocation
    {
        public int LocationId { set; get; }
        public string LocationName { set; get; }
        public string Description { set; get; }

        //Konstruktory  eksponatu sql:
        public SqlLocation()
        {
        }
        public SqlLocation(int locationId, string locationName, string description)
        {
            LocationId = locationId;
            LocationName = locationName;
            Description = description;

        }
        public SqlLocation(Location e)
        {
            LocationId = e.LocationId;
            LocationName = e.LocationName;
            Description = e.Description;
        }

        public Location SqlLocation2Location()
        {
            return new Location(LocationId, LocationName, Description);
        }
    }
}
