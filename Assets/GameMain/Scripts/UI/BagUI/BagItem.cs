using GameFramework.DataTable;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityGameFramework.Runtime;

namespace StarForce
{
    public class BagItem : UGuiForm
    {
        GameObject item;
        GameObject icon_go;
        Text item_count;
        Image item_icon;
        Image item_quality;
        GameObject item_select;


        [HideInInspector]
        public bool has_item = false;

        [HideInInspector]
        public enum ItemType
        {
            None,
            Equip,
            Prop,
            Pet,
            Other
        }
        public ItemType enumType;
        private void Awake()
        {
            item = transform.Find("Item").gameObject;
            icon_go = transform.Find("Item/Icon").gameObject;
            item_quality = transform.Find("Item/Quality").GetComponent<Image>();
            item_icon = icon_go.GetComponent<Image>();
            item_count = transform.Find("Item/Count").GetComponent<Text>();
            item_select = transform.Find("Select").gameObject;
            item_select.SetActive(false);
        }

        private void Start()
        {
            UpdateInfo();
        }

        public void UpdateInfo(BagData info = null)
        {
            SetSelect(false);
            item.SetActive(false);
            if(info == null)
            {
                return;
            }
            item.SetActive(true);
            IDataTable<DRItem> dtItem = GameEntry.DataTable.GetDataTable<DRItem>();
            DRItem data = dtItem.GetDataRow(1);
        }

        public void SetSelect(bool select = true)
        {
            if (select)
            {
                item_select.SetActive(true);

                Dotween(DotweenType.ScaleMove, icon_go, new Vector3(1.15f, 1.15f, 1.15f), 0.5f);
            }
            else
            {
                item_select.SetActive(false);
                Dotween(DotweenType.ScaleMove, icon_go, new Vector3(1f, 1f, 1f), 0.5f);
            }
        }
    }
}
