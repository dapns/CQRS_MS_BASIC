using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.ClientCall
{
    public class ClientResponse<R>
    {
        #region Variables
        public R model { get; set; }
        public string error { get; set; }
        #endregion

        #region Constructors
        public ClientResponse()
        {
            // Set values for instance variables
            model = default;
            error = null;
        } // End of the constructor
        #endregion

        #region Get methods
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        } // End of the ToString method
        #endregion
    }
}

