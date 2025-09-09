using Projekat18.Helpers;
using Projekat18.View;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;
using Projekat18.Model;
using System.Collections.ObjectModel;

namespace Projekat18.ViewModel
{
    public class MainWindowViewModel : BaseViewModel
    {
        private readonly ObservableCollection<Administrator> users = new ObservableCollection<Administrator>();
        private readonly Window _currentWindow;

        private string _username;
        public string Username
        {
            get { return _username; }
            set
            {
                _username = value;
                OnPropertyChanged(nameof(Username));
            }
        }

        private string _password;
        public string Password
        {
            get { return _password; }
            set
            {
                _password = value;
                OnPropertyChanged(nameof(Password));
            }
        }

        public MyICommand<object> LoginCommand { get; }

        public MainWindowViewModel(Window currentWindow)
        {
            _currentWindow = currentWindow;
            LoginCommand = new MyICommand<object>(Login);
            users.Add(new Administrator { UserName = "Stefan", Certificate = "Admin", Permissions = "Add/Delete/Edit", Password = "Stefan" });
            users.Add(new Administrator { UserName = "Veljko", Certificate = "Admin", Permissions = "Add/Delete/Edit", Password = "Veljko" });
            users.Add(new Administrator { UserName = "Marko", Certificate = "User", Permissions = "", Password = "Marko" });
            users.Add(new Administrator { UserName = "Djole", Certificate = "PM", Permissions = "Add/Edit", Password = "Djole" });
        }

        private void Login(object parameter)
        {
            bool found = false;
            if (parameter is PasswordBox passwordBox)
            {
                string password = passwordBox.Password;
                foreach (var u in users)
                {
                    if (Username.Equals(u.UserName) && password.Equals(u.Password))
                    {
                        var databaseViewModel = new UserViewModel(u);
                        UserView uv = new UserView(u);
                        _currentWindow.Close();
                        uv.Show();
                        found = true;
                    }
                }

                if (!found)
                {
                    MessageBox.Show("Invalid username or password");
                }
            }
        }
    }
}
