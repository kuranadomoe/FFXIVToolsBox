using System;
using static System.Console;
using static Kuranado.Moe.FFXIV.CommonSetting;
using HtmlAgilityPack;
using System.IO;
using System.Linq;
using System.Xml;
using System.Collections.Generic;

namespace Kuranado.Moe.FFXIV
{
    class Program
    {
        private const string WikiLink = @"https://ff14.huijiwiki.com/wiki/";
        private const string AirshipLink = WikiLink + "部队飞空艇";
        private const string SubmarineLink = WikiLink + "部队潜水艇";

        /// <summary>
        /// 飞空艇wiki
        /// </summary>
        private static readonly HtmlDocument AirShipWeb = new HtmlWeb().Load(AirshipLink);
        /// <summary>
        /// 潜艇wiki
        /// </summary>
        private static readonly HtmlDocument SubmarineWeb = new HtmlWeb().Load(SubmarineLink);

        /// <summary>
        /// 0.0
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            //  存一下网页原始数据
            if (!Directory.Exists(WikiDataDir))
                Directory.CreateDirectory(WikiDataDir);
            var airshipSavePath = Path.Combine(WikiDataDir, "部队飞空艇.html");
            var submarineSavePath = Path.Combine(WikiDataDir, "部队潜水艇.html");
            if (!File.Exists(airshipSavePath))
                File.WriteAllText(airshipSavePath, AirShipWeb.DocumentNode.OuterHtml, System.Text.Encoding.UTF8);
            if (!File.Exists(submarineSavePath))
                File.WriteAllText(submarineSavePath, SubmarineWeb.DocumentNode.OuterHtml, System.Text.Encoding.UTF8);


            var wikiPageXPath = @"//div[@class=""mw-parser-output""]";
            //  飞空艇部件
            var airshipXPath = wikiPageXPath + @"/table[@class=""wikitable shiftable mw-collapsible mw-collapsed""]";
            var airshipItemNodes = AirShipWeb.DocumentNode.SelectSingleNode(airshipXPath).ChildNodes;
            var allAirshipItem = from item in airshipItemNodes
                                 where item.Name == "tr"
                                 select item;
            foreach (var itemNode in allAirshipItem)
            {
                var tmp = itemNode.SelectNodes(@".//a");
                if (tmp == null)
                    continue;

            }

            //  潜水艇部件
            var submarineXPath =wikiPageXPath+ @"/table[@class=""wikitable shiftable mw-collapsible""]";
            var submarineItemNodes = SubmarineWeb.DocumentNode.SelectSingleNode(submarineXPath);
            Write(submarineItemNodes.OuterHtml);
            ReadKey(true);

            //  飞空艇和潜艇部件
            List<JointConstrItem> itemList = new List<JointConstrItem>();

            //  把处理好的数据存文档
            XmlDocument ffxivItem = new XmlDocument();
            XmlElement element = ffxivItem.CreateElement("");
        }

        private (string url, string name) GetItemURL(HtmlNode itemNode)
        {
            return (null, null);
        }


    }   //  end of class
}
