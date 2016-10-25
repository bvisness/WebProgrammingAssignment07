using System;
using Microsoft.AspNet.Mvc;
using System.Net;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Assignment07.Controllers
{
    [Route("[controller]")]
    public class MarioController : Controller
    {
        private static Random rand = new Random();

        [HttpGet("jump")]
        public IActionResult Jump()
        {
            var mightFail = MightFail(5);
            if (mightFail != null)
            {
                return mightFail;
            }
            return new JsonResult(new MessageNextStep() { Message = "Mario made the jump!", NextStep = NextStep() });
        }

        [HttpGet("run")]
        public IActionResult Run()
        {
            var mightFail = MightFail(5);
            if (mightFail != null)
            {
                return mightFail;
            }
            return new JsonResult(new MessageNextStep() { Message = "Mario ran fast!", NextStep = NextStep() });
        }

        [HttpGet("walk")]
        public IActionResult Walk()
        {
            var mightFail = MightFail(3);
            if (mightFail != null)
            {
                return mightFail;
            }
            return new JsonResult(new MessageNextStep() { Message = "Mario walked super well!", NextStep = NextStep() });
        }

        [HttpGet("wait")]
        public IActionResult Wait()
        {
            var mightFail = MightFail(1);
            if (mightFail != null)
            {
                return mightFail;
            }
            return new JsonResult(new MessageNextStep() { Message = "Mario waited patiently!", NextStep = NextStep() });
        }

        private string[] choices = new string[] { "jump", "run", "walk", "wait" };


        private IActionResult MightFail(int chance)
        {
            if (rand.Next(1, 100) <= chance)
            {
                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                return new JsonResult(new MessageNextStep() { Message = "Too bad. Mario didn't make it.", NextStep = "none" });
            }
            return null;
        }

        private string NextStep()
        {
            return choices[rand.Next(0, choices.Length)];
        }

        private class MessageNextStep
        {
            public string Message { get; set; }
            public string NextStep { get; set; }
        }
    }
}
