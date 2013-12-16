using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;


namespace muzeum_v3.ViewModels.Author
{
    public class Author
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, e);
        }
        //Id eksponatu używane jest tylko w bazie danych
        private int authorId;
        private string authorName;
        private string description;
        private string dateOfBirth;
        private string dateOfDeath;
        public int AuthorId { get { return authorId; } }
        public string AuthorName
        {
            get { return authorName; }
            set
            {
                authorName = value; OnPropertyChanged(new PropertyChangedEventArgs("AuthorName"));
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
        public string DateOfBirth
        {
            get { return dateOfBirth; }
            set
            {
                dateOfBirth = value; OnPropertyChanged(new PropertyChangedEventArgs("DateOfBirth"));
            }
        }
        public string DateOfDeath
        {
            get { return dateOfDeath; }
            set
            {
                dateOfDeath = value; OnPropertyChanged(new PropertyChangedEventArgs("DateOfDeath"));
            }
        }
        //Konstruktory  
        public Author()
        {
        }

        public Author(int authorId, string authorName, DateTime dateOfBirth, DateTime dateOfDeath, string description)
        {
            this.authorId = authorId;
            AuthorName = authorName;
            Description = description;
            DateOfBirth = dateOfBirth.ToShortDateString();
            DateOfDeath = dateOfDeath.ToShortDateString();
        }

        //funkcja dodająca do bazy dancyh
         public void toDataBase(SqlAuthor sqlAuthor)
         {
             this.authorId = sqlAuthor.AuthorId;
         }

         public void CopyAuthor(Author p)
         {
             this.authorId = p.AuthorId;
             AuthorName = p.AuthorName;
             Description = p.Description;
             DateOfBirth = p.DateOfBirth;
             DateOfDeath = p.DateOfDeath;
         }
    }
    public class SqlAuthor
    {
        public int AuthorId { set; get; }
        public string AuthorName { set; get; }
        public string Description { set; get; }
        public DateTime DateOfBirth { set; get; }
        public DateTime DateOfDeath { set; get; }
        //Konstruktory  eksponatu sql:
        public SqlAuthor()
        {
        }
        public SqlAuthor(int authorId, string authorName, DateTime dateOfBirth, DateTime dateOfDeath, string description)
        {
            AuthorId = authorId;
            AuthorName = authorName;
            Description = description;
            DateOfBirth = dateOfBirth;
            DateOfDeath = dateOfDeath;
        }
        public SqlAuthor(Author e)
        {
            AuthorId = e.AuthorId;
            AuthorName = e.AuthorName;
            Description = e.Description;
            DateOfBirth = Convert.ToDateTime(e.DateOfBirth);
            DateOfDeath = Convert.ToDateTime(e.DateOfDeath);
        }

        public Author SqlAuthor2Author()
        {
            return new Author(AuthorId, AuthorName, DateOfBirth, DateOfDeath,Description);
        }
    }
}
