using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Windows.Input;
using System.Linq;
using System.Text;
using System.ComponentModel;
using MvvmFoundation.Wpf;

namespace muzeum_v3.ViewModels.Author
{
    class AuthorSelector : INotifyPropertyChanged
    {
        public AuthorSelector()
        {
            dataItems = new MyObservableCollection<Author>();
            DataItems = App.AuthorQuery.GetAuthors();
            listBoxCommand = new RelayCommand(() => SelectionHasChanged());
            App.Messenger.Register("AuthorCleared", (Action)(() => SelectedAuthor = null));
            App.Messenger.Register("GetAuthors", (Action)(() => GetAuthors()));
            App.Messenger.Register("UpdateAuthor", (Action<Author>)(param => UpdateAuthor(param)));
            App.Messenger.Register("AddAuthor", (Action<Author>)(param => AddAuthor(param)));
        }

        private void AddAuthor(Author e)
        {
            dataItems.Add(e);
        }

        private void GetAuthors()
        {
            DataItems = App.AuthorQuery.GetAuthors();
            if (App.AuthorQuery.hasError)
                App.Messenger.NotifyColleagues("SetStatus", App.AuthorQuery.errorMessage);
        }
       
        private void UpdateAuthor(Author e)
        {
            int index = dataItems.IndexOf(selectedAuthor);
            dataItems.ReplaceItem(index, e);
            SelectedAuthor = e;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, e);
        }

        private MyObservableCollection<Author> dataItems;
        public MyObservableCollection<Author> DataItems
        {
            get { return dataItems; }
            //If dataItems replaced by new collection, WPF must be told
            set { dataItems = value; OnPropertyChanged(new PropertyChangedEventArgs("DataItems")); }
        }

        private Author selectedAuthor;
        public Author SelectedAuthor
        {
            get { return selectedAuthor; }
            set { selectedAuthor = value; OnPropertyChanged(new PropertyChangedEventArgs("SelectedAuthor")); }
        }

        private RelayCommand listBoxCommand;
        public ICommand ListBoxCommand
        {
            get { return listBoxCommand; }
        }

        private void SelectionHasChanged()
        {
            Messenger messenger = App.Messenger;
            messenger.NotifyColleagues("AuthorSelectionChanged", selectedAuthor);
        }
    }


    public class MyObservableCollection<Author> : ObservableCollection<Author>
    {
        public void UpdateCollection()
        {
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(
                                NotifyCollectionChangedAction.Reset));
        }


        public void ReplaceItem(int index, Author item)
        {
             base.SetItem(index, item);      
        }

    } // class MyObservableCollection
}
