using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MvvmFoundation.Wpf;
using System.ComponentModel;
using System.Windows.Input;
using System.Windows.Media;

namespace muzeum_v3.ViewModels.Exhibit
{
    public class ExhibitSearch : INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, e);
        }

        private RelayCommand useSuperQueryExhibitCommand;
        public ICommand UseSuperQueryExhibitCommand
        {
            get { return useSuperQueryExhibitCommand ?? (useSuperQueryExhibitCommand = new RelayCommand(() => UseSuperQueryExhibit())); }
        }

        private ExhibitParameters parameters = new ExhibitParameters();
        public ExhibitParameters Parameters
        {
            get { return parameters; }
            set { parameters = value; OnPropertyChanged(new PropertyChangedEventArgs("Parameters")); }
        }

        private void UseSuperQueryExhibit()
        {
            App.Messenger.NotifyColleagues("UseSuperQueryExhibit", Parameters);
        }

       
        private void Clear()
        {
            Parameters = new ExhibitParameters();
        }

        public ExhibitSearch()
        {
            Messenger messenger = App.Messenger;
            messenger.Register("Clear", (Action)(() => Clear()));

        }
    }

    public class ExhibitParameters
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, e);
        }
        private string exhibitNameParameter;
        private string authorParameter;
        private string ownerParameter;

        public string ExhibitNameParameter
        {
            get { return exhibitNameParameter; }
            set
            {
                exhibitNameParameter = value; OnPropertyChanged(new PropertyChangedEventArgs("ExhibitNameParameter"));
            }
        }

        public string AuthorParameter
        {
            get { return authorParameter; }
            set
            {
                authorParameter = value; OnPropertyChanged(new PropertyChangedEventArgs("AuthorParameter"));
            }
        }

        public string OwnerParameter
        {
            get { return ownerParameter; }
            set
            {
                ownerParameter = value; OnPropertyChanged(new PropertyChangedEventArgs("OwnerParameter"));
            }
        }

    }
}
