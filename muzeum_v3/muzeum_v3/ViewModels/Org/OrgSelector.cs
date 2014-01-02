using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Windows.Input;
using System.Linq;
using System.Text;
using System.ComponentModel;
using MvvmFoundation.Wpf;

namespace muzeum_v3.ViewModels.Org
{
    class OrgSelector : INotifyPropertyChanged
    {
        public OrgSelector()
        {
            dataItems = new MyObservableCollection<Org>();
            DataItems = App.OrgQuery.GetOrgs();
            listBoxCommand = new RelayCommand(() => SelectionHasChanged());
            App.Messenger.Register("OrgCleared", (Action)(() => SelectedOrg = null));
            App.Messenger.Register("GetOrgs", (Action)(() => GetOrgs()));
            App.Messenger.Register("UpdateOrg", (Action<Org>)(param => UpdateOrg(param)));
            App.Messenger.Register("AddOrg", (Action<Org>)(param => AddOrg(param)));
           
            App.Messenger.Register("UseSuperQueryOrg", (Action<OrgParameters>)(param => UseSuperQueryOrg(param)));
        }

        private void UseSuperQueryOrg(OrgParameters e)
        {
            if (e != null)
            {
                DataItems = App.OrgQuery.SuperQuery(e.OrgNameParameter, e.City);
                if (App.OrgQuery.hasError)
                    App.Messenger.NotifyColleagues("SetStatus", App.OrgQuery.errorMessage);
            }

        }
        private void AddOrg(Org e)
        {
            dataItems.Add(e);
        }
        private void GetOrgs()
        {
            DataItems = App.OrgQuery.GetOrgs();
            if (App.OrgQuery.hasError)
                App.Messenger.NotifyColleagues("SetStatus", App.OrgQuery.errorMessage);
        }

        private void UpdateOrg(Org e)
        {
            int index = dataItems.IndexOf(selectedOrg);
            dataItems.ReplaceItem(index, e);
            SelectedOrg = e;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, e);
        }

        private MyObservableCollection<Org> dataItems;
        public MyObservableCollection<Org> DataItems
        {
            get { return dataItems; }
            //If dataItems replaced by new collection, WPF must be told
            set { dataItems = value; OnPropertyChanged(new PropertyChangedEventArgs("DataItems")); }
        }

        private Org selectedOrg;
        public Org SelectedOrg
        {
            get { return selectedOrg; }
            set { selectedOrg = value; OnPropertyChanged(new PropertyChangedEventArgs("SelectedOrg")); }
        }

        private RelayCommand listBoxCommand;
        public ICommand ListBoxCommand
        {
            get { return listBoxCommand; }
        }

        private void SelectionHasChanged()
        {
            Messenger messenger = App.Messenger;
            messenger.NotifyColleagues("OrgSelectionChanged", selectedOrg);
        }
    }


    public class MyObservableCollection<Org> : ObservableCollection<Org>
    {
        public void UpdateCollection()
        {
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(
                                NotifyCollectionChangedAction.Reset));
        }


        public void ReplaceItem(int index, Org item)
        {
            base.SetItem(index, item);
        }

    }
}
