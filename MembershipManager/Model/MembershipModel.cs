using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using System;

namespace MembershipDemoMVVM.Model
{
    /// <summary>
    /// Class for individual memberships
    /// </summary>
    public class MembershipModel : ObservableObject
    {
        // Exception strings
        private const string ExceptionInputTooLong = "Exception: Input must be 25 characters or less.";
        private const string ExceptionInvalidEmailFormat = "Exception: Invalid e-mail format.";
        private const string ExceptionInputEmpty = "Exception: Field cannot be left empty.";


        // Membership data
        private string firstName;
        private string lastName;
        private string email;

        // Data type
        public string DataType = "Member";

        /// <summary>
        /// Default constructor for Member class.
        /// </summary>
        public MembershipModel()
        {
            firstName = "";
            lastName = "";
            email = "";
        }

        /// <summary>
        /// Parametrized constructor for Member class.
        /// </summary>
        /// <param name="_firstName"></param>
        /// <param name="_lastName"></param>
        /// <param name="_email"></param>
        public MembershipModel(string _firstName, string _lastName, string _email)
        {
            firstName = _firstName;
            lastName = _lastName;
            email = _email;
        }

        /// <summary>
        /// Copy constructor.
        /// </summary>
        /// <param name="_membership"></param>
        public MembershipModel(MembershipModel _membership)
        {
            firstName = _membership.FirstName;
            lastName = _membership.LastName;
            email = _membership.Email;
        }

        /// <summary>
        /// Property for firstName
        /// </summary>
        public string FirstName
        {
            get => firstName;
            set
            {
                try
                {
                    // Validating input.
                    ValidateString(value);
                    firstName = value;
                }
                catch (Exception e)
                {
                    // If input does not meet validation notify user using SendException.
                    SendException(e);
                    return;
                }
            }
        }

        /// <summary>
        /// Property for lastName
        /// </summary>
        public string LastName
        {
            get => lastName;
            set
            {
                try
                {
                    // Validates input.
                    ValidateString(value); 
                    lastName = value;
                }
                catch (Exception e)
                {
                    // If input does not meet validation then notify user using SendException.
                    SendException(e);
                    return;
                }
            }
        }

        /// <summary>
        /// Property for email
        /// </summary>
        public string Email
        {
            get => email;
            set
            {
                try
                {
                    // Checking input.
                    ValidateString(value);
                    ValidateEmailString(value);
                    email = value;
                }
                catch (Exception e)
                {
                    // If input does not qualify notify user using SendException.
                    SendException(e);
                    return;
                }
            }
        }

        /// <summary>
        /// Method to return string with all membership attributes
        /// </summary>
        /// <returns></returns>
        public string GetDisplayText() => $"first = {firstName}, last = {lastName} - email = {email}";

        /// <summary>
        /// Sends Message to notify user of user input exception
        /// </summary>
        /// <param name="e"></param>
        private void SendException(Exception e)
        {
            Messenger.Default.Send(new NotificationMessage(e.Message));
        }

        /// <summary>
        /// Verifies user input string length
        /// </summary>
        /// <param name="_input"></param>
        private void ValidateString(String _input)
        {
            if (_input.Length < 1)
                throw new Exception(ExceptionInputEmpty);
            if (_input.Length > 25)
                throw new Exception(ExceptionInputTooLong);
        }

        /// <summary>
        /// Validates user email input. Must meet requirements.
        /// </summary>
        /// <param name="_input"></param>
        private void ValidateEmailString(String _input)
        {
            if (_input.Length < 5 ||
                !_input.Contains("@") ||
                !_input.Substring(_input.IndexOf("@") + 1, _input.Length - (_input.IndexOf("@") + 2)).Contains(".") ||
                _input.IndexOf("@") == 0 ||
                _input.IndexOf("@") > _input.Length - 4)
            {
                // Throws exception if e-mail does not contain @ sign in appropriate location,
                // or a dot also in the appropriate location.
                throw new Exception(ExceptionInvalidEmailFormat);
            }
        }

        /// <summary>
        /// Allows comparison between Membership objects.
        /// </summary>
        /// <param name="m"></param>
        /// <returns></returns>
        public bool Equals(MembershipModel m)
        {
            // Return true if all attributes match.
            if (firstName.Equals(m.firstName))
                if (lastName.Equals(m.lastName))
                    if (email.Equals(m.email))
                        return true;

            // Return false if at least one attribute is different.
            return false;
        }
    }
}
