using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;


namespace muzeum_v3.ViewModels.Exhibit
{
    public class Exhibit
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, e);
        }
        //Id eksponatu używane jest tylko w bazie danych
        private int exhibitId;
        private string exhibitName;
        private string description;
        private string author;
        private string owner;
        public int ExhibitId { get { return exhibitId; } }
        public string ExhibitName
        {
            get { return exhibitName; }
            set
            {
                exhibitName = value; OnPropertyChanged(new PropertyChangedEventArgs("ExhibitName"));
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
        
        public string Author
        {
            get { return author; }
            set
            {
                author = value; OnPropertyChanged(new PropertyChangedEventArgs("Author"));
            }
        }

        public string Owner
        {
            get { return owner; }
            set
            {
                owner = value; OnPropertyChanged(new PropertyChangedEventArgs("Owner"));
            }
        }

        //Konstruktory  eksponatu:
         public Exhibit()
        {
        }

         public Exhibit(int exhibitId, string exhibitName, string description, string author, string owner)
        {
            this.exhibitId = exhibitId;
            ExhibitName = exhibitName;
            Description = description;
            Author = author;
            Owner = owner;
        }

        //funkcja dodająca produkt do bazy dancyh
         public void toDataBase(SqlExhibit sqlExhibit)
         {
             this.exhibitId = sqlExhibit.ExhibitId;
         }

         public void CopyExhibit(Exhibit p)
         {
             this.exhibitId = p.ExhibitId;
             ExhibitName = p.ExhibitName;
             Description = p.Description;
             Author = p.Author;
             Owner = p.Owner;
         }
    }
    public class SqlExhibit
    {
        public int ExhibitId { set; get; }
        public string ExhibitName { set; get; }
        public string Description { set; get; }
        public string Author { set; get; }
        public string Owner { set; get; }
        //Konstruktory  eksponatu sql:
        public SqlExhibit()
        {
        }
        public SqlExhibit(int exhibitId, string exhibitName, string description, string author, string owner)
        {
            ExhibitId = exhibitId;
            ExhibitName = exhibitName;
            Description = description;
            Author = author;
            Owner = owner;
        }
        public SqlExhibit(Exhibit e)
        {
            ExhibitId = e.ExhibitId;
            ExhibitName = e.ExhibitName;
            Description = e.Description;
            Author = e.Author;
            Owner = e.Owner;
        }

        public Exhibit SqlExhibit2Exhibit()
        {
            return new Exhibit(ExhibitId, ExhibitName, Description, Author, Owner);
        }
    }

}
