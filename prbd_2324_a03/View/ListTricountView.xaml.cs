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
using prbd_2324_a03.Model;
using prbd_2324_a03.ViewModel;
using PRBD_Framework;


namespace prbd_2324_a03.View
{
  
    public partial class ListTricountView : UserControl
    {
        public ListTricountView() {
            InitializeComponent();
        }

        private void ListView_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e) {
            if (listView.SelectedItem != null) {
                var tricount = listView.SelectedItem as Tricounts;
                var tricountDetailsViewModel = new TricountDetailsViewModel(tricount);
                vm.TricountDetailsView.Execute(tricountDetailsViewModel); // je passe a la vue la viewModel du tricount Actuelle 

            }
        }

    }
}
