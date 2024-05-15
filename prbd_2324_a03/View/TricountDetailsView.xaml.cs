﻿using prbd_2324_a03.Model;
using prbd_2324_a03.ViewModel;
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

namespace prbd_2324_a03.View
{
    /// <summary>
    /// Interaction logic for TricountDetailsView.xaml
    /// </summary>
    public partial class TricountDetailsView : UserControl
    {
        private readonly TricountDetailsViewModel _vm;
        public TricountDetailsView(Tricounts tricounts) {
            InitializeComponent();

            DataContext = _vm = new TricountDetailsViewModel(tricounts);
        }
    }
}