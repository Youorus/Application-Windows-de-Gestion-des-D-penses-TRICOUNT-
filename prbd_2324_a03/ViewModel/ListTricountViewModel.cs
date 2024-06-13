using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using PRBD_Framework;
using prbd_2324_a03.Model;

namespace prbd_2324_a03.ViewModel
{
    public class ListTricountViewModel : ViewModelCommon
    {
        private readonly int _userId = CurrentUser.UserId;

      

        public ICommand NewTricount { get; set; }
        public ICommand TricountDetailsView { get; set; }

        public ObservableCollection<TricountCardViewModel> Tricounts { get; set; } = new ObservableCollection<TricountCardViewModel>();

        public ListTricountViewModel() {

            NewTricount = new RelayCommand(() => { NotifyColleagues(App.Messages.MSG_NEW_TRICOUNT, new Tricounts()); });

            TricountDetailsView = new RelayCommand<TricountDetailsViewModel>(vm => { NotifyColleagues(App.Messages.MSG_DISPLAY_TRICOUNT, vm.Tricount); });

            OnRefreshData();
            Register<Tricounts>(App.Messages.MSG_TRICOUNT_CHANGED, tricount => OnRefreshData());
        }

        protected override void OnRefreshData() {
            Tricounts.Clear();

            // Obtenez les tricounts créés par l'utilisateur
            var tricountsCreatedByUser = Context.Tricounts
                .Where(t => t.Creator == CurrentUser.UserId);

            // Obtenez les tricounts auxquels l'utilisateur participe
            var tricountsParticipatedByUser = Context.Tricounts
                .Where(t => t.Subscriptions.Any(s => s.UserId == CurrentUser.UserId));

            // Combinez les deux ensembles de tricounts
            var allTricounts = tricountsCreatedByUser
                .Union(tricountsParticipatedByUser)
                .Select(t => new {
                    Tricount = t,
                    LastOperationDate = t.Operations.Any() ? t.Operations.Max(o => o.OperationDate) : (DateTime?)null,
                    CreatedAt = t.Created_at
                })
                .OrderByDescending(t => t.LastOperationDate ?? t.CreatedAt)
                .ToList();

            // Ajoutez chaque tricount trié à la collection
            foreach (var tricountInfo in allTricounts) {
                var viewModel = new TricountCardViewModel(tricountInfo.Tricount);
                Tricounts.Add(viewModel);
            }
        }



    }
}
