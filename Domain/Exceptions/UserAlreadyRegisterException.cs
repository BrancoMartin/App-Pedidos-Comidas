using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Exceptions
{
    public class UserAlreadyRegisterException : Exception
    {
        public UserAlreadyRegisterException(string messaje) : base(messaje) {
        
        }
    }
}
