using Microsoft.EntityFrameworkCore.Metadata;
using prbd_2324_a03.Model;
using PRBD_Framework;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace prbd_2324_a03.ViewModel
{
    public class AddOperationViewModel : DialogViewModelBase<Operations, PridContext> {

        private readonly int _userId = 1;
        private Tricounts _tricount;
        public Tricounts Tricount {
            get => _tricount;
            set => SetProperty(ref _tricount, value);
        }

        private ObservableCollectionFast<User> _allUsers;
        public ObservableCollectionFast<User> AllUsers {
            get => _allUsers;
            set => SetProperty(ref _allUsers, value);
        }

        private ObservableCollectionFast<RepartitionOperationViewModel> _allUsersRepartition = new ObservableCollectionFast<RepartitionOperationViewModel>();
        public ObservableCollectionFast<RepartitionOperationViewModel> AllUsersRepartition {
            get => _allUsersRepartition;
            set => SetProperty(ref _allUsersRepartition, value);
        }

        private Operations _operation;
        public Operations Operation {
            get => _operation;
            set => SetProperty(ref _operation, value);
        }

        private DateTime _creationDate = DateTime.Now;
        public DateTime CreationDate {
            get => (DateTime)(Operation?.OperationDate);
            set => SetProperty(Operation.OperationDate, value, Operation, (t, v) => {
                t.OperationDate = v;

            });
        }

        private User _currentUser;
        public User ActualUser {
            get => _currentUser;
            set => SetProperty(ref _currentUser, value);
        }

        private bool _isNew;
        public bool IsNew {
            get => _isNew;
            set => SetProperty(ref _isNew, value);
        }

        private string _title;
        public string Title {
            get => Operation?.Title;
            set => SetProperty(Operation.Title, value, Operation, (o, v) => {
                o.Title = v;
                Validate();
            });
        }

        private double _amount;
        public double Amount {
            get => Operation.Amount;
            set => SetProperty(Operation.Amount, value,Operation, (o, v) => {
                o.Amount = v; // Si v est null, assignez 0 à la propriété Amount de votre modèle de données
                UpdateRepartition();
                Validate();
            });
        }

     



        public ICommand AddOperation { get; set; }


        private bool _isChecked;
        public bool IsChecked {
            get => _isChecked;
            set {
                SetProperty(ref _isChecked, value);
                // Mettre à jour la propriété ShowAmount en fonction de l'état de la case à cocher
                ShowAmount = _isChecked;
            }
        }

        private bool _showAmount;
        public bool ShowAmount {
            get => _showAmount;
            set => SetProperty(ref _showAmount, value);
        }

        private double _calculatedAmount;
        public double CalculatedAmount {
            get => _calculatedAmount;
            set => SetProperty(ref _calculatedAmount, value);
        }


        private User _selectedParticipant;
        public User SelectedParticipant {
            get => _selectedParticipant;
            set => SetProperty(ref _selectedParticipant, value);
        }

        private int _selectedValue;
        public int SelectedValue {
            get => _selectedValue;
            set => SetProperty(ref _selectedValue, value);
        }



        public override bool Validate() {
            ClearErrors();
            Operation.Validate();
            AddErrors(Operation.Errors);

           
        

            return !HasErrors;
        }



        public AddOperationViewModel(Tricounts tricount, Operations operation, bool isNew) {
            Operation = operation;
            Tricount = tricount;
            IsNew = isNew;
            AllUsersRepartition = new ObservableCollectionFast<RepartitionOperationViewModel>();
            OnRefreshData();


        }

        public void UpdateRepartition() {
            if (Operation == null || Amount <= 0) {
                foreach (var userRepartition in AllUsersRepartition) {
                    userRepartition.CalculatedAmount = 0;
                }
                return;
            }

            double totalWeight = AllUsersRepartition.Sum(u => u.SelectedValue);
            if (totalWeight == 0) {
                foreach (var userRepartition in AllUsersRepartition) {
                    userRepartition.CalculatedAmount = 0;
                }
                return;
            }

            double totalAmount = Amount; // Assuming Amount is the total expense
            foreach (var userRepartition in AllUsersRepartition) {
                userRepartition.CalculatedAmount = (userRepartition.SelectedValue / totalWeight) * totalAmount;
            }
        }


        protected override void OnRefreshData() {
            // Récupérer les utilisateurs par ordre alphabétique, en excluant l'utilisateur admin
            var users = Context.Users
                              .Where(u => u.Role == 0) // Exclure l'utilisateur admin
                              .OrderBy(u => u.Full_name) // Tri par ordre alphabétique du nom complet
                              .ToList();

            AllUsers = new ObservableCollectionFast<User>(users);

            foreach (var user in AllUsers) {
                var x = new RepartitionOperationViewModel(Operation, user, this);
                AllUsersRepartition.Add(x);
            }

            // Définir l'utilisateur actuel sur le premier utilisateur de la liste
            _currentUser = AllUsers.FirstOrDefault();

            CreationDate = DateTime.Now;

            UpdateRepartition();

            AddOperation = new RelayCommand(AddAction, () => !HasErrors);
        }

        private void AddAction() {
            if (Operation != null) {
                Operation.Tricount = Tricount;
                Operation.Creator = ActualUser;
                Operation.OperationDate = CreationDate;

                // Ajout des répartitions
                foreach (var repartitionVM in AllUsersRepartition) {
                    if (repartitionVM.IsChecked) {
                        var repartition = new Repartitions {
                            Operations = Operation,
                            User = repartitionVM.SelectedParticipant,
                            Weight = repartitionVM.SelectedValue
                        };
                        Operation.Repartitions.Add(repartition);
                    }
                }

                Context.Operations.Add(Operation);
                Context.SaveChanges();
                NotifyColleagues(App.Messages.MSG_VIEWTRICOUNT_CHANGED, Tricount);
            }
        }
    
}
}
