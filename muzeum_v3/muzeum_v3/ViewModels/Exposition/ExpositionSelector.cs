using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Windows.Input;
using System.Linq;
using System.Text;
using System.ComponentModel;
using MvvmFoundation.Wpf;
namespace muzeum_v3.ViewModels.Exposition
{
    class ExpositionSelector : INotifyPropertyChanged
    {
        public ExpositionSelector()
        {
            dataItems = new MyObservableCollection<Exposition>();
            DataItems = App.ExpositionQuery.GetExpositions();

            dataItemsForSearch = new MyObservableCollection<Exposition>();

            listBoxCommand = new RelayCommand(() => SelectionHasChanged());

            App.Messenger.Register("UpdateSaleForExposition", (Action)(() =>  GetExpositions()));
            App.Messenger.Register("LocationSelectionChanged", (Action<Location.Location>)(param => GetExpositionsForLocation(param)));
            App.Messenger.Register("OrgSelectionChanged", (Action<Org.Org>)(param => GetExpositionsForOrg(param)));           
            App.Messenger.Register("ExpositionCleared", (Action)(() => SelectedExposition = null));
            App.Messenger.Register("GetExpositions", (Action)(() => GetExpositions()));
            App.Messenger.Register("UpdateExposition", (Action<Exposition>)(param => UpdateExposition(param)));
            App.Messenger.Register("DeleteExposition", (Action)(() => DeleteExposition()));
            App.Messenger.Register("AddExposition", (Action<Exposition>)(param => AddExposition(param)));
            App.Messenger.Register("ClearList", (Action)(() => dataItemsForSearch.Clear()));
        
            App.Messenger.Register("UseSuperQueryExposition", (Action<ExpositionParameters>)(param => UseSuperQueryExposition(param)));    
        }

        private void UseSuperQueryExposition(ExpositionParameters e)
        {
            if (e != null)
            {
                DataItems = App.ExpositionQuery.SuperQuery(e.ExpositionNameParameter,e.Org,e.Location,e.NumberOfTicketsFROM,e.NumberOfTicketsTO,e.ProfitFROM,e.ProfitTO);
                if (App.ExpositionQuery.hasError)
                    App.Messenger.NotifyColleagues("SetStatus", App.ExpositionQuery.errorMessage);
            }

        }

        

        private void GetExpositionsForOrg(Org.Org e)
        {
            if (e != null)
            {
                DataItemsForSearch = App.ExpositionQuery.GetExpositionsForOrg(e.OrgId);
                if (App.ExpositionQuery.hasError)
                    App.Messenger.NotifyColleagues("SetStatus", App.ExpositionQuery.errorMessage);
            }

        }

        private void GetExpositionsForLocation(Location.Location e)
        {
            if (e != null)
            {
                DataItemsForSearch = App.ExpositionQuery.GetExpositionsForLocation(e.LocationId);
                if (App.ExpositionQuery.hasError)
                    App.Messenger.NotifyColleagues("SetStatus", App.ExpositionQuery.errorMessage);
            }

        }

        private void GetExpositions()
        {
            DataItems = App.ExpositionQuery.GetExpositions();
            if (App.ExpositionQuery.hasError)
                App.Messenger.NotifyColleagues("SetStatus", App.ExpositionQuery.errorMessage);
        }


        private void AddExposition(Exposition e)
        {
            dataItems.Add(e);
        }


        private void UpdateExposition(Exposition e)
        {
            int index = dataItems.IndexOf(selectedExposition);
            dataItems.ReplaceItem(index, e);
            SelectedExposition = e;
        }


        private void DeleteExposition()
        {
            dataItems.Remove(selectedExposition);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, e);
        }

        private MyObservableCollection<Exposition> dataItems;
        public MyObservableCollection<Exposition> DataItems
        {
            get { return dataItems; }
            //If dataItems replaced by new collection, WPF must be told
            set { dataItems = value; OnPropertyChanged(new PropertyChangedEventArgs("DataItems")); }
        }

        private MyObservableCollection<Exposition> dataItemsForSearch;
        public MyObservableCollection<Exposition> DataItemsForSearch
        {
            get { return dataItemsForSearch; }
            //If dataItems replaced by new collection, WPF must be told
            set { dataItemsForSearch = value; OnPropertyChanged(new PropertyChangedEventArgs("DataItemsForSearch")); }
        }

        private Exposition selectedExposition;
        public Exposition SelectedExposition
        {
            get { return selectedExposition; }
            set { selectedExposition = value; OnPropertyChanged(new PropertyChangedEventArgs("SelectedExposition")); }
        }

        private RelayCommand listBoxCommand;
        public ICommand ListBoxCommand
        {
            get { return listBoxCommand; }
        }
       
        private void SelectionHasChanged()
        {
            Messenger messenger = App.Messenger;
            messenger.NotifyColleagues("ExpositionSelectionChanged", selectedExposition);
        }
    }


    public class MyObservableCollection<Exposition> : ObservableCollection<Exposition>
    {
        public void UpdateCollection()
        {
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(
                                NotifyCollectionChangedAction.Reset));
        }


        public void ReplaceItem(int index, Exposition item)
        {
            base.SetItem(index, item);
        }

    } // class MyObservableCollection
}

