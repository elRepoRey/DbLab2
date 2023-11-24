using DbLab2.ViewModels;
using DbLab2.ViewModels.StoreModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DbLab2.View.StoreViews
{
    /// <summary>
    /// Interaction logic for StockBalanceManagementView.xaml
    /// </summary>
    public partial class ManageStoreView : UserControl
    {
        public ManageStoreView(ManageStoreViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }
    }
}
