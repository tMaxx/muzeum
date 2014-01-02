using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MvvmFoundation.Wpf;
using System.ComponentModel;
using System.Windows.Input;
using System.Windows.Media;

namespace muzeum_v3.ViewModels.Location
{
    public class LocationSearch : INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, e);
        }

        private RelayCommand useSuperQueryLocationCommand;
        public ICommand UseSuperQueryLocationCommand
        {
            get { return useSuperQueryLocationCommand ?? (useSuperQueryLocationCommand = new RelayCommand(() => UseSuperQueryLocation())); }
        }

        private LocationParameters parameters = new LocationParameters();
        public LocationParameters Parameters
        {
            get { return parameters; }
            set { parameters = value; OnPropertyChanged(new PropertyChangedEventArgs("Parameters")); }
        }

        private void UseSuperQueryLocation()
        {
            App.Messenger.NotifyColleagues("UseSuperQueryLocation", Parameters);
        }


        private void Clear()
        {
            Parameters = new LocationParameters();
        }

        public LocationSearch()
        {
            Messenger messenger = App.Messenger;
            messenger.Register("Clear", (Action)(() => Clear()));

        }
    }

    public class LocationParameters
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, e);
        }
        
        private string locationNameParameter;
        
        public string LocationNameParameter
        {
            get { return locationNameParameter; }
            set
            {
                locationNameParameter = value; OnPropertyChanged(new PropertyChangedEventArgs("LocationNameParameter"));
            }
        }
    }
}
