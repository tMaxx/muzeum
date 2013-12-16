using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MvvmFoundation.Wpf;
using System.ComponentModel;
using System.Windows.Input;
using System.Windows.Media;
using muzeum_v3.ViewModels.Hall;

namespace muzeum_v3.ViewModels.Hall
{
    class HallDisplayModel : INotifyPropertyChanged
    {
         private bool isSelected = false;

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, e);
        }

        private readonly HallDisplayStatusModel stat = new HallDisplayStatusModel();
        public HallDisplayStatusModel Stat { get { return stat; } }
        private Hall displayedHall = new Hall();
       
        private RelayCommand getHallsCommand;
        private RelayCommand clearCommand;
        private RelayCommand updateCommand;
        private RelayCommand addCommand;

        public Hall DisplayedHall
        {
            get { return displayedHall; }
            set { displayedHall = value; OnPropertyChanged(new PropertyChangedEventArgs("DisplayedHall")); }
        }

        public ICommand GetHallsCommand
        {
            get { return getHallsCommand ?? (getHallsCommand = new RelayCommand(() => GetHalls())); }
        }

        private void GetHalls()
        {
            isSelected = false;
            stat.clearStatus();
            DisplayedHall = new Hall();
          //  App.Messenger.NotifyColleagues("GetHalls");
        }

        public ICommand ClearCommand
        {
            get { return clearCommand ?? (clearCommand = new RelayCommand(() => ClearHallDisplay())); }
        }

        private void ClearHallDisplay()
        {
            isSelected = false;
            stat.clearStatus();
            DisplayedHall = new Hall();
            App.Messenger.NotifyColleagues("HallCleared");
        } 

        public ICommand UpdateCommand
        {
            get { return updateCommand ?? (updateCommand = new RelayCommand(() => UpdateHall(), ()=>isSelected)); }
        }

        private void UpdateHall()
        {
            if (!stat.CheckHallForAdd(DisplayedHall)) return;
            if (!App.HallQuery.UpdateHall(DisplayedHall))
                {
                    stat.Status = App.HallQuery.errorMessage;
                    return;
                }
                App.Messenger.NotifyColleagues("UpdateHall", DisplayedHall);
        }

        public ICommand AddCommand
        {
            get { return addCommand ?? (addCommand = new RelayCommand(() => AddHall(), () => !isSelected)); }
        }

        private void AddHall()
        {
            if (!stat.CheckHallForAdd(DisplayedHall)) return;
            if (!App.HallQuery.AddHall(DisplayedHall))
            {
                stat.Status = App.HallQuery.errorMessage;
                return;
            }
            App.Messenger.NotifyColleagues("AddHall", DisplayedHall);
        }

        public HallDisplayModel()
        {
            Messenger messenger = App.Messenger;
            messenger.Register("HallSelectionChanged", (Action<Hall>)(param => ProcessHall(param)));
            messenger.Register("SetStatus", (Action<String>)(param => stat.Status = param));
            messenger.Register("Clear", (Action)(() => ClearHallDisplay()));
        }

        public void ProcessHall(Hall p)
        {
            if (p == null) {
                isSelected = false;  return; 
            }
            Hall temp = new Hall();
            temp.CopyHall(p);
            DisplayedHall = temp;
            isSelected = true;
            stat.clearStatus();
        } 
    }
}
