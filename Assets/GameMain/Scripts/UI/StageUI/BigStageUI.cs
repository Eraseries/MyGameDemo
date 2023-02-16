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
    public class BigStageUI : UGuiForm
    {
        private Transform content;
        private Transform top;

        Button backBtn;
        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
            Name = "BigStageUI";
            content = transform.Find("Background");
            top = content.Find("Content1/Top");
            backBtn = top.Find("BackBtn").GetComponent<Button>();
            AddBtnEvent(backBtn, () => { 
                GameEntry.UI.OpenUIForm(UIFormId.MainUI, this);
                Close(true);
            });// Close();
            AddBtnEvent(content.Find("Content1/PreBtn").GetComponent<Button>(), () => {
                //GameEntry.UI.OpenUIForm(UIFormId.SmallStageUI);
                (GameEntry.Procedure.CurrentProcedure as ProcedureDemo).m_GoToBattle = true;
            });
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