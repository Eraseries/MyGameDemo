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
        Image item_select;

        [HideInInspector]
        public bool has_item = false;
        public enum ItemType
        {
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
            item_select = transform.Find("Select").GetComponent<Image>();
            item_select.color = new Color(1, 1, 1, 0);
        }

        private void Start()
        {
            InitItem();
        }

        private void InitItem()
        {
            int ran = Random.Range(0, 2);
            if (ran == 0)
            {
                has_item = true;

            }
            else
            {
                has_item = true;
            }
            item.SetActive(has_item);
            item_count.text = Random.Range(0, 100).ToString();
        }

        private void UpdateItem()
        {
            
        }


        public void SetSelect(bool select = true)
        {
            if (select)
            {
                item_select.color = new Color(1, 1, 1, 1);
                Dotween(DotweenType.ScaleMove, icon_go, new Vector3(1.15f, 1.15f, 1.15f), 0.5f);
            }
            else
            {
                item_select.color = new Color(1, 1, 1, 0);
                Dotween(DotweenType.ScaleMove, icon_go, new Vector3(1f, 1f, 1f), 0.5f);
            }
        }

        private void OnDisable()
        {
            SetSelect(false);
        }
    }
}
