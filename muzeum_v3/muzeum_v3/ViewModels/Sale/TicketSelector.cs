using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Windows.Input;
using System.Linq;
using System.Text;
using System.ComponentModel;
using MvvmFoundation.Wpf;

namespace muzeum_v3.ViewModels.Ticket
{
    class TicketSelector : INotifyPropertyChanged
    {
        public TicketSelector()
        {
            dataItems = new MyObservableCollection<Ticket>();
            DataItems = App.SaleQuery.GetTickets();
            listBoxCommand = new RelayCommand(() => SelectionHasChanged());
            App.Messenger.Register("GetTickets", (Action)(() => GetTickets()));
        }

        private void GetTickets()
        {
            DataItems = App.SaleQuery.GetTickets();
            if (App.SaleQuery.hasError)
                App.Messenger.NotifyColleagues("SetStatus", App.SaleQuery.errorMessage);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, e);
        }

        private MyObservableCollection<Ticket> dataItems;
        public MyObservableCollection<Ticket> DataItems
        {
            get { return dataItems; }
            //If dataItems replaced by new collection, WPF must be told
            set { dataItems = value; OnPropertyChanged(new PropertyChangedEventArgs("DataItems")); }
        }

        private Ticket selectedTicket;
        public Ticket SelectedTicket
        {
            get { return selectedTicket; }
            set { selectedTicket = value; OnPropertyChanged(new PropertyChangedEventArgs("SelectedTicket")); }
        }

        private RelayCommand listBoxCommand;
        public ICommand ListBoxCommand
        {
            get { return listBoxCommand; }
        }

        private void SelectionHasChanged()
        {
            Messenger messenger = App.Messenger;
            messenger.NotifyColleagues("TicketSelectionChanged", selectedTicket);
        }
    }


    public class MyObservableCollection<Ticket> : ObservableCollection<Ticket>
    {
        public void UpdateCollection()
        {
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(
                                NotifyCollectionChangedAction.Reset));
        }


        public void ReplaceItem(int index, Ticket item)
        {
            base.SetItem(index, item);
        }

    }
}
