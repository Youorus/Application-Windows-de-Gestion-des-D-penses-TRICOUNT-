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

        private bool _isEdit;
        public bool IsEdit {
            get => _isEdit;
            set => SetProperty(ref _isEdit, value);
        }

        private bool _IsOperation;
        public bool IsOperation {
            get => _IsOperation;
            set => SetProperty(ref _IsOperation, value);
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

            Register<Tricounts>(App.Messages.MSG_VIEWTRICOUNT_CHANGED, tricount => OnRefreshData());
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

        public void EditOperation(Operations operation) {

            if (operation != null) {
                App.ShowDialog<AddOperationViewModel, Operations, PridContext>(Tricount, operation, false);
            }

           

        }





        protected override void OnRefreshData() {
            if (Tricount == null) return;

            // Clear the Operations collection
            Operations.Clear();

            // Fetch the operations related to the current Tricount
            var operationsTricount = Context.Operations
                .Include(o => o.Creator)
                .Where(o => o.TricountId == Tricount.Id)
                .OrderByDescending(o => o.OperationDate) // Sort by date in descending order
                .ToList();

            // Add each operation to the Operations collection
            foreach (var operation in operationsTricount) {
                var viewModel = new OperationCardViewModel(operation);
                Operations.Add(viewModel);
            }

            // Check if there are any operations for the current Tricount
            IsOperation = Context.Operations.Any(o => o.TricountId == Tricount.Id);

            // Check if the current user is the creator or if the user is an admin
            IsEdit = Context.Tricounts.Any(t => t.Creator == CurrentUser.UserId) || Context.Users.Any(u => u.Full_name == "Admin");

          
        }

    }
}
