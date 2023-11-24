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
    public class AddAuthorViewModel : INotifyPropertyChanged
    {
        private AuthorService? _authorService;

        private string _firstName;
        private string _lastName;
        private string _birthDate;

        public string FirstName
        {
            get => _firstName;
            set
            {
                _firstName = value;
                OnPropertyChanged(nameof(FirstName));
            }
        }

        public string LastName
        {
            get => _lastName;
            set
            {
                _lastName = value;
                OnPropertyChanged(nameof(LastName));
            }
        }

        public string BirthDate
        {
            get => _birthDate;
            set
            {
                _birthDate = value;
                OnPropertyChanged(nameof(BirthDate));
            }
        }
        public ICommand AddAuthorCommand { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        public event Action AuthorAdded;

        public AddAuthorViewModel()
        {
            _authorService = new AuthorService();
            AddAuthorCommand = new RelayCommand(AddAuthor);

        }

        private void AddAuthor()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(FirstName) || string.IsNullOrWhiteSpace(LastName) || string.IsNullOrWhiteSpace(BirthDate))
                {
                    MessageBox.Show("Please fill in all the fields");
                    return;
                }
                else if (DateTime.TryParse(BirthDate, out DateTime birthDateParsed))
                {

                    Author newAuthor = new Author
                    {
                        FirstName = FirstName,
                        LastName = LastName,
                        BirthDate = birthDateParsed
                    };

                    _authorService.AddAuthor(newAuthor);

                    AuthorAdded?.Invoke();

                }
                else
                {
                    MessageBox.Show("Invalid date format, please use the following format: dd/mm/yyyy");
                    return;
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
