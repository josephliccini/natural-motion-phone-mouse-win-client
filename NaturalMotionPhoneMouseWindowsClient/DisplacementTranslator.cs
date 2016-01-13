using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NaturalMotionPhoneMouseWindowsClient
{
    abstract class DisplacementTranslator
    {
        public DisplacementTranslator()
        {

        }

        public abstract MouseMotionDelta TranslateData(JObject json);
    }
}
