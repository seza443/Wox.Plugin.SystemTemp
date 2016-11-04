using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;

namespace Wox.Plugin.SystemTemp
{
    /// <summary>
    /// TODO
    /// author: seza443
    /// </summary>
    public class SystemTemp : IPlugin
    {
        private PluginInitContext _context;

        public void Init(PluginInitContext context)
        {
            _context = context;
        }

        public List<Result> Query(Query query)
        {
            List<Result> results = Temperature.Temperatures.Select(p => new Result()
            {
                Title = p.CurrentValue.ToString() + " °C",
                SubTitle = p.InstanceName,
                IcoPath = "Images\\Typicons_e101(0)_64.png",  //relative path to your plugin directory
                Action = e =>
                {
                        // return false to tell Wox not to hide query window, otherwise Wox will hide it automatically
                        return false;
                    
                }
            }).ToList();

            return results;
        }
    }

    public class Temperature
    {
        public double CurrentValue { get; set; }
        public string InstanceName { get; set; }
        public static List<Temperature> Temperatures
        {
            get
            {
                List<Temperature> result = new List<Temperature>();
                ManagementObjectSearcher searcher = new ManagementObjectSearcher(@"root\WMI", "SELECT * FROM MSAcpi_ThermalZoneTemperature");
                foreach (ManagementObject obj in searcher.Get())
                {
                    Double temp = Convert.ToDouble(obj["CurrentTemperature"].ToString());
                    temp = (temp - 2732) / 10.0;
                    result.Add(new Temperature { CurrentValue = temp, InstanceName = obj["InstanceName"].ToString() });
                }
                return result;

            }
        }
    }
}
