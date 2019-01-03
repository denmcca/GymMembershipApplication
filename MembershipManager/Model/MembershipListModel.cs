using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;

namespace MembershipDemoMVVM.Model
{
    // Delegate to handle events
    public delegate void ChangeHandler(object sender, EventArgs args);

    /// <summary>
    /// Class that holds multiple memberships as a list.
    /// </summary>
    public class MembershipListModel
    {
        // Event to be called during change
        public event ChangeHandler Changed;

        // Observable collection that holds Membership data
        public ObservableCollection<MembershipModel> memberships;

        public string DataType = "MembershipList";
        private string filePath = "./membershipDB.txt";
        private string FileInUseMsg = "File current being used by other operation." +
                    "\nSkipping save process.";

        /// <summary>
        /// Default constructor for MemmbershipList class.
        /// </summary>
        public MembershipListModel()
        {
            memberships = new ObservableCollection<MembershipModel>();
        }

        /// <summary>
        /// Copy constructor
        /// </summary>
        /// <param name="mList"></param>
        public MembershipListModel(MembershipListModel mList)
        {
            memberships = new ObservableCollection<MembershipModel>();
            Changed = null;
            foreach (var m in mList)
            {
                Add(m);
            }
        }

        public IEnumerator<MembershipModel> GetEnumerator() => memberships.GetEnumerator();

        /// <summary>
        /// Indexer for membershipList.
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public MembershipModel this[int i]
        {
            get => memberships[i];
            set
            {
                memberships[i] = value;
                OnChanged(this, new MembershipListEventArgs(this));
            }
        }

        public int Count { get => memberships.Count; }

        // Properties for membershipList
        public ObservableCollection<MembershipModel> Memberships
        {
            get
            {
                return memberships;
            }
            set
            {
                memberships = new ObservableCollection<MembershipModel>();

                foreach (var m in value)
                {
                    Add(m);
                }
            }
        }

        /// <summary>
        /// Method that adds member to membershipList
        /// </summary>
        /// <param name="_member"></param>
        public void Add(MembershipModel _member)
        {
            memberships.Add(_member);
            OnChanged(this, new MembershipListEventArgs(this));
        }

        /// <summary>
        /// Method to remove member from membershipList
        /// </summary>
        /// <param name="_member"></param>
        public void Remove(MembershipModel _member)
        {
            // Note that when removing an element from ObservableCollection
            // it must be the exact element and not a copy of it as the argument.
            // Certain unexpected values may not prove equal.
            Memberships.Remove(_member);
            OnChanged(this, new MembershipListEventArgs(this));
        }

        /// <summary>
        /// Method that loads members from file from path
        /// </summary>
        public void Write() // Object = MembershipData // write from file
        {
            using (StreamReader file = new StreamReader(@filePath))
            {
                Memberships = new ObservableCollection<MembershipModel>();

                // Copies file data to list
                while(!file.EndOfStream)
                {
                    string first = file.ReadLine();
                    string last = file.ReadLine();
                    string email = file.ReadLine();
                    Memberships.Add(new MembershipModel(first, last, email));
                }
            }
            OnChanged(this, new MembershipListEventArgs(this));
        }

        /// <summary>
        /// Method that saves members to file at path
        /// </summary>
        public void Save() // write to file
        {
            try
            {
                using (StreamWriter file = new StreamWriter(@filePath, append: false))
                {
                    // For each member in list write each attribute on its own line
                    foreach (var membership in memberships)
                    {
                        file.WriteLine(membership.FirstName);
                        file.WriteLine(membership.LastName);
                        file.WriteLine(membership.Email);
                    }
                }
            }
            catch (IOException e)
            {
                System.Console.WriteLine(FileInUseMsg);
            }
        }

        /// <summary>
        /// Method that checks if membershipList contains member
        /// </summary>
        /// <param name="m"></param>
        /// <returns></returns>
        public bool Contains(MembershipModel m)
        {
            foreach (var mem in Memberships)
                if (mem.FirstName.Equals(m.FirstName))
                    if (mem.LastName.Equals(m.LastName))
                        if (mem.Email.Equals(m.Email))
                            return true;

            return false;
        }

        /// <summary>
        /// Overload for add operator
        /// </summary>
        /// <param name="_membershipList"></param>
        /// <param name="_member"></param>
        /// <returns></returns>
        public static MembershipListModel operator +(MembershipListModel _mL, MembershipModel _m)
        {
            if (!_mL.Contains(_m))
                _mL.Add(_m);

            return _mL;
        }

        /// <summary>
        /// Overload for subtract operator
        /// </summary>
        /// <param name="_membershipList"></param>
        /// <param name="_member"></param>
        /// <returns></returns>
        public static MembershipListModel operator -(MembershipListModel _mL, MembershipModel _m)
        {
            if(_mL.Contains(_m))
                _mL.Remove(_m);

            return _mL;
        }

        /// <summary>
        /// Method that returns index of membership that matches input
        /// Returns -1 if membership not found
        /// </summary>
        /// <param name="m"></param>
        /// <returns></returns>
        public int IndexOf(MembershipModel m) => Memberships.IndexOf(m);

        /// <summary>
        /// Event method for data changes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        protected virtual void OnChanged(object sender, EventArgs args)
        {
            ChangeHandler handler = Changed;
            handler?.Invoke(this, (MembershipListEventArgs)args);
        }
    }

    /// <summary>
    /// Event args for changed data event
    /// </summary>
    public class MembershipListEventArgs : EventArgs
    {
        // Message that is sent to event handler.
        public string Msg { get; private set; }

        // Message assigned data type of object.
        public MembershipListEventArgs(MembershipListModel mList)
            => Msg = mList.DataType;
    }
}
