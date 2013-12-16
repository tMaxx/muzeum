using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MvvmFoundation.Wpf;
using System.ComponentModel;
using System.Windows.Input;
using System.Windows.Media;

namespace muzeum_v3.ViewModels.Exhibit
{
    public class ExhibitDisplayModel : INotifyPropertyChanged
    {
        private bool isSelected = false;

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, e);
        }

        private readonly ExhibitDisplayStatusModel stat = new ExhibitDisplayStatusModel();
        public ExhibitDisplayStatusModel Stat { get { return stat; } }

        private Exhibit displayedExhibit = new Exhibit();
       
        private RelayCommand getExhibitsCommand;
        private RelayCommand clearCommand;
        private RelayCommand updateCommand;
        private RelayCommand deleteCommand;
        private RelayCommand addCommand;

        public Exhibit DisplayedExhibit
        {
            get { return displayedExhibit; }
            set { displayedExhibit = value; OnPropertyChanged(new PropertyChangedEventArgs("DisplayedExhibit")); }
        }

        public ICommand GetExhibitsCommand
        {
            get { return getExhibitsCommand ?? (getExhibitsCommand = new RelayCommand(() => GetExhibits())); }
        }

        private void GetExhibits()
        {
            isSelected = false;
            stat.clearStatus();
            DisplayedExhibit = new Exhibit();
            App.Messenger.NotifyColleagues("GetExhibits");
        }

        public ICommand ClearCommand
        {
            get { return clearCommand ?? (clearCommand = new RelayCommand(() => ClearExhibitDisplay())); }
        }

        private void ClearExhibitDisplay()
        {
            isSelected = false;
            stat.clearStatus();
            DisplayedExhibit = new Exhibit();
            App.Messenger.NotifyColleagues("ExhibitCleared");
        } 

        public ICommand UpdateCommand
        {
            get { return updateCommand ?? (updateCommand = new RelayCommand(() => UpdateExhibit(), ()=>isSelected)); }
        }

        private void UpdateExhibit()
        {
            if (!stat.CheckExhibitForAdd(DisplayedExhibit)) return;
                if(!App.ExhibitQuery.UpdateExhibit(DisplayedExhibit))
                {
                    stat.Status = App.ExhibitQuery.errorMessage;
                    return;
                }
                App.Messenger.NotifyColleagues("UpdateExhibit", DisplayedExhibit);
        } 

        public ICommand DeleteCommand
        {
            get { return deleteCommand ?? (deleteCommand = new RelayCommand(() => DeleteExhibit(), () => isSelected)); }
        }

        private void DeleteExhibit()
        {
            if (!App.ExhibitQuery.DeleteExhibit(DisplayedExhibit.ExhibitId))
            {
                stat.Status = App.ExhibitQuery  .errorMessage;
                return;
            }
            isSelected = false;
            App.Messenger.NotifyColleagues("DeleteExhibit");
        }

        public ICommand AddCommand
        {
            get { return addCommand ?? (addCommand = new RelayCommand(() => AddExhibit(), () => !isSelected)); }
        }

        private void AddExhibit()
        {
            if (!stat.CheckExhibitForAdd(DisplayedExhibit)) return;
            if (!App.ExhibitQuery.AddExhibit(DisplayedExhibit))
            {
                stat.Status = App.ExhibitQuery.errorMessage;
                return;
            }
            App.Messenger.NotifyColleagues("AddExhibit", DisplayedExhibit);
        }

        public ExhibitDisplayModel()
        {
            Messenger messenger = App.Messenger;
            messenger.Register("ChangeAuthorName", (Action<String>)(param => ChangeAuthorName(param)));
            messenger.Register("ChangeOwnerName", (Action<String>)(param => ChangeOwnerName(param)));
            messenger.Register("ExhibitSelectionChanged", (Action<Exhibit>)(param => ProcessExhibit(param)));
            messenger.Register("SetStatus", (Action<String>)(param => stat.Status = param));
            messenger.Register("Clear", (Action)(() => ClearExhibitDisplay()));
        }
       
        public void ProcessExhibit(Exhibit p)
        {
            if (p == null) {
                isSelected = false;  return; 
            }
            Exhibit temp = new Exhibit();
            temp.CopyExhibit(p);
            DisplayedExhibit = temp;
            isSelected = true;
            stat.clearStatus();
        }

        public void ChangeAuthorName(string str)
        {
            if (str != null)
            {
                Exhibit temp = new Exhibit();
                temp = DisplayedExhibit;
                temp.Author = str;
                DisplayedExhibit = temp;
              //  isSelected = true;
                stat.clearStatus();
            }
        }

        public void ChangeOwnerName(string str)
        {
            if (str != null)
            {
                Exhibit temp = new Exhibit();
                temp = DisplayedExhibit;
                temp.Owner = str;
                DisplayedExhibit = temp;
            //    isSelected = true;
                stat.clearStatus();
            }
        } 
    }
}
