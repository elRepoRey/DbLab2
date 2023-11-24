using GalaSoft.MvvmLight.Command;
using System;
using System.ComponentModel;
using System.Windows.Controls;
using System.Windows.Input;
using DbLab2.View;
using DbLab2.Utils;
using DbLab2.DbBookContext;

using DbLab2.ViewModels.AuthorModels;
using DbLab2.DataModels;
using DbLab2.Services;
using DbLab2.View.AuthorViews;
using DbLab2.View.BookViews;
using DbLab2.ViewModels.BookModels;
using DbLab2.ViewModels.StoreModels;
using DbLab2.View.StoreViews;

namespace DbLab2.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public ICommand NavigateCommand { get; private set; }
        public UserControl NavbarView { get; private set; }
        BookstoreContext bookstoreContext;
        BookService bookService;
        private UserControl _currentView;
        public UserControl CurrentView
        {
            get { return _currentView; }
            set
            {
                if (_currentView != value)
                {
                    _currentView = value;
                    OnPropertyChanged(nameof(CurrentView));
                }
            }
        }

        public MainViewModel()
        {
            NavigateCommand = new RelayCommand<EnumNavbarItems>(NavigateTo);
            NavbarView = new Navbar();
            NavbarView.DataContext = this;
            CurrentView = new WelcomeView();
            bookstoreContext = new BookstoreContext();
            bookService = new BookService();

        }

        private void NavigateTo(EnumNavbarItems item)
        {
            switch (item)
            {
                case EnumNavbarItems.StoresManagement:
                    ShowStoreListView();
                    break;
                case EnumNavbarItems.AuthorsManagement:
                 var authorManagementViewModel = new AuthorManagementViewModel();
                    authorManagementViewModel.RequestAddAuthorView += ShowAddAuthorView;   
                    CurrentView = new AuthorManagement(authorManagementViewModel);
                    ShowAuthorListView();
                    break;
                case EnumNavbarItems.BooksManagement:
                    ShowBookListView();
                    break;
                case EnumNavbarItems.Welcome:
                    CurrentView = new WelcomeView();
                    break;
            }
        }
        private void ShowAddAuthorView()
        {
            var addAuthorViewModel = new AddAuthorViewModel();
    
            addAuthorViewModel.AuthorAdded -= OnAuthorAdded;
           
            addAuthorViewModel.AuthorAdded += OnAuthorAdded;
 
            CurrentView = new AddAuthorView(addAuthorViewModel);
        }

        private void OnAuthorAdded()       
         {

            ShowAuthorListView();
         }

        private void ShowAuthorListView()
        {
            var authorManagementViewModel = new AuthorManagementViewModel();
            authorManagementViewModel.RequestAddAuthorView += ShowAddAuthorView;
            authorManagementViewModel.RequestEditAuthorView += ShowEditAuthorView;

            CurrentView = new AuthorManagement(authorManagementViewModel);
        }
        private void ShowEditAuthorView(Author author)
        {
            
            var editAuthorViewModel = new EditAuthorViewModel(new AuthorService(), author);
            editAuthorViewModel.AuthorUpdated += () =>
            {
              
                ShowAuthorListView();
            };
            CurrentView = new EditAuthorView(editAuthorViewModel);
        }

        private void ShowEditBookView(Book bookToEdit)
        {
           
            var editBookViewModel = new EditBookViewModel(bookService, new AuthorService(), bookToEdit);
            editBookViewModel.BookUpdated += () =>
            {
                ShowBookListView();
            };
            CurrentView = new EditBookView(editBookViewModel);
        }

        private void ShowBookListView()
        {
            var bookManagementViewModel = new BookManagementViewModel();                  
            bookManagementViewModel.RequestEditBookView += ShowEditBookView;
            bookManagementViewModel.RequestAddBookView += ShowAddBookView;
            CurrentView = new BookListView(bookManagementViewModel);
        }

        private void ShowAddBookView()
        {
            var addBookViewModel = new AddBookViewModel(new BookService(), new AuthorService());
            addBookViewModel.BookAdded += () =>
            {
                ShowBookListView();
            };
            CurrentView = new AddBookView(addBookViewModel);
            
        }
        private void ShowStoreListView()
        {
            var storeManagementViewModel = new StoreManagementModel(new StoreService(new BookstoreContext()));
            storeManagementViewModel.RequestManageStoreView += ShowManageStoreView;
            CurrentView = new StoreListView(storeManagementViewModel);
        }

        private void ShowManageStoreView(Store store)
        {    
            BookstoreContext bookstoreContext = new BookstoreContext();

            var manageStoreViewModel = new ManageStoreViewModel(new StoreService(bookstoreContext), new BookService(), store.ID);
            CurrentView = new ManageStoreView(manageStoreViewModel);
        }
    }
}
