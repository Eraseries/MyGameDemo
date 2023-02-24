using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GameFramework.DataTable;
using GameFramework.Event;
using UnityGameFramework.Runtime;
using ProcedureOwner = GameFramework.Fsm.IFsm<GameFramework.Procedure.IProcedureManager>;
/// <summary>
/// 主界面模块
/// </summary>
namespace StarForce
{
    public class ChatUI : UGuiForm
    {
        private Transform content;
        private Transform top;


        [HideInInspector]
        public Button backBtn;

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
            Name = "ChatUI";
            content = transform.Find("Background");
            top = content.Find("content_1/Top");
            backBtn = top.Find("BackBtn").GetComponent<Button>();
            AddBtnEvent(backBtn, () => { Close(); });// Close();
            AddBtnEvent(content.GetComponent<Button>(), () => { Close(); });// Close();
        }

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
        }

        protected override void OnClose(bool isShutdown, object userData)
        {

            base.OnClose(isShutdown, userData);
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