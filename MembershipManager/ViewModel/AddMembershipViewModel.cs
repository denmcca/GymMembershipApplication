using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using MembershipDemoMVVM.Model;
using MembershipDemoMVVM.View;
using System;
using System.Windows.Input;

namespace MembershipDemoMVVM.ViewModel
{
    /// <summary>
    /// Class that handles AddMembershipView logic
    /// </summary>
    public class AddMembershipViewModel : ViewModelBase, IClosable
    {
        // Default property that holds input data.
        public MembershipModel Membership { get; private set; }

        // Message to notify user to fill in all fields.
        private string EmptyFieldMsg = "Must complete all Fields";

        // Message sent to process input.
        private string AddOption = "Add";

        /// <summary>
        /// Handles button binds
        /// </summary>
        public ICommand CancelCommand { get; private set; }
        public ICommand SaveCommand { get; private set; }

        /// <summary>
        /// Default constructor.
        /// </summary>
        public AddMembershipViewModel()
        {
            Membership = new MembershipModel();

            CancelCommand = new RelayCommand<IClosable>(CancelMethod);
            SaveCommand = new RelayCommand<IClosable>(SaveMethod);
        }

        /// <summary>
        /// Method for SaveCommand that sends user input data using messenger
        /// </summary>
        /// <param name="window"></param>
        private void SaveMethod(IClosable window)
        {
            // Checks each field for empty.
            if (Membership.FirstName.Length != 0)
                if (Membership.LastName.Length != 0)
                    if (Membership.Email.Length != 0)
                    {
                        // No fields are empty.
                        // Process data and return to Main.
                        Messenger.Default.Send(new MessageMemberModel(Membership, AddOption));
                        BackToMaintenance(window);
                        Membership = new MembershipModel();
                        return;
                    }

            // Field found to be empty.
            Messenger.Default.Send<NotificationMessage>(new NotificationMessage(EmptyFieldMsg));
        }

        /// <summary>
        /// Method for Cancel Command that resets membership and calls method 
        /// to return to Maintenance.
        /// </summary>
        /// <param name="window"></param>
        private void CancelMethod(IClosable window)
        {
            Membership = new MembershipModel();
            BackToMaintenance(window);
        }

        /// <summary>
        /// Method that contains code to open Maintenance window
        /// and close Add window.
        /// </summary>

        private void BackToMaintenance(IClosable window)
        {
            var mainWindow = new MembershipMaintenance();
            mainWindow.Show();
            window.Close();
        }

        // Close method for IClosable.
        // Must be public.
        public void Close(){}
    }
}