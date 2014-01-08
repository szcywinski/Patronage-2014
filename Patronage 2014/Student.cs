using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Patronage_2014
{
    [DataContract] 
    public class Student : INotifyPropertyChanged
    {
        private String firstName=""; 
        private String lastName="";
        private Decimal grade=2m;


        [DataMember] 
        public String FirstName
        {
            get { return firstName; }
            set { firstName = value; NotifyPropertyChanged("FirstName"); }
        }

        [DataMember] 
        public String LastName
        {
            get { return lastName; }
            set { lastName = value; NotifyPropertyChanged("LastName"); }
        }

        [DataMember] 
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
