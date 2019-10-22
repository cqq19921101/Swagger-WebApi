using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

namespace Liteon.Mes.Utility
{
    /// <summary>
    /// WindowFramework需要調用的接口類
    /// </summary>
    public class MesForm:IMesForm
    {
        public System.Windows.Forms.Form CreateForm(string assemblyName, string namespaceName, string formName, string[] args)
        {
            System.Windows.Forms.Form frm = (System.Windows.Forms.Form)Assembly.Load(assemblyName)
                                               .CreateInstance(namespaceName + "." + formName,
                                                               false,
                                                               BindingFlags.Default,
                                                               null,
                                                               args,
                                                               null,
                                                               null);
            return frm;
        }
    }
}
