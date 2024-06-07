using prbd_2324_a03.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace prbd_2324_a03.ViewModel
{
    public class ParticipantsListViewModel : ViewModelCommon {

        private readonly int _userId = CurrentUser.UserId;


        private User _user;
        public User User {
            get => _user;
            set => SetProperty(ref _user, value);
        }

        private int _tricountId;
        public int TricountId {
            get => _tricountId;
            set => SetProperty(ref _tricountId, value);
        }

        private string _name;
        public string FullName {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        private bool _isDefault;
        public bool IsDefault {
            get => _isDefault;
            set => SetProperty(ref _isDefault, value);
        }

        private bool _isExpense;
        public bool IsExpense {
            get => _isExpense;
            set => SetProperty(ref _isExpense, value);
        }

        private int _expense;
        public int Expense {
            get => _expense;
            set => SetProperty(ref _expense, value);
        }


        public ParticipantsListViewModel(User user, int Tricount) {
            User = user;
            TricountId = Tricount;
            OnRefreshData();
        }

        protected override void OnRefreshData() {
            ExpenseUser();

            var Userx = Context.Users.Where(u => u.UserId == User.UserId).FirstOrDefault();

            FullName = Userx.Full_name;

            if (Userx.UserId != _userId) {
                IsDefault = false;
            } else {
                IsDefault = true;
            }

        }

        private void ExpenseUser() {
            Expense = Context.Operations.Where(o => o.TricountId == TricountId && (o.InitiatorId == User.UserId || o.Repartitions.Any(r => r.UserId == User.UserId))).Count();


            if (Expense != 0) {
                IsExpense = true;
            } else {
                IsDefault = false;
            }
        }
    }
        
}
