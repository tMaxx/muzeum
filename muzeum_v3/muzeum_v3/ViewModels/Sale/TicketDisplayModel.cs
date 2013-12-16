using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MvvmFoundation.Wpf;
using System.ComponentModel;
using System.Windows.Input;
using System.Windows.Media;

namespace muzeum_v3.ViewModels.Ticket
{
    class TicketDisplayModel : INotifyPropertyChanged
    {
        private bool isSelected = false;

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, e);
        }

        private Ticket displayedTicket = new Ticket();

        private RelayCommand getTicketsCommand;
        
        public Ticket DisplayedTicket
        {
            get { return displayedTicket; }
            set { displayedTicket = value; OnPropertyChanged(new PropertyChangedEventArgs("DisplayedTicket")); }
        }

        public ICommand GetTicketsCommand
        {
            get { return getTicketsCommand ?? (getTicketsCommand = new RelayCommand(() => GetTickets())); }
        }

        private void GetTickets()
        {
            isSelected = false;
            DisplayedTicket = new Ticket();
            App.Messenger.NotifyColleagues("GetTickets");
        }

        public TicketDisplayModel()
        {
            Messenger messenger = App.Messenger;
            messenger.Register("TicketSelectionChanged", (Action<Ticket>)(param => ProcessTicket(param)));
        }

        public void ProcessTicket(Ticket p)
        {
            if (p == null)
            {
                isSelected = false; return;
            }
            Ticket temp = new Ticket();
            temp.CopyTicket(p);
            DisplayedTicket = temp;
            isSelected = true;
        }
    }
}
