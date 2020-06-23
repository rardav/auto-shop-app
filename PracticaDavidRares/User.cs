using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace PracticaDavidRares
{
    public class User
    {
        private string lastName;
        private string firstName;
        private string position;

        public User()
        {
            lastName = "";
            firstName = "";
            position = "";
        }

        public User(string f, string l, string pos)
        {
            firstName = f;
            lastName = l;
            position = pos;
        }

        public string LastName
        {
            get { return lastName; }
        }

        public string FirstName
        {
            get { return firstName; }
        }

        public string Position
        {
            get { return position; }
        }
    }
}
