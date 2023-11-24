using System;
using System.ComponentModel;
using System.Windows.Input;
using GalaSoft.MvvmLight.CommandWpf;
using DbLab2.DataModels;
using DbLab2.Services;
using GalaSoft.MvvmLight.Messaging;
using System.Windows;

namespace DbLab2.ViewModels.AuthorModels
{
    public class EditAuthorViewModel : INotifyPropertyChanged
    {
        private readonly AuthorService _authorService;
        private Author _author;

        public int ID { get; private set; }
        public string FirstName
        {
            get => _author.FirstName;
            set
            {
                _author.FirstName = value;
                OnPropertyChanged(nameof(FirstName));
            }
        }
        public string LastName
        {
            get => _author.LastName;
            set
            {
                _author.LastName = value;
                OnPropertyChanged(nameof(LastName));
            }
        }
        public DateTime BirthDate
        {
            get => _author.BirthDate;
            set
            {
                _author.BirthDate = value;
                OnPropertyChanged(nameof(BirthDate));
            }
        }

        public ICommand SaveAuthorCommand { get; private set; }
        public event PropertyChangedEventHandler PropertyChanged;
        public event Action AuthorUpdated;

        public EditAuthorViewModel(AuthorService authorService, Author author)
        {
            _authorService = authorService ?? throw new ArgumentNullException(nameof(authorService));
            _author = author ?? throw new ArgumentNullException(nameof(author));
            ID = _author.ID;

            SaveAuthorCommand = new RelayCommand(UpdateAuthor);
        }

        private void UpdateAuthor()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(FirstName) || string.IsNullOrWhiteSpace(LastName))
                {
                    MessageBox.Show("First name and last name cannot be empty.");
                    return;
                }

                if (!DateTime.TryParse(BirthDate.ToString(), out DateTime birthDateParsed))
                {
                    MessageBox.Show("Birth date is not a valid date. Format: dd/mm/yyyy");
                    return;
                }
                if (BirthDate > DateTime.Now)
                {
                    MessageBox.Show("Birth date cannot be in the future.");
                    return;
                }

                _authorService.UpdateAuthor(_author);
                AuthorUpdated?.Invoke();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
