using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;


namespace muzeum_v3.ViewModels.Org
{
    public class Org
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, e);
        }
        //Id eksponatu używane jest tylko w bazie danych
        private int orgId;
        private string orgName;
        private string city;
        private string email;
        private string phoneNumber;

        public int OrgId { get { return orgId; } }
        public string OrgName
        {
            get { return orgName; }
            set
            {
                orgName = value; OnPropertyChanged(new PropertyChangedEventArgs("OrgName"));
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
        public Org()
        {
        }

        public Org(int orgId, string orgName, string city, string email, string phoneNumber)
        {
            this.orgId = orgId;
            OrgName = orgName;
            City = city;
            Email = email;
            PhoneNumber = phoneNumber;
        }

        //funkcja dodająca produkt do bazy dancyh
        public void toDataBase(SqlOrg sqlOrg)
        {
            this.orgId = sqlOrg.OrgId;
        }

        public void CopyOrg(Org p)
        {
            this.orgId = p.OrgId;
            OrgName = p.OrgName;
            City = p.City;
            Email = p.Email;
            PhoneNumber = p.PhoneNumber;
        }
    }
    public class SqlOrg
    {
        public int OrgId { set; get; }
        public string OrgName { set; get; }
        public string City { set; get; }
        public string Email { set; get; }
        public string PhoneNumber { set; get; }
        //Konstruktory  eksponatu sql:
        public SqlOrg()
        {
        }
        public SqlOrg(int orgId, string orgName, string city, string email, string phoneNumber)
        {
            OrgId = orgId;
            OrgName = orgName;
            City = city;
            Email = email;
            PhoneNumber = phoneNumber;
        }
        public SqlOrg(Org e)
        {
            OrgId = e.OrgId;
            OrgName = e.OrgName;
            City = e.City;
            Email = e.Email;
            PhoneNumber = e.PhoneNumber;
        }

        public Org SqlOrg2Org()
        {
            return new Org(OrgId, OrgName, City, Email, PhoneNumber);
        }
    }
}
