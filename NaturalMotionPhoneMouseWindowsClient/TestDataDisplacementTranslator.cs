using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace NaturalMotionPhoneMouseWindowsClient
{
    class TestDataDisplacementTranslator : DisplacementTranslator
    {
        public override MouseMotionDelta TranslateData(JObject json)
        {
            var x = json.GetValue("displacementX").Value<double>();
            var y = json.GetValue("displacementY").Value<double>();

            return new MouseMotionDelta
            {
                DisplacementX = Math.Floor(x) * 1.3,
                DisplacementY = Math.Floor(y) * 1.3
            };
        }
    }
}
