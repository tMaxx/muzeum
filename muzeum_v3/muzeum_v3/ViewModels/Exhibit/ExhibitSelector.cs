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
namespace muzeum_v3.ViewModels.Exhibit
{
    class ExhibitSelector : INotifyPropertyChanged
    {
        public ExhibitSelector()
        {
            dataItems = new MyObservableCollection<Exhibit>();
            DataItems = App.ExhibitQuery.GetExhibits();
            listBoxCommand = new RelayCommand(() => SelectionHasChanged());
            App.Messenger.Register("ExpositionSelectionChanged", (Action<Exposition.Exposition>)(param => GetExhibitsForExposition(param)));
            App.Messenger.Register("ExhibitCleared", (Action)(() => SelectedExhibit = null));
            App.Messenger.Register("GetExhibits", (Action)(() => GetExhibits()));
            App.Messenger.Register("UpdateExhibit", (Action<Exhibit>)(param => UpdateExhibit(param)));
            App.Messenger.Register("DeleteExhibit", (Action)(() => DeleteExhibit()));
            App.Messenger.Register("AddExhibit", (Action<Exhibit>)(param => AddExhibit(param)));
        }

        private void GetExhibitsForExposition(Exposition.Exposition e)
        {
            if (e != null)
            {
                DataItems = App.ExhibitQuery.GetExhibitsForExposition(e.ExpositionId);
                if (App.ExhibitQuery.hasError)
                    App.Messenger.NotifyColleagues("SetStatus", App.ExhibitQuery.errorMessage);
            }

        }

        private void GetExhibits()
        {
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

