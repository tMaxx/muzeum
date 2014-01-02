using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MvvmFoundation.Wpf;
using System.ComponentModel;
using System.Windows.Input;
using System.Windows.Media;

namespace muzeum_v3.ViewModels.Hall
{
    public class HallSearch : INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, e);
        }

        private RelayCommand useSuperQueryHallCommand;
        public ICommand UseSuperQueryHallCommand
        {
            get { return useSuperQueryHallCommand ?? (useSuperQueryHallCommand = new RelayCommand(() => UseSuperQueryHall())); }
        }

        private HallParameters parameters = new HallParameters();
        public HallParameters Parameters
        {
            get { return parameters; }
            set { parameters = value; OnPropertyChanged(new PropertyChangedEventArgs("Parameters")); }
        }

        private void UseSuperQueryHall()
        {
            App.Messenger.NotifyColleagues("UseSuperQueryHall", Parameters);
        }


        private void Clear()
        {
            Parameters = new HallParameters();
        }

        public HallSearch()
        {
            Messenger messenger = App.Messenger;
            messenger.Register("Clear", (Action)(() => Clear()));

        }
    }

    public class HallParameters
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, e);
        }
        private string hallNameParameter;
        private string locationParameter;
         
        public string HallNameParameter
        {
            get { return hallNameParameter; }
            set
            {
                hallNameParameter = value; OnPropertyChanged(new PropertyChangedEventArgs("HallNameParameter"));
            }
        }

        public string LocationParameter
        {
            get { return locationParameter; }
            set
            {
                locationParameter = value; OnPropertyChanged(new PropertyChangedEventArgs("LocationParameter"));
            }
        }
    }
}
