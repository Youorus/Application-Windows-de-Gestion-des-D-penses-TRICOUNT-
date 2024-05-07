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

    public ICommand NewTricount { get; set; }


    //on n'informe l'application que l'on veut creer un nouveau tricount
    public TricountViewModel() {
        NewTricount = new RelayCommand(() => { NotifyColleagues(App.Messages.MSG_NEW_TRICOUNT, new Tricounts()); });
    }
   
}

