using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using prbd_2324_a03.Model;
using PRBD_Framework;

namespace prbd_2324_a03.ViewModel;

public class TricountViewModel : ViewModelCommon
    {

    public ICommand ReloadDataCommand { get; set; }
    public ICommand ResetDataCommand { get; set; }


    public TricountViewModel() {

        ReloadDataCommand = new RelayCommand(() => {
            // refuser un reload s'il y a des changements en cours
            if (Context.ChangeTracker.HasChanges()) return;
            // permet de renouveller le contexte EF
            App.ClearContext();
            // notifie tout le monde qu'il faut rafraîchir les données
            NotifyColleagues(ApplicationBaseMessages.MSG_REFRESH_DATA);
        });

        ResetDataCommand = new RelayCommand(() => {
            NotifyColleagues(App.Messages.MSG_RESET, CurrentUser);
        });



}

    public static string TitleWindows {
        get => $"My Tricount ({CurrentUser?.Mail})";
    }

}

