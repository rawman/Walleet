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
using Spring.Http;
using Spring.Rest.Client;
using System.Linq;

namespace Walleet
{
    public class WalleetServiceClient
    {
        private readonly ITokenProvider _tokenProvider;
        private readonly RestTemplate _rest;

        public WalleetServiceClient(RegistrationService tokenProvider)
        {
            _tokenProvider = tokenProvider;
            _rest = new RestTemplate(new Uri("http://10.12.216.102:8888"));
            _rest.MessageConverters.Add(new NJsonHttpMessageConverter());
        
        }

        public group[] GetGroups(Action<group[]> calback)
        {
            HttpEntity entity = new HttpEntity();
            entity.Headers.Add("X-Api-Token", _tokenProvider.GetToken());
            entity.Headers.Add("X-Api-Client", "WP7rocks");

            _rest.ExchangeAsync<GetGroupsResponce>("/api/v1/groups.json", HttpMethod.GET, entity, r =>
            {
                if(r.Error == null)
                {
                    calback(r.Response.Body.items.Select(x => x.group).ToArray());
                }
                else
                throw new Exception("get groups failed", r.Error);
            });

            return null;
        }

        public group GetGroupInfo(int id, Action<group> calback)
        {
            HttpEntity entity = new HttpEntity();
            entity.Headers.Add("X-Api-Token", _tokenProvider.GetToken());
            entity.Headers.Add("X-Api-Client", "WP7rocks");

            _rest.ExchangeAsync<GetGroupInfoResponce>(string.Format("/api/v1/groups/{0}.json", id), HttpMethod.GET, entity, r =>
            {
                if (r.Error == null)
                {
                    calback(r.Response.Body.group);
                }
                else
                    throw new Exception("get groups failed", r.Error);
            });

            return null;
        }
    }

    public class AsyncOperationFinishedEventArgs<T> : EventArgs
    {
        public T Responce { get; private set; }
        public Exception Error { get; set; }

    }

    public class GetGroupsResponce
    {
        public GroupItem[] items { get; set; }
    }

    public class GetGroupInfoResponce
    {
        public group group { get; set; }
    }
    
    public class GroupItem
    {
        public group group {get ;set;}
    }

    public class group
    {
        public int id { get; set; }
        public string name { get; set; }
        public string created_at { get; set; }  
        public string updated_at { get; set; }
        public bool visible { get; set; }
        public int currency_id { get; set; }
        public member[] members { get; set; }
    }

    public class member
    {
        public double amount { get; set; }
        public string name { get; set; }

    }
}
