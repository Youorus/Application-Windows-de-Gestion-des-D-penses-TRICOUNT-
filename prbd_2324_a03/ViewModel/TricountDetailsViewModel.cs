using Microsoft.EntityFrameworkCore;
using prbd_2324_a03.Model;
using prbd_2324_a03.View;
using PRBD_Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace prbd_2324_a03.ViewModel
{
    public class TricountDetailsViewModel : ViewModelCommon
    {
        private Tricounts _tricount;
        public Tricounts Tricount {
            get => _tricount;
            set => SetProperty(ref _tricount, value);
        }

        public ICommand EditTricountCommand { get; set; }

        public ObservableCollectionFast<Operations> Operations { get; set; } = new();

        public TricountDetailsViewModel(Tricounts tricount) {
            Tricount = tricount;

            Operations.RefreshFromModel(Context.Operations.Include(o => o.Creator).Where(o => o.TricountId == Tricount.Id));


            EditTricountCommand = new RelayCommand(() => { NotifyColleagues(App.Messages.MSG_NEW_TRICOUNT, Tricount); });





        }

    }
}
