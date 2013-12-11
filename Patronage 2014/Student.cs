using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Patronage_2014
{
    public class Student : INotifyPropertyChanged
    {
        private String firstName="";
        private String lastName="";
        private Decimal grade=2m;

        
        public String FirstName
        {
            get { return firstName; }
            set { firstName = value; NotifyPropertyChanged("FirstName"); }
        }

        public String LastName
        {
            get { return lastName; }
            set { lastName = value; NotifyPropertyChanged("LastName"); }
        }

        public Decimal Grade
        {
            get { return grade; }
            set { grade = value; NotifyPropertyChanged("Grade"); }
        }
        
        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
