using HtmlAgilityPack;
using System;
using System.Collections.Generic;
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
        private const string craftInfoPattern = @" (?<name>[\u4e00-\u9fa5]+)\s*?   #物品名字
                                                   ×(?<num>\d+)\s*?               #物品数量
                                                   \[
                                                   (\s*?/?\s*?[\u4e00-\u9fa5]:\d{1,3}★?\d?){0,2}   #0个到2个制作职业信息,例如 锻:39 / 甲:37 
                                                   (\s*?/?\s*?理符)?               #可能存在理符获得途径
                                                   (\s*?/?\s*?(?<cost>\d+)G)?      #价格
                                                   (\s*?/?\s*?(?<cvt>兑换))?       #是否可兑换
                                                   \]";
        #endregion

        #region Properties
        private static PartInfo[,] _airship = null;
        private static PartInfo[,] _submarine = null;

        /// <summary>
        /// 飞空艇船体(第一格)
        /// </summary>
        public static PartInfo[] AirshipHull
        {
            get => throw new NotImplementedException("未实现");
            private set => throw new NotImplementedException("未实现");
        }
        /// <summary>
        /// 飞空艇舾装(第二格)
        /// </summary>
        public static PartInfo[] AirshipOutfitting
        {
            get => throw new NotImplementedException("未实现");
            private set => throw new NotImplementedException("未实现");
        }
        /// <summary>
        /// 飞空艇船首(第三格)
        /// </summary>
        public static PartInfo[] AirshipBow
        {
            get => throw new NotImplementedException("未实现");
            private set => throw new NotImplementedException("未实现");
        }
        /// <summary>
        /// 飞空艇船尾(第四格)
        /// </summary>
        public static PartInfo[] AirshipStern
        {
            get => throw new NotImplementedException("未实现");
            private set => throw new NotImplementedException("未实现");
        }

        /// <summary>
        /// 潜艇船体(第一格)
        /// </summary>
        public static PartInfo[] SubmarineHull
        {
            get => throw new NotImplementedException("未实现");
            private set => throw new NotImplementedException("未实现");
        }
        /// <summary>
        /// 潜艇船尾(第二格)
        /// </summary>
        public static PartInfo[] SubmarineStern
        {
            get => throw new NotImplementedException("未实现");
            private set => throw new NotImplementedException("未实现");
        }
        /// <summary>
        /// 潜艇船首(第三格)
        /// </summary>
        public static PartInfo[] SubmarineBow
        {
            get => throw new NotImplementedException("未实现");
            private set => throw new NotImplementedException("未实现");
        }
        /// <summary>
        /// 潜艇舰桥(第四格)
        /// </summary>
        public static PartInfo[] SubmarineBridge
        {
            get => throw new NotImplementedException("未实现");
            private set => throw new NotImplementedException("未实现");
        }
        #endregion

        #region 废案代码

        //private static PartInfo[,] _airship = null;
        //private static PartInfo[,] _submarine = null;

        ///// <summary>
        ///// 获取指定代号的飞空艇套装
        ///// </summary>
        ///// <param name="code">
        ///// <para>4位的数字代号,例如2571指:</para>
        ///// <para>2号图的无敌级船体</para>
        ///// <para>5号图的奥德赛级气囊</para>
        ///// <para>7号图的威尔特甘斯级船首</para>
        ///// <para>1号图的野马级船尾</para>
        ///// <para>另外,0代表此项空缺</para>
        ///// </param>
        ///// <returns></returns>
        //public static PartInfo?[] ArishipSuit(string code)
        //{
        //    return null;
        //}
        ///// <summary>
        ///// 获取指定代号的潜水艇套装
        ///// </summary>
        ///// <param name="code">
        ///// <para>4~8位的数字和'+'组成的代号</para>
        ///// <para>一位数字后面最多只能有一个'+'</para>
        ///// <para>'+'代表改级</para>
        ///// <para>另外,0代表此项空缺</para>
        ///// </param>
        ///// <returns></returns>
        //public static PartInfo?[] SubmarineSuit(string code)
        //{
        //    return null;
        //}

        #endregion

        /// <summary>
        /// <para>从wiki爬取并刷新飞空艇和潜艇部件信息</para>
        /// <para>注意: 此方法可能导致阻塞</para>
        /// </summary>
        public static void Reflush()
        {
            var web = new HtmlWeb();
            foreach (var suit in airshipParts)
            {
                foreach (var partName in suit)
                {
                    var partInfo = CrawlPage(partName);
                    partInfo.Name = partName;
                }
            }
            foreach (var suit in submarineParts)
            {
                foreach (var partName in suit)
                {
                    var partInfo = CrawlPage(partName);
                    partInfo.Name = partName;
                }
            }
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

        /// <summary>
        /// 爬取并解析部件信息
        /// </summary>
        /// <param name="link">要爬取的页面链接</param>
        /// <returns></returns>
        private static PartInfo CrawlPage(string link)
        {
            var result = new PartInfo();

            var html = new HtmlWeb().Load(link);
            var partInfoXPath = @"//div[@class=""infobox-item ff14-content-box""]";
            var craftXPath = @"//div[@class=""item-craft-table-list filter-div""]//table/tr[4]/td/ul/li/ul";

            //  part info
            var partInfoHtml = html.DocumentNode.SelectSingleNode(partInfoXPath);
            var partRegex = new Regex(partInfoPattern, RegexOptions.Compiled | RegexOptions.ExplicitCapture | RegexOptions.IgnorePatternWhitespace);
            var partMatch = partRegex.Match(partInfoHtml.OuterHtml);
            result.UsingLv = byte.Parse(partMatch.Groups["lv"].Value);
            result.Weight = byte.Parse(partMatch.Groups["weight"].Value);
            result.ExplorePerf = short.Parse(partMatch.Groups["explore"].Value);
            result.CollectionPerf = short.Parse(partMatch.Groups["collection"].Value);
            result.CruisingSpeed = short.Parse(partMatch.Groups["speed"].Value);
            result.SailingDistance = short.Parse(partMatch.Groups["distance"].Value);
            result.Lucky = short.Parse(partMatch.Groups["lucky"].Value);
            result.RepairCost = byte.Parse(partMatch.Groups["repair"].Value);

            //  craft info
            var craftInfoHtml = html.DocumentNode.SelectSingleNode(craftXPath);
            var craftRegex = new Regex(craftInfoPattern, RegexOptions.Compiled | RegexOptions.ExplicitCapture | RegexOptions.IgnorePatternWhitespace);
            var craftList = new List<(ItemSimInfo item, short num)>();
            foreach (var craftItem in craftInfoHtml.ChildNodes)
            {
                var innerText = craftItem.ChildNodes[0].InnerText;
                var craftMatch = craftRegex.Match(innerText).Groups;
                var craftInfo = new ItemSimInfo();
                var num = short.Parse(craftMatch["nun"].Value);
                craftInfo.Name = craftMatch["name"].Value;
                craftInfo.Convertible = craftMatch["cvt"].Value != "";
                if (craftMatch["cost"].Value != "")
                    craftInfo.Cost = short.Parse(craftMatch["cost"].Value);
                craftList.Add((craftInfo, num));
            }
            result.MeterialList = craftList.ToArray();

            return result;
        }
    }// end of class
}// end of namespace
