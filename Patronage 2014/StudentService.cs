using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Patronage_2014
{
    public sealed class StudentService
    {
        private StudentService() { }

        public List<Student> Students { get; private set;}
        
        
        private static StudentService instance;

        static public StudentService Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new StudentService();
                    instance.Students = new List<Student>();
                }
                return instance;
            }
        }


    }
}
