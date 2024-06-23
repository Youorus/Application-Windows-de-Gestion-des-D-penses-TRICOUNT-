using prbd_2324_a03.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using PRBD_Framework;
using System.Collections.ObjectModel;

namespace prbd_2324_a03.ViewModel
{
    public class TricountCardViewModel : ViewModelCommon
    {
        private Tricounts _tricount;
        private readonly int userId = CurrentUser.UserId;

        public DateTime LastOperationDate { get; set; }
        public int CountUser { get; set; }

        public string Title { get; set; }
        public string Description { get; set; }
        public User Creator { get; set; }
        public DateTime Created_at { get; set; }
        public int CountOperation { get; set; }
        public double ExpenseTricount { get; set; }
        public double MyBalanceTricount { get; set; }
        public double MyExpenseTricount { get; set; }

        public bool IsTextBoxVisible { get; set; } = true;

        public string Colors { get; set; }

        private ObservableCollection<Operations> _operations;

        public ObservableCollection<Operations> Operations {
            get => _operations;
            set => SetProperty(ref _operations, value);
        }

        public Tricounts Tricount {
            get => _tricount;
            set => SetProperty(ref _tricount, value);
        }

        public TricountCardViewModel(Tricounts tricount) {
            Tricount = tricount;
            OnRefreshData();
        }

        protected override void OnRefreshData() {
            CalculateLastOperationDate();
            CountUserTricount();
            CountOperationTricount();
            ExpenseOfTricount();
            MyExpenseTricountAndBalance();
            TricountCardColor();
            IsOperationTricount();
            LoadOperations();
            Title = Tricount.Title;
            Description = Tricount.Description;

            Creator = Tricount.CreatorTricount;
            Created_at = Tricount.Created_at;
        }

        private void CalculateLastOperationDate() {
            LastOperationDate = Context.Operations
                .Where(o => o.TricountId == Tricount.Id)
                .OrderByDescending(o => o.OperationDate)
                .Select(o => o.OperationDate)
                .FirstOrDefault();

            LastOperationDate = LastOperationDate != DateTime.MinValue ? LastOperationDate : DateTime.MinValue;
        }

        private void CountUserTricount() {
            CountUser = Context.Subscriptions
                .Where(s => s.TricountId == Tricount.Id && s.UserId != userId)
                .Select(s => s.UserId)
                .Distinct()
                .Count();
        }

        private void IsOperationTricount() {
            if (CountOperation == 0) {
                IsTextBoxVisible = false;
            }
        }

        private void TricountCardColor() {
            if (MyBalanceTricount < 0) {
                Colors = "LightPink";
            } else if (MyBalanceTricount > 0) {
                Colors = "LightGreen";
            } else {
                Colors = "LightGray";
            }
        }

        private void CountOperationTricount() {
            CountOperation = Context.Operations
                .Where(o => o.TricountId == Tricount.Id).Count();
        }

        private void ExpenseOfTricount() {
            ExpenseTricount = Context.Operations
                .Where(o => o.TricountId == Tricount.Id)
                .Sum(o => o.Amount);
        }

        private void MyExpenseTricountAndBalance() {
            var expenses = GetUserTotalExpenseInTricount(userId, Tricount.Id);
            var userBalances = CalculateUserBalances(Tricount.Id);
            double balanceForUser = userBalances.ContainsKey(userId) ? userBalances[userId] : 0.0;

            MyBalanceTricount = balanceForUser;

            if (balanceForUser < 0) {
                decimal absoluteBalance = (decimal)balanceForUser * -1;
                MyExpenseTricount = expenses + (double)absoluteBalance;
            } else {
                MyExpenseTricount = expenses + balanceForUser;
            }
        }

        public double GetUserTotalExpenseInTricount(int userId, int tricountId) {
            return Context.Operations
                .Where(o => o.TricountId == tricountId && o.InitiatorId == userId)
                .Sum(o => o.Amount);
        }

        public Dictionary<int, double> CalculateUserBalances(int tricountId) {
            var userBalances = new Dictionary<int, double>();

            foreach (var userId in GetUserIdsInTricount(tricountId)) {
                userBalances[userId] = 0;
            }

            var expenses = Context.Operations.Where(o => o.TricountId == tricountId).ToList();
            foreach (var expense in expenses) {
                double totalExpense = expense.Amount;
                var repartitions = Context.Repartitions.Where(r => r.OperationId == expense.Id).ToList();

                foreach (var repartition in repartitions) {
                    int userId = repartition.UserId;
                    decimal weight = repartition.Weight;
                    double userShare = (double)totalExpense * ((double)weight / (double)TotalWeightsInExpense(expense.Id));
                    userBalances[userId] -= userShare;
                }

                userBalances[expense.InitiatorId] += totalExpense;
            }

            return userBalances;
        }

        public double GetUserBalanceInTricount(int userId, int tricountId) {
            var userBalances = CalculateUserBalances(tricountId);
            return userBalances.ContainsKey(userId) ? userBalances[userId] : 0.0;
        }

        private void LoadOperations() {
            Operations = new ObservableCollection<Operations>(Context.Operations
                .Where(o => o.TricountId == Tricount.Id)
                .OrderByDescending(o => o.OperationDate)
                .ToList());
        }

        private IEnumerable<int> GetUserIdsInTricount(int tricountId) {
            return Context.Subscriptions
                .Where(s => s.TricountId == tricountId)
                .Select(s => s.UserId)
                .Distinct();
        }

        private decimal TotalWeightsInExpense(int operationId) {
            return Context.Repartitions.Where(r => r.OperationId == operationId).Sum(r => r.Weight);
        }
    }
}
