using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Demo
{
    [RequireComponent(typeof(UnityEngine.UI.LoopScrollRect))]
    [DisallowMultipleComponent]
    public class InitOnStartMulti : MonoBehaviour, LoopScrollPrefabSource, LoopScrollMultiDataSource
    {
        public LoopScrollRectMulti m_LoopScrollRect;

        public LoopListBankBase m_LoopListBank;


        // Cell Prefab
        public GameObject m_Item;

        [HideInInspector]
        public string m_ClickUniqueID = "";
        [HideInInspector]
        public object m_ClickObject;

        protected virtual void Awake()
        {
            pool_parent = transform.Find("Pool");
            m_LoopScrollRect.prefabSource = this;
            m_LoopScrollRect.dataSource = this;
            m_LoopScrollRect.totalCount = m_LoopListBank.GetListLength();
            m_LoopScrollRect.RefillCells();
        }

        // Implement your own Cache Pool here. The following is just for example.
        Stack<Transform> pool = new Stack<Transform>();
        Transform pool_parent;

        public virtual GameObject GetObject(int index)
        {
            Transform candidate = null;
            ScrollIndexCallbackBase TempScrollIndexCallbackBase = null;
            if (pool.Count == 0)
            {
                candidate = Instantiate(m_Item.transform, pool_parent);
            }
            else
            {
                candidate = pool.Pop();
            }

            // One Cell Prefab, Set PreferredSize as runtime.
            TempScrollIndexCallbackBase = candidate.GetComponent<ScrollIndexCallbackBase>();
            if (null != TempScrollIndexCallbackBase)
            {
                TempScrollIndexCallbackBase.SetPrefabName(m_Item.name);
                if (m_LoopScrollRect.horizontal)
                {
                    float RandomWidth = m_LoopListBank.GetCellPreferredSize(index).x;
                    TempScrollIndexCallbackBase.SetLayoutElementPreferredWidth(RandomWidth);
                }

                if (m_LoopScrollRect.vertical)
                {
                    float RandomHeight = m_LoopListBank.GetCellPreferredSize(index).y;
                    TempScrollIndexCallbackBase.SetLayoutElementPreferredHeight(RandomHeight);
                }
            }

            TempScrollIndexCallbackBase = candidate.gameObject.GetComponent<ScrollIndexCallbackBase>();
            if (null != TempScrollIndexCallbackBase)
            {
                TempScrollIndexCallbackBase.SetUniqueID(m_LoopListBank.GetLoopListBankData(index).UniqueID);
            }

            return candidate.gameObject;
        }

        public virtual void ReturnObject(Transform trans)
        {
            trans.SendMessage("ScrollCellReturn", SendMessageOptions.DontRequireReceiver);
            trans.gameObject.SetActive(false);
            trans.SetParent(pool_parent, false);
            pool.Push(trans);
        }

        public virtual void ProvideData(Transform transform, int idx)
        {
            //transform.SendMessage("ScrollCellIndex", idx);

            // Use direct call for better performance
            transform.GetComponent<ScrollIndexCallbackBase>()?.ScrollCellIndex(idx, m_LoopListBank.GetLoopListBankData(idx).Content, m_ClickUniqueID, m_ClickObject);
        }
    }
}