using DigitalPlatform.CirculationClient;
using DigitalPlatform.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace dp2mini
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {

            // 前端信息
            ClientInfo.TypeOfProgram = typeof(Program);

            // 检查是否是开发模式,即命令行是否有develop
            if (StringUtil.IsDevelopMode() == false)
                ClientInfo.PrepareCatchException();

            string strClientVersion = Assembly.GetAssembly(typeof(Program)).GetName().Version.ToString();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());

        }
    }
}
