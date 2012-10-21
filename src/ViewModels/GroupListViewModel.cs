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
    public class GroupListViewModel : Screen
    {
        private INavigationService _navigationService;
        private WalleetServiceClient _serviceClient;

        public GroupListViewModel(WalleetServiceClient serviceClient, INavigationService navigationService)
        {
            _serviceClient = serviceClient;
            _navigationService = navigationService;
            
            Groups = new ObservableCollection<Group>();
        }

        public ObservableCollection<Group> Groups { get; set; } 

        protected override void OnActivate()
        {
            base.OnActivate();

            _serviceClient.GetGroups(g => Execute.OnUIThread(() =>
                                                                 {
                                                                     Groups.Clear();
                                                                     foreach (var @group in g)
                                                                     {
                                                                         
                                                                         Groups.Add(@group);
                                                                     }
                                                                 }));
        }

        public void ShowGroup(Group g)
        {
            _navigationService.UriFor<GroupDetailsViewModel>()
                .WithParam(x => x.GroupId, g.id).Navigate();
        }
    }
}
