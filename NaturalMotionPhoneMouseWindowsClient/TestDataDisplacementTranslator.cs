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
        public double MultiplicativeConstant { get; set; }

        public TestDataDisplacementTranslator()
        {
            this.MultiplicativeConstant = 1.5;
        }

        public override MouseMotionDelta TranslateData(JObject json)
        {
            var x = json.GetValue("displacementX").Value<double>();
            var y = json.GetValue("displacementY").Value<double>();

            double combinedDisplacement = Math.Abs(x) + Math.Abs(y);

            if (combinedDisplacement > 3.0)
            {
                shortMessageCount = 0.0;
            }
            else
            {
                ++shortMessageCount;
            }

            double multiplicativeFactor = Math.Max(MultiplicativeConstant - (shortMessageCount / 2.0), 0);

            return new MouseMotionDelta
            {
                DisplacementX = Math.Floor(x) * multiplicativeFactor,
                DisplacementY = Math.Floor(y) * multiplicativeFactor 
            };
        }
    }
}
