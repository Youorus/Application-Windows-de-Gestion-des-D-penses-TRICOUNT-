using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PRBD_Framework;
using System.Threading.Tasks;
using prbd_2324_a03.Model;

namespace prbd_2324_a03.ViewModel
{
    public class ListTricountViewModel : ViewModelCommon {
        int userId = 1;

        public ObservableCollectionFast<Tricounts> Tricounts { get; set; } = new();

        public ListTricountViewModel() {
            OnRefreshData();
        }

        protected override void OnRefreshData() {
           

            Tricounts.RefreshFromModel(Context.Tricounts
                                          .Where(t => t.Creator == userId));

            CalculateLastOperationDate();
            CountUserTricount();
            CountOperationTricount();
            ExpenseTricount();
            MyExpenseTricountAndBalance();

        }


        private void CalculateLastOperationDate() {
            foreach (var tricount in Tricounts) {
                var lastOperationDate = Context.Operations
            .Where(o => o.TricountId == tricount.Id)
            .OrderByDescending(o => o.OperationDate)
            .Select(o => o.OperationDate)
            .FirstOrDefault();


                tricount.LastOperationDate = lastOperationDate != DateTime.MinValue ? lastOperationDate : DateTime.MinValue;
            }
        }

        private void CountUserTricount() {

            foreach (var tricount in Tricounts) {
               var countUsers = Context.Subscriptions
        .Where(s => s.TricountId == tricount.Id && s.UserId != userId)
        .Select(s => s.UserId)
        .Distinct()
        .Count();

                tricount.CountUser = countUsers;
            }


           
            }

        private void CountOperationTricount() {

            foreach (var tricount in Tricounts) {
                var countOperation = Context.Operations
            .Where(o => o.TricountId == tricount.Id).Count();

                tricount.CountOperation = countOperation;

            }
        }

        private void ExpenseTricount() {

            foreach (var tricount in Tricounts) {
                var totalExpense = Context.Operations
      .Where(o => o.TricountId == tricount.Id)
      .Sum(o => o.Amount);

                tricount.ExpenseTricount = totalExpense;

            }
        }
      

        private void MyExpenseTricountAndBalance() {

            foreach (var tricount in Tricounts) {
                var expenses = (from o in Context.Operations
                                where o.TricountId == tricount.Id && o.InitiatorId == userId
                                select o.Amount).Sum();

                tricount.MyExpenseTricount = expenses;

                tricount.MyBalanceTricount = tricount.MyExpenseTricount - tricount.CountOperation;

            }
        }

    }

   


}