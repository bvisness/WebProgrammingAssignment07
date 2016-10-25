using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Assignment07.Models
{
    public class MarioActionResponseModel : IMarioResponseModel
    {
        public string Message { get; private set; }

        public string NextStep { get; set; }

        private static Dictionary<string, string> actionDescriptions = new Dictionary<string, string>()
        {
            {"walk", "Yoshi and Mario took a leisurely stroll."},
            {"jump", "Yoshi flutter-jumped super high!"},
            {"wait", "Yoshi and Mario waited for a bit."},
            {"run", "Yoshi and Mario ran toward the finish!"}
        };

        public static bool ActionIsValid(string action)
        {
            return actionDescriptions.ContainsKey(action);
        }

        public bool TrySetAction(string action)
        {
            if (!ActionIsValid(action))
            {
                return false;
            }

            string message;
            actionDescriptions.TryGetValue(action, out message);
            this.Message = message;
            return true;
        }
    }
}
