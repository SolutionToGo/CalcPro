using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalcPro
{
    public static class Position
    {

        /// <summary>
        /// Variables to hold position's actual values
        /// </summary>
        public static decimal MA_ListPrice = 0;
        public static decimal MO_ListPrice = 0;
        public static decimal MA_EP = 0;
        public static decimal MO_EP = 0;
        public static decimal MA_SK = 0;
        public static decimal MO_SK = 0;
        public static decimal MA_VK = 0;
        public static decimal MO_VK = 0;


        public static decimal MA_Multi1 = 0;
        public static decimal MA_Multi2 = 0;
        public static decimal MA_Multi3 = 0;
        public static decimal MA_Multi4 = 0;
        public static decimal MA_GrundMulti = 0;

        public static decimal MO_Multi1 = 0;
        public static decimal MO_Multi2 = 0;
        public static decimal MO_Multi3 = 0;
        public static decimal MO_Multi4 = 0;
        public static decimal MO_GrundMulti = 0;

        public static bool HaveDetailsKz = false;

        public static string OldWG = string.Empty;
        public static string OldWA = string.Empty;
    }
}
