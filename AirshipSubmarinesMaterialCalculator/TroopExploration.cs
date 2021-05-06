using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;

namespace Kuranado.Moe.FFXIV
{
    /// <summary>
    /// 部队探索相关(飞空艇/潜水艇)
    /// </summary>
    internal static class TroopExploration
    {
        #region Type Defined
        /// <summary>
        /// 飞空艇/潜水艇部件信息
        /// </summary>
        public struct PartInfo
        {
            /// <summary>
            /// 部件名
            /// </summary>
            public string Name;
            /// <summary>
            /// 装备该部件所需的潜艇/飞艇等级
            /// </summary>
            public byte UsingLv;
            /// <summary>
            /// 该部件占用的重量
            /// </summary>
            public byte Weight;
            /// <summary>
            /// 修理该部件需要的魔导机械修理材料数量
            /// </summary>
            public byte RepairCost;
            /// <summary>
            /// 探索性能
            /// </summary>
            public short ExplorePerf;
            /// <summary>
            /// 收集性能
            /// </summary>
            public short CollectionPerf;
            /// <summary>
            /// 巡航速度
            /// </summary>
            public short CruisingSpeed;
            /// <summary>
            /// 航行距离
            /// </summary>
            public short SailingDistance;
            /// <summary>
            /// 恩惠属性
            /// </summary>
            public short Lucky;
            /// <summary>
            /// 制作素材列表
            /// </summary>
            public (ItemSimInfo item, short num)[] MeterialList;
        }

        /// <summary>
        /// 物品简易信息
        /// </summary>
        public struct ItemSimInfo
        {
            /// <summary>
            /// 物品名
            /// </summary>
            public string Name;
            /// <summary>
            /// 是否可以在npc处交换/购买
            /// </summary>
            public bool Convertible;
            /// <summary>
            /// 该物品的购买价格(如果不可购买则为0)
            /// </summary>
            public short Cost;
        }
        #endregion

        #region Constant Data
        private const string rootLink = @"https://ff14.huijiwiki.com/wiki/物品:";
        private static readonly string dataPath = Path.Combine(CommonSetting.WikiDataDir, "TroopExplorationData.xml");
        private static readonly string[][] airshipParts =
        {
            new string[]{"野马级船体","野马级气囊","野马级船首","野马级船尾"},
            new string[]{"无敌级船体","无敌级螺旋桨","无敌级船首","无敌级船尾"},
            new string[]{"企业级船体","企业级气囊","企业级船首","企业级船尾"},
            new string[]{"无敌改级船体","无敌改级螺旋桨","无敌改级船首","无敌改级船尾"},
            new string[]{"奥德赛级船体","奥德赛级气囊","奥德赛级船首","奥德赛级船尾"},
            new string[]{"塔塔诺拉级船体","塔塔诺拉级螺旋桨","塔塔诺拉级船首","塔塔诺拉级船尾"},
            new string[]{"威尔特甘斯级船体","威尔特甘斯级灵翼","威尔特甘斯级船首","威尔特甘斯级船尾"}
        };
        private static readonly string[][] submarineParts =
        {
            new string[]{"鲨鱼级船体","鲨鱼级船尾","鲨鱼级船首","鲨鱼级舰桥"},
            new string[]{"甲鲎级船体","甲鲎级船尾","甲鲎级船首","甲鲎级舰桥"},
            new string[]{"须鲸级船体","须鲸级船尾","须鲸级船首","须鲸级舰桥"},
            new string[]{"腔棘鱼级船体","腔棘鱼级船尾","腔棘鱼级船首","腔棘鱼级舰桥"},
            new string[]{"希尔德拉级船体","希尔德拉级船尾","希尔德拉级船首","希尔德拉级舰桥"},
            new string[]{"鲨鱼改级船体","鲨鱼改级船尾","鲨鱼改级船首","鲨鱼改级舰桥"},
            new string[]{"甲鲎改级船体","甲鲎改级船尾","甲鲎改级船首","甲鲎改级舰桥"},
            new string[]{"须鲸改级船体","须鲸改级船尾","须鲸改级船首","须鲸改级舰桥"},
            new string[]{"腔棘鱼改级船体","腔棘鱼改级船尾","腔棘鱼改级船首","腔棘鱼改级舰桥"},
            new string[]{"希尔德拉改级船体","希尔德拉改级船尾","希尔德拉改级船首","希尔德拉改级舰桥"}
        };

