/*
  In App.xaml:
  <Application.Resources>
      <vm:ViewModelLocator xmlns:vm="clr-namespace:Lab4_Gym_Membership"
                           x:Key="Locator" />
  </Application.Resources>
  
  In the View:
  DataContext="{Binding Source={StaticResource Locator}, Path=ViewModelName}"

  You can also use Blend to do all this with the tool's support.
  See http://www.galasoft.ch/mvvm
*/

using CommonServiceLocator;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Messaging;
using System.Windows;
//using Microsoft.Practices.ServiceLocation;

namespace MembershipDemoMVVM.ViewModel
{
    /// <summary>
    /// This class contains static references to all the view models in the
    /// application and provides an entry point for the bindings.
    /// </summary>
    public class ViewModelLocator
    {
        /// <summary>
        /// Initializes a new instance of the ViewModelLocator class.
        /// </summary>
        public ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);
            // Registering viewmodels to locator
            SimpleIoc.Default.Register<MembershipMaintenanceViewModel>(); 
            SimpleIoc.Default.Register<AddMembershipViewModel>();
            SimpleIoc.Default.Register<ChangeMembershipViewModel>();
            Messenger.Default.Register<NotificationMessage>(this, NotifyUserMethod);
        }

        /// <summary>
        /// Sets up to use with data context of MembershipMaintenanceViewModel
        /// </summary>
        public MembershipMaintenanceViewModel Main
        {
            get => ServiceLocator.Current.GetInstance<MembershipMaintenanceViewModel>();
        }

        /// <summary>
        /// Sets up to use with data context of AddMembershipViewModel
        /// </summary>
        public AddMembershipViewModel Add
        {
            get => ServiceLocator.Current.GetInstance<AddMembershipViewModel>();
        }

        /// <summary>
        /// Sets up to use with data context of ChangeMembershipViewModel
        /// </summary>
        public ChangeMembershipViewModel Change
        {
            get => ServiceLocator.Current.GetInstance<ChangeMembershipViewModel>();
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        public void NotifyUserMethod(NotificationMessage message)
        {
            MessageBox.Show(message.Notification, "Notification");
        }

        

        public static void Cleanup()
        {
            // TODO Clear the ViewModels
        }
    }
}