using System.Collections.Generic;
using UnityEngine;

namespace Demo
{
    //UniqueID 是一个system生成的一个特殊的id
    public class CustomListBank : LoopListBankBase
    {
        private List<int> m_ContentsForInitData = new List<int>
        {
            1, 2, 3, 4, 5, 6, 7,
            8, 9, 10,11, 12, 13, 14,
            15, 16, 17, 18, 19, 20, 21,
            22, 23, 24, 25, 26, 27, 28
        };
        

        private List<LoopListBankData> _LoopListBankDataList;

        //数据表
        private List<LoopListBankData> m_LoopListBankDataList
        {
            get
            {
                if (_LoopListBankDataList == null)
                {
                    _LoopListBankDataList = new List<LoopListBankData>();
                    _LoopListBankDataList = InitLoopListBankDataList();
                }

                return _LoopListBankDataList;
            }
            set { _LoopListBankDataList = value; }
        }

        // Cell Sizes
        public List<Vector2> m_CellSizes = new List<Vector2> 
        {
            new Vector2(90, 96),
            new Vector2(90, 96),
            new Vector2(90, 96)
        };

        //初始化Item个数
        public override List<LoopListBankData> InitLoopListBankDataList()
        {
            m_LoopListBankDataList.Clear();
            LoopListBankData TempCustomData = null;
            for (int i = 0; i < m_ContentsForInitData.Count; ++i)
            {
                TempCustomData = new LoopListBankData();
                TempCustomData.Content = m_ContentsForInitData[i];
                TempCustomData.UniqueID = System.Guid.NewGuid().ToString();
                m_LoopListBankDataList.Add(TempCustomData);
            }

            return m_LoopListBankDataList;
        }

        public override int GetListLength()
        {
            return m_LoopListBankDataList.Count;
        }


        //通过index获取数据表里的某个数据
        public override LoopListBankData GetLoopListBankData(int index)
        {
            if(m_LoopListBankDataList.Count <= index)
            {
                return new LoopListBankData();
            }
            return m_LoopListBankDataList[index];
        }


        //获取数据表
        public override List<LoopListBankData> GetLoopListBankDatas()
        {
            return m_LoopListBankDataList;
        }

        //设置数据表
        public override void SetLoopListBankDatas(List<LoopListBankData> newDatas)
        {
            m_LoopListBankDataList = newDatas;
        }


        //寻找某个id
        public int FindUniqueID(string UniqueID)
        {
            if (string.IsNullOrEmpty(UniqueID))
            {
                return -1;
            }

            for (int i = 0; i < m_LoopListBankDataList.Count; ++i)
            {
                if (m_LoopListBankDataList[i].UniqueID == UniqueID)
                {
                    return i;
                }
            }

            return -1;
        }


        //
        public void AddContent(object newContent)
        {
            LoopListBankData TempCustomData = new LoopListBankData();
            TempCustomData.Content = newContent;
            TempCustomData.UniqueID = System.Guid.NewGuid().ToString();
            m_LoopListBankDataList.Add(TempCustomData);
        }

        //通过index删除某个item
        public void DelContentByIndex(int index)
        {
            if (m_LoopListBankDataList.Count <= index)
            {
                return;
            }
            m_LoopListBankDataList.RemoveAt(index);
        }


        //
        public void SetContents(List<int> newContents)
        {
            m_ContentsForInitData = newContents;
            InitLoopListBankDataList();
        }

        public override int GetCellPreferredTypeIndex(int index)
        {
            var TempConten = GetLoopListBankData(index).Content;

            int TempData = (int)TempConten;
            int ResultIndex = Mathf.Abs(TempData) % 3;

            return ResultIndex;
        }

        //获取Item的PreferredSize
        public override Vector2 GetCellPreferredSize(int index)
        {
            int ResultIndex = GetCellPreferredTypeIndex(index);

            Vector2 FinalValue = m_CellSizes[ResultIndex];

            return FinalValue;
        }
    }
}