using System;

namespace Kuranado.Moe.FFXIV
{
    public static class CommonSetting
    {
        /// <summary>
        /// 0v0我自己写的全部ffxiv小工具的配置文件路径
        /// </summary>
        public const string ConfigPath = @".\kconfig.json";

        /// <summary>
        /// 存放程序数据的目录
        /// </summary>
        public static readonly string DataDir = @"./MyData";

        /// <summary>
        /// 灰机wiki相关数据目录
        /// </summary>
        public static readonly string WikiDataDir = @"./WikiData";

        /// <summary>
        /// 部队合建物品分类
        /// </summary>
        public enum JointConstrType
        {
            Airship = 1,
            Submarine = Airship >> 1,
        }

        /// <summary>
        /// 部队合建道具信息
        /// </summary>
        public struct JointConstrItem
        {
            /// <summary>
            /// 道具ID
            /// </summary>
            public int ID;
            /// <summary>
            /// 道具名
            /// </summary>
            public string Name;
            /// <summary>
            /// 灰机Wiki链接
            /// </summary>
            public string WikiLink;
            /// <summary>
            /// 这个道具出现的版本号
            /// </summary>
            public string VerNo;
            /// <summary>
            /// 制作该道具的材料
            /// </summary>
            public JointConstrItem[] ProdMat;
            /// <summary>
            /// 物品品级
            /// </summary>
            public int ItemLv;
            /// <summary>
            /// 物品的可装备等级
            /// </summary>
            public int EquipableLv;
            /// <summary>
            /// 物品重量(飞空艇潜水艇)
            /// </summary>
            public int Weight;
            /// <summary>
            /// 合建主分类
            /// </summary>
            public JointConstrType MajorType;
            /// <summary>
            /// 合建子分类
            /// </summary>
            public JointConstrType MinorType;
            /// <summary>
            /// 其它额外信息
            /// </summary>
            public object ExtraInfo;
        }

        /// <summary>
        /// FFXIV道具信息
        /// </summary>
        public struct ItemInfo
        {
        }
    }
}
