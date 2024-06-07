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
using Xceed.Wpf.Toolkit.Primitives;

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


        private void Remove_Click(object sender, RoutedEventArgs e) {

            var button = sender as Button;
            if (button != null) {
                var participant = button.CommandParameter as ParticipantsListViewModel; // Cast to the appropriate type
                if (participant != null) {
                    // Maintenant, vous avez l'objet participant que vous pouvez utiliser pour le supprimer
                    _addTricountVm.RemoveParticipantAction(participant);
                }
            }
        }


        private void Cancel_Click(object sender, RoutedEventArgs e) {



            _addTricountVm.RestoreRemovedParticipants();
            _addTricountVm.Tricount.Reload();
            _addTricountVm.RaisePropertyChanged();

            DataContext = _vm;


            // Rétablir la visibilité appropriée
            _vm.IsVisibleDetailsTricount = false;
            _vm.IsVisibleOperationTricount = true;

        }

        private void Save_Click(object sender, RoutedEventArgs e) {

            DataContext = _vm;

            _vm.UpdateListTricout();

            // Rétablir la visibilité appropriée
            _vm.IsVisibleDetailsTricount = false;
            _vm.IsVisibleOperationTricount = true;

        }



        private void ListView_MouseDoubleClick(object sender, MouseButtonEventArgs e) {

           

            if (listView.SelectedItem != null) {
                var operation = listView.SelectedItem as OperationCardViewModel;

                _vm.EditOperation(operation.Operation);
            }

         
        }


    }
}
