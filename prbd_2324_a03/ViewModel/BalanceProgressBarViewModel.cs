using prbd_2324_a03.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows;

namespace prbd_2324_a03.ViewModel
{
    public class BalanceProgressBarViewModel : ViewModelCommon
    {

        private readonly int _userId = CurrentUser.UserId;

        private User _user;
        public User User {
            get => _user;
            set => SetProperty(ref _user, value);
        }
        private bool _isDefault;
        public bool IsDefault {
            get => _isDefault;
            set => SetProperty(ref _isDefault, value);
        }

        private Brush _progressBarBackground;
        public Brush ProgressBarBackground {
            get => _progressBarBackground;
            set => SetProperty(ref _progressBarBackground, value);
        }

        private FlowDirection _progressBarFlowDirection;
        public FlowDirection ProgressBarFlowDirection {
            get => _progressBarFlowDirection;
            set => SetProperty(ref _progressBarFlowDirection, value);
        }

        private Tricounts _tricount;
        public Tricounts Tricount {
            get => _tricount;
            set => SetProperty(ref _tricount, value);
        }

        private string _fullname;
        public string Fullname {
            get => _fullname;
            set => SetProperty(ref _fullname, value);
        }

        private double _userBalance;
        public double UserBalance {
            get => _userBalance;
            set {
                if (SetProperty(ref _userBalance, value)) {
                    UpdateProgressBarSettings();
                }
            }
        }

        public BalanceProgressBarViewModel(User user, Tricounts tricount) {
            User = user;
            Tricount = tricount;
            IsDefault = (user.UserId == _userId);
            OnRefreshData();

            Register<Tricounts>(App.Messages.MSG_VIEWTRICOUNT_CHANGED, tricount => OnRefreshData());
        }

        private void CalculateUserBalance() {
            var tricountCard = new TricountCardViewModel(Tricount);
            UserBalance = tricountCard.GetUserBalanceInTricount(User.UserId, Tricount.Id);
        }

        private void UpdateProgressBarSettings() {
            if (UserBalance > 0) {
                ProgressBarBackground = Brushes.LightGreen;
                ProgressBarFlowDirection = FlowDirection.LeftToRight;
            } else if (UserBalance < 0) {
                ProgressBarBackground = Brushes.LightPink;
                ProgressBarFlowDirection = FlowDirection.RightToLeft;
            } else {
                ProgressBarBackground = Brushes.LightGray;
                ProgressBarFlowDirection = FlowDirection.LeftToRight;
            }
        }

        protected override void OnRefreshData() {
            Fullname = User.Full_name;
            CalculateUserBalance();
        }
    }
}
