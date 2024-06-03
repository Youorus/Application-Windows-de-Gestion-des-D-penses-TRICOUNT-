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

            Register<Tricounts>(App.Messages.MSG_NEW_TRICOUNT, tricounts => DoDisplayTricount(tricounts, true));

            //Register<Tricounts>(App.Messages.MSG_NEW_OPERATION, tricounts => DoDisplayOperation(tricounts, true));


            Register<Tricounts>(App.Messages.MSG_DISPLAY_TRICOUNT, tricounts => DoDisplayTricountDetails(tricounts));

            Register<Tricounts>(App.Messages.MSG_CANCEL_TRICOUNT, tricounts => DoDisplayListTricount(tricounts));


            Register<Tricounts>(App.Messages.MSG_TITLE_CHANGED, tricounts => DoRenameTab(string.IsNullOrEmpty(tricounts.Title) ? "<New Tricount>" : tricounts.Title));
        }

        private void DoDisplayTricount(Tricounts tricounts, bool isNew) {
            if (tricounts != null)
                OpenTab(isNew ? "<New Tricount>" : tricounts.Title, tricounts.Title, () => new AddTricountView(tricounts, isNew)); 
        }

        //private void DoDisplayOperation(Tricounts tricounts, bool isNew) {
        //    if (tricounts != null)
        //        OpenTab("Add operation", "Add operation", () => new AddOperationView(tricounts, isNew));
        //}


        private void OpenTab(string header, string tag, Func<UserControl> createView) {
            var tab = tabControl.FindByTag(tag);
            if (tab == null)
                tabControl.Add(createView(), header, tag);
            else
                tabControl.SetFocus(tab);
        }

        private void DoDisplayTricountDetails(Tricounts tricounts) {
            if (tricounts != null)
            
                OpenTab(tricounts.Title, tricounts.Title, () => new TricountDetailsView(tricounts));
        }

        private void DoDisplayListTricount(Tricounts tricounts) {
             tabControl.CloseByTag(string.IsNullOrEmpty(tricounts.Title) ? "<New Tricount>" : tricounts.Title);
        }


        private void DoRenameTab(string header) {
            if (tabControl.SelectedItem is TabItem tab) {
                MyTabControl.RenameTab(tab, header);
                tab.Tag = header;
            }
        }
    }
}