        private const string partInfoPattern = @"等级：(?<lv>\d+)[^配]+
                                                 配件重量：(?<weight>\d+)[^探]+
                                                 探索性能：(?<explore>[\d+\-]+)[^收]+
                                                 收集性能：(?<collection>[\d+-]+)[^巡]+
                                                 巡航速度：(?<speed>[\d+-]+)[^航]+
                                                 航行距离：(?<distance>[\d+-]+)[^恩]+
                                                 恩惠：(?<lucky>[\d+-]+)[^魔]+
                                                 魔导机械修理材料×(?<repair>\d+)";
        private const string craftInfoPattern = @" (?<name>[\u4e00-\u9fa5]+)\s*?                    #物品名字
                                                   ×(?<num>\d+)\s*?                                #物品数量
                                                   \[
                                                   (\s*?/?\s*?[\u4e00-\u9fa5]:\d{1,3}★?\d?){0,3}   #0个到3个制作职业信息,例如 锻:52 / 甲:52 / 雕:52 
                                                   (\s*?/?\s*?[理符任务副本]{2,2}){0,3}             #可能存在的获得途径
                                                   (\s*?/?\s*?(?<cost>\d+)G)?                       #价格
                                                   (\s*?/?\s*?(?<cvt>兑换))?                        #是否可兑换
                                                   \]";
        #endregion

        #region Properties
        /// <summary>
        /// 所有飞空艇部件信息
        /// </summary>
        private static PartInfo[][] Airship { get; } = new PartInfo[7][]
        {
            new PartInfo[4],
            new PartInfo[4],
            new PartInfo[4],
            new PartInfo[4],
            new PartInfo[4],
            new PartInfo[4],
            new PartInfo[4]
        };
        /// <summary>
        /// 飞空艇船体(第一格)
        /// </summary>
        public static ReadOnlyCollection<PartInfo> AirshipHull { get; } = new ReadOnlyCollection<PartInfo>((from suit in Airship select suit[0]).ToList());
        /// <summary>
        /// 飞空艇舾装(第二格)
        /// </summary>
        public static ReadOnlyCollection<PartInfo> AirshipOutfitting { get; } = new ReadOnlyCollection<PartInfo>((from suit in Airship select suit[1]).ToList());
        /// <summary>
        /// 飞空艇船首(第三格)
        /// </summary>
        public static ReadOnlyCollection<PartInfo> AirshipBow { get; } = new ReadOnlyCollection<PartInfo>((from suit in Airship select suit[2]).ToList());
        /// <summary>
        /// 飞空艇船尾(第四格)
        /// </summary>
        public static ReadOnlyCollection<PartInfo> AirshipStern { get; } = new ReadOnlyCollection<PartInfo>((from suit in Airship select suit[3]).ToList());

        /// <summary>
        /// 所有潜艇部件信息
        /// </summary>
        private static PartInfo[][] Submarine { get; } = new PartInfo[10][]
        {
            new PartInfo[4],
            new PartInfo[4],
            new PartInfo[4],
            new PartInfo[4],
            new PartInfo[4],
            new PartInfo[4],
            new PartInfo[4],
            new PartInfo[4],
            new PartInfo[4],
            new PartInfo[4]
        };
        /// <summary>
        /// 潜艇船体(第一格)
        /// </summary>
        public static ReadOnlyCollection<PartInfo> SubmarineHull { get; } = new ReadOnlyCollection<PartInfo>((from suit in Airship select suit[0]).ToList());
        /// <summary>
        /// 潜艇船尾(第二格)
        /// </summary>
        public static ReadOnlyCollection<PartInfo> SubmarineStern { get; } = new ReadOnlyCollection<PartInfo>((from suit in Airship select suit[1]).ToList());
        /// <summary>
        /// 潜艇船首(第三格)
        /// </summary>
        public static ReadOnlyCollection<PartInfo> SubmarineBow { get; } = new ReadOnlyCollection<PartInfo>((from suit in Airship select suit[2]).ToList());
        /// <summary>
        /// 潜艇舰桥(第四格)
        /// </summary>
        public static ReadOnlyCollection<PartInfo> SubmarineBridge { get; } = new ReadOnlyCollection<PartInfo>((from suit in Airship select suit[3]).ToList());
        #endregion

