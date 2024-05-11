using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PRBD_Framework;
using System.Threading.Tasks;
using prbd_2324_a03.Model;
using System.Reflection;
using System.Windows.Input;

namespace prbd_2324_a03.ViewModel
{
    public class ListTricountViewModel : ViewModelCommon {
        int userId = 1;

        public string TitleWindows;

        public ICommand NewTricount { get; set; }


        //on n'informe l'application que l'on veut creer un nouveau tricount
      

        public ObservableCollectionFast<Tricounts> Tricounts { get; set; } = new();

        public ListTricountViewModel() {
            OnRefreshData();
            Register<Tricounts>(App.Messages.MSG_TRICOUNT_CHANGED, Tricounts => OnRefreshData());
        }

       

        protected override void OnRefreshData() {


            Tricounts.RefreshFromModel(Context.Tricounts
                              .Where(t => t.Creator == userId)
                              .OrderByDescending(t => t.Created_at));


            CalculateLastOperationDate();
            CountUserTricount();
            CountOperationTricount();
            ExpenseTricount();
            MyExpenseTricountAndBalance();

            NewTricount = new RelayCommand(() => { NotifyColleagues(App.Messages.MSG_NEW_TRICOUNT, new Tricounts()); });

            TitleWindows = GetEmailUser();

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
                // Calculer les dépenses de l'utilisateur dans le tricount
                var expenses = GetUserTotalExpenseInTricount(userId, tricount.Id);

                // Calculer la balance de l'utilisateur dans le tricount
                var userBalances = CalculateUserBalances(tricount.Id);

                // Récupérer la balance de l'utilisateur spécifique
                double balanceForUser = userBalances[userId];

                // Affecter la balance de l'utilisateur spécifique à MyBalanceTricount
                tricount.MyBalanceTricount = balanceForUser;

                if (tricount.MyBalanceTricount < 0) {
                    
                }

                // Calculer la dépense de l'utilisateur en prenant en compte sa balance
                if (tricount.MyBalanceTricount < 0) {
                    // Si la balance est négative, ajouter la valeur absolue de la balance aux dépenses
                    decimal absoluteBalance = (decimal)tricount.MyBalanceTricount * -1;
                    tricount.MyExpenseTricount = expenses + (double)absoluteBalance;
                } else {
                    // Sinon, ajouter simplement la balance aux dépenses
                    tricount.MyExpenseTricount = expenses + tricount.MyBalanceTricount;
                }
            }
        }


        public Dictionary<int, double> CalculateUserBalances(int tricountId) {
            var userBalances = new Dictionary<int, double>();

            // Initialiser les soldes de chaque utilisateur participant au tricount à zéro
            foreach (var x in GetUserIdsInTricount(tricountId)) {
                userBalances.Add(x, 0);
            }

            // Parcourir chaque dépense dans le tricount
            var expenses = Context.Operations.Where(o => o.TricountId == tricountId).ToList();
            foreach (var expense in expenses) {
                double totalExpense = expense.Amount;

                // Parcourir chaque répartition dans la dépense
                var repartitions = Context.Repartitions.Where(r => r.OperationId == expense.Id).ToList();
                foreach (var repartition in repartitions) {
                    int userId = repartition.UserId;
                    decimal weight = repartition.Weight;

                    // Calculer la part de la dépense pour l'utilisateur concerné
                    double userShare = (double)totalExpense * ((double)weight / (double)TotalWeightsInExpense(expense.Id));

                    // Ajouter la part de la dépense au solde de l'utilisateur concerné
                    userBalances[userId] -= userShare;
                }

                // Ajouter le montant total de la dépense au solde de l'initiateur de la dépense
                userBalances[expense.InitiatorId] += totalExpense;
            }

            return userBalances;
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

        public double GetUserTotalExpenseInTricount(int userId, int tricountId) {
            double totalExpense = 0;

            // Parcourir chaque dépense dans le tricount
            var expenses = Context.Operations.Where(o => o.TricountId == tricountId).ToList();
            foreach (var expense in expenses) {
                // Vérifier si l'utilisateur est l'initiateur de la dépense
                if (expense.InitiatorId == userId) {
                    totalExpense += expense.Amount;
                }
            }

            return totalExpense;
        }

        public string GetEmailUser() {
          var x = Context.Users.Where(u => u.UserId == userId).Select(u => u.Mail);

            return "My Tricount (" + x +")";

        }






    }




}