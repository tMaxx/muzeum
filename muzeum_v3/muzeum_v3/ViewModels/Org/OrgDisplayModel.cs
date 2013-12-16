using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MvvmFoundation.Wpf;
using System.ComponentModel;
using System.Windows.Input;
using System.Windows.Media;

namespace muzeum_v3.ViewModels.Org
{
    class OrgDisplayModel : INotifyPropertyChanged
    {
        private bool isSelected = false;

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, e);
        }

        private readonly OrgDisplayStatusModel stat = new OrgDisplayStatusModel();
        public OrgDisplayStatusModel Stat { get { return stat; } }
        private Org displayedOrg = new Org();

        private RelayCommand getOrgsCommand;
        private RelayCommand clearCommand;
        private RelayCommand updateCommand;
        private RelayCommand addCommand;
        private RelayCommand sendOrgNameCommand;

        public ICommand SendOrgNameCommand
        {
            get { return sendOrgNameCommand ?? (sendOrgNameCommand = new RelayCommand(() => ChangeOrgName())); }
        }

        private void ChangeOrgName()
        {
            if (!stat.CheckOrgForAdd(DisplayedOrg)) return;
            App.Messenger.NotifyColleagues("ChangeOrgName", DisplayedOrg.OrgName);
        }
        public Org DisplayedOrg
        {
            get { return displayedOrg; }
            set { displayedOrg = value; OnPropertyChanged(new PropertyChangedEventArgs("DisplayedOrg")); }
        }

        public ICommand GetOrgsCommand
        {
            get { return getOrgsCommand ?? (getOrgsCommand = new RelayCommand(() => GetOrgs())); }
        }

        private void GetOrgs()
        {
            isSelected = false;
            stat.clearStatus();
            DisplayedOrg = new Org();
            App.Messenger.NotifyColleagues("GetOrgs");
        }

        public ICommand ClearCommand
        {
            get { return clearCommand ?? (clearCommand = new RelayCommand(() => ClearOrgDisplay())); }
        }

        private void ClearOrgDisplay()
        {
            isSelected = false;
            stat.clearStatus();
            DisplayedOrg = new Org();
            App.Messenger.NotifyColleagues("OrgCleared");
        }

        public ICommand UpdateCommand
        {
            get { return updateCommand ?? (updateCommand = new RelayCommand(() => UpdateOrg(), () => isSelected)); }
        }

        private void UpdateOrg()
        {
            if (!stat.CheckOrgForAdd(DisplayedOrg)) return;
            if (!App.OrgQuery.UpdateOrg(DisplayedOrg))
            {
                stat.Status = App.OrgQuery.errorMessage;
                return;
            }
            App.Messenger.NotifyColleagues("UpdateOrg", DisplayedOrg);
        }

        public ICommand AddCommand
        {
            get { return addCommand ?? (addCommand = new RelayCommand(() => AddOrg(), () => !isSelected)); }
        }

        private void AddOrg()
        {
            if (!stat.CheckOrgForAdd(DisplayedOrg)) return;
            if (!App.OrgQuery.AddOrg(DisplayedOrg))
            {
                stat.Status = App.OrgQuery.errorMessage;
                return;
            }
            App.Messenger.NotifyColleagues("AddOrg", DisplayedOrg);
        }

        public OrgDisplayModel()
        {
            Messenger messenger = App.Messenger;
            messenger.Register("OrgSelectionChanged", (Action<Org>)(param => ProcessOrg(param)));
            messenger.Register("SetStatus", (Action<String>)(param => stat.Status = param));
            messenger.Register("Clear", (Action)(() => ClearOrgDisplay()));
        }

        public void ProcessOrg(Org p)
        {
            if (p == null)
            {
                isSelected = false; return;
            }
            Org temp = new Org();
            temp.CopyOrg(p);
            DisplayedOrg = temp;
            isSelected = true;
            stat.clearStatus();
        }
    }
}
