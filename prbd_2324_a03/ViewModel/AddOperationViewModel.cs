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

        private Tricounts _tricount;
        public Tricounts Tricount {
            get => _tricount;
            set => SetProperty(ref _tricount, value);
        }

        public event Action RequestClose;

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
                Validate();
            });
        }

        public string TitleDialogWindows;

        private User _currentUser;
        public User ActualUser {
            get => _currentUser;
            set => SetProperty(ref _currentUser, value);
        }

        private bool _isNew;
        public bool IsNew {
            get => _isNew;
            set {
                SetProperty(ref _isNew, value);

                TitleOperationNameWindows();
            }
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
                AmountValid = v <= 0 || v == 0;
                o.Amount = v; 
                UpdateRepartition();
                Validate();
                UpdateIsCheckValid();
            });
        }

        private bool _amountValid = true;
        public bool AmountValid {
            get => _amountValid;
            set => SetProperty(ref _amountValid, value);
        }

        private bool _isCheckValid = true;
        public bool IsCheckValid {
            get => _isCheckValid;
            set => SetProperty(ref _isCheckValid, value);
        }


        private void TitleOperationNameWindows() {
            TitleDialogWindows = IsNew ? "Add Operation" : "Edit Operation";
        }

        public DateTime CreationTricountDate { get; set;}


        public ICommand AddOperation { get; set; }

        public event Func<bool> DeleteOperations;


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

            // Vérifier si le titre est déjà utilisé par l'utilisateur courant dans le même Tricount
            if (Context.Operations.Any(t => t.TricountId == Tricount.Id && t.Title == Title && t.Id != Operation.Id)) {
                AddError(nameof(Title), "You already have a Operation with this title");
            }

            // Vérifier si la date de création est dans le futur
            if (CreationDate > DateTime.Now) {
                AddError(nameof(CreationDate), "The creation date can't be in the future.");
            }

            // Vérifier si la date de création est avant la date de création du Tricount
            if (CreationDate < Tricount.Created_at) {
                AddError(nameof(CreationDate), "The creation date can't be before the original creation date.");
            }

            // Vérifier si le titre est vide
            if (string.IsNullOrEmpty(Title)) {
                AddError(nameof(Title), "Title is required.");
            }

            // Vérifier si le montant est valide
            if (Amount <= 0) {
                AddError(nameof(Amount), "Amount must be greater than zero.");
            }


            return !HasErrors;
        }


        private bool CanSaveAction() {
            if (IsNew || !IsNew)
                return !string.IsNullOrEmpty(Title) && !AmountValid && !IsCheckValid && !HasErrors;
            return Operation != null && Operation.IsModified;
        }
        public ICommand DeleteOperationCommand { get; set; }

        public AddOperationViewModel(Tricounts tricount, Operations operation, bool isNew, User user) {
            Operation = operation;
            Tricount = tricount;
            IsNew = isNew;
            ActualUser = user; 

            CreationTricountDate = Tricount.Created_at;

      

            DeleteOperationCommand = new RelayCommand(DeleteOperation);

            AllUsersRepartition = new ObservableCollectionFast<RepartitionOperationViewModel>();
            OnRefreshData();
            AddOperation = new RelayCommand(AddAction, CanSaveAction);

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


        public void UpdateIsCheckValid() {
            IsCheckValid = AmountValid == false && AllUsersRepartition.All(user => !user.IsChecked);
        }



        protected override void OnRefreshData() {
            // Récupérer les utilisateurs par ordre alphabétique, en excluant l'utilisateur admin
            var users = Context.Users
                              .Where(u => u.Role == 0) // Exclure l'utilisateur admin
                              .OrderBy(u => u.Full_name) // Tri par ordre alphabétique du nom complet
                              .ToList();

            AllUsers = new ObservableCollectionFast<User>(users);

            foreach (var user in AllUsers) {
                var repartitionVM = new RepartitionOperationViewModel(Operation, user, this);
                AllUsersRepartition.Add(repartitionVM);

                // Mettre à jour IsChecked et SelectedValue pour chaque utilisateur
                repartitionVM.IsChecked = Operation.Repartitions.Any(r => r.User == user);

                // Récupérer la répartition associée à cet utilisateur pour cette opération
                var repartition = Operation.Repartitions.FirstOrDefault(r => r.User == user);

                if (repartition != null) {
                    repartitionVM.SelectedValue = repartition.Weight;
                }

            }

          

            if (!IsNew) {
                AmountValid = false;
            } else {
                CreationDate = DateTime.Now;
            }

          

            UpdateRepartition();

         


        }


        public void DeleteOperation() {
            if (!(DeleteOperations?.Invoke() ?? false)) return;

            Operation.Delete();
            NotifyColleagues(App.Messages.MSG_CANCEL_TRICOUNT, Tricount);
            NotifyColleagues(App.Messages.MSG_VIEWTRICOUNT_CHANGED, Tricount);
        }


        private void AddAction() {
            if (!Validate()) return;

            if (IsNew) {
                // Nouvelle opération
                Operation = new Operations {
                    Tricount = Tricount,
                    Creator = ActualUser,
                    OperationDate = CreationDate,
                    Title = Title,
                    Amount = Amount,
                    Repartitions = new List<Repartitions>()
                };

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
            } else {
                // Mise à jour de l'opération existante
                Operation.Title = Title;
                Operation.Amount = Amount;
                Operation.OperationDate = CreationDate;
                Operation.Tricount = Tricount;
                Operation.Creator = ActualUser;

                // Mise à jour des répartitions
                foreach (var repartitionVM in AllUsersRepartition) {
                    var repartition = Operation.Repartitions.FirstOrDefault(r => r.User == repartitionVM.SelectedParticipant);
                    if (repartition != null) {
                        repartition.Weight = repartitionVM.SelectedValue;
                    } else if (repartitionVM.IsChecked) {
                        var newRepartition = new Repartitions {
                            Operations = Operation,
                            User = repartitionVM.SelectedParticipant,
                            Weight = repartitionVM.SelectedValue
                        };
                        Operation.Repartitions.Add(newRepartition);
                    }
                }
            }

            Context.SaveChanges();
            NotifyColleagues(App.Messages.MSG_VIEWTRICOUNT_CHANGED, Tricount);
            NotifyColleagues(App.Messages.MSG_TRICOUNT_CHANGED, Tricount);
            RequestClose?.Invoke();
        }


    }
}
