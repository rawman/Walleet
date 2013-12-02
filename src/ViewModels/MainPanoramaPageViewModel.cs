using System;
using System.Collections;
using System.Collections.Generic;
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
    public class MainPanoramaPageViewModel : Conductor<IPage>.Collection.OneActive
    {

        public MainPanoramaPageViewModel(IEnumerable<IPage> pages)
        {
            Items.AddRange(pages.OrderBy(x => x.Order));
        }

        protected override void OnActivate()
        {
            base.OnActivate();

            ActiveItem = Items.First();
        }
    }
}
