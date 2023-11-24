using DbLab2.DataModels;
using DbLab2.Services;
using DbLab2.View.AuthorViews;
using GalaSoft.MvvmLight.CommandWpf;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace DbLab2.ViewModels.AuthorModels
{
    public class AuthorManagementViewModel : INotifyPropertyChanged
    {
        private readonly AuthorService _authorService;
        public event Action? RequestAddAuthorView;
        public event Action<Author> RequestEditAuthorView;


        public ObservableCollection<Author> Authors { get; private set; }

        public ICommand EditAuthorCommand { get; private set; }
        public ICommand DeleteAuthorCommand { get; private set; }
        public ICommand AddAuthorCommand { get; private set; }

        public AuthorManagementViewModel()
        {

            _authorService = new AuthorService();
            Authors = new ObservableCollection<Author>(_authorService.GetAllAuthors());

            EditAuthorCommand = new RelayCommand<Author>(EditAuthor, CanEditOrDelete);
            DeleteAuthorCommand = new RelayCommand<Author>(DeleteAuthor, CanEditOrDelete);
            AddAuthorCommand = new RelayCommand(AddAuthor);
            RequestAddAuthorView = null;
        }

        private bool CanEditOrDelete(Author author)
        {
            return author != null;
        }

        private void EditAuthor(Author author)
        {
            RequestEditAuthorView?.Invoke(author);
        }


        private void DeleteAuthor(Author author)
        {
            if (author != null)
            {

                bool authorRemoved = _authorService.DeleteAuthor(author);
                if (authorRemoved)
                {
                    Authors.Remove(author);
                    OnPropertyChanged(nameof(Authors));
                }
            }
            else
            {
                MessageBox.Show("Please select an author to delete");
                return;
            }
        }

        private void AddAuthor()
        {
            RequestAddAuthorView?.Invoke();

        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
