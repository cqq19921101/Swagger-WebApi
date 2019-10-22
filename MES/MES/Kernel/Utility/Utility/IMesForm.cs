using System;
using System.Collections.Generic;
using System.Text;

namespace Liteon.Mes.Utility
{
    public interface IMesForm
    {
        /// <summary>
        /// 創建窗體實例
        /// </summary>
        /// <param name="assemblyName">Assembly Name</param>
        /// <param name="namespaceName">命名空間名稱</param>
        /// <param name="formName">窗體名，不含.cs</param>
        /// <param name="startArgs">啟動參數，要和窗體構造函數參數一致</param>
        /// <returns></returns>
        System.Windows.Forms.Form CreateForm(string assemblyName, string namespaceName, string formName, string[] startArgs);
    }
}
