using System;
using System.ComponentModel;
using System.IO;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using MembershipDemoMVVM.Model;
using MembershipDemoMVVM.View;

namespace MembershipDemoMVVM.ViewModel
{
    /// <summary>
    /// ViewModel that handles logic for main window
    /// </summary>
    public class MembershipMaintenanceViewModel : ViewModelBase, INotifyPropertyChanged, IClosable
    {
        // Object that contains collection of memberships and functions to 
        // facilitate execution
        public MembershipListModel Memberships { get; private set; }

        // Object that hold the values for the membership data that is currently
        // selected on listbox. Maybe modify to make set private?
        public MembershipModel SelectedMember { get; set; }

        // Strings that contain messages for message box that notifies user
        //private string DBUpdateMsg = "Database Updated";
        private string MembershipsLoadMsg = "Memberships Loaded";
        private string MembershipNotFoundMsg = "Error: Membership not found." +
            "/nNot action has taken place";
        private string MergingMembershipsMsg = "Merging memberships.";
        private string MemberAlreadyExistMsg = "Membership already exists.";
        private string DeleteMsg = "Membership removed.";


        // String that contains path of text file which acts as database for membership data
        private string filepath = "./membershipDB.txt";

        /// <summary>
        /// Objectst that facilitate the communications between view and view model
        /// </summary>
        public ICommand AddCommand { get; private set; }
        public ICommand ChangeCommand { get; private set; } //1
        public ICommand ExitCommand { get; private set; }
        public ICommand LoadCommand { get; private set; }
        //public ICommand SaveCommand { get; private set; }

        /// <summary>
        /// Constructor for main view model
        /// </summary>
        public MembershipMaintenanceViewModel()
        {
            // checks if database file is present. If not, creates file.
            if (!File.Exists(filepath))
            {
                using (StreamWriter s = new StreamWriter(filepath, append: false)){}
            }
        
            Memberships = new MembershipListModel();
            SelectedMember = new MembershipModel();

            // instantiating view command objects
            AddCommand = new RelayCommand<IClosable>(AddMethod);
            ChangeCommand = new RelayCommand<IClosable>(ChangeMethod); //2
            ExitCommand = new RelayCommand<IClosable>(ExitMethod);
            LoadCommand = new RelayCommand(LoadMethod);
            //SaveCommand = new RelayCommand(SaveMethod); // removed to better fit assignment description

            // instantiating messenger to receive notifications from other view models
            Messenger.Default.Register<MessageMemberModel>(this, ReceiveMembership);

            // Initializing window for initial field population (work-around)
            // first click on listbox does not populate name and email fields otherwise
            var initChangeWin = new ChangeMembershipView();
            initChangeWin.Hide();
            initChangeWin.Close();

            // Wiring Changed event to event handler. 
            Memberships.Changed += RaisePropertyChanges;

            // Wiring Changed event to Save
            Memberships.Changed += SaveMethod;
        }

        /// <summary>
        /// Method for LoadCommand that initiates for the database to transfer members to local
        /// object list
        /// </summary>
        private void LoadMethod()
        {
            // Loads data from file into memberships.
            Memberships.Write();

            // Notify user that membership data is loaded.
            Messenger.Default.Send<NotificationMessage>(new NotificationMessage(MembershipsLoadMsg));
        }

        /// <summary>
        /// Method for AddCommand that opens the closable add membership view
        /// </summary>
        /// <param name="window"></param>
        private void AddMethod(IClosable window)
        {
            // Open AddView and close MaintenanceView
            ChangeView(window, "Add");
        }

        /// <summary>
        /// Method for ChangeCommand that opens the window for updating or
        /// deleting listbox entry
        /// </summary>
        /// <param name="window"></param>
        private void ChangeMethod(IClosable window)
        {
            if (SelectedMember != null)
            {
                // Notify
                Messenger.Default.Send(new MessageMemberModel(SelectedMember, "ListSelect"));

                ChangeView(window, "Change");
            }
        }

        /// <summary>
        /// Method for ExitCommand that closes associated view's window
        /// </summary>
        /// <param name="window"></param>
        private void ExitMethod(IClosable window)
        {
            //// Update database on exit
            //SaveMethod(); // Removed, auto-saves after list update.

            if (window != null)
            {
                window.Close();
            }
        }

        /// <summary>
        /// Method to open given view and close maintenance view.
        /// </summary>
        /// <param name="window"></param>
        /// <param name="windowName"></param>
        private void ChangeView(IClosable window, string windowName)
        {
            // If true open AddView
            if (windowName.Equals("Add"))
            {
                var changeView = new AddMembershipView();
                changeView.Show();
            }
            // If true open ChangeView
            else if (windowName.Equals("Change"))
            {
                var changeView = new ChangeMembershipView();
                changeView.Show();
            }

            // Close Main View
            window.Close();
        }

        /// <summary>
        /// Method that receives messenger calls to do embedded functions
        /// </summary>
        /// <param name="member"></param>
        private void ReceiveMembership(MessageMemberModel member)
        {
            // For updating membership
            if (member.Selection == "Update")
            {
                if (!Memberships.Contains(member.Membership))
                {
                    // Using indexer.
                    Memberships[Memberships.IndexOf(SelectedMember)] = member.Membership;
                }
                else if (!SelectedMember.Equals(member.Membership))
                {
                    // Notify user about duplicate membership
                    Messenger.Default.Send<NotificationMessage>(new
                        NotificationMessage($"{MemberAlreadyExistMsg}\n" +
                        $"{MergingMembershipsMsg}"));

                    // Removing outdated data using minus operator.
                    Memberships -= SelectedMember;
                }
            }
            // For adding membership
            else if (member.Selection == "Add")
            {
                // Checks if membership is already in list.
                if (Memberships.Contains(member.Membership))
                {
                    // Notifies user that it exists then returns.
                    Messenger.Default.Send<NotificationMessage>(new
                        NotificationMessage(MemberAlreadyExistMsg));
                    return;
                }

                // Membership not found then add using add operator.
                Memberships += member.Membership;
            }
            // For deleting membership
            else if (member.Selection == "Delete")
            {
                // Confirms that membership is in list
                if (Memberships.Contains(member.Membership))
                {
                    // Removes found membership.
                    Memberships.Remove(SelectedMember);
                    Messenger.Default.Send(new NotificationMessage(DeleteMsg));
                }
            }
            else if (member.Selection == "ListSelect")
            {
                return;
            }
            else
            {
                Messenger.Default.Send<NotificationMessage>(new NotificationMessage(MembershipNotFoundMsg));
                return;
            }
        }

        /// <summary>
        /// Method required for IClosable interface which facilitates the
        /// closing of view windows
        /// </summary>
        public void Close(){}

        /// <summary>
        /// Method for SaveCommand that initiates the membership list to be save to database.
        /// Set up to be wired with Changed event from MembershipList.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void SaveMethod(object sender, EventArgs args) // save to file
        {
            // Saves memberships data to file.
            Memberships.Save();

            //// Sends message to activate messagebox to notify user that database was updated.
            //Messenger.Default.Send<NotificationMessage>(new NotificationMessage(DBUpdateMsg)); // Removed
        }

        /// <summary>
        /// Method to notify Listbox that members have been updated.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        public void RaisePropertyChanges(object sender, EventArgs args)
        {
            RaisePropertyChanged(() => Memberships);
        }
    }
}