        #region PrivateMethod

        /// <summary>
        /// 爬取并解析部件信息
        /// </summary>
        /// <param name="link">要爬取的页面链接</param>
        /// <param name="savePath">页面保存路径,为空则不保存</param>
        /// <returns></returns>
        private static PartInfo CrawlPage(string link, string savePath = null)
        {
            var result = new PartInfo();

            var html = new HtmlWeb().Load(link);
            var partInfoXPath = @"//div[@class=""infobox-item ff14-content-box""]";
            var craftXPath = @"//div[@class=""item-craft-table-list filter-div""]//table/tr[4]/td/ul/li/ul";

            if (savePath != null)
                File.WriteAllText(savePath, html.DocumentNode.OuterHtml);

            //  part info
            var partInfoHtml = html.DocumentNode.SelectSingleNode(partInfoXPath);
            var partRegex = new Regex(partInfoPattern, RegexOptions.Compiled | RegexOptions.ExplicitCapture | RegexOptions.IgnorePatternWhitespace);
            var partMatch = partRegex.Match(partInfoHtml.OuterHtml).Groups;
            result.UsingLv = byte.Parse(partMatch["lv"].Value);
            result.Weight = byte.Parse(partMatch["weight"].Value);
            result.ExplorePerf = short.Parse(partMatch["explore"].Value);
            result.CollectionPerf = short.Parse(partMatch["collection"].Value);
            result.CruisingSpeed = short.Parse(partMatch["speed"].Value);
            result.SailingDistance = short.Parse(partMatch["distance"].Value);
            result.Lucky = short.Parse(partMatch["lucky"].Value);
            result.RepairCost = byte.Parse(partMatch["repair"].Value);

            //  craft info
            var craftInfoHtml = html.DocumentNode.SelectSingleNode(craftXPath);
            var craftRegex = new Regex(craftInfoPattern, RegexOptions.Compiled | RegexOptions.ExplicitCapture | RegexOptions.IgnorePatternWhitespace);
            var craftList = new List<(ItemSimInfo item, short num)>();
            foreach (var craftItem in craftInfoHtml.ChildNodes)
            {
                var innerText = craftItem.ChildNodes[0].InnerText;
                var craftMatch = craftRegex.Match(innerText).Groups;
                var craftInfo = new ItemSimInfo();
                var num = short.Parse(craftMatch["num"].Value);
                craftInfo.Name = craftMatch["name"].Value;
                craftInfo.Convertible = craftMatch["cvt"].Value != "";
                if (craftMatch["cost"].Value != "")
                    craftInfo.Cost = short.Parse(craftMatch["cost"].Value);
                craftList.Add((craftInfo, num));
            }
            result.MeterialList = craftList.ToArray();

            return result;
        }

