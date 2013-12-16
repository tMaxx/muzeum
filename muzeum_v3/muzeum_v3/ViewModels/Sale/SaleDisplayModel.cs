using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MvvmFoundation.Wpf;
using System.ComponentModel;
using System.Windows.Input;
using System.Windows.Media;
using muzeum_v3.ViewModels.Exposition;

namespace muzeum_v3.ViewModels.Sale
{
    class SaleDisplayModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, e);
        }

        private readonly SaleDisplayStatusModel stat = new SaleDisplayStatusModel();
        public SaleDisplayStatusModel Stat { get { return stat; } }
        private Sale displayedSale = new Sale();


        private RelayCommand addCommand;

        public Sale DisplayedSale
        {
            get { return displayedSale; }
            set { displayedSale = value; OnPropertyChanged(new PropertyChangedEventArgs("DisplayedSale")); }
        }

        public ICommand AddCommand
        {
            get { return addCommand ?? (addCommand = new RelayCommand(() => AddSale())); }
        }

        private void AddSale()
        {
            if (!stat.CheckSaleForAdd(DisplayedSale)) return;
            for (int i = 0; i < DisplayedSale.NumberOfTickets; i++) if (!App.SaleQuery.AddSale(DisplayedSale))
                {
                    stat.Status = App.SaleQuery.errorMessage;
                    return;
                }
            App.Messenger.NotifyColleagues("AddSale", DisplayedSale);
        }

        public SaleDisplayModel()
        {
            Messenger messenger = App.Messenger;
            messenger.Register("ExpositionSelectionChanged", (Action<Exposition.Exposition>)(param => ChangeExpositionName(param)));
            messenger.Register("SetStatus", (Action<String>)(param => stat.Status = param));
            messenger.Register("TicketSelectionChanged", (Action<Ticket.Ticket>)(param => ChangeTicketName(param)));
        }

        public void ChangeTicketName(Ticket.Ticket e)
        {
            if (e == null) return;
            Sale temp = new Sale();
            temp.ExpositionName = DisplayedSale.ExpositionName;
            temp.NameOfTicket = e.NameOfTicket;
            temp.PriceOfTicket = e.PriceOfTicket;
            DisplayedSale = temp;
            stat.clearStatus();
        }

        public void ChangeExpositionName(Exposition.Exposition e)
        {
            if (e == null) return;
            Sale temp = new Sale();
            temp.ExpositionName = e.ExpositionName;
            DisplayedSale = temp;
            stat.clearStatus();
        }
        //public void ProcessSale(Sale p)
        // {
        //   if (p == null)
        //   {
        //      isSelected = false; return;
        //  }
        ///  Sale temp = new Sale();
        //  temp.CopySale(p);
        //  DisplayedSale = temp;
        //  isSelected = true;
        //  stat.clearStatus();
        // }
    }
}
