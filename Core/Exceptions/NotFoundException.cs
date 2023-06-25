using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Exceptions
{
    public class NotFoundException:CoreApiException
    {
        public NotFoundException(string title, string message) : base(title, message)
        {
        }
    }
}
