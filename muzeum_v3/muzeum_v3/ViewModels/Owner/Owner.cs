using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;


namespace muzeum_v3.ViewModels.Owner
{
    public class Owner
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, e);
        }
        //Id eksponatu używane jest tylko w bazie danych
        private int ownerId;
        private string ownerName;
        private string city;
        private string country;
        private string email;
        private string phoneNumber;

        public int OwnerId { get { return ownerId; } }
        public string OwnerName
        {
            get { return ownerName; }
            set
            {
                ownerName = value; OnPropertyChanged(new PropertyChangedEventArgs("OwnerName"));
            }
        }

        public string City
        {
            get { return city; }
            set
            {
                city = value; OnPropertyChanged(new PropertyChangedEventArgs("City"));
            }
        }

        public string Country
        {
            get { return country; }
            set
            {
                country = value; OnPropertyChanged(new PropertyChangedEventArgs("Country"));
            }
        }

        public string Email
        {
            get { return email; }
            set
            {
                email = value; OnPropertyChanged(new PropertyChangedEventArgs("Email"));
            }
        }

        public string PhoneNumber
        {
            get { return phoneNumber; }
            set
            {
                phoneNumber = value; OnPropertyChanged(new PropertyChangedEventArgs("PhoneNumber"));
            }
        }

        //Konstruktory  eksponatu:
        public Owner()
        {
        }

        public Owner(int ownerId, string ownerName, string city, string country, string email, string phoneNumber)
        {
            this.ownerId = ownerId;
            OwnerName = ownerName;
            City = city;
            Country = country;
            Email = email;
            PhoneNumber = phoneNumber;
        }

        //funkcja dodająca produkt do bazy dancyh
        public void toDataBase(SqlOwner sqlOwner)
        {
            this.ownerId = sqlOwner.OwnerId;
        }

        public void CopyOwner(Owner p)
        {
            this.ownerId = p.OwnerId;
            OwnerName = p.OwnerName;
            City = p.City;
            Country = p.Country;
            Email = p.Email;
            PhoneNumber = p.PhoneNumber;
        }
    }
    public class SqlOwner
    {
        public int OwnerId { set; get; }
        public string OwnerName { set; get; }
        public string City { set; get; }
        public string Country { set; get; }
        public string Email { set; get; }
        public string PhoneNumber { set; get; }
        //Konstruktory  eksponatu sql:
        public SqlOwner()
        {
        }
        public SqlOwner(int ownerId, string ownerName, string city, string country, string email, string phoneNumber)
        {
            OwnerId = ownerId;
            OwnerName = ownerName;
            City = city;
            Country = country;
            Email = email;
            PhoneNumber = phoneNumber;
        }
        public SqlOwner(Owner e)
        {
            OwnerId = e.OwnerId;
            OwnerName = e.OwnerName;
            City = e.City;
            Country = e.Country;
            Email = e.Email;
            PhoneNumber = e.PhoneNumber;
        }

        public Owner SqlOwner2Owner()
        {
            return new Owner(OwnerId, OwnerName, City, Country, Email, PhoneNumber);
        }
    }
}
