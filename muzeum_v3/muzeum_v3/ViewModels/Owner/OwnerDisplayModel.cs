using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MvvmFoundation.Wpf;
using System.ComponentModel;
using System.Windows.Input;
using System.Windows.Media;

namespace muzeum_v3.ViewModels.Owner
{
    class OwnerDisplayModel : INotifyPropertyChanged
    {
        private bool isSelected = false;

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, e);
        }

        private readonly OwnerDisplayStatusModel stat = new OwnerDisplayStatusModel();
        public OwnerDisplayStatusModel Stat { get { return stat; } }
        private Owner displayedOwner = new Owner();
       
        private RelayCommand getOwnersCommand;
        private RelayCommand clearCommand;
        private RelayCommand updateCommand;
        private RelayCommand addCommand;
        private RelayCommand sendOwnerNameCommand;

        public ICommand SendOwnerNameCommand
        {
            get { return sendOwnerNameCommand ?? (sendOwnerNameCommand = new RelayCommand(() => ChangeOwnerName())); }
        }

        private void ChangeOwnerName()
        {
            if (!stat.CheckOwnerForAdd(DisplayedOwner)) return;
            App.Messenger.NotifyColleagues("ChangeOwnerName", DisplayedOwner.OwnerName);
        }
        public Owner DisplayedOwner
        {
            get { return displayedOwner; }
            set { displayedOwner = value; OnPropertyChanged(new PropertyChangedEventArgs("DisplayedOwner")); }
        }

        public ICommand GetOwnersCommand
        {
            get { return getOwnersCommand ?? (getOwnersCommand = new RelayCommand(() => GetOwners())); }
        }

        private void GetOwners()
        {
            isSelected = false;
            stat.clearStatus();
            DisplayedOwner = new Owner();
            App.Messenger.NotifyColleagues("GetOwners");
        }

        public ICommand ClearCommand
        {
            get { return clearCommand ?? (clearCommand = new RelayCommand(() => ClearOwnerDisplay())); }
        }

        private void ClearOwnerDisplay()
        {
            isSelected = false;
            stat.clearStatus();
            DisplayedOwner = new Owner();
            App.Messenger.NotifyColleagues("OwnerCleared");
        } 

        public ICommand UpdateCommand
        {
            get { return updateCommand ?? (updateCommand = new RelayCommand(() => UpdateOwner(), ()=>isSelected)); }
        }

        private void UpdateOwner()
        {
            if (!stat.CheckOwnerForAdd(DisplayedOwner)) return;
            if (!App.OwnerQuery.UpdateOwner(DisplayedOwner))
                {
                    stat.Status = App.OwnerQuery.errorMessage;
                    return;
                }
                App.Messenger.NotifyColleagues("UpdateOwner", DisplayedOwner);
        }

        public ICommand AddCommand
        {
            get { return addCommand ?? (addCommand = new RelayCommand(() => AddOwner(), () => !isSelected)); }
        }

        private void AddOwner()
        {
            if (!stat.CheckOwnerForAdd(DisplayedOwner)) return;
            if (!App.OwnerQuery.AddOwner(DisplayedOwner))
            {
                stat.Status = App.OwnerQuery.errorMessage;
                return;
            }
            App.Messenger.NotifyColleagues("AddOwner", DisplayedOwner);
        }

        public OwnerDisplayModel()
        {
            Messenger messenger = App.Messenger;
            messenger.Register("OwnerSelectionChanged", (Action<Owner>)(param => ProcessOwner(param)));
            messenger.Register("SetStatus", (Action<String>)(param => stat.Status = param));
            messenger.Register("Clear", (Action)(() => ClearOwnerDisplay()));
        }

        public void ProcessOwner(Owner p)
        {
            if (p == null) {
                isSelected = false;  return; 
            }
            Owner temp = new Owner();
            temp.CopyOwner(p);
            DisplayedOwner = temp;
            isSelected = true;
            stat.clearStatus();
        } 
    }
}
