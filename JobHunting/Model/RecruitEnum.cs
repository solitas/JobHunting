using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobHunting.Model
{
    public enum RecruitType
    {
        [Description("공개채용")]
        Public,
        [Description("수시채용")]
        Private,
        [Description("인턴채용")]
        Internship,
        [Description("경력채용")]
        Career
    }

    public enum ScreeningStep
    {
        [Description("서류전형")]
        Paper,
        [Description("면접전형")]
        Interview,
        [Description("인적성")]
        Test
    }
}
