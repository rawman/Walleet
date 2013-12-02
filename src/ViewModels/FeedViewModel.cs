using System;
using System.Collections.ObjectModel;
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
using System.Linq;

namespace Walleet.ViewModels
{
    public class FeedViewModel : Screen, IPage
    {
        private WalleetServiceClient _serviceClient;

        public FeedViewModel(WalleetServiceClient serviceClient)
        {
            _serviceClient = serviceClient;
            DisplayName = "updates";

        }

        protected override void OnActivate()
        {
            base.OnActivate();
            
            if(FeedItems == null)
            {
                _serviceClient.GetFeed(DateTime.Now, Calback);
            }
        }

        private void Calback(FeedItem[] feedItems, DateTime? dateTime)
        {
            FeedItems = new ObservableCollection<FeedViewItem>(feedItems.Select(x => new FeedViewItem(x)));
            NotifyOfPropertyChange(() => FeedItems);
        }

        public ObservableCollection<FeedViewItem> FeedItems { get; set; }

        public int Order
        {
            get { return 0; }
        }

        public class FeedViewItem
        {
            public string Footer { get; private set; }
            public string Content { get; private set; }

            public FeedViewItem(FeedItem feedItem)
            {
                Content = String.Format("{0} {1} {2} {3}", feedItem.feed_type, feedItem.amount, feedItem.currency.symbol, feedItem.text);
                Footer = feedItem.Date.ToShortDateString();
            }

        }
    }
}
