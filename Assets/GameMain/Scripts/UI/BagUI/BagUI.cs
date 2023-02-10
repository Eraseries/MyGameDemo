using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
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

        GameObject[] tap_menu_group = new GameObject[5];
        private GameObject select_tap_menu;
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

            //AddBtnEvent(content.GetComponent<Button>(), () =>
            //{
            //    ShowItemPanel(false);
            //    ShowStatusTip(false);
            //    ShowSortType(false);
            //});


            //top = content.Find("Top");
            //AddToggleEvent(top.Find("ItemPanelToggle").GetComponent<Toggle>(), (bool bo) =>
            //{
            //    enable_item_panel = bo;
            //});

            //right = content.Find("Right");
            //item_prefab = right.Find("Bag_Item").gameObject;
            //item_prefab.SetActive(false);


            //left = content.Find("Left");
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

            //select_tap_menu = right.Find("Tap_Menu/GameObject/Select").gameObject;
            //for (int i = 0; i < 5; i++)
            //{
            //    int current_type = i;
            //    tap_menu_group[current_type] = right.Find(string.Format("Tap_Menu/GameObject/btn_{0}", current_type)).gameObject;
            //    AddBtnEvent(tap_menu_group[current_type].GetComponent<Button>(), () =>
            //    {
            //        SelectTapMenuPanel(current_type);
            //    });// Close();
            //}


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

            //InitBagPanel();
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
        private void InitBagPanel()
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
            //先处理前一个选择的按钮
            tap_menu_group[pre_type].GetComponent<Text>().color = new Color(0.2901961f, 0.6745098f, 0.9686275f, 1);
            //在处理现在所选的按钮
            tap_menu_group[current_type].GetComponent<Text>().color = Color.white;

            pre_type = current_type;

            Dotween(DotweenType.RectMove, select_tap_menu, new Vector2(tap_menu_group[current_type].GetComponent<RectTransform>().anchoredPosition.x, -72), 0.5f);

            //TODO 这里切换格子栏
            switch (current_type)
            {
                default:
                    break;
            }
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

        private void UseBtnEvent()
        {
            if (selectBagItem)
            {
                Log.Error(selectBagItem.transform.name);
            }
        }


        //显示排序类型按钮
        private void ShowSortType(bool bo = true)
        {
            if (bo)
            {
                bottom_menu.Find("sort/type").localScale = new Vector3(-2.5f, 2.5f, 2.5f);
                sort_btn[0].SetActive(true);
                Dotween(DotweenType.AlphaMove, sort_btn[0], 1f, 0.1f, () =>
                {
                    sort_btn[1].SetActive(true);
                    Dotween(DotweenType.AlphaMove, sort_btn[1], 1f, 0.1f, () =>
                    {
                        sort_btn[2].SetActive(true);
                        Dotween(DotweenType.AlphaMove, sort_btn[2], 1f, 0.1f, () =>
                        {
                        });
                    });
                });
            }
            else
            {
                Dotween(DotweenType.AlphaMove, sort_btn[2], 0f, 0.1f, () =>
                {
                    sort_btn[2].SetActive(false);
                    Dotween(DotweenType.AlphaMove, sort_btn[1], 0f, 0.1f, () =>
                    {
                        sort_btn[1].SetActive(false);
                        Dotween(DotweenType.AlphaMove, sort_btn[0], 0f, 0.1f, () =>
                        {
                            sort_btn[0].SetActive(false);
                            bottom_menu.Find("sort/type").localScale = new Vector3(2.5f, 2.5f, 2.5f);
                            bottom_menu.Find("sort").GetComponent<Toggle>().isOn = false;
                        });
                    });
                });
            }
        }

        //排序
        private void SortItem(int type = 0)
        {
            switch (type)
            {
                case 1:
                    Debug.LogError("种类排序");
                    break;
                case 2:
                    Debug.LogError("等级排序");
                    break;
                default:
                    Debug.LogError("品质排序");
                    break;
            }
            ShowSortType(false);
        }
    }

}