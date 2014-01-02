using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Windows.Input;
using System.Linq;
using System.Text;
using System.ComponentModel;
using MvvmFoundation.Wpf;

namespace muzeum_v3.ViewModels.Presentation
{
    class PresentationSelector : INotifyPropertyChanged
    {
        public PresentationSelector()
        {
            dataItems = new MyObservableCollection<Presentation>();
            listBoxCommand = new RelayCommand(() => SelectionHasChanged());
            App.Messenger.Register("PresentationCleared", (Action)(() => SelectedPresentation = null));
            App.Messenger.Register("ExhibitSelectionChanged", (Action<Exhibit.Exhibit>)(param => GetPresentations(param)));
            App.Messenger.Register("UpdatePresentation", (Action<Presentation>)(param => UpdatePresentation(param)));
            App.Messenger.Register("AddPresentation", (Action<Presentation>)(param => AddPresentation(param)));
            App.Messenger.Register("DeletePresentation", (Action)(() => DeletePresentation()));

        }

        private void DeletePresentation()
        {
            dataItems.Remove(SelectedPresentation);
        }

        private void AddPresentation(Presentation e)
        {
            dataItems.Add(e);
        }

        private void GetPresentations(Exhibit.Exhibit e)
        {
            if (e == null) return;
            DataItems = App.LinqPresentation.GetPresentations(e.ExhibitId);
            if (App.LinqPresentation.hasError)
                App.Messenger.NotifyColleagues("SetStatus", App.LinqPresentation.errorMessage);
        }

        private void UpdatePresentation(Presentation e)
        {
            int index = dataItems.IndexOf(selectedPresentation);
            dataItems.ReplaceItem(index, e);
            SelectedPresentation = e;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, e);
        }

        private MyObservableCollection<Presentation> dataItems;
        public MyObservableCollection<Presentation> DataItems
        {
            get { return dataItems; }
            //If dataItems replaced by new collection, WPF must be told
            set { dataItems = value; OnPropertyChanged(new PropertyChangedEventArgs("DataItems")); }
        }

        private Presentation selectedPresentation;
        public Presentation SelectedPresentation
        {
            get { return selectedPresentation; }
            set { selectedPresentation = value; OnPropertyChanged(new PropertyChangedEventArgs("SelectedPresentation")); }
        }

        private RelayCommand listBoxCommand;
        public ICommand ListBoxCommand
        {
            get { return listBoxCommand; }
        }

        private void SelectionHasChanged()
        {
            Messenger messenger = App.Messenger;
            messenger.NotifyColleagues("PresentationSelectionChanged", selectedPresentation);
        }
    }


    public class MyObservableCollection<Presentation> : ObservableCollection<Presentation>
    {
        public void UpdateCollection()
        {
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(
                                NotifyCollectionChangedAction.Reset));
        }


        public void ReplaceItem(int index, Presentation item)
        {
            base.SetItem(index, item);
        }

    } // class MyObservableCollection
}
