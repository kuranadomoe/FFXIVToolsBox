using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
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
            var test = new HtmlWeb().Load(@"https://ff14.huijiwiki.com/wiki/%E7%89%A9%E5%93%81:%E6%97%A0%E6%95%8C%E7%BA%A7%E8%88%B9%E4%BD%93").DocumentNode;
            var xp1 = @"//div[@class=""infobox-item ff14-content-box""]";
            var xp2 = @"//div[@class=""item-craft-table-list filter-div""]//table/tr[4]/td/ul/li/ul";
            var node1 = test.SelectSingleNode(xp1);
            var node2 = test.SelectSingleNode(xp2);
            //System.IO.File.WriteAllText("test.html", test.OuterHtml);
            //System.IO.File.WriteAllText("test1.html", node1.OuterHtml);
            //System.IO.File.WriteAllText("test2.html", node2.OuterHtml);
            var result = new TroopExploration.PartInfo();
            var reg = new Regex(@"等级：(?<lv>\d+)[^配]+
                                  配件重量：(?<weight>\d+)[^探]+
                                  探索性能：(?<explore>[\d+\-]+)[^收]+
                                  收集性能：(?<collection>[\d+-]+)[^巡]+
                                  巡航速度：(?<speed>[\d+-]+)[^航]+
                                  航行距离：(?<distance>[\d+-]+)[^恩]+
                                  恩惠：(?<lucky>[\d+-]+)[^魔]+
                                  魔导机械修理材料×(?<repair>\d+)",
                                  RegexOptions.Compiled | RegexOptions.ExplicitCapture | RegexOptions.IgnorePatternWhitespace);
            var match = reg.Match(node1.OuterHtml);
            MessageBox.Show("等级" + match.Groups["lv"].Value + System.Environment.NewLine +
           "配件重量" + match.Groups["weight"].Value + System.Environment.NewLine +
           "探索性能" + match.Groups["explore"].Value + System.Environment.NewLine +
           "收集性能" + match.Groups["collection"].Value + System.Environment.NewLine +
           "巡航速度" + match.Groups["speed"].Value + System.Environment.NewLine +
           "航行距离" + match.Groups["distance"].Value + System.Environment.NewLine +
           "恩惠" + match.Groups["lucky"].Value + System.Environment.NewLine +
           "修理材料" + match.Groups["repair"].Value);

            var reg2 = new Regex(@" (?<name>[\u4e00-\u9fa5]+)\s*?   #物品名字
                                    ×(?<num>\d+)\s*?               #物品数量
                                    \[
                                    (\s*?/?\s*?[\u4e00-\u9fa5]:\d{1,3}★?\d?){0,2}   #0个到2个制作职业信息,例如 锻:39 / 甲:37 
                                    (\s*?/?\s*?理符)?               #可能存在理符获得途径
                                    (\s*?/?\s*?(?<cost>\d+)G)?      #价格
                                    (\s*?/?\s*?(?<cvt>兑换))?       #是否可兑换
                                    \]",
                                  RegexOptions.Compiled | RegexOptions.ExplicitCapture | RegexOptions.IgnorePatternWhitespace);
            var testOutput = "";
            var matchOutput = "";
            foreach (var node in node2.ChildNodes)
            {
                testOutput += node.ChildNodes[0].InnerText + System.Environment.NewLine;
                var tmp = reg2.Match(node.ChildNodes[0].InnerText);
                matchOutput += $@"物品{tmp.Groups["name"]}-数量{tmp.Groups["num"]}-价格{tmp.Groups["cost"]}-兑换:{tmp.Groups["cvt"]}{System.Environment.NewLine}";
            }
            MessageBox.Show(testOutput);
            MessageBox.Show(matchOutput);
        }
    }
}
