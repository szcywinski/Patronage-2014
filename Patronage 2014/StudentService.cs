using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Patronage_2014
{
    public sealed class StudentService
    {
        private StudentService() { }
        private static readonly object syncRoot = new object();
        private List<Student> studentList;

        public ReadOnlyCollection<Student> GetStudentList()
        {
            return studentList.AsReadOnly();
        } 
        
        public void AddStudent(Student student)
        {
            studentList.Add(student);
        }
        
        private static StudentService instance;

        static public StudentService Instance
        {
            get
            {
                lock (syncRoot)
                {
                    if (instance == null)
                    {
                        instance = new StudentService();
                        instance.studentList = new List<Student>();
                    }
                    return instance;
                }
            }
        }


    }
}
