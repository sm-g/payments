using System;
using System.Security;
using System.Windows.Input;
using Payments.Model;
using EventAggregator;
using System.Collections.Generic;
using PaymentsWpfApplication.Properties;

namespace PaymentsWpfApplication.ViewModels
{
    public class LoginViewModel : ViewModelBase
    {
        private ICommand _loginCommand;
        private string _username;
        private SecureString _password;
        private bool _wrongpassword;

        public string Username
        {
            get { return _username; }
            set
            {
                _username = value;
                RemoveAllErrors("CurrentUserName");
                OnPropertyChanged(() => Username);
            }
        }

        public SecureString Password
        {
            private get { return _password; }
            set
            {
                _password = value;
                RemoveAllErrors("IsWrongPassword");
            }
        }

        public bool IsLoginEnabled
        {
            get
            {
                return !String.IsNullOrWhiteSpace(Username ?? "") && Password != null && Password.Length > 0;
            }
        }

        public bool IsWrongPassword
        {
            get { return _wrongpassword; }
            set
            {
                if (_wrongpassword != value)
                {
                    _wrongpassword = value;
                    OnPropertyChanged(() => IsWrongPassword);
                };
            }
        }

        public ICommand LoginCommand
        {
            get
            {
                return _loginCommand ?? (_loginCommand = new RelayCommand(
                    () => LogIn(), () => IsLoginEnabled));
            }
        }

        private void LogIn()
        {
            Password.MakeReadOnly();
            try
            {
                UserController.Instance.TryLogin(Username, Password);
                this.Send((int)EventId.UserLoggedIn);
            }
            catch (NoUsernameException)
            {
                AddError("Username", Resources.ErrorNoUsername);
            }
            catch (FieldConstraintException)
            {
                AddError("IsWrongPassword", Resources.ErrorWrongPassword);
            }

        }
    }
}