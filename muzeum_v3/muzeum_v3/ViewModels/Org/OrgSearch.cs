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
    public class OrgSearch : INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, e);
        }

        private RelayCommand useSuperQueryOrgCommand;
        public ICommand UseSuperQueryOrgCommand
        {
            get { return useSuperQueryOrgCommand ?? (useSuperQueryOrgCommand = new RelayCommand(() => UseSuperQueryOrg())); }
        }

        private OrgParameters parameters = new OrgParameters();
        public OrgParameters Parameters
        {
            get { return parameters; }
            set { parameters = value; OnPropertyChanged(new PropertyChangedEventArgs("Parameters")); }
        }

        private void UseSuperQueryOrg()
        {
            App.Messenger.NotifyColleagues("UseSuperQueryOrg", Parameters);
        }


        private void Clear()
        {
            Parameters = new OrgParameters();
        }

        public OrgSearch()
        {
            Messenger messenger = App.Messenger;
            messenger.Register("Clear", (Action)(() => Clear()));

        }
    }

    public class OrgParameters
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, e);
        }
        private string orgNameParameter;
        private string city;

        public string OrgNameParameter
        {
            get { return orgNameParameter; }
            set
            {
                orgNameParameter = value; OnPropertyChanged(new PropertyChangedEventArgs("OrgNameParameter"));
            }
        }

        public string City
        {
            get { return city; }
            set
            {
                city = value; OnPropertyChanged(new PropertyChangedEventArgs("City"));
            }
        }
    }
}
