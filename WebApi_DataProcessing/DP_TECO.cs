using LiteOn.EA.DAL;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApi_CacheHelper;

namespace WebApi_DataProcessing
{
    public  class DP_TECO : DP_Standard
    {        
        /// <summary>
        /// CZ OPS TECO
        /// </summary>
        /// <param name="PlantNo">2301</param>
        /// <returns></returns>
        public override DataTable ReturnTECOByCZOPS(string PlantNo,string LineCode)
        {
                return ComputeTecoYield(PlantNo, LineCode, DateTime.Now); 
        }


    }
}
