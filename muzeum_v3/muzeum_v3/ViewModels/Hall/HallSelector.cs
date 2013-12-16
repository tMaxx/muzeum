using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Windows.Input;
using System.Linq;
using System.Text;
using System.ComponentModel;
using MvvmFoundation.Wpf;


namespace muzeum_v3.ViewModels.Hall
{
    class HallSelector : INotifyPropertyChanged
    {
        public HallSelector()
        {
            dataItems = new MyObservableCollection<Hall>();
            DataItems = App.HallQuery.GetHalls();
            listBoxCommand = new RelayCommand(() => SelectionHasChanged());
            App.Messenger.Register("HallCleared", (Action)(() => SelectedHall = null));
            App.Messenger.Register("GetHalls", (Action)(() => GetHalls()));
            App.Messenger.Register("UpdateHall", (Action<Hall>)(param => UpdateHall(param)));
            App.Messenger.Register("AddHall", (Action<Hall>)(param => AddHall(param)));
            App.Messenger.Register("ExpositionSelectionChanged", (Action<Exposition.Exposition>)(param => GetHallsInLocation(param)));

        }

        private void GetHallsInLocation(Exposition.Exposition e)
        {
            if (e != null)
            {
                DataItems = App.HallQuery.GetHallsInLocation(e.LocationName);
                if (App.HallQuery.hasError)
                    App.Messenger.NotifyColleagues("SetStatus", App.HallQuery.errorMessage);
            }
        }

        private void AddHall(Hall e)
        {
            dataItems.Add(e);
        }

        private void GetHalls()
        {
            DataItems = App.HallQuery.GetHalls();
            if (App.HallQuery.hasError)
                App.Messenger.NotifyColleagues("SetStatus", App.HallQuery.errorMessage);
        }

        private void UpdateHall(Hall e)
        {
            int index = dataItems.IndexOf(selectedHall);
            dataItems.ReplaceItem(index, e);
            SelectedHall = e;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, e);
        }

        private MyObservableCollection<Hall> dataItems;
        public MyObservableCollection<Hall> DataItems
        {
            get { return dataItems; }
            //If dataItems replaced by new collection, WPF must be told
            set { dataItems = value; OnPropertyChanged(new PropertyChangedEventArgs("DataItems")); }
        }

        private Hall selectedHall;
        public Hall SelectedHall
        {
            get { return selectedHall; }
            set { selectedHall = value; OnPropertyChanged(new PropertyChangedEventArgs("SelectedHall")); }
        }

        private RelayCommand listBoxCommand;
        public ICommand ListBoxCommand
        {
            get { return listBoxCommand; }
        }

        private void SelectionHasChanged()
        {
            Messenger messenger = App.Messenger;
            messenger.NotifyColleagues("HallSelectionChanged", selectedHall);
        }
    }


    public class MyObservableCollection<Hall> : ObservableCollection<Hall>
    {
        public void UpdateCollection()
        {
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(
                                NotifyCollectionChangedAction.Reset));
        }


        public void ReplaceItem(int index, Hall item)
        {
             base.SetItem(index, item);      
        }

    } // class MyObservableCollection
}
