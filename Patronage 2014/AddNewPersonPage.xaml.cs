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
using System.Windows.Data;

namespace Patronage_2014
{
    public partial class AddNewPersonPage : PhoneApplicationPage
    {
        public IEnumerable <Decimal> Grades { get; set; }
        public Student CurrentStudent { get; set; }

        public AddNewPersonPage()
        {
            InitializeComponent();
            BuildLocalizedApplicationBar();
            Grades = new List<Decimal>() { 2, 2.5m, 3, 3.5m, 4, 4.5m, 5 };
            CurrentStudent = new Student();
        }

        private void BuildLocalizedApplicationBar()
        {

            ApplicationBar = new ApplicationBar();

            ApplicationBarIconButton appBarButton =
                new ApplicationBarIconButton(new
                Uri("/Images/save.png", UriKind.Relative));
            appBarButton.Text = AppResources.Save;
            appBarButton.IsEnabled = false;
            appBarButton.Click += new EventHandler(SaveButton_Click);
            ApplicationBar.Buttons.Add(appBarButton);

        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            StudentService.Instance.AddStudent(CurrentStudent);
            NavigationService.GoBack();
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            BindingExpression bindingExpression =
            ((TextBox)sender).GetBindingExpression(TextBox.TextProperty);
            if (bindingExpression != null)
            {
                bindingExpression.UpdateSource();
            }

            ApplicationBarIconButton appBarButton = (ApplicationBarIconButton)ApplicationBar.Buttons[0];
            if (appBarButton.Text == AppResources.Save)
            {
                if(CurrentStudent.FirstName.Length>0 || CurrentStudent.LastName.Length>0 )
                {
                    appBarButton.IsEnabled = true;
                }
                else
                {
                    appBarButton.IsEnabled = false;
                }
            }
        }
    }
}