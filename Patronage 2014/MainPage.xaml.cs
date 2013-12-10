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
using Patronage_2014.Model;

namespace Patronage_2014
{
    public partial class MainPage : PhoneApplicationPage
    {
        
        public MainPage()
        {
            InitializeComponent();
            BuildLocalizedApplicationBar();
            Loaded += new RoutedEventHandler(MainPage_Loaded);
        }

        private void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            List<Student> studentList = new List<Student>()
            {
                new Student(){FirstName="Janusz", LastName="Pień", Grade=3m},
                new Student(){FirstName="Mariusz", LastName="Łoś", Grade=3.5m},
            };

            StudentList.ItemsSource = studentList;
            

        }

        private void BuildLocalizedApplicationBar()
        {
            
            ApplicationBar = new ApplicationBar();

            ApplicationBarIconButton appBarButton =
                new ApplicationBarIconButton(new
                Uri("/Images/add.png", UriKind.Relative));
            appBarButton.Text = AppResources.Add;
            appBarButton.Click += new EventHandler(AddButton_Click);
            ApplicationBar.Buttons.Add(appBarButton);
            
        }

        private void AddButton_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/AddNewPersonPage.xaml", UriKind.Relative));
        }

    }
}