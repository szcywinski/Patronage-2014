using Microsoft.Phone.Shell;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Streams;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace Patronage_2014
{
    public sealed class StudentService : INotifyPropertyChanged
    {
        
        private static StudentService instance;
        private static readonly object syncRoot = new object();
        private Decimal averageGrade;
        private List<Student> students;

        private StudentService() { }

        public Decimal AverageGrade 
        {
            get { return averageGrade; }
            private set { averageGrade = value; NotifyPropertyChanged("AverageGrade"); }
        }

        public ReadOnlyCollection<Student> Students 
        {
            get { return students.AsReadOnly(); }
        }
                
        public void AddStudent(Student student)
        {
            students.Add(student);
            NotifyPropertyChanged("Students");
            CalculateAverage();
        }
       
        static public StudentService Instance
        {
            get
            {
                lock (syncRoot)
                {
                    if (instance == null)
                    {
                        instance = new StudentService();
                        LoadState();
                    }
                    return instance;
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public Student CloneStudent(Student student)
        {
            Student copy = new Student
            {
                FirstName = student.FirstName,
                LastName = student.LastName,
                Grade = student.Grade
            };
            return copy;
        }

        public void CopyStudentTo(Student source, Student destination)
        {
            destination.FirstName = source.FirstName;
            destination.LastName = source.LastName;
            if (destination.Grade != source.Grade)
            {
                destination.Grade = source.Grade;
                CalculateAverage();
            }
        }

        private void CalculateAverage()
        {
            if (Students.Count>0)
                AverageGrade = Students.Sum(s => s.Grade) / Students.Count;
        }

        public void SaveState()
        {
            try
            {
                PhoneApplicationService.Current.State["StudentList"] = this.students;

                IsolatedStorageFile isoFile = System.IO.IsolatedStorage.IsolatedStorageFile.GetUserStoreForApplication(); 
                DataContractSerializer serializer = new DataContractSerializer(typeof(IList<Student>));

                using (var targetFile = isoFile.CreateFile("students"))
                {
                    serializer.WriteObject(targetFile, this.students);
                } 
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }

        }

        private static void LoadState()
        {

            if(PhoneApplicationService.Current.State.ContainsKey("StudentList"))
            {
                instance.students = PhoneApplicationService.Current.State["StudentList"] as List<Student>;
                instance.NotifyPropertyChanged("Students");
                instance.CalculateAverage();
            }
            else
            {
                try
                {
                    instance.students = new List<Student>();
                    instance.averageGrade = 0m;

                    IsolatedStorageFile isoFile = System.IO.IsolatedStorage.IsolatedStorageFile.GetUserStoreForApplication(); 
                    DataContractSerializer serializer = new DataContractSerializer(typeof(IList<Student>));

                    if (isoFile.FileExists("students")) 
                    using (var sourceStream = isoFile.OpenFile("students", FileMode.Open)) 
                    {
                        instance.students.AddRange((Student[])serializer.ReadObject(sourceStream));
                    } 

                    instance.NotifyPropertyChanged("Students");
                    instance.CalculateAverage();
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                }
            }
        }

    }
}
