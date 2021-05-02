using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace Kuranado.Moe.FFXIV
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
#if DEBUG
            if (!File.Exists(CommonSetting.ConfigPath))
                File.Create(CommonSetting.ConfigPath).Close();
#endif

            if (!File.Exists(CommonSetting.ConfigPath))
                MessageBox.Show("配置文件缺失或没有访问权限!", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            else
                Application.Run(new Mainform());
        }
    }
}
