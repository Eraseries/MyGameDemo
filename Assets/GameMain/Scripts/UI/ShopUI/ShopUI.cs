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
    public class ShopUI : UGuiForm
    {
        private Transform content;
        private Transform top;
        private Transform right;
        private Transform left;

        GameObject[] panel_group = new GameObject[3];

        GameObject[] tap_menu_group = new GameObject[5];

        Button backBtn;
        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
            Name = "ShopUI";

            content = transform.Find("Background");

            top = content.Find("Top");
            backBtn = top.Find("BackBtn").GetComponent<Button>();
            AddBtnEvent(backBtn, () => { Close(true); });// Close();

            right = content.Find("Right");


            left = content.Find("Left");
            for (int i = 0; i < 3; i++)
            {
                panel_group[i] = left.GetChild(i).gameObject;
            }
        }


        public void SelectPanel(int type = 1)
        {
            for (int i = 0; i < 3; i++)
            {
                panel_group[i].SetActive(i == type - 1);
            }
        }

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
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
    }
}