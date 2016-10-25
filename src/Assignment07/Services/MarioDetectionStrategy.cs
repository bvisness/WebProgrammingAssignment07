using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Practices.TransientFaultHandling;
using System.Net;

namespace Assignment07.Services
{
    public class MarioDetectionStrategy : ITransientErrorDetectionStrategy
    {
        public bool IsTransient(Exception ex)
        {
            var webException = ex as WebException;
            if (webException != null)
            {
                var webResponse = webException.Response as HttpWebResponse;
                if (webResponse != null)
                {
                    return webResponse.StatusCode == HttpStatusCode.ServiceUnavailable;
                }
            }

            return false;
        }
    }
}