        private static void Serialization(XmlDocument doc, XmlElement root, PartInfo[][] partInfos)
        {
            foreach (var suit in partInfos)
            {
                var suitElem = doc.CreateElement("Suit");
                foreach (var part in suit)
                {
                    var partElem = doc.CreateElement(part.Name);
                    partElem.SetAttribute(nameof(part.UsingLv), part.UsingLv.ToString());
                    partElem.SetAttribute(nameof(part.Weight), part.Weight.ToString());
                    partElem.SetAttribute(nameof(part.RepairCost), part.RepairCost.ToString());
                    partElem.SetAttribute(nameof(part.ExplorePerf), part.ExplorePerf.ToString());
                    partElem.SetAttribute(nameof(part.CollectionPerf), part.CollectionPerf.ToString());
                    partElem.SetAttribute(nameof(part.CruisingSpeed), part.CruisingSpeed.ToString());
                    partElem.SetAttribute(nameof(part.SailingDistance), part.SailingDistance.ToString());
                    partElem.SetAttribute(nameof(part.Lucky), part.Lucky.ToString());
                    foreach (var (item, num) in part.MeterialList)
                    {
                        var meteriaElem = doc.CreateElement("meteria");
                        meteriaElem.SetAttribute(nameof(item.Name), item.Name);
                        meteriaElem.SetAttribute(nameof(item.Convertible), item.Convertible.ToString());
                        meteriaElem.SetAttribute(nameof(item.Cost), item.Cost.ToString());
                        meteriaElem.SetAttribute(nameof(num), num.ToString());
                        partElem.AppendChild(meteriaElem);
                    }
                    suitElem.AppendChild(partElem);
                }
                root.AppendChild(suitElem);
            }
        }

        #endregion

        /// <summary>
        /// <para>从wiki爬取并刷新飞空艇和潜艇部件信息</para>
        /// <para>注意: 此方法可能导致阻塞</para>
        /// </summary>
        public static void Reflush()
        {
            //  airship
            for (var indexSuit = 0; indexSuit < airshipParts.Length; ++indexSuit)
            {
                for (var indexPart = 0; indexPart < 4; ++indexPart)
                {
                    var partName = airshipParts[indexSuit][indexPart];
                    var link = rootLink + partName;
                    var savePath = Path.Combine(CommonSetting.WikiDataDir, partName + ".html");
                    var partInfo = CrawlPage(link, savePath);
                    partInfo.Name = partName;
                    Airship[indexSuit][indexPart] = partInfo;
                }
            }
            //  submarine
            for (var indexSuit = 0; indexSuit < submarineParts.Length; ++indexSuit)
            {
                for (var indexPart = 0; indexPart < 4; ++indexPart)
                {
                    var partName = submarineParts[indexSuit][indexPart];
                    var link = rootLink + partName;
                    var savePath = Path.Combine(CommonSetting.WikiDataDir, partName + ".html");
                    var partInfo = CrawlPage(link, savePath);
                    partInfo.Name = partName;
                    Submarine[indexSuit][indexPart] = partInfo;
                }
            }
        }

        /// <summary>
        /// 保存飞空艇和潜水艇部件信息到本地
        /// </summary>
        /// <param name="path">可选的保存文件路径</param>
        public static void SaveArchive(string path = null)
        {
            var dir = Path.Combine(CommonSetting.DataDir, nameof(TroopExploration));
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);
            path = path ?? Path.Combine(dir, "PartsInfo.xml");

            var doc = new XmlDocument();
            var root = doc.CreateElement(nameof(TroopExploration));
            doc.AppendChild(root);

            var airshipElem = doc.CreateElement(nameof(Airship));
            Serialization(doc, airshipElem, Airship);
            root.AppendChild(airshipElem);

            var submarineElem = doc.CreateElement(nameof(Submarine));
            Serialization(doc, submarineElem, Submarine);
            root.AppendChild(submarineElem);

            doc.Save(path);
        }

        /// <summary>
        /// 从本地文件加载飞空艇和潜水艇部件信息,
        /// 若本地文件不存在,并且参数reflush为true,
        /// 则从wiki爬取并刷新信息
        /// </summary>
        /// <param name="reflush">若文件不存在,是否进行爬取并刷新</param>
        public static void LoadArchive(bool reflush = true)
        {
            if (!File.Exists(dataPath))
            {
                if (reflush)
                    Reflush();
                return;
            }
            var doc = new XmlDocument();
            doc.Load(dataPath);
        }
    }// end of class
}// end of namespace
