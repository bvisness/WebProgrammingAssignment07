using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Assignment07.Services
{
    public enum MarioActionResult { OK, Died, Error };

    public struct MarioResult
    {
        public MarioActionResult ActionResult;
        public string NextStep;
    }

    public interface IMarioService
    {
        Task<MarioResult> GetMarioActionResult(string action);
    }
}
