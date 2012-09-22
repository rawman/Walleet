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
        private WalleetServiceClient _serviceClient;

        public GroupDetailsViewModel(WalleetServiceClient walleetService)
        {
            _serviceClient = walleetService;
            Members = new ObservableCollection<member>();
        }

        public int GroupId { get; set; }


        public ObservableCollection<member> Members { get; set; } 


        protected override void OnActivate()
        {
            base.OnActivate();

            _serviceClient.GetGroupInfo(GroupId, g => Execute.OnUIThread(() =>
            {
                foreach (var mem in g.members)
                {
                    Members.Add(mem);
                }
            }));
        }
    }
}
