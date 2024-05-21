using prbd_2324_a03.Model;
using PRBD_Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace prbd_2324_a03.ViewModel
{
    class DialogDeleteTricountViewModel : ViewModelCommon
    {

        public ICommand YesCommand { get; }
        public ICommand NoCommand { get; }

        private Tricounts _tricount;
        public Tricounts Tricount {
            get => _tricount;
            set => SetProperty(ref _tricount, value);
        }

        public DialogDeleteTricountViewModel(Tricounts tricounts) {
            Tricount = tricounts

            YesCommand = new RelayCommand(YesAction);
            NoCommand = new RelayCommand(NoAction);

        }

        private void YesAction() {
           
        }

        private void NoAction() {
         
        }
    }
}
