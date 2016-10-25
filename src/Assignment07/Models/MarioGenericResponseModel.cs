using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Assignment07.Models
{
    public class MarioGenericResponseModel : IMarioResponseModel
    {
        public string Message { get; set; }

        public MarioGenericResponseModel(string message)
        {
            this.Message = message;
        }
    }
}
