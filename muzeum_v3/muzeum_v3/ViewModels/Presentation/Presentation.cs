using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace muzeum_v3.ViewModels.Presentation
{
    public class Presentation
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, e);
        }
        //Id eksponatu używane jest tylko w bazie danych
        private int presentationId;
        private string dateOfBegin;
        private string dateOfEnd;
        private string presentedExhibit;
        private string exposition;
        private string location;
        private string hall;

        public int PresentationId { get { return presentationId; } }

        public string Location
        {
            get { return location; }
            set
            {
                location = value; OnPropertyChanged(new PropertyChangedEventArgs("Location"));
            }
        }

        public string PresentedExhibit
        {
            get { return presentedExhibit; }
            set
            {
                presentedExhibit = value; OnPropertyChanged(new PropertyChangedEventArgs("PresentedExhibit"));
            }
        }

        public string Exposition
        {
            get { return exposition; }
            set
            {
                exposition = value; OnPropertyChanged(new PropertyChangedEventArgs("Exposition"));
            }
        }

        public string Hall
        {
            get { return hall; }
            set
            {
                hall = value; OnPropertyChanged(new PropertyChangedEventArgs("Hall"));
            }
        }

        public string DateOfBegin
        {
            get { return dateOfBegin; }
            set
            {
                dateOfBegin = value; OnPropertyChanged(new PropertyChangedEventArgs("DateOfBegin"));
            }
        }

        public string DateOfEnd
        {
            get { return dateOfEnd; }
            set
            {
                dateOfEnd = value; OnPropertyChanged(new PropertyChangedEventArgs("DateOfEnd"));
            }
        }
        //Konstruktory  
        public Presentation()
        {
        }

        public Presentation(int presentationId, DateTime dateOfBegin, DateTime dateOfEnd, string presentedExhibit, string exposition, string location, string hall)
        {
            this.presentationId = PresentationId;
            DateOfBegin = dateOfBegin.ToString();
            DateOfEnd = dateOfEnd.ToString();
            PresentedExhibit = presentedExhibit;
            Exposition = exposition;
            Location = location;
            Hall = hall;
        }

        //funkcja dodająca do bazy dancyh
        public void toDataBase(SqlPresentation sqlPresentation)
        {
            this.presentationId = sqlPresentation.PresentationId;
        }

        public void CopyPresentation(Presentation p)
        {
            this.presentationId = PresentationId;
            DateOfBegin = p.DateOfBegin;
            DateOfEnd = p.DateOfEnd;
            PresentedExhibit = p.PresentedExhibit;
            Exposition = p.Exposition;
            Location = p.Location;
            Hall = p.Hall;
        }
    }
    public class SqlPresentation
    {
        public int PresentationId { set; get; }
        public DateTime DateOfBegin { set; get; }
        public DateTime DateOfEnd { set; get; }
        public string PresentedExhibit { set; get; }
        public string Exposition { set; get; }
        public string Hall { set; get; }
        public string Location { set; get; }
        //Konstruktory  eksponatu sql:
        public SqlPresentation()
        {
        }
        public SqlPresentation(int presentationId, DateTime dateOfBegin, DateTime dateOfEnd, string presentedExhibit, string exposition, string location, string hall)
        {
            PresentationId = presentationId;
            DateOfBegin = dateOfBegin;
            DateOfEnd = dateOfEnd;
            PresentedExhibit = presentedExhibit;
            Exposition = exposition;
            Location = location;
            Hall = hall;
        }
        public SqlPresentation(Presentation e)
        {
            PresentationId = e.PresentationId;
            DateOfBegin = Convert.ToDateTime(e.DateOfBegin);
            DateOfEnd = Convert.ToDateTime(e.DateOfEnd);
            PresentedExhibit = e.PresentedExhibit;
            Exposition = e.Exposition;
            Location = e.Location;
            Hall = e.Hall;
        }

        public Presentation SqlPresentation2Presentation()
        {
            return new Presentation(PresentationId, DateOfBegin, DateOfEnd, PresentedExhibit, Exposition, Location, Hall);
        }
    }
}
