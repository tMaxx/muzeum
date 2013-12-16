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

            dataItemsForExposition = new MyObservableCollection<Exhibit>();
           // dataItemsForExposition = App.ExhibitQuery.GetExhibits();

            listBoxCommand = new RelayCommand(() => SelectionHasChanged());
            App.Messenger.Register("ExpositionSelectionChanged", (Action<Exposition.Exposition>)(param => GetExhibitsForExposition(param)));
            App.Messenger.Register("ExhibitCleared", (Action)(() => SelectedExhibit = null));
            App.Messenger.Register("GetExhibits", (Action)(() => GetExhibits()));
            App.Messenger.Register("UpdateExhibit", (Action<Exhibit>)(param => UpdateExhibit(param)));
            App.Messenger.Register("DeleteExhibit", (Action)(() => DeleteExhibit()));
            App.Messenger.Register("AddExhibit", (Action<Exhibit>)(param => AddExhibit(param)));
             App.Messenger.Register("ClearList", (Action)(() => dataItemsForExposition.Clear()));
        }

        private void GetExhibitsForExposition(Exposition.Exposition e)
        {
            if (e != null)
            {
                DataItemsForExposition = App.ExhibitQuery.GetExhibitsForExposition(e.ExpositionId);
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
        private MyObservableCollection<Exhibit> dataItemsForExposition;
        public MyObservableCollection<Exhibit> DataItemsForExposition
        {
            get { return dataItemsForExposition; }
            //If dataItems replaced by new collection, WPF must be told
            set { dataItemsForExposition = value; OnPropertyChanged(new PropertyChangedEventArgs("DataItemsForExposition")); }
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

