using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using Assignment07.Models;
using System.Net;
using Assignment07.Services;

namespace Assignment07.Controllers
{
    [Route("api/[controller]")]
    public class MarioLevelController : Controller
    {
        private IMarioService marioService;

        public MarioLevelController(IMarioService marioService)
        {
            this.marioService = marioService;
        }

        [HttpGet("{marioAction}")]
        public async Task<IActionResult> Get(string marioAction)
        {
            if (!MarioActionResponseModel.ActionIsValid(marioAction))
            {
                var invalidActionResult = new ContentResult();
                invalidActionResult.StatusCode = (int)HttpStatusCode.BadRequest;
                invalidActionResult.Content = "The action \"" + marioAction + "\" was not valid.";
                return invalidActionResult;
            }

            var actionResult = await marioService.GetMarioActionResult(marioAction);

            IMarioResponseModel responseModel;
            HttpStatusCode resultStatusCode = HttpStatusCode.OK;
            if (actionResult.ActionResult == MarioActionResult.OK)
            {
                var resultModel = new MarioActionResponseModel();
                resultModel.TrySetAction(marioAction); // I don't bother to check here since we already validated it above.
                resultModel.NextStep = actionResult.NextStep;
                responseModel = resultModel;
            }
            else if (actionResult.ActionResult == MarioActionResult.Died)
            {
                responseModel = new MarioGenericResponseModel("Mario was taken away...");
            }
            else
            {
                responseModel = new MarioGenericResponseModel("An error occurred when talking to the server.");
                resultStatusCode = HttpStatusCode.InternalServerError;
            }

            var result = new JsonResult(responseModel);
            result.StatusCode = (int)resultStatusCode;

            return result;
        }
    }
}
