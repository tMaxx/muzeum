using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Windows.Input;
using System.Linq;
using System.Text;
using System.ComponentModel;
using MvvmFoundation.Wpf;
using muzeum_v3.ViewModels.Author;
using System.IO;

namespace muzeum_v3.ViewModels.Exhibit
{
    class ExhibitSelector : INotifyPropertyChanged
    {
        public ExhibitSelector()
        {
            dataItems = new MyObservableCollection<Exhibit>();
            DataItems = App.ExhibitQuery.GetExhibits();

            dataItemsForSearch = new MyObservableCollection<Exhibit>();
           // dataItemsForExposition = App.ExhibitQuery.GetExhibits();

            listBoxCommand = new RelayCommand(() => SelectionHasChanged());
            App.Messenger.Register("HallSelectionChanged", (Action<Hall.Hall>)(param => GetExhibitsForHall(param)));           
            App.Messenger.Register("OwnerSelectionChanged", (Action<Owner.Owner>)(param => GetExhibitsForOwner(param)));
            App.Messenger.Register("AuthorSelectionChanged", (Action<Author.Author>)(param => GetExhibitsForAuthor(param)));
            App.Messenger.Register("ExpositionSelectionChanged", (Action<Exposition.Exposition>)(param => GetExhibitsForExposition(param)));
            App.Messenger.Register("ExhibitCleared", (Action)(() => SelectedExhibit = null));
            App.Messenger.Register("GetExhibits", (Action)(() => GetExhibits()));
            App.Messenger.Register("UpdateExhibit", (Action<Exhibit>)(param => UpdateExhibit(param)));
            App.Messenger.Register("DeleteExhibit", (Action)(() => DeleteExhibit()));
            App.Messenger.Register("AddExhibit", (Action<Exhibit>)(param => AddExhibit(param)));
            App.Messenger.Register("ClearList", (Action)(() => dataItemsForSearch.Clear()));

            App.Messenger.Register("UseSuperQueryExhibit", (Action<ExhibitParameters>)(param => UseSuperQueryExhibit(param)));    
        }

        private void UseSuperQueryExhibit(ExhibitParameters e)
        {
            if (e != null)
            {
                DataItems = App.ExhibitQuery.SuperQuery(e.ExhibitNameParameter,e.AuthorParameter,e.OwnerParameter);
                if (App.ExhibitQuery.hasError)
                    App.Messenger.NotifyColleagues("SetStatus", App.ExhibitQuery.errorMessage);
            }

        }

        private void GetExhibitsForHall(Hall.Hall e)
        {
            if (e != null)
            {
                DataItemsForSearch = App.ExhibitQuery.GetExhibitsForHall(e.HallId);
                if (App.ExhibitQuery.hasError)
                    App.Messenger.NotifyColleagues("SetStatus", App.ExhibitQuery.errorMessage);
            }

        }

        private void GetExhibitsForExposition(Exposition.Exposition e)
        {
            if (e != null)
            {
                DataItemsForSearch = App.ExhibitQuery.GetExhibitsForExposition(e.ExpositionId);
                if (App.ExhibitQuery.hasError)
                    App.Messenger.NotifyColleagues("SetStatus", App.ExhibitQuery.errorMessage);
            }

        }

        private void GetExhibitsForAuthor(Author.Author e)
        {
            if (e != null)
            {
                DataItemsForSearch = App.ExhibitQuery.GetExhibitsForAuthor(e.AuthorId);
                if (App.ExhibitQuery.hasError)
                    App.Messenger.NotifyColleagues("SetStatus", App.ExhibitQuery.errorMessage);
            }

        }

        private void GetExhibitsForOwner(Owner.Owner e)
        {
            if (e != null)
            {
                DataItemsForSearch = App.ExhibitQuery.GetExhibitsForOwner(e.OwnerId );
                if (App.ExhibitQuery.hasError)
                    App.Messenger.NotifyColleagues("SetStatus", App.ExhibitQuery.errorMessage);
            }

        }

        private void GetExhibits()
        {
            using (StreamWriter writer = new StreamWriter("Eksponaty.txt", true))
            {
                foreach (Exhibit e in DataItems)
                {
                    writer.WriteLine(e.ExhibitName + "\t" + e.ExhibitId + "\t" + e.Description + "\t" + e.Author + "\t" + e.Owner + "\t" + DataItems.IndexOf(e));

                }
                writer.WriteLine(DataItems.Count);
            }

            DataItems = App.ExhibitQuery.GetExhibits();
            if (App.ExhibitQuery.hasError)
                App.Messenger.NotifyColleagues("SetStatus", App.ExhibitQuery.errorMessage);

        }

        private void AddExhibit(Exhibit e)
        {
            dataItems.Add(e);
        }


        private void UpdateExhibit(Exhibit e)
        {
            using (StreamWriter writer = new StreamWriter("Update.txt", true))
            {
               
                writer.WriteLine(e.ExhibitName + "\t" + e.ExhibitId + "\t" + e.Description + "\t" + e.Author  + "\t" + e.Owner   + "\t" +  DataItems.IndexOf(e));
                writer.WriteLine(DataItems.Count);
            }
            if (e == null) return;
            int index = dataItems.IndexOf(selectedExhibit);
            dataItems.ReplaceItem(index, e);
            SelectedExhibit = e;
        }


        private void DeleteExhibit()
        {
            dataItems.Remove(selectedExhibit);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, e);
        }
        private MyObservableCollection<Exhibit> dataItems;
        public MyObservableCollection<Exhibit> DataItems
        {
            get { return dataItems; }
            //If dataItems replaced by new collection, WPF must be told
            set { dataItems = value; OnPropertyChanged(new PropertyChangedEventArgs("DataItems")); }
        }

        private MyObservableCollection<Exhibit> dataItemsForSearch;
        public MyObservableCollection<Exhibit> DataItemsForSearch
        {
            get { return dataItemsForSearch; }
            //If dataItems replaced by new collection, WPF must be told
            set { dataItemsForSearch = value; OnPropertyChanged(new PropertyChangedEventArgs("DataItemsForSearch")); }
        }
       
        private Exhibit selectedExhibit;
        public Exhibit SelectedExhibit
        {
            get { return selectedExhibit; }
            set { selectedExhibit = value; OnPropertyChanged(new PropertyChangedEventArgs("SelectedExhibit")); }
        }

        private RelayCommand listBoxCommand;
        public ICommand ListBoxCommand
        {
            get { return listBoxCommand; }
        }

        private void SelectionHasChanged()
        {
            Messenger messenger = App.Messenger;
            messenger.NotifyColleagues("ExhibitSelectionChanged", selectedExhibit);
        }
    }


    public class MyObservableCollection<Exhibit> : ObservableCollection<Exhibit>
    {
        public void UpdateCollection()
        {
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(
                                NotifyCollectionChangedAction.Reset));
        }


        public void ReplaceItem(int index, Exhibit item)
        {
            base.SetItem(index, item);
        }

    } // class MyObservableCollection
}

