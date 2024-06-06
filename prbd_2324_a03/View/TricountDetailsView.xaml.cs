using prbd_2324_a03.Model;
using prbd_2324_a03.ViewModel;
using PRBD_Framework;
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
        private readonly AddTricountViewModel _addTricountVm;

        public TricountDetailsView(Tricounts tricounts) {
            InitializeComponent();

            _vm = new TricountDetailsViewModel(tricounts);

            _addTricountVm = new AddTricountViewModel(tricounts, false);


            _vm.IsVisibleDetailsTricount = false;
            _vm.IsVisibleOperationTricount = true;

            _vm.DeleteTricount += () => App.Confirm("You're about to delete this Tricount\nDo you confirm?");

            DataContext = _vm;  // Set initial DataContext



        }
        private void EditTricountButton_Click(object sender, RoutedEventArgs e) {
            DataContext = _addTricountVm;  // Set DataContext to AddTricountViewModel

        _addTricountVm.IsVisibleOperationTricount = false;

        }


        private void Cancel_Click(object sender, RoutedEventArgs e) {

           
            _addTricountVm.Tricount.Reload();
            _addTricountVm.RaisePropertyChanged();

            DataContext = _vm;


            // Rétablir la visibilité appropriée
            _vm.IsVisibleDetailsTricount = false;
            _vm.IsVisibleOperationTricount = true;

        }

    }
}
