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

        private decimal CalculateAverage()
        {
            return AverageGrade = Students.Sum(s => s.Grade) / Students.Count;
        }

        public async void SaveState()
        {
            PhoneApplicationService.Current.State["StudentService"] = this;
           
            // Serialize our Product class into a string             
            string jsonContents = JsonConvert.SerializeObject(students);
            // Get the app data folder and create or replace the file we are storing the JSON in.            
            StorageFolder localFolder = ApplicationData.Current.LocalFolder;
            StorageFile textFile = await localFolder.CreateFileAsync("students",
                                            CreationCollisionOption.ReplaceExisting);
            // Open the file...      
            using (IRandomAccessStream textStream = await textFile.OpenAsync(FileAccessMode.ReadWrite))
            {
                // write the JSON string!
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
            }
            else
            {
                StorageFolder localFolder = ApplicationData.Current.LocalFolder;
                try
                {
                    // Getting JSON from file if it exists, or file not found exception if it does not  
                    StorageFile textFile = await localFolder.GetFileAsync("students");
                    using (IRandomAccessStream textStream = await textFile.OpenReadAsync())
                    {
                        // Read text stream     
                        using (DataReader textReader = new DataReader(textStream))
                        {
                            //get size                       
                            uint textLength = (uint)textStream.Size;
                            await textReader.LoadAsync(textLength);
                            // read it                    
                            string jsonContents = textReader.ReadString(textLength);
                            // deserialize back to our product!  
                            //instance = JsonConvert.DeserializeObject<IList<Student>>(jsonContents) as List<Student>;
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
