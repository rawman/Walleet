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
    public class AddDebtViewModel : Screen
    {
        private INavigationService _navigationService;
        private WalleetServiceClient _serviceClient;

        public AddDebtViewModel(WalleetServiceClient serviceClient, INavigationService navigationService)
        {
            _serviceClient = serviceClient;
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

                Taker = Members.First();
                Giver = Members.First();

            }));
        }

        private Member _giver;
        public Member Giver
        {
            get { return _giver; }
            set
            {
                _giver = value;
                NotifyOfPropertyChange(() => Giver);
            }
        }

        private Member _taker;
        public Member Taker
        {
            get { return _taker; }
            set
            {
                _taker = value;
                NotifyOfPropertyChange(() => Taker);
            }
        }

        public double Amount { get; set; }

        public void Save()
        {
            if (Giver == Taker)
            {
                MessageBox.Show("Giver and Taker can't be the same");
                return;  
            }
            _serviceClient.AddDebt(new Debt(Giver.id, Taker.id, Amount),
                () => Execute.OnUIThread(() => _navigationService.GoBack()));
        }
    }
}
