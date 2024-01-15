using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Isogeo.Utils.LogManager;

namespace Isogeo.Utils.Box
{
    public static class BoxUtils
    {
        public static List<double> BoxStringToDoubles(string box)
        {
            var list = box.Split(',');
            return list.Select(item => double.Parse(item, CultureInfo.InvariantCulture)).ToList();
        }

        private static bool NearlyEqual(double a, double b, double epsilon)
        {
            const double minNormal = 2.2250738585072014E-308d;
            var absA = Math.Abs(a);
            var absB = Math.Abs(b);
            var diff = Math.Abs(a - b);

            if (a.Equals(b))
            { // shortcut, handles infinities
                return true;
            }

            if (a == 0 || b == 0 || absA + absB < minNormal)
            {
                // a or b is zero or both are extremely close to it
                // relative error is less meaningful here
                return diff < (epsilon * minNormal);
            }

            // use relative error
            return diff / (absA + absB) < epsilon;
        }

        /// <summary>
        /// Compare two box. A degree of precision is taken in argument
        /// </summary>
        public static bool BoxAreEquals(string box1, string box2, double precision)
        {
            try
            {
                var list1 = BoxStringToDoubles(box1);
                var list2 = BoxStringToDoubles(box2);

                return (NearlyEqual(list1[0], list2[0], precision) ||
                        NearlyEqual(list1[1], list2[1], precision) ||
                        NearlyEqual(list1[2], list2[2], precision) ||
                        NearlyEqual(list1[3], list2[3], precision));
            }
            catch (Exception e)
            {
                Log.Logger.Debug("box 1 = " + '"' + box1 + '"');
                Log.Logger.Debug("box 2 = " + '"' + box2 + '"');
                Log.Logger.Debug(e);
            }
            return false;
        }
    }
}
