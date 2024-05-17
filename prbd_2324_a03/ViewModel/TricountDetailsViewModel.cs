using Microsoft.EntityFrameworkCore;
using prbd_2324_a03.Model;
using PRBD_Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace prbd_2324_a03.ViewModel
{
    public class TricountDetailsViewModel : ViewModelCommon
    {
        private Tricounts _tricount;
        public Tricounts Tricount {
            get => _tricount;
            set => SetProperty(ref _tricount, value);
        }

        public ObservableCollectionFast<Operations> Operations { get; set; } = new();

        public TricountDetailsViewModel(Tricounts tricount) {
            Tricount = tricount;

            Operations.RefreshFromModel(Context.Operations.Include(o => o.Creator).Where(o => o.TricountId == Tricount.Id));
        }

    }
}
