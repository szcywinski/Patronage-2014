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
using System.Collections.ObjectModel;

namespace Patronage_2014
{
    public partial class MainPage : PhoneApplicationPage
    {

        public  ReadOnlyCollection <Student> Students { get; set; }
        
        public MainPage()
        {
            InitializeComponent();
            BuildLocalizedApplicationBar();
            StudentService.Instance.AddStudent(new Student(){FirstName="Janusz", LastName="Pień", Grade=3m});
            StudentService.Instance.AddStudent(new Student() { FirstName = "Mariusz", LastName = "Łoś", Grade = 3.5m });
        
            Students = StudentService.Instance.GetStudentList();

            StudentService.Instance.AddStudent(new Student() { FirstName = "Rajmund", LastName = "Wafel", Grade = 5m });

            
        }

        //private void MainPage_Loaded(object sender, RoutedEventArgs e)
        //{
        //    StudentService.Instance.Students.Add(new Student(){FirstName="Janusz", LastName="Pień", Grade=3m});
        //    StudentService.Instance.Students.Add(new Student(){FirstName="Mariusz", LastName="Łoś", Grade=3.5m});
        //    StudentService.Instance.Students.Add(new Student(){FirstName="Rajmund", LastName="Wafel", Grade=5m});
            
        //    //StudentList.ItemsSource =StudentService.Instance.Students;
            

        //}

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