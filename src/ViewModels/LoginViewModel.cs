using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Caliburn.Micro;
using Walleet.Views;

namespace Walleet.ViewModels
{
    public class LoginViewModel : Screen
    {
        private readonly RegistrationService _registrationService;
        private readonly INavigationService _navigationService;

        public LoginViewModel(RegistrationService registrationService, INavigationService navigationService)
        {
            _registrationService = registrationService;
            _navigationService = navigationService;
            _registrationService.AuthorizationFinished += OnAuthorizationFinished;
            UserName = "tomasz.romaniuk@gmail.com";
        }

        private void OnAuthorizationFinished(object sender, AsyncOperationFinishedEventArgs e)
        {
            if (e.Exception == null)
            {
                _navigationService.UriFor<MainPanoramaPageViewModel>().Navigate();
            }
            else
            {
                MessageBox.Show(e.Exception.Message);
            }
        }

        public string UserName { get; set; }

        private string Password
        {
            get { return ((LoginView)GetView()).passwordBox.Password; }
        }
   
        public void Login()
        {
            _registrationService.Login(new Credentials(UserName, Password));

        }
    }
}
