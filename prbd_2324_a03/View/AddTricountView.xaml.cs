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
    /// Logique d'interaction pour Add_Tricount.xaml
    /// </summary>
    public partial class AddTricountView : UserControl
    {
        private readonly AddTricountViewModel _vm;
        public AddTricountView(Tricounts tricounts, bool isNew, TricountDetailsViewModel tricountDetailsViewModel) {
            InitializeComponent();

            DataContext = _vm = new AddTricountViewModel(tricounts, isNew, tricountDetailsViewModel);
        }
    }
}
