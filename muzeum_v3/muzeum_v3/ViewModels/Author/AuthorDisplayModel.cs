using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MvvmFoundation.Wpf;
using System.ComponentModel;
using System.Windows.Input;
using System.Windows.Media;
using muzeum_v3.ViewModels.Author;
using muzeum_v3.ViewModels.Exhibit;

namespace muzeum_v3.ViewModels.Author
{
    class AuthorDisplayModel : INotifyPropertyChanged
    {
        private bool isSelected = false;

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, e);
        }

        private readonly AuthorDisplayStatusModel stat = new AuthorDisplayStatusModel();
        public AuthorDisplayStatusModel Stat { get { return stat; } }
        private Author displayedAuthor = new Author();
       
        private RelayCommand getAuthorsCommand;
        private RelayCommand clearCommand;
        private RelayCommand updateCommand;
        private RelayCommand addCommand;
        private RelayCommand sendAuthorNameCommand;

        public ICommand SendAuthorNameCommand
        {
            get { return sendAuthorNameCommand ?? (sendAuthorNameCommand = new RelayCommand(() => ChangeAuthorName())); }
        }

        private void ChangeAuthorName()
        {
            if (!stat.CheckAuthorForAdd(DisplayedAuthor)) return;
            App.Messenger.NotifyColleagues("ChangeAuthorName", DisplayedAuthor.AuthorName);
        }

        public Author DisplayedAuthor
        {
            get { return displayedAuthor; }
            set { displayedAuthor = value; OnPropertyChanged(new PropertyChangedEventArgs("DisplayedAuthor")); }
        }

        public ICommand GetAuthorsCommand
        {
            get { return getAuthorsCommand ?? (getAuthorsCommand = new RelayCommand(() => GetAuthors())); }
        }

        private void GetAuthors()
        {
            isSelected = false;
            stat.clearStatus();
            DisplayedAuthor = new Author();
            App.Messenger.NotifyColleagues("GetAuthors");
        }

        public ICommand ClearCommand
        {
            get { return clearCommand ?? (clearCommand = new RelayCommand(() => ClearAuthorDisplay())); }
        }

        private void ClearAuthorDisplay()
        {
            isSelected = false;
            stat.clearStatus();
            DisplayedAuthor = new Author();
            App.Messenger.NotifyColleagues("AuthorCleared");
        } 

        public ICommand UpdateCommand
        {
            get { return updateCommand ?? (updateCommand = new RelayCommand(() => UpdateAuthor(), ()=>isSelected)); }
        }

        private void UpdateAuthor()
        {
            if (!stat.CheckAuthorForAdd(DisplayedAuthor)) return;
                if(!App.AuthorQuery.UpdateAuthor(DisplayedAuthor))
                {
                    stat.Status = App.AuthorQuery.errorMessage;
                    return;
                }
                App.Messenger.NotifyColleagues("UpdateAuthor", DisplayedAuthor);
        }

        public ICommand AddCommand
        {
            get { return addCommand ?? (addCommand = new RelayCommand(() => AddAuthor(), () => !isSelected)); }
        }

        private void AddAuthor()
        {
            if (!stat.CheckAuthorForAdd(DisplayedAuthor)) return;
            if (!App.AuthorQuery.AddAuthor(DisplayedAuthor))
            {
                stat.Status = App.AuthorQuery.errorMessage;
                return;
            }
            App.Messenger.NotifyColleagues("AddAuthor", DisplayedAuthor);
        }

        public AuthorDisplayModel()
        {
            Messenger messenger = App.Messenger;
            messenger.Register("AuthorSelectionChanged", (Action<Author>)(param => ProcessAuthor(param)));
            messenger.Register("SetStatus", (Action<String>)(param => stat.Status = param));
            messenger.Register("Clear", (Action)(() => ClearAuthorDisplay()));
        }

        public void ProcessAuthor(Author p)
        {
            if (p == null) {
                isSelected = false;  return; 
            }
            Author temp = new Author();
            temp.CopyAuthor(p);
            DisplayedAuthor = temp;
            isSelected = true;
            stat.clearStatus();
        } 
    }
}
