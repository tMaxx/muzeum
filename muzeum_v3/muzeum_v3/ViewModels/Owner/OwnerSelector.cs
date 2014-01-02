using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Windows.Input;
using System.Linq;
using System.Text;
using System.ComponentModel;
using MvvmFoundation.Wpf;

namespace muzeum_v3.ViewModels.Owner
{
    class OwnerSelector : INotifyPropertyChanged
    {
        public OwnerSelector()
        {
            dataItems = new MyObservableCollection<Owner>();
            DataItems = App.OwnerQuery.GetOwners();
            listBoxCommand = new RelayCommand(() => SelectionHasChanged());
            App.Messenger.Register("OwnerCleared", (Action)(() => SelectedOwner = null));
            App.Messenger.Register("GetOwners", (Action)(() => GetOwners()));
            App.Messenger.Register("UpdateOwner", (Action<Owner>)(param => UpdateOwner(param)));
            App.Messenger.Register("AddOwner", (Action<Owner>)(param => AddOwner(param)));
            
            App.Messenger.Register("UseSuperQueryOwner", (Action<OwnerParameters>)(param => UseSuperQueryOwner(param)));
        }

        private void UseSuperQueryOwner(OwnerParameters e)
        {
            if (e != null)
            {
                DataItems = App.OwnerQuery.SuperQuery(e.OwnerNameParameter, e.City, e.Country);
                if (App.OwnerQuery.hasError)
                    App.Messenger.NotifyColleagues("SetStatus", App.OwnerQuery.errorMessage);
            }

        }
 		private void AddOwner(Owner e)
        {
            dataItems.Add(e);
        }
        private void GetOwners()
        {
            DataItems = App.OwnerQuery.GetOwners();
            if (App.OwnerQuery.hasError)
                App.Messenger.NotifyColleagues("SetStatus", App.OwnerQuery.errorMessage);
        }

        private void UpdateOwner(Owner e)
        {
            int index = dataItems.IndexOf(selectedOwner);
            dataItems.ReplaceItem(index, e);
            SelectedOwner = e;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, e);
        }

        private MyObservableCollection<Owner> dataItems;
        public MyObservableCollection<Owner> DataItems
        {
            get { return dataItems; }
            //If dataItems replaced by new collection, WPF must be told
            set { dataItems = value; OnPropertyChanged(new PropertyChangedEventArgs("DataItems")); }
        }

        private Owner selectedOwner;
        public Owner SelectedOwner
        {
            get { return selectedOwner; }
            set { selectedOwner = value; OnPropertyChanged(new PropertyChangedEventArgs("SelectedOwner")); }
        }

        private RelayCommand listBoxCommand;
        public ICommand ListBoxCommand
        {
            get { return listBoxCommand; }
        }

        private void SelectionHasChanged()
        {
            Messenger messenger = App.Messenger;
            messenger.NotifyColleagues("OwnerSelectionChanged", selectedOwner);
        }
    }


    public class MyObservableCollection<Owner> : ObservableCollection<Owner>
    {
        public void UpdateCollection()
        {
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(
                                NotifyCollectionChangedAction.Reset));
        }


        public void ReplaceItem(int index, Owner item)
        {
             base.SetItem(index, item);      
        }

    }
}
