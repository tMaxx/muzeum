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
    class ExpositionDisplayModel : INotifyPropertyChanged
    {
         private bool isSelected = false;

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, e);
        }

        private readonly ExpositionDisplayStatusModel stat = new ExpositionDisplayStatusModel();
        public ExpositionDisplayStatusModel Stat { get { return stat; } }
        private Exposition displayedExposition = new Exposition();
       
        private RelayCommand getExpositionsCommand;
        private RelayCommand clearCommand;
        private RelayCommand updateCommand;
        private RelayCommand deleteCommand;
        private RelayCommand addCommand;

        public Exposition DisplayedExposition
        {
            get { return displayedExposition; }
            set { displayedExposition = value; OnPropertyChanged(new PropertyChangedEventArgs("DisplayedExposition")); }
        }

        public ICommand GetExpositionsCommand
        {
            get { return getExpositionsCommand ?? (getExpositionsCommand = new RelayCommand(() => GetExpositions())); }
        }

        private void GetExpositions()
        {
            isSelected = false;
            stat.clearStatus();
            DisplayedExposition = new Exposition();
            App.Messenger.NotifyColleagues("GetExpositions");
        }

        public ICommand ClearCommand
        {
            get { return clearCommand ?? (clearCommand = new RelayCommand(() => ClearExpositionDisplay())); }
        }

        private void ClearExpositionDisplay()
        {
            isSelected = false;
            stat.clearStatus();
            DisplayedExposition = new Exposition();
            App.Messenger.NotifyColleagues("ExpositionCleared");
        } 

        public ICommand UpdateCommand
        {
            get { return updateCommand ?? (updateCommand = new RelayCommand(() => UpdateExposition(), ()=>isSelected)); }
        }

        private void UpdateExposition()
        {
            if (!stat.CheckExpositionForAdd(DisplayedExposition)) return;
                if(!App.ExpositionQuery.UpdateExposition(DisplayedExposition))
                {
                    stat.Status = App.ExpositionQuery.errorMessage;
                    return;
                }
                App.Messenger.NotifyColleagues("UpdateExposition", DisplayedExposition);
        } 

        public ICommand DeleteCommand
        {
            get { return deleteCommand ?? (deleteCommand = new RelayCommand(() => DeleteExposition(), () => isSelected)); }
        }

        private void DeleteExposition()
        {
            if (!App.ExpositionQuery.DeleteExposition(DisplayedExposition.ExpositionId))
            {
                stat.Status = App.ExpositionQuery .errorMessage;
                return;
            }
            isSelected = false;
            App.Messenger.NotifyColleagues("DeleteExposition");
        }

        public ICommand AddCommand
        {
            get { return addCommand ?? (addCommand = new RelayCommand(() => AddExposition(), () => !isSelected)); }
        }

        private void AddExposition()
        {
            if (!stat.CheckExpositionForAdd(DisplayedExposition)) return;
            if (!App.ExpositionQuery.AddExposition(DisplayedExposition))
            {
                stat.Status = App.ExpositionQuery.errorMessage;
                return;
            }
            App.Messenger.NotifyColleagues("AddExposition", DisplayedExposition);
        }

        public ExpositionDisplayModel()
        {
            Messenger messenger = App.Messenger;
            messenger.Register("AddSale", (Action<Sale.Sale>)(param => Sold(param)));
            messenger.Register("ChangeLocationName", (Action<String>)(param => ChangeLocationName(param)));
            messenger.Register("ChangeOrgName", (Action<String>)(param => ChangeOrgName(param)));
            messenger.Register("ExpositionSelectionChanged", (Action<Exposition>)(param => ProcessExposition(param)));
            messenger.Register("SetStatus", (Action<String>)(param => stat.Status = param));
            messenger.Register("Clear", (Action)(()=>ClearExpositionDisplay()));
        }

        public void Sold(Sale.Sale p)
        {
            if (p == null)
            {
                isSelected = false; return;
            }
            Exposition temp = new Exposition();
            temp.CopyExposition(DisplayedExposition);
            temp.NumberOfTickets += p.NumberOfTickets;
            temp.Profit += p.PriceOfTicket * p.NumberOfTickets;
            DisplayedExposition = temp;
            isSelected = true;
            stat.clearStatus();
        }

        public void ProcessExposition(Exposition p)
        {
            if (p == null) {
                isSelected = false;  return; 
            }
            Exposition temp = new Exposition();
            temp.CopyExposition(p);
            DisplayedExposition = temp;
            isSelected = true;
            stat.clearStatus();
        }

        public void ChangeLocationName(string str)
        {
            if (str != null)
            {
                Exposition temp = new Exposition();
                temp = DisplayedExposition;
                temp.LocationName = str;
                DisplayedExposition = temp;
                //  isSelected = true;
                stat.clearStatus();
            }
        }
        public void ChangeOrgName(string str)
        {
            if (str != null)
            {
                Exposition temp = new Exposition();
                temp = DisplayedExposition;
                temp.OrganizerName = str;
                DisplayedExposition = temp;
                //    isSelected = true;
                stat.clearStatus();
            }
        } 
    }
}
