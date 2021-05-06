using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using HtmlAgilityPack;

namespace Kuranado.Moe.FFXIV
{
    public partial class Mainform : Form
    {
        public Mainform()
        {
            InitializeComponent();
            this.Load += this.MainformLoad;
            this.FormClosing += this.MainformFormClosing;
        }

        private void MainformLoad(object sender, EventArgs e)
        {
            if (!Directory.Exists(CommonSetting.WikiDataDir))
                Directory.CreateDirectory(CommonSetting.WikiDataDir);
            if (!Directory.Exists(CommonSetting.DataDir))
                Directory.CreateDirectory(CommonSetting.DataDir);
            this.BeginInvoke(new Action(() => TroopExploration.LoadArchive()));
        }

        private void MainformFormClosing(object sender, FormClosingEventArgs e)
        {
            TroopExploration.SaveArchive();
        }
    }
}
