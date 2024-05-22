using prbd_2324_a03.Model;
using PRBD_Framework;
using System;

namespace prbd_2324_a03.ViewModel
{
    public class OperationCardViewModel : ViewModelCommon
    {
        private Operations _operation;
        public Operations Operation {
            get => _operation;
            set => SetProperty(ref _operation, value);
        }


        public string Title { get; set; }

        public User Creator { get; set; }

        public double Amount { get; set; }

        public DateTime Create_at { get; set; }

        public OperationCardViewModel(Operations operation) {
            Operation = operation;

            Title = operation.Title;
            Creator = operation.Creator;
            Amount = operation.Amount;
            Create_at = operation.OperationDate;
        }
    }
}
