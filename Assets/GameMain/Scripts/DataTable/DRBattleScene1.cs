//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2021 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------
// 此文件由工具自动生成，请勿直接修改。
// 生成时间：2023-02-10 14:15:23.275
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
    /// 物品。
    /// </summary>
    public class DRBattleScene1 : DataRowBase
    {
        private int m_Id = 0;

        /// <summary>
        /// 获取编号。
        /// </summary>
        public override int Id
        {
            get
            {
                return m_Id;
            }
        }

        /// <summary>
        /// 物品名称。
        /// </summary>
        public float X
        {
            get;
            private set;
        }

        /// <summary>
        /// 背包每格可放最大容量。
        /// </summary>
        public float Y
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取品质。
        /// </summary>
        public float Z
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取物品描述。
        /// </summary>
        public string Type
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
            X = float.Parse(columnStrings[index++]);
            Y = float.Parse(columnStrings[index++]);
            Z = float.Parse(columnStrings[index++]);
            Type = columnStrings[index++];
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
                    X = binaryReader.ReadSingle();
                    Y = binaryReader.ReadSingle();
                    Z = binaryReader.ReadSingle();
                    Type = binaryReader.ReadString();
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
