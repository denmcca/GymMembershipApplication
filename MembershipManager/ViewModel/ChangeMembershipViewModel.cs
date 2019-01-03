using System;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using MembershipDemoMVVM.Model;
using MembershipDemoMVVM.View;

namespace MembershipDemoMVVM.ViewModel
{
    public class ChangeMembershipViewModel : ViewModelBase, IClosable
    {
        private readonly string UpdateMsg = "Update";
        private readonly string AllFieldsReqMsg = "All fields required.";
        private readonly string DeleteMsg = "Delete";

        /// <summary>
        /// Holds data for input
        /// </summary>
        public MembershipModel Membership { get; private set; }

        /// <summary>
        /// Handles button binds
        /// </summary>
        public ICommand CancelCommand { get; private set; }
        public ICommand SaveCommand { get; private set; }
        public ICommand DeleteCommand { get; private set; }

        /// <summary>
        /// Default constructor for Change View Model
        /// </summary>
        public ChangeMembershipViewModel()
        {
            Membership = new MembershipModel();
            Messenger.Default.Register<MessageMemberModel>(this, ReceivedMessageMember);

            CancelCommand = new RelayCommand<IClosable>(CancelMethod);
            DeleteCommand = new RelayCommand<IClosable>(DeleteMethod);
            SaveCommand = new RelayCommand<IClosable>(SaveMethod);
        }

        /// <summary>
        /// Method that receives message and model
        /// </summary>
        /// <param name="msg"></param>
        private void ReceivedMessageMember(MessageMemberModel msg)
        {
            if(msg.Selection == "ListSelect")
                Membership = new MembershipModel(msg.Membership);
        }
        
        // Method for Save Command. When Save button is pressed.
        private void SaveMethod(IClosable window)
        {
            // Checks for empty fields.
            if (Membership.FirstName.Length != 0 
                && Membership.LastName.Length != 0 
                && Membership.Email.Length != 0)
            {
                // No empty fields found. Send Membership and keyword for processing.
                Messenger.Default.Send(new MessageMemberModel(Membership, UpdateMsg));

                // Go back to Main and set membership to null for next time.
                BackToMaintenance(window);
                Membership = null;
            }
            else Messenger.Default.Send<NotificationMessage>(new NotificationMessage(AllFieldsReqMsg));
        }

        // Method for Cancel Command for cancel button.
        private void CancelMethod(IClosable window)
        {
            BackToMaintenance(window);
        }

        // Method for Delete Command for delete button.
        private void DeleteMethod(IClosable window)
        {
            Messenger.Default.Send(new MessageMemberModel(Membership, DeleteMsg));
            BackToMaintenance(window);
        }

        // Method for opening Main View and closing Change View.
        private void BackToMaintenance(IClosable window)
        {

            var mainView = new MembershipMaintenance();
            mainView.Show();

            if (window != null)
            {
                window.Close();
            }
        }
        
        // Method required for implementing IClosable.
        public void Close(){}
    }
}
