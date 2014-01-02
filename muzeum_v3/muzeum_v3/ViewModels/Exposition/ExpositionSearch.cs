using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MvvmFoundation.Wpf;
using System.ComponentModel;
using System.Windows.Input;
using System.Windows.Media;

namespace muzeum_v3.ViewModels.Exposition
{
    public class ExpositionSearch : INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, e);
        }

        private RelayCommand useSuperQueryExpositionCommand;
        public ICommand UseSuperQueryExpositionCommand
        {
            get { return useSuperQueryExpositionCommand ?? (useSuperQueryExpositionCommand = new RelayCommand(() => UseSuperQueryExposition())); }
        }

        private ExpositionParameters parameters = new ExpositionParameters();
        public ExpositionParameters Parameters
        {
            get { return parameters; }
            set { parameters = value; OnPropertyChanged(new PropertyChangedEventArgs("Parameters")); }
        }

        private void UseSuperQueryExposition()
        {
            App.Messenger.NotifyColleagues("UseSuperQueryExposition", Parameters);
        }


        private void Clear()
        {
            Parameters = new ExpositionParameters();
        }

        public ExpositionSearch()
        {
            Messenger messenger = App.Messenger;
            messenger.Register("Clear", (Action)(() => Clear()));

        }
    }

    public class ExpositionParameters
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, e);
        }
        private string expositionNameParameter;
        private string org;
        private string location;
        private int numberOfTicketsFROM;
        private int numberOfTicketsTO;
        private decimal profitFROM;
        private decimal profitTO;


        public int NumberOfTicketsFROM
        {
            get { return numberOfTicketsFROM; }
            set
            {
                numberOfTicketsFROM = value; OnPropertyChanged(new PropertyChangedEventArgs("NumberOfTicketsFROM"));
            }
        }

        public int NumberOfTicketsTO
        {
            get { return numberOfTicketsTO; }
            set
            {
                numberOfTicketsTO = value; OnPropertyChanged(new PropertyChangedEventArgs("NumberOfTicketsTO"));
            }
        }

        public decimal ProfitFROM
        {
            get { return profitFROM; }
            set
            {
                profitFROM = value; OnPropertyChanged(new PropertyChangedEventArgs("ProfitFROM"));
            }
        }

        public decimal ProfitTO
        {
            get { return profitTO; }
            set
            {
                profitTO = value; OnPropertyChanged(new PropertyChangedEventArgs("ProfitTO"));
            }
        }

        public string ExpositionNameParameter
        {
            get { return expositionNameParameter; }
            set
            {
                expositionNameParameter = value; OnPropertyChanged(new PropertyChangedEventArgs("ExpositionNameParameter"));
            }
        }

        public string Org
        {
            get { return org; }
            set
            {
                org = value; OnPropertyChanged(new PropertyChangedEventArgs("Org"));
            }
        }

        public string Location
        {
            get { return location; }
            set
            {
                location = value; OnPropertyChanged(new PropertyChangedEventArgs("Location"));
            }
        }

    }
}
