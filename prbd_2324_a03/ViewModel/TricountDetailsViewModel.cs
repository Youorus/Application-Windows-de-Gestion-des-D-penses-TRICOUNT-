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
            set {
                if (SetProperty(ref _tricount, value)) {
                    OnRefreshData();
                }
            }
        }

        public ICommand EditTricountCommand { get; set; }
        public ICommand AddOperationTricountCommand { get; set; }
        public ObservableCollectionFast<OperationCardViewModel> Operations { get; set; } = new ObservableCollectionFast<OperationCardViewModel>();

        public TricountDetailsViewModel(Tricounts tricount) {
            Tricount = tricount;
            OnRefreshData();

            EditTricountCommand = new RelayCommand(EditTricount);
            AddOperationTricountCommand = new RelayCommand(AddOperation);
        }

        private void EditTricount() {
            NotifyColleagues(App.Messages.MSG_EDIT_TRICOUNT, Tricount);
        }

        private void AddOperation() {
            NotifyColleagues(App.Messages.MSG_NEW_OPERATION, Tricount);
        }

        protected override void OnRefreshData() {
            if (Tricount == null) return;

            Operations.Clear();
            var operationsTricount = Context.Operations
                                             .Include(o => o.Creator)
                                             .Where(o => o.TricountId == Tricount.Id)
                                             .ToList();

            foreach (var operation in operationsTricount) {
                var viewModel = new OperationCardViewModel(operation);
                Operations.Add(viewModel);
            }
        }
    }
}
