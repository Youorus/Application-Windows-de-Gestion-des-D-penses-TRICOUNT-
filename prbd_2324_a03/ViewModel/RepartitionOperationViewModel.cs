using Azure;
using prbd_2324_a03.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Xceed.Wpf.Toolkit.Calculator;

namespace prbd_2324_a03.ViewModel
{
    public class RepartitionOperationViewModel : ViewModelCommon
    {

        private Operations _operation;
        public Operations Operation {
            get => _operation;
            set => SetProperty(ref _operation, value);
        }

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

        private double _calculatedAmount = 0;
        public double CalculatedAmount {
            get => _calculatedAmount;
            set => SetProperty(ref _calculatedAmount, value);
        }

        private User _selectedParticipant;
        public User SelectedParticipant {
            get => _selectedParticipant;
            set => SetProperty(ref _selectedParticipant, value);
        }

        private readonly AddOperationViewModel addOperationViewModel;

        private int _selectedValue;
        public int SelectedValue {
            get => _selectedValue;
            set {
                SetProperty(ref _selectedValue, value);
                addOperationViewModel.UpdateRepartition();
            }
        }



        public string Fullname { get; set; }
        public RepartitionOperationViewModel(Operations operations, User user, AddOperationViewModel vm) {
            Operation = operations;
            SelectedParticipant = user;
            addOperationViewModel = vm;
            OnRefreshData();
        }

        protected override void OnRefreshData() { 
        
            Fullname = SelectedParticipant.Full_name;

        }




        }
}
