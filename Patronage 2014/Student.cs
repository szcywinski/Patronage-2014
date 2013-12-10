using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Patronage_2014
{
    public class Student
    {
        private String firstName="";
        private String lastName="";
        private Decimal grade=2m;

        
        public String FirstName
        {
            get { return firstName; }
            set { firstName = value; }
        }

        public String LastName
        {
            get { return lastName; }
            set { lastName = value; }
        }

        public Decimal Grade
        {
            get { return grade; }
            set { grade = value; }
        }
    }
}
