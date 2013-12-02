using System;
using System.IO;
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
using Spring.Http.Client.Interceptor;
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
            _rest = new RestTemplate(Settings.Host);
            _rest.MessageConverters.Add(new NJsonHttpMessageConverter());
        }

        public Group[] GetGroups(Action<Group[]> calback)
        {
            var entity = CreateEntity();

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

        private HttpEntity CreateEntity()
        {
            HttpEntity entity = new HttpEntity();
            entity.Headers.Add("X-Api-Token", _tokenProvider.GetToken());
            entity.Headers.Add("X-Api-Client", "WP7rocks");
            return entity;
        }

        private HttpEntity CreateEntity(object body)
        {
            HttpEntity entity = new HttpEntity(body);
            entity.Headers.Add("X-Api-Token", _tokenProvider.GetToken());
            entity.Headers.Add("X-Api-Client", "WP7rocks");
            return entity;
        }

        public void GetGroupInfo(int id, Action<Group> calback)
        {
            var entity = CreateEntity(); 

            _rest.ExchangeAsync<GetGroupInfoResponce>(string.Format("/api/v1/groups/{0}.json", id), HttpMethod.GET, entity, r =>
            {
                if (r.Error == null)
                {
                    calback(r.Response.Body.group);
                }
                else
                    throw new Exception("get groups failed", r.Error);
            });
        }

        public void GetFeed(DateTime from, Action<FeedItem[],DateTime?> calback)
        {
            var entity = CreateEntity();

            _rest.ExchangeAsync<GetFeedResponce>("/api/v1/person/feed.json?time=" + from.ToString("yyyy-MM-ddTHH:mm:ssZ"), HttpMethod.GET, entity, r =>
            {
                if (r.Error == null)
                {
                    calback(r.Response.Body.items, r.Response.Body.next_timestamp);
                }
                else
                    throw new Exception("get groups failed", r.Error);
            });
        }

        public void AddDebt(Debt debt, Action calback)
        {
            var entity = CreateEntity(debt);
 
            _rest.ExchangeAsync("/api/v1/debts.json", HttpMethod.POST, entity, r =>
            {
                if (r.Error == null)
                {
                    calback();     
                    return;
                }
                throw new Exception("creating debt failed", r.Error);
            });

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
        public Group group { get; set; }
    }

    public class GetFeedResponce
    {
        public FeedItem[] items { get; set; }
        public DateTime? next_timestamp { get; set; }
    }
    
    public class GroupItem
    {
        public Group group {get ;set;}
    }

    public class Group
    {
        public int id { get; set; }
        public string name { get; set; }
        public string created_at { get; set; }  
        public string updated_at { get; set; }
        public bool visible { get; set; }
        public int currency_id { get; set; }
        public Member[] members { get; set; }
    }

    public class Member
    {
        public int id { get; set; }
        public double amount { get; set; }
        public string name { get; set; }

        public override string ToString()
        {
            return name;
        }
    }

    public class Debt
    {
        public string giver_id { get; set; }
        public string taker_ids { get; set; }
        public string amount { get; set; }

        public Debt()
        {
        }

        public Debt(int giverId, int takerId, double amount)
        {
            giver_id = giverId.ToString();
            taker_ids = takerId.ToString();
            this.amount = amount.ToString();
        }
    }

    public class Currency
    {
        public int decimal_precision { get; set; }
        public string decimal_separator { get; set; }
        public int id { get; set; }
        public string symbol { get; set; }
        public string thousands_separator { get; set; }
    }

    public class FeedItem
    {
        public double amount { get; set; }
        public DateTime Date { get; set; }
        public string feed_type { get; set; }
        public Currency currency { get; set; }
        public string text { get; set; }
    }
}
