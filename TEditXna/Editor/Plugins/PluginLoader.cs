using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TEditXna.ViewModel;

namespace TEditXna.Editor.Plugins
{
    class PluginLoader
    {
        List<Assembly> m_assembly;
        WorldViewModel m_wvm;
        public PluginLoader(WorldViewModel wvm, string searchPath = @".\plugin\")
        {
            m_wvm = wvm;
            m_assembly = new List<Assembly>();
            string[] pluginFiles = Directory.GetFiles(Path.Combine(Application.StartupPath, searchPath), "*.dll");

            for (int i = 0; i < pluginFiles.Length; i++)
            {
                try
                {
                    m_assembly.Add(Assembly.LoadFile(pluginFiles[i]));
                }
                catch (Exception e1)
                {
                }
            }
            foreach(var e in m_assembly)
            {
                foreach (Type type in e.GetTypes())
                {
                    if (type.IsClass && type.BaseType.Name == "BasePlugin")
                    {
                        m_wvm.Plugins.Add((IPlugin)Activator.CreateInstance(type, m_wvm));
                    }
                }
            }
        }
    }
}
