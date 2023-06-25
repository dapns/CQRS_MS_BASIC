using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Exceptions
{
    public class CoreApiException: Exception
    {
        public CoreApiException(string title, string message) : base(message) => Title = title;
        public string Title { get; set; }
    }
}
