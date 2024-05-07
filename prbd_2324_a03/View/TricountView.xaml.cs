using prbd_2324_a03.Model;
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
 
    public partial class TricountView : WindowBase
    {
        public TricountView() {
            InitializeComponent();

            Register<Tricounts>(App.Messages.MSG_NEW_TRICOUNT, tricounts => DoDisplayMember(tricounts, true));
        }

        private void DoDisplayMember(Tricounts tricounts, bool isNew) {
            if (tricounts != null)
                OpenTab(isNew ? "<New Tricount>" : tricounts.Title, tricounts.Title, () => new AddTricountView(tricounts, isNew)); 
        }

        private void OpenTab(string header, string tag, Func<UserControl> createView) {
            var tab = tabControl.FindByTag(tag);
            if (tab == null)
                tabControl.Add(createView(), header, tag);
            else
                tabControl.SetFocus(tab);
        }
    }
}
