using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobHunting.Model
{
    public class Question : PropertyChangedBase
    {
        public string Subject { set; get; }
        public string Content { set; get; }
        public int PermitCharCount { set; get; }
        public int InputCharCount { set; get; }
        public Dictionary<DateTime, string> ChangeHistory { set; get; }

        public Question()
        {
            ChangeHistory = new Dictionary<DateTime, string>();
        }
    }
}
