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
            listBoxCommand = new RelayCommand(() => SelectionHasChanged());
            App.Messenger.Register("ExpositionCleared", (Action)(() => SelectedExposition = null));
            App.Messenger.Register("GetExpositions", (Action)(() => GetExpositions()));
            App.Messenger.Register("UpdateExposition", (Action<Exposition>)(param => UpdateExposition(param)));
            App.Messenger.Register("DeleteExposition", (Action)(() => DeleteExposition()));
            App.Messenger.Register("AddExposition", (Action<Exposition>)(param => AddExposition(param)));
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

