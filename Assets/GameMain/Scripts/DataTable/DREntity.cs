//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2021 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------
// 此文件由工具自动生成，请勿直接修改。
// 生成时间：2023-02-10 14:15:23.290
//------------------------------------------------------------

using GameFramework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace StarForce
{
    /// <summary>
    /// 实体表。
    /// </summary>
    public class DREntity : DataRowBase
    {
        private int m_Id = 0;

        /// <summary>
        /// 获取实体编号。
        /// </summary>
        public override int Id
        {
            get
            {
                return m_Id;
            }
        }

        /// <summary>
        /// 获取资源名称。
        /// </summary>
        public string AssetName
        {
            get;
            private set;
        }

        /// <summary>
        /// 模型名字
        /// </summary>
        public string Name
        {
            get;
            private set;
        }

        /// <summary>
        /// 稀有度
        /// </summary>
        public int Rarity
        {
            get;
            private set;
        }

        /// <summary>
        /// 模型类型
        /// </summary>
        public string Type
        {
            get;
            private set;
        }

        /// <summary>
        /// 经验值(稀有度*等级*10)
        /// </summary>
        public string Exp
        {
            get;
            private set;
        }

        /// <summary>
        /// 优先级（战斗力的出战顺序）
        /// </summary>
        public int Priority
        {
            get;
            private set;
        }

        /// <summary>
        /// 基础血量(50 + 等级*稀有度*2)
        /// </summary>
        public string BaseHp
        {
            get;
            private set;
        }


        /// <summary>
        /// 提供的经验(稀有度*5 + 等级 * 2)
        /// </summary>
        public string DeadOfferExp
        {
            get;
            private set;
        }

        /// <summary>
        /// 每回合可以使用的卡牌数
        /// </summary>
        public int RoundUseCardCount
        {
            get;
            private set;
        }
        
        public override bool ParseDataRow(string dataRowString, object userData)
        {
            string[] columnStrings = dataRowString.Split(DataTableExtension.DataSplitSeparators);
            for (int i = 0; i < columnStrings.Length; i++)
            {
                columnStrings[i] = columnStrings[i].Trim(DataTableExtension.DataTrimSeparators);
            }

            int index = 0;
            index++;
            m_Id = int.Parse(columnStrings[index++]);
            index++;
            AssetName = columnStrings[index++];
            Name = columnStrings[index++];
            Rarity = int.Parse(columnStrings[index++]);
            Type = columnStrings[index++];
            Exp = columnStrings[index++];
            Priority = int.Parse(columnStrings[index++]);
            BaseHp = columnStrings[index++];
            DeadOfferExp = columnStrings[index++];
            RoundUseCardCount = int.Parse(columnStrings[index++]);
            GeneratePropertyArray();
            return true;
        }

        public override bool ParseDataRow(byte[] dataRowBytes, int startIndex, int length, object userData)
        {
            using (MemoryStream memoryStream = new MemoryStream(dataRowBytes, startIndex, length, false))
            {
                using (BinaryReader binaryReader = new BinaryReader(memoryStream, Encoding.UTF8))
                {
                    m_Id = binaryReader.Read7BitEncodedInt32();
                    AssetName = binaryReader.ReadString();
                    Name = binaryReader.ReadString();
                    Rarity = binaryReader.Read7BitEncodedInt32();
                    Type = binaryReader.ReadString();
                    Exp = binaryReader.ReadString();
                    Priority = binaryReader.Read7BitEncodedInt32();
                    BaseHp = binaryReader.ReadString();
                    DeadOfferExp = binaryReader.ReadString();
                    RoundUseCardCount = binaryReader.Read7BitEncodedInt32();
                }
            }

            GeneratePropertyArray();
            return true;
        }

        private void GeneratePropertyArray()
        {

        }
    }
}
