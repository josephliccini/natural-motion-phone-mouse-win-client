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
        private double shortMessageCount = 0.0;
        public double SensitivityConstant { get; set; }

        public TestDataDisplacementTranslator()
        {
            this.SensitivityConstant = 1.5;
        }

        public override MouseMotionDelta TranslateData(JObject json)
        {
            var x = json.GetValue("displacementX").Value<double>();
            var y = json.GetValue("displacementY").Value<double>();

            double combinedDisplacement = Math.Abs(x) + Math.Abs(y);

            double toSubtract = 0;
            if (shortMessageCount > 0)
            {
                toSubtract = Math.Log(shortMessageCount);
            }

            if (combinedDisplacement > 2.25)
            {
                shortMessageCount = Math.Max(0.0, shortMessageCount - 2.0);
            }
            else
            {
                if (!(toSubtract > SensitivityConstant))
                    ++shortMessageCount;
            }

            double multiplicativeFactor = Math.Max(SensitivityConstant - toSubtract, 0);

            Console.WriteLine(multiplicativeFactor);

            return new MouseMotionDelta
            {
                DisplacementX = Math.Floor(x) * multiplicativeFactor,
                DisplacementY = Math.Floor(y) * multiplicativeFactor 
            };
        }
    }
}
