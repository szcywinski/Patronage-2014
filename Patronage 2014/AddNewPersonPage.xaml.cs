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
        
        private Student editedStudent;
        private bool edit;
        public AddNewPersonPage()
        {
            InitializeComponent();
            BuildLocalizedApplicationBar();
            Grades = new List<Decimal>() { 2, 2.5m, 3, 3.5m, 4, 4.5m, 5 };

            var student = PhoneApplicationService.Current.State["CurrentStudent"];
            if (student != null && student is Student)
            {
                editedStudent = (Student)student;
                CurrentStudent = StudentService.Instance.CloneStudent(editedStudent);
                edit = true;
            }
            else
            {
                edit=false;
                CurrentStudent = new Student();
            }
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
            if (edit)
            {
                StudentService.Instance.CopyStudentTo(CurrentStudent, editedStudent);
            }
            else
            {
                StudentService.Instance.AddStudent(CurrentStudent);
            }
            NavigationService.GoBack();
        }

        private void Form_TextChanged(object sender, TextChangedEventArgs e)
        {
            BindingExpression bindingExpression =
            ((TextBox)sender).GetBindingExpression(TextBox.TextProperty);
            if (bindingExpression != null)
            {
                bindingExpression.UpdateSource();
            }
            EnableButton();
        }

        private void ListPicker_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            EnableButton();
        }

        private void EnableButton()
        {
            ApplicationBarIconButton appBarButton = (ApplicationBarIconButton)ApplicationBar.Buttons[0];
            if (appBarButton.Text == AppResources.Save)
            {
                if (!String.IsNullOrEmpty(CurrentStudent.FirstName) && !String.IsNullOrEmpty(CurrentStudent.LastName) && 
                    (edit ? CurrentStudent.FirstName != editedStudent.FirstName 
                        || CurrentStudent.LastName != editedStudent.LastName 
                        || CurrentStudent.Grade != editedStudent.Grade : true))
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