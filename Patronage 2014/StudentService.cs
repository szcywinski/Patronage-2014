using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            AverageGrade = Students.Sum(s => s.Grade) / Students.Count;
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
                AverageGrade = Students.Sum(s => s.Grade) / Students.Count;
            }
        }
    }
}
