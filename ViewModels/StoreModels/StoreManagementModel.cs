using DbLab2.DataModels;
using DbLab2.Services;
using GalaSoft.MvvmLight.CommandWpf;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace DbLab2.ViewModels.StoreModels;

public class StoreManagementModel
{
    private readonly StoreService _storeService;
    private Store _selectedStore;
    public ICommand ManageStoreCommand { get; private set; }

    public event Action<Store> RequestManageStoreView;
    public ObservableCollection<Store> Stores { get; private set; }  
    
    
    public Store SelectedStore
    {
        get => _selectedStore;
        set
        {
            _selectedStore = value;
          
        }
    } 
    public ICommand AddBookCommand { get; private set; }
    public ICommand RemoveBookCommand { get; private set; }

    public StoreManagementModel(StoreService storeService)
    {
        _storeService = storeService ?? throw new ArgumentNullException(nameof(storeService));
        Stores = new ObservableCollection<Store>(_storeService.GetStores());
        ManageStoreCommand = new RelayCommand<Store>(ManageStore);
    }

    private void ManageStore(Store store)
    {
        RequestManageStoreView.Invoke(store);
    }
}
