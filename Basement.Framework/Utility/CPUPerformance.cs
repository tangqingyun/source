using System;
using System.Collections.Generic;
using System.Text;

namespace Basement.Framework.Utility
{

    static class CPUPerformance
    {
        /// <summary>
        /// CPU’º”√
        /// </summary>
        /// <returns></returns>
        public static int GetHeat()
        {
            System.Management.ManagementClass mc = new System.Management.ManagementClass("Win32_Processor");
            System.Management.ManagementObjectCollection moc = mc.GetInstances();
            int MyCPUHeat = 0;
            int count = 0;
            int sum = 0;
            foreach (System.Management.ManagementObject mo in moc)
            {
                if (int.TryParse(mo.Properties["LoadPercentage"].Value.ToString(), out MyCPUHeat))
                {
                    sum += MyCPUHeat;
                    count++;
                }
            }

            return sum / count;
        }
    }
}
