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
    public class SettingUI : UGuiForm
    {
        private Transform content;
        Transform panel_1;
        Transform panel_2;

        Button[] save_btns;
        Button backBtn;
        Button testBtn;
        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
            Name = "SettingUI";
            content = transform.Find("Background");
            panel_1 = content.Find("Panel1");
            panel_1.gameObject.SetActive(true);
            panel_2 = content.Find("Panel2");
            panel_2.gameObject.SetActive(false);
            backBtn = content.Find("back/CloseBtn").GetComponent<Button>();
            AddBtnEvent(panel_1.Find("Group_Right/Button_List/SaveBtn").GetComponent<Button>(), () =>
            {
                panel_2.gameObject.SetActive(true);
                panel_1.gameObject.SetActive(false);
            });
            AddBtnEvent(backBtn, () => { 
                if (panel_2.gameObject.activeInHierarchy)
                {
                    panel_2.gameObject.SetActive(false);
                    panel_1.gameObject.SetActive(true);
                }
                else
                {
                    Close(true);
                }});
            save_btns = new Button[4];
            //for (int i = 0; i < 4; i++)
            //{
            //    int index = i + 1;

            //    AddBtnEvent(panel_2.Find("Slot" + index + "/LoadBtn").GetComponent<Button>(),()=>{
            //        GameEntry.Data.Operate("load",index);
            //    });
            //    AddBtnEvent(panel_2.Find("Slot" + index + "/SaveBtn").GetComponent<Button>(), () => {
            //        GameEntry.Data.Operate("save", index);
            //    });
            //}
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