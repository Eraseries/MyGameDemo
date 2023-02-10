using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using UnityEngine.UI;
using StarForce;

namespace Demo
{
    [RequireComponent(typeof(Button))]
    public class ScrollIndexCallbackBase : MonoBehaviour
    {
        public LayoutElement m_Element;
        public Button m_Button;
        private int m_IndexID = 0;
        private string m_UniqueID = "";
        private string m_PrefabName = "";
        private object m_Content;
        private bool m_IsUpdateGameObjectName = true;
        private BagItem bagItem;
        protected virtual void Awake()
        {
            bagItem = transform.GetComponent<BagItem>();
            m_Button.onClick.AddListener(() => {
                if (bagItem.has_item)
                {
                    bagItem.SetSelect();
                }
            });
        }

        protected virtual void OnDestroy()
        {
            m_Button.onClick.RemoveAllListeners();
        }


        // Get IndexID
        public int GetIndexID()
        {
            return m_IndexID;
        }

        public string GetUniqueID()
        {
            return m_UniqueID;
        }

        public void SetUniqueID(string UniqueID)
        {
            m_UniqueID = UniqueID;
        }

        public void SetPrefabName(string name)
        {
            m_PrefabName = name;
        }

        // Get PrefabName
        public string GetPrefabName()
        {
            return m_PrefabName;
        }

        public void SetIsUpdateGameObjectName(bool value)
        {
            m_IsUpdateGameObjectName = value;
        }

        public object GetContent()
        {
            return m_Content;
        }

        // Set Element PreferredWidth
        public virtual void SetLayoutElementPreferredWidth(float value)
        {
            m_Element.preferredWidth = value;
        }

        // Set Element PreferredHeight
        public virtual void SetLayoutElementPreferredHeight(float value)
        {
            m_Element.preferredHeight = value;
        }

        //获取每个item的index
        public virtual void ScrollCellIndex(int idx, object content, string ClickUniqueID = "", object ClickObject = null)
        {
            m_IndexID = idx;
            m_Content = content;

            if (m_IsUpdateGameObjectName)
            {
                gameObject.name = string.Format("{0}_{1}", m_PrefabName, idx.ToString());
            }
        }

        public virtual void RefeashUI(string ClickUniqueID, object ClickContent)
        {

        }
    }
}
