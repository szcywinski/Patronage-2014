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
using Newtonsoft.Json;
using System.Diagnostics;

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
                        instance.students = new List<Student>();
                        instance.averageGrade = 0m;
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
            AverageGrade = Students.Sum(s => s.Grade) / Students.Count;
        }

        public async void SaveState()
        {
            PhoneApplicationService.Current.State["StudentService"] = this;
           
                      
            string jsonContents = JsonConvert.SerializeObject(students);
                      
            StorageFolder localFolder = ApplicationData.Current.LocalFolder;
            StorageFile textFile = await localFolder.CreateFileAsync("students",
                                            CreationCollisionOption.ReplaceExisting);
                  
            using (IRandomAccessStream textStream = await textFile.OpenAsync(FileAccessMode.ReadWrite))
            {
                using (DataWriter textWriter = new DataWriter(textStream))
                {
                    textWriter.WriteString(jsonContents);
                    await textWriter.StoreAsync();
                }
            }
            

        }

        public async static void LoadState()
        {

            if(PhoneApplicationService.Current.State.ContainsKey("StudentService"))
            {
                instance = PhoneApplicationService.Current.State["StudentService"] as StudentService;
                instance.NotifyPropertyChanged("Students");
                instance.CalculateAverage();
            }
            else
            {
                StorageFolder localFolder = ApplicationData.Current.LocalFolder;
                try
                {
                    
                    StorageFile textFile = await localFolder.GetFileAsync("students");
                    using (IRandomAccessStream textStream = await textFile.OpenReadAsync())
                    {
                        
                        using (DataReader textReader = new DataReader(textStream))
                        {
                            
                            uint textLength = (uint)textStream.Size;
                            await textReader.LoadAsync(textLength);
                            
                            string jsonContents = textReader.ReadString(textLength);
                            
                            instance.students = JsonConvert.DeserializeObject<IList<Student>>(jsonContents) as List<Student>;
                            instance.NotifyPropertyChanged("Students");
                            instance.CalculateAverage();
                            
                        }
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                }
            }
        }

    }
}
