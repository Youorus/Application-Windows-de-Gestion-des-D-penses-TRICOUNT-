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
        public event Func<bool> DeleteTricount;

        private Tricounts _tricount;
        public Tricounts Tricount {
            get => _tricount;
            set {
                if (SetProperty(ref _tricount, value)) {
                    OnRefreshData();
                }
            }
        }

        private bool _isVisibleDetailsTricount;
        public bool IsVisibleDetailsTricount {
            get => _isVisibleDetailsTricount;
            set => SetProperty(ref _isVisibleDetailsTricount, value);
        }

        private bool _isVisibleOperationTricount;
        public bool IsVisibleOperationTricount {
            get => _isVisibleOperationTricount;
            set => SetProperty(ref _isVisibleOperationTricount, value);
        }

        public ICommand DeleteTricountCommand { get; set; }
        public ICommand AddOperationTricountCommand { get; set; }
        public ObservableCollectionFast<OperationCardViewModel> Operations { get; set; } = new ObservableCollectionFast<OperationCardViewModel>();

        public TricountDetailsViewModel(Tricounts tricount) {
            Tricount = tricount;
            OnRefreshData();

            DeleteTricountCommand = new RelayCommand(DeleteTricountAction);
            AddOperationTricountCommand = new RelayCommand(AddOperation);
        }

        private void DeleteTricountAction() {
            if (!(DeleteTricount?.Invoke() ?? false)) return;

            Tricount.Delete();
            NotifyColleagues(App.Messages.MSG_CANCEL_TRICOUNT, Tricount);
            NotifyColleagues(App.Messages.MSG_TRICOUNT_CHANGED, Tricount);
        }

        private void AddOperation() {
            App.ShowDialog<AddOperationViewModel, Operations, PridContext>(Tricount, new Operations(), true);
           
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
