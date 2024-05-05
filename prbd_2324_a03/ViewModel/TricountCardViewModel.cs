using prbd_2324_a03.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PRBD_Framework;

namespace prbd_2324_a03.ViewModel
{
    public class TricountCardViewModel : ViewModelCommon
    {
        private readonly Tricounts _tricount;

        public DateTime LastOperationDate { get; private set; }


        public Tricounts Tricounts {
            get => _tricount;
            private init => SetProperty(ref _tricount, value);
        }

        public TricountCardViewModel(Tricounts tricounts) {
            Tricounts = tricounts;
        }
    }
}
