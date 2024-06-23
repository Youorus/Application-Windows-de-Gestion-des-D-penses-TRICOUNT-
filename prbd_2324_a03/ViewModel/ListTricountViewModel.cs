using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using PRBD_Framework;
using prbd_2324_a03.Model;
using Newtonsoft.Json.Linq;

namespace prbd_2324_a03.ViewModel
{
    public class ListTricountViewModel : ViewModelCommon
    {
        private readonly int _userId = CurrentUser.UserId;


        private string _filter;
        public string Filter {
            get => _filter;
            set { SetProperty(ref _filter, value, OnRefreshData);
                FilterExecute(value);
            }
        }

        public ICommand ClearFilter { get; set; }

        public ICommand NewTricount { get; set; }
        public ICommand TricountDetailsView { get; set; }

        public ObservableCollection<TricountCardViewModel> Tricounts { get; set; } = new ObservableCollection<TricountCardViewModel>();

         public ObservableCollection<TricountCardViewModel> FilterTricounts { get; set; } = new ObservableCollection<TricountCardViewModel>();
        public ListTricountViewModel() {

            ClearFilter = new RelayCommand(ClearAction);

            NewTricount = new RelayCommand(() => { NotifyColleagues(App.Messages.MSG_NEW_TRICOUNT, new Tricounts()); });

            TricountDetailsView = new RelayCommand<TricountDetailsViewModel>(vm => { NotifyColleagues(App.Messages.MSG_DISPLAY_TRICOUNT, vm.Tricount); });

            Register<Tricounts>(App.Messages.MSG_TRICOUNT_CHANGED, Tricount => OnRefreshData());

            
            OnRefreshData();
           
        }

        public void ClearAction() {

            FilterTricounts.Clear();
            OnRefreshData();
            Filter = "";
        }



        public void FilterExecute(string val) {
            FilterTricounts.Clear();

            foreach (var tricount in Tricounts) {
                // Filtrer par titre de tricount
                if (!string.IsNullOrEmpty(tricount.Title) && tricount.Title.Contains(val, StringComparison.OrdinalIgnoreCase)) {
                    if (!FilterTricounts.Contains(tricount)) {
                        FilterTricounts.Add(tricount);
                    }
                }

                // Filtrer par nom du créateur
                if (tricount.Creator != null && !string.IsNullOrEmpty(tricount.Creator.Full_name) && tricount.Creator.Full_name.Contains(val, StringComparison.OrdinalIgnoreCase)) {
                    if (!FilterTricounts.Contains(tricount)) {
                        FilterTricounts.Add(tricount);
                    }
                }

                var operations = tricount.Operations;

                foreach (var operation in operations) {
                    if (!string.IsNullOrEmpty(operation.Title) && operation.Title.Contains(val, StringComparison.OrdinalIgnoreCase)) {
                        if (!FilterTricounts.Contains(tricount)) {
                            FilterTricounts.Add(tricount);
                        }
                        break; // Sortir de la boucle dès qu'une opération correspondante est trouvée
                    }
                }


                // Filtrer par description du tricount
                if (!string.IsNullOrEmpty(tricount.Description)) {
                    if (tricount.Description.Contains(val, StringComparison.OrdinalIgnoreCase)) {
                        if (!FilterTricounts.Contains(tricount)) {
                            FilterTricounts.Add(tricount);
                        }
                    }
                } else {
                    // Ajouter les tricounts avec description vide uniquement si la valeur de filtre est "description vide"
                    if (val.Equals("description vide", StringComparison.OrdinalIgnoreCase)) {
                        if (!FilterTricounts.Contains(tricount)) {
                            FilterTricounts.Add(tricount);
                        }
                    }
                }
            }

            if (FilterTricounts.Count > 0) {
                Tricounts.Clear();
                foreach (var tricount in FilterTricounts) {
                    Tricounts.Add(tricount);
                }
            } else {
                // Si aucun filtre n'est appliqué ou aucun résultat n'est trouvé, rafraîchir la liste complète
                OnRefreshData();
            }
        }



        protected override void OnRefreshData() {
            Tricounts.Clear();

            if (CurrentUser.Role == Role.Administrator) {
                var tricounts = Context.Tricounts.ToList();
                foreach (var tricount in tricounts) {
                    var viewModel = new TricountCardViewModel(tricount);
                    Tricounts.Add(viewModel);
                }
            }


                var tricountsCreatedByUser = Context.Tricounts
                    .Where(t => t.Creator == CurrentUser.UserId)
                    .ToList();

                var tricountsParticipatedByUser = Context.Tricounts
                    .Where(t => t.Subscriptions.Any(s => s.UserId == CurrentUser.UserId))
                    .ToList();

                var allTricounts = tricountsCreatedByUser
                    .Union(tricountsParticipatedByUser)
                    .Select(t => new {
                        Tricount = t,
                        Creator = t.CreatorTricount,
                        LastOperationDate = t.Operations.Any() ? t.Operations.Max(o => o.OperationDate) : (DateTime?)null,
                        CreatedAt = t.Created_at
                    })
                    .OrderByDescending(t => t.LastOperationDate ?? t.CreatedAt)
                    .ToList();

                foreach (var tricountInfo in allTricounts) {
                    var viewModel = new TricountCardViewModel(tricountInfo.Tricount);
                    Tricounts.Add(viewModel);
                }
            
        }






    }
}
