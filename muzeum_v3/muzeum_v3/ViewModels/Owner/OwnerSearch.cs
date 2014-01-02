using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MvvmFoundation.Wpf;
using System.ComponentModel;
using System.Windows.Input;
using System.Windows.Media;

namespace muzeum_v3.ViewModels.Owner
{
    public class OwnerSearch : INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, e);
        }

        private RelayCommand useSuperQueryOwnerCommand;
        public ICommand UseSuperQueryOwnerCommand
        {
            get { return useSuperQueryOwnerCommand ?? (useSuperQueryOwnerCommand = new RelayCommand(() => UseSuperQueryOwner())); }
        }

        private OwnerParameters parameters = new OwnerParameters();
        public OwnerParameters Parameters
        {
            get { return parameters; }
            set { parameters = value; OnPropertyChanged(new PropertyChangedEventArgs("Parameters")); }
        }

        private void UseSuperQueryOwner()
        {
            App.Messenger.NotifyColleagues("UseSuperQueryOwner", Parameters);
        }


        private void Clear()
        {
            Parameters = new OwnerParameters();
        }

        public OwnerSearch()
        {
            Messenger messenger = App.Messenger;
            messenger.Register("Clear", (Action)(() => Clear()));

        }
    }

    public class OwnerParameters
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, e);
        }
        private string ownerNameParameter;
        private string city;
        private string country;

        public string OwnerNameParameter
        {
            get { return ownerNameParameter; }
            set
            {
                ownerNameParameter = value; OnPropertyChanged(new PropertyChangedEventArgs("OwnerNameParameter"));
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

    }
}
