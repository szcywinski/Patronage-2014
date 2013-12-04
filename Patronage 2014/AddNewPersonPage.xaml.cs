using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Patronage_2014.Resources;

namespace Patronage_2014
{
    public partial class AddNewPersonPage : PhoneApplicationPage
    {
        public AddNewPersonPage()
        {
            InitializeComponent();
        }

        private void BuildLocalizedApplicationBar()
        {

            ApplicationBar = new ApplicationBar();

            ApplicationBarIconButton appBarButton =
                new ApplicationBarIconButton(new
                Uri("/Images/save.png", UriKind.Relative));
            appBarButton.Text = AppResources.Add;
            appBarButton.IsEnabled = false;
            ApplicationBar.Buttons.Add(appBarButton);

        }
    }
}