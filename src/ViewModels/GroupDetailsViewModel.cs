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

namespace Walleet.ViewModels
{
    public class GroupDetailsViewModel : Screen
    {
        private readonly WalleetServiceClient _serviceClient;
        private readonly INavigationService _navigationService;

        public GroupDetailsViewModel(WalleetServiceClient walleetService, INavigationService navigationService)
        {
            _serviceClient = walleetService;
            _navigationService = navigationService;
            Members = new ObservableCollection<Member>();
        }

        public int GroupId { get; set; }


        public ObservableCollection<Member> Members { get; set; } 


        protected override void OnActivate()
        {
            base.OnActivate();

            _serviceClient.GetGroupInfo(GroupId, g => Execute.OnUIThread(() =>
            {
                Members.Clear();
                foreach (var mem in g.members)
                {
                    Members.Add(mem);
                }
            }));
        }

        public void AddDebt()
        {
            _navigationService.UriFor<AddDebtViewModel>()
                .WithParam(x => x.GroupId, GroupId).Navigate();
        }
    }
}
