using prbd_2324_a03.Model;
using prbd_2324_a03.ViewModel;
using PRBD_Framework;
using System;
using Xceed.Wpf.Toolkit;
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
    /// Interaction logic for AddOperationView.xaml
    /// </summary>
    public partial class AddOperationView : DialogWindowBase
    {
        private readonly AddOperationViewModel _vm;
        public AddOperationView(Tricounts tricounts, Operations operations, bool IsNew) {
            InitializeComponent();
            DataContext = _vm = new AddOperationViewModel(tricounts, operations, IsNew);

            _vm.RequestClose += () => this.Close();
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e) {
            foreach (char c in e.Text) {
                if (!char.IsDigit(c) && c != '.' && c != ',') // Permet également les décimales
                {
                    e.Handled = true;
                    return;
                }
            }
        }

        private void DoubleUpDown_LostFocus(object sender, RoutedEventArgs e) {
            var doubleUpDown = sender as DoubleUpDown;
            if (doubleUpDown != null && string.IsNullOrEmpty(doubleUpDown.Text)) {
                doubleUpDown.Value = 0; // Mettez à jour la valeur à 0 si le texte est vide
                _vm.AmountValid = true;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e) {
            _vm.Operation.Reload();
            Close();
            _vm.RaisePropertyChanged();
        }
    }
}
