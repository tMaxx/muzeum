using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MvvmFoundation.Wpf;
using System.ComponentModel;
using System.Windows.Input;
using System.Windows.Media;

namespace muzeum_v3.ViewModels.Author
{
    public class AuthorSearch : INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, e);
        }

        private RelayCommand useSuperQueryAuthorCommand;
        public ICommand UseSuperQueryAuthorCommand
        {
            get { return useSuperQueryAuthorCommand ?? (useSuperQueryAuthorCommand = new RelayCommand(() => UseSuperQueryAuthor())); }
        }

        private AuthorParameters parameters = new AuthorParameters();
        public AuthorParameters Parameters
        {
            get { return parameters; }
            set { parameters = value; OnPropertyChanged(new PropertyChangedEventArgs("Parameters")); }
        }

        private void UseSuperQueryAuthor()
        {
            App.Messenger.NotifyColleagues("UseSuperQueryAuthor", Parameters);
        }


        private void Clear()
        {
            Parameters = new AuthorParameters();
        }

        public AuthorSearch()
        {
            Messenger messenger = App.Messenger;
            messenger.Register("Clear", (Action)(() => Clear()));

        }
    }

    public class AuthorParameters
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, e);
        }
        private string authorNameParameter;
        private string birthFROM;
        private string birthTO;
        private string deathFROM;
        private string deathTO;

        public string AuthorNameParameter
        {
            get { return authorNameParameter; }
            set
            {
                authorNameParameter = value; OnPropertyChanged(new PropertyChangedEventArgs("AuthorNameParameter"));
            }
        }

        public string BirthFROM
        {
            get { return birthFROM; }
            set
            {
                birthFROM = value; OnPropertyChanged(new PropertyChangedEventArgs("BirthFROM"));
            }
        }

        public string BirthTO
        {
            get { return birthTO; }
            set
            {
                birthTO = value; OnPropertyChanged(new PropertyChangedEventArgs("BirthTO"));
            }
        }

        public string DeathFROM
        {
            get { return deathFROM; }
            set
            {
                deathFROM = value; OnPropertyChanged(new PropertyChangedEventArgs("DeathFROM"));
            }
        }

        public string DeathTO
        {
            get { return deathTO; }
            set
            {
                deathTO = value; OnPropertyChanged(new PropertyChangedEventArgs("DeathTO"));
            }
        }

    }
}
