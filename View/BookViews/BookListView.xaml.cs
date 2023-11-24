using DbLab2.ViewModels.BookModels;
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

namespace DbLab2.View.BookViews
{
    /// <summary>
    /// Interaction logic for BookManagementView.xaml
    /// </summary>
    public partial class BookListView : UserControl
    {
        public BookListView(BookManagementViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }
    }
}
