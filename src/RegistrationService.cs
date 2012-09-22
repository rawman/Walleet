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
using HackrkGuessWP7;
using Spring.Rest.Client;

namespace Walleet
{
    public class RegistrationService : ITokenProvider
    {
        public event EventHandler<AsyncOperationFinishedEventArgs> AuthorizationFinished = delegate { }; 
       
        private readonly RestTemplate _rest;
        private string _token;

        public RegistrationService()
        {
            _rest = new RestTemplate(new Uri("http://10.12.216.102:8888"));
            _rest.MessageConverters.Add(new NJsonHttpMessageConverter());
        }

        public void Login(Credentials credentials)
        {
            var body = new RegistrationRequest(credentials);

            _rest.PostForObjectAsync<RegistrationResponce>("/api/v1/person/sign_in.json", body, r =>
            {
                if(r.Error == null)
                {
                    _token = r.Response.api_token;
                }

                AuthorizationFinished(this, new AsyncOperationFinishedEventArgs(r.Error));
            }); 
        }

        public string GetToken()
        {
            return _token;
        }
    }

    public class RegistrationRequest    
    {
        public person person { get; set; }

        public RegistrationRequest(Credentials credentials)
        {
            person = new person();
            person.email = credentials.UserName;
            person.password = credentials.Password;
        }
    }

    public class RegistrationResponce
    {

        public string api_token { get; set; }
        public string created_at { get; set; }
        public string email { get; set; }
        public int id { get; set; }
        public string updated_at { get; set; }
    }

    public class person
    {

        public string email { get; set; }
        public string password { get; set; }

    }

    public class AsyncOperationFinishedEventArgs : EventArgs
    {
        public Exception Exception { get; private set; }

        public AsyncOperationFinishedEventArgs(Exception exception)
        {
            Exception = exception;
        }
    }
}
