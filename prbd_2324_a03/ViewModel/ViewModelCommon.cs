using prbd_2324_a03.Model;
using System.Windows;

namespace prbd_2324_a03.ViewModel;

public abstract class ViewModelCommon : PRBD_Framework.ViewModelBase<User, PridContext> 
{
    public static bool IsAdmin => App.IsLoggedIn && App.CurrentUser is Administrator;

    public static bool IsNotAdmin => !IsAdmin;
}