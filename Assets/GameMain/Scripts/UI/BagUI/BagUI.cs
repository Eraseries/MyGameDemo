using DG.Tweening;
using GameFramework.DataTable;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework.Runtime;

/// <summary>
/// 主界面模块
/// </summary>
namespace StarForce
{
    public class BagUI : UGuiForm
    {
        private BagItem selectBagItem;
        public BagItem SelectBagItem
        {
            get { return selectBagItem; }
            set { selectBagItem = value; }
        }

        private Transform content;
        private Transform top;
        private Transform right;
        private Transform left;


        private GameObject item_panel;
        private Vector2 item_panel_orign_rect;

        string[] tap_menu_name = {
            "Text_All",
            "Text_Equip",
            "Text_Consume",
            "Text_Material",
            "Text_Other",
        };
        string[] tap_panel_name = {
            "Panel_All",
            "Panel_Equip",
            "Panel_Consume",
            "Panel_Material",
            "Panel_Other",
        };
        GameObject[] tap_menu_group = new GameObject[5];
        GameObject[] tap_panel_group = new GameObject[5];
        private Transform bottom_menu;
        private int pre_type = 0;

        private Transform status_tip;
        private GameObject item_prefab;

        Dictionary<int, GameObject> slots = new Dictionary<int, GameObject>();

        private bool enable_item_panel = false;


        Button backBtn;
        GameObject[] sort_btn;
        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
            Name = "BagUI";
            content = transform.Find("Background");
            top = content.Find("Top");
            backBtn = top.Find("BackBtn").GetComponent<Button>();
            AddBtnEvent(backBtn, () => { Close(true); });// Close();
            AddBtnEvent(content.Find("TestBtn").GetComponent<Button>(), () => { Test(); });// Close();
            left = content.Find("Left");
            for (int i = 0; i < 5; i++)
            {
                int current_type = i;
                tap_menu_group[current_type] = left.Find("Tap_Menu/GameObject/"+ tap_menu_name[current_type]).gameObject;
                tap_panel_group[current_type] = left.Find("ScrollRect/" + tap_panel_name[current_type]).gameObject;
                AddToggleEvent(tap_menu_group[current_type].GetComponent<Toggle>(), (isOn) => {
                    if(isOn)
                    {
                        SelectTapMenuPanel(current_type);
                    }
                });
            }
            //status_tip = left.Find("Tip");
            //status_tip.gameObject.SetActive(false);
            //for (int i = 0; i < left.Find("Stats").childCount; i++)
            //{
            //    Transform child = left.Find("Stats").GetChild(i);
            //    AddBtnEvent(child.GetComponent<Button>(), () => { ShowStatusTip(true, child.Find("Icon")); });
            //}

            //item_panel = left.Find("ItemPanel").gameObject;
            //item_panel_orign_rect = item_panel.GetComponent<RectTransform>().anchoredPosition;
            //AddBtnEvent(item_panel.transform.Find("BottomMenu/SellBtn").GetComponent<Button>(), SellBtnEvent);
            //AddBtnEvent(item_panel.transform.Find("BottomMenu/UseBtn").GetComponent<Button>(), UseBtnEvent);
            //item_panel.SetActive(false);




            //bottom_menu = right.Find("Bottom_Menu");
            //AddToggleEvent(bottom_menu.Find("sort").GetComponent<Toggle>(), (bo) =>
            //{
            //    ShowSortType(bo);
            //});
            //sort_btn = new GameObject[bottom_menu.Find("sort/group").childCount];
            //for (int i = 0; i < bottom_menu.Find("sort/group").childCount; i++)
            //{
            //    int index = i;
            //    Transform child = bottom_menu.Find("sort/group").GetChild(index);
            //    sort_btn[index] = child.gameObject;
            //    sort_btn[index].SetActive(false);
            //    AddBtnEvent(child.GetComponent<Button>(), () =>
            //    {
            //        SortItem(index);
            //    });
            //}

