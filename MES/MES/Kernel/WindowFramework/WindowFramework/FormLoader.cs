using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using Liteon.Mes.Utility;
using System.IO;

namespace WindowFramework
{
    public static class FormLoader
    {
        public static Dictionary<string, IMesForm> LoadForms(string path)
        {
            try
            {
                Dictionary<string, IMesForm> plugins = new Dictionary<string, IMesForm>();
                string[] assemblyFiles = Directory.GetFiles(path, "*.dll");
                foreach (string file in assemblyFiles)
                {
                    Assembly assembly = Assembly.LoadFrom(file);
                    foreach (Type t in assembly.GetExportedTypes())
                    {
                        if (t.IsClass && typeof(IMesForm).IsAssignableFrom(t))
                        {
                            FileInfo fi = new FileInfo(file);
                            IMesForm plugin = Activator.CreateInstance(t) as IMesForm;
                            plugins.Add(fi.Name, plugin);
                            fi = null;
                        }
                    }
                }
                return plugins;
            }
            catch
            {
                return null;
            }
        }

        public static List<MesForm> LoadOneForm(string dllFullName)
        {
            try
            {
                List<MesForm> onePlugin = new List<MesForm>();

                Assembly assembly = Assembly.LoadFrom(dllFullName);
                foreach (Type t in assembly.GetExportedTypes())
                {
                    if (t.IsClass && typeof(MesForm).IsAssignableFrom(t))
                    {
                        MesForm plugin = Activator.CreateInstance(t) as MesForm;
                        onePlugin.Add(plugin);
                        break;
                    }
                }
                return onePlugin;
            }
            catch
            {
                return null;
            }
        }
    }
}
