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
    class LocationDisplayModel : INotifyPropertyChanged
    {
        private bool isSelected = false;

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, e);
        }

        private readonly LocationDisplayStatusModel stat = new LocationDisplayStatusModel();
        public LocationDisplayStatusModel Stat { get { return stat; } }
        private Location displayedLocation = new Location();
       
        private RelayCommand getLocationsCommand;
        private RelayCommand clearCommand;
        private RelayCommand updateCommand;
        private RelayCommand addCommand;
        private RelayCommand sendLocationNameCommand;

        public ICommand SendLocationNameCommand
        {
            get { return sendLocationNameCommand ?? (sendLocationNameCommand = new RelayCommand(() => ChangeLocationName())); }
        }

        private void ChangeLocationName()
        {
            if (!stat.CheckLocationForAdd(DisplayedLocation)) return;
            App.Messenger.NotifyColleagues("ChangeLocationName", DisplayedLocation.LocationName);
        }
        public Location DisplayedLocation
        {
            get { return displayedLocation; }
            set { displayedLocation = value; OnPropertyChanged(new PropertyChangedEventArgs("DisplayedLocation")); }
        }

        public ICommand GetLocationsCommand
        {
            get { return getLocationsCommand ?? (getLocationsCommand = new RelayCommand(() => GetLocations())); }
        }

        private void GetLocations()
        {
            isSelected = false;
            stat.clearStatus();
            DisplayedLocation = new Location();
            App.Messenger.NotifyColleagues("GetLocations");
        }

        public ICommand ClearCommand
        {
            get { return clearCommand ?? (clearCommand = new RelayCommand(() => ClearLocationDisplay())); }
        }

        private void ClearLocationDisplay()
        {
            isSelected = false;
            stat.clearStatus();
            DisplayedLocation = new Location();
            App.Messenger.NotifyColleagues("LocationCleared");
        } 

        public ICommand UpdateCommand
        {
            get { return updateCommand ?? (updateCommand = new RelayCommand(() => UpdateLocation(), ()=>isSelected)); }
        }

        private void UpdateLocation()
        {
            if (!stat.CheckLocationForAdd(DisplayedLocation)) return;
                if(!App.LocationQuery.UpdateLocation(DisplayedLocation))
                {
                    stat.Status = App.LocationQuery.errorMessage;
                    return;
                }
                App.Messenger.NotifyColleagues("UpdateLocation", DisplayedLocation);
        }

        public ICommand AddCommand
        {
            get { return addCommand ?? (addCommand = new RelayCommand(() => AddLocation(), () => !isSelected)); }
        }

        private void AddLocation()
        {
            if (!stat.CheckLocationForAdd(DisplayedLocation)) return;
            if (!App.LocationQuery.AddLocation(DisplayedLocation))
            {
                stat.Status = App.LocationQuery.errorMessage;
                return;
            }
            App.Messenger.NotifyColleagues("AddLocation", DisplayedLocation);
        }

        public LocationDisplayModel()
        {
            Messenger messenger = App.Messenger;
            messenger.Register("LocationSelectionChanged", (Action<Location>)(param => ProcessLocation(param)));
            messenger.Register("SetStatus", (Action<String>)(param => stat.Status = param));
            messenger.Register("Clear", (Action)(() => ClearLocationDisplay()));
        }

        public void ProcessLocation(Location p)
        {
            if (p == null) {
                isSelected = false;  return; 
            }
            Location temp = new Location();
            temp.CopyAuthor(p);
            DisplayedLocation = temp;
            isSelected = true;
            stat.clearStatus();
        } 
    }
}
