﻿using DbLab2.ViewModels.StoreModels;
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
    /// Interaction logic for StoreListView.xaml
    /// </summary>
    public partial class StoreListView : UserControl
    {
        public StoreListView(StoreManagementModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }
    }
}
