using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MvvmFoundation.Wpf;
using System.ComponentModel;
using System.Windows.Input;
using System.Windows.Media;
using muzeum_v3.ViewModels.Presentation;
using muzeum_v3.ViewModels.Exhibit;

namespace muzeum_v3.ViewModels.Presentation
{
    class PresentationDisplayModel : INotifyPropertyChanged
    {
        private bool isSelected = false;

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, e);
        }

        private readonly PresentationDisplayStatusModel stat = new PresentationDisplayStatusModel();
        public PresentationDisplayStatusModel Stat { get { return stat; } }
        private Presentation displayedPresentation = new Presentation();

        private RelayCommand getPresentationsCommand;
        private RelayCommand clearCommand;
        private RelayCommand updateCommand;
        private RelayCommand addCommand;

        public Presentation DisplayedPresentation
        {
            get { return displayedPresentation; }
            set { displayedPresentation = value; OnPropertyChanged(new PropertyChangedEventArgs("DisplayedPresentation")); }
        }

        public ICommand GetPresentationsCommand
        {
            get { return getPresentationsCommand ?? (getPresentationsCommand = new RelayCommand(() => GetPresentations())); }
        }

        private void GetPresentations()
        {
            isSelected = false;
            stat.clearStatus();
            DisplayedPresentation = new Presentation();
            App.Messenger.NotifyColleagues("GetPresentations");
        }

        public ICommand ClearCommand
        {
            get { return clearCommand ?? (clearCommand = new RelayCommand(() => ClearPresentationDisplay())); }
        }

        private void ClearPresentationDisplay()
        {
            isSelected = false;
            stat.clearStatus();
            DisplayedPresentation = new Presentation();
            App.Messenger.NotifyColleagues("PresentationCleared");
        }

        public ICommand UpdateCommand
        {
            get { return updateCommand ?? (updateCommand = new RelayCommand(() => UpdatePresentation(), () => isSelected)); }
        }

        private void UpdatePresentation()
        {
            if (!stat.CheckPresentationForAdd(DisplayedPresentation)) return;
            if (!App.LinqPresentation.UpdatePresentation(DisplayedPresentation))
            {
                stat.Status = App.LinqPresentation.errorMessage;
                return;
            }
            App.Messenger.NotifyColleagues("UpdatePresentation", DisplayedPresentation);
        }

        public ICommand AddCommand
        {
            get { return addCommand ?? (addCommand = new RelayCommand(() => AddPresentation(), () => !isSelected)); }
        }

        private void AddPresentation()
        {
            if (!stat.CheckPresentationForAdd(DisplayedPresentation)) return;
            if (!App.LinqPresentation.AddPresentation(DisplayedPresentation))
            {
                stat.Status = App.LinqPresentation.errorMessage;
                return;
            }
            App.Messenger.NotifyColleagues("AddPresentation", DisplayedPresentation);
        }

        public PresentationDisplayModel()
        {
            Messenger messenger = App.Messenger;
            messenger.Register("PresentationSelectionChanged", (Action<Presentation>)(param => ProcessPresentation(param)));
            messenger.Register("ExhibitSelectionChanged", (Action<Exhibit.Exhibit>)(param => ChangeExhibitName(param)));
            messenger.Register("ExpositionSelectionChanged", (Action<Exposition.Exposition>)(param => ChangeExpositionName(param)));
            messenger.Register("HallSelectionChanged", (Action<Hall.Hall>)(param => ChangeHallName(param)));
            messenger.Register("SetStatus", (Action<String>)(param => stat.Status = param));
            messenger.Register("Clear", (Action)(() => ClearPresentationDisplay()));
        }

        public void ChangeHallName(Hall.Hall p)
        {
            if (p == null)
            {
                isSelected = false; return;
            }
            Presentation temp = new Presentation();
            temp.CopyPresentation(DisplayedPresentation);
            temp.Hall = p.HallName;
            DisplayedPresentation = temp;
            //  isSelected = true;
            stat.clearStatus();
        }

        public void ChangeExhibitName(Exhibit.Exhibit p)
        {
            if (p == null)
            {
                isSelected = false; return;
            }
            Presentation temp = new Presentation();
            temp.CopyPresentation(DisplayedPresentation);
            temp.PresentedExhibit = p.ExhibitName;
            DisplayedPresentation = temp;
          //  isSelected = true;
            stat.clearStatus();
        }

        public void ChangeExpositionName(Exposition.Exposition p)
        {
            if (p == null)
            {
                isSelected = false; return;
            }
            Presentation temp = new Presentation();
            temp.CopyPresentation(DisplayedPresentation);
            temp.Exposition = p.ExpositionName;
            temp.Location = p.LocationName;
            DisplayedPresentation = temp;
            //  isSelected = true;
            stat.clearStatus();
        }

        public void ProcessPresentation(Presentation p)
        {
            if (p == null)
            {
                isSelected = false; return;
            }
            Presentation temp = new Presentation();
            temp.CopyPresentation(p);
            DisplayedPresentation = temp;
            isSelected = true;
            stat.clearStatus();
        }
    }
}
