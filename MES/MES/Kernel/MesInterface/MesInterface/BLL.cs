using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

namespace MesInterface
{
    public class BLL
    {
        public virtual DataSet DoQuery(string cmdCode,string factoryId, Dictionary<string, object> param, string urlString)
        {
            return null;
        }

        public virtual DataTable DoAdd(string cmdCode, string factoryId, Dictionary<string, object> param, string urlString)
        {
            return null;
        }
    }
}