            InitBagPanel();
        }

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            selectBagItem = null;
        }

        public override void Close()
        {
            base.Close();
        }


        protected override void OnDepthChanged(int uiGroupDepth, int depthInUIGroup)
        {
            base.OnDepthChanged(uiGroupDepth, depthInUIGroup);
        }

        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);
        }


        //初始化背包面板
        private void InitBagPanel1()
        {
            Transform parent = right.Find("ScrollParent/ScrollView/Content");
            for (int i = 0; i < parent.childCount; i++)
            {
                int index = i;
                slots.Add(index, parent.GetChild(index).gameObject);
                AddBtnEvent(slots[index].GetComponent<Button>(), () =>
                {
                    BagItem temp_item = slots[index].GetComponent<BagItem>();
                    if (temp_item.has_item)
                    {
                        if (selectBagItem != null)
                        {
                            if (selectBagItem == temp_item)
                            {
                                temp_item.SetSelect(false);
                                selectBagItem = null;
                                return;
                            }
                            selectBagItem.SetSelect(false);
                            temp_item.SetSelect();
                            selectBagItem = temp_item;
                            UpdateItemPanel(selectBagItem);
                        }
                        else
                        {
                            temp_item.SetSelect();
                            selectBagItem = temp_item;
                            UpdateItemPanel(selectBagItem);

                        }
                        ShowItemPanel();
                    }
                });
            }
        }


        private void InitBagPanel()
        {
            Transform parent = left.Find("ScrollRect/Panel_All");
            for (int i = 0; i < parent.childCount; i++)
            {
                int index = i;
                GameObject slot = parent.GetChild(index).gameObject;
                slot.name = "Bag_Slot_" + (index);
                slots.Add(index,slot.gameObject);
                AddBtnEvent(slots[index].GetComponent<Button>(), () =>
                {
                    BagItem temp_item = slots[index].GetComponent<BagItem>();
                    if (temp_item.has_item)
                    {
                        if (selectBagItem != null)
                        {
                            if (selectBagItem == temp_item)
                            {
                                temp_item.SetSelect(false);
                                selectBagItem = null;
                                return;
                            }
                            selectBagItem.SetSelect(false);
                            temp_item.SetSelect();
                            selectBagItem = temp_item;
                            UpdateItemPanel(selectBagItem);
                        }
                        else
                        {
                            temp_item.SetSelect();
                            selectBagItem = temp_item;
                            UpdateItemPanel(selectBagItem);

                        }
                        //ShowItemPanel();
                    }
                });
            }
        }

        //显示物品详情面板
        private void ShowItemPanel(bool _show = true)
        {
            if (enable_item_panel == false || _show == false)
            {
                Dotween(DotweenType.RectMove, item_panel, new Vector2(item_panel_orign_rect.x, item_panel_orign_rect.y), 0.5f, () =>
                {
                    item_panel.SetActive(false);
                }, Ease.InBack);
                return;
            }
            if (item_panel.activeSelf)
            {
                return;
            }
            item_panel.SetActive(true);
            Dotween(DotweenType.RectMove, item_panel, new Vector2(item_panel_orign_rect.x, item_panel_orign_rect.y - 800), 0.5f, null, Ease.OutBack);
        }


        private void UpdateItemPanel(BagItem item)
        {
            if (item == null)
            {
                return;
            }



        }


        //切换背包按钮
        private void SelectTapMenuPanel(int current_type)
        {
            tap_menu_group[pre_type].GetComponent<Text>().color = new Color(1,1,1,0.5f);
            tap_menu_group[current_type].GetComponent<Text>().color = Color.white;
            pre_type = current_type;

            Transform slot_parent = tap_panel_group[current_type].transform;
            foreach (var slot in slots)
            {
                slot.Value.transform.SetParent(slot_parent);
                slot.Value.GetComponent<BagItem>().UpdateInfo();
            }
            UpdateBagData(tap_panel_name[current_type]);
        }

        //初始化背包里的数据
        private void UpdateBagData(string panel_name = "Panel_All")
        {
            PlayerDataConfig playerData = GameEntry.PlayerData.GetPlayerData();
            if(playerData.Bag.Count == 0)
            {
                //初始化存档数据
                Dictionary<string, Dictionary<int, BagData>> temp_bag = new Dictionary<string, Dictionary<int, BagData>>();
                foreach (var item in tap_panel_name)
                {
                    Dictionary<int, BagData> temp_bag_detail = new Dictionary<int, BagData>();
                    temp_bag.Add(item, temp_bag_detail);
                }
                playerData.Bag = temp_bag;
                GameEntry.PlayerData.SetPlayerData(playerData);
            }
            else
            {
                foreach (var item in playerData.Bag)
                {
                    Log.Error(item.Key);
                    if(item.Key == panel_name)
                    {
                        Dictionary<int, BagData> temp_bag_detail = item.Value;
                        foreach (var bag_item in temp_bag_detail)
                        {
                            slots[bag_item.Key].GetComponent<BagItem>().UpdateInfo(bag_item.Value);
                        }
                    }
                }
            }
        }

        private void Test(string panel_name = "Panel_All")
        {
            PlayerDataConfig playerData = GameEntry.PlayerData.GetPlayerData();
            foreach (var item in playerData.Bag)
            {
                Log.Error(item.Key);
                if (item.Key == panel_name)
                {
                    Dictionary<int, BagData> temp_bag_detail = item.Value;
                    for (int i = 0; i < 10; i++)
                    {
                        BagData bagDataConfig = new BagData();
                        bagDataConfig.guid = System.Guid.NewGuid().ToString();
                        bagDataConfig.count = UnityEngine.Random.Range(1, 100);
                        bagDataConfig.index = i;
                        temp_bag_detail.Add(i, bagDataConfig);
                    }
                    foreach (var bag_item in temp_bag_detail)
                    {
                        slots[bag_item.Key].GetComponent<BagItem>().UpdateInfo(bag_item.Value);
                    }
                }
            }
            GameEntry.PlayerData.SetPlayerData(playerData);
        }

        private void ShowStatusTip(bool _show = true, Transform target = null)
        {
            if (_show == false)
            {
                status_tip.gameObject.SetActive(false);
                return;
            }
            status_tip.SetParent(target);
            status_tip.GetComponent<RectTransform>().anchoredPosition = new Vector2(155, 35);
            status_tip.SetParent(left);
            status_tip.SetAsLastSibling();
            status_tip.gameObject.SetActive(true);
            //TODO
            //更新文本内容
        }

        private void SellBtnEvent()
        {
            if (selectBagItem)
            {
                Log.Error(selectBagItem.transform.name);
            }
        }
    }

}