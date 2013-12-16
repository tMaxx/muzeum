using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Windows.Input;
using System.Linq;
using System.Text;
using System.ComponentModel;
using MvvmFoundation.Wpf;

namespace muzeum_v3.ViewModels.Location
{
    class LocationSelector : INotifyPropertyChanged
    {
        public LocationSelector()
        {
            dataItems = new MyObservableCollection<Location>();
            DataItems = App.LocationQuery.GetLocations();
            listBoxCommand = new RelayCommand(() => SelectionHasChanged());
            App.Messenger.Register("LocationCleared", (Action)(() => SelectedLocation = null));
            App.Messenger.Register("GetLocations", (Action)(() => GetLocations()));
            App.Messenger.Register("UpdateLocation", (Action<Location>)(param => UpdateLocation(param)));
            App.Messenger.Register("AddLocation", (Action<Location>)(param => AddLocation(param)));
        }

        private void AddLocation(Location e)
        {
            dataItems.Add(e);
        }

        private void GetLocations()
        {
            DataItems = App.LocationQuery.GetLocations();
            if (App.LocationQuery.hasError)
                App.Messenger.NotifyColleagues("SetStatus", App.LocationQuery.errorMessage);
        }


        private void UpdateLocation(Location e)
        {
            int index = dataItems.IndexOf(SelectedLocation);
            dataItems.ReplaceItem(index, e);
            SelectedLocation = e;
        }



        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, e);
        }

        private MyObservableCollection<Location> dataItems;
        public MyObservableCollection<Location> DataItems
        {
            get { return dataItems; }
            //If dataItems replaced by new collection, WPF must be told
            set { dataItems = value; OnPropertyChanged(new PropertyChangedEventArgs("DataItems")); }
        }

        private Location selectedLocation;
        public Location SelectedLocation
        {
            get { return selectedLocation; }
            set { selectedLocation = value; OnPropertyChanged(new PropertyChangedEventArgs("SelectedLocation")); }
        }

        private RelayCommand listBoxCommand;
        public ICommand ListBoxCommand
        {
            get { return listBoxCommand; }
        }

        private void SelectionHasChanged()
        {
            Messenger messenger = App.Messenger;
            messenger.NotifyColleagues("LocationSelectionChanged", selectedLocation);
        }
    }


    public class MyObservableCollection<Loaction> : ObservableCollection<Loaction>
    {
        public void UpdateCollection()
        {
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(
                                NotifyCollectionChangedAction.Reset));
        }


        public void ReplaceItem(int index, Loaction item)
        {
            base.SetItem(index, item);
        }
    }
}
