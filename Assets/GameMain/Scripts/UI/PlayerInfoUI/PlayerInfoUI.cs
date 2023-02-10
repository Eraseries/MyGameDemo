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
    public class PlayerInfoUI : UGuiForm
    {
        private Transform content;
        private Transform top;
        InputField inputField;
        Button backBtn;
        Button editBtn;
        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
            Name = "PlayerInfoUI";
            content = transform.Find("Background/Content");
            top = content.Find("Top");
            editBtn = top.Find("PlayerName/EditBtn").GetComponent<Button>();
            inputField = top.Find("PlayerName/Text").GetComponent<InputField>();
            inputField.interactable = false;
            backBtn = content.Find("BackBtn").GetComponent<Button>();


            AddBtnEvent(backBtn, () => { Close(true); });// Close();
            AddBtnEvent(editBtn, () => {

                inputField.interactable = !inputField.interactable;
            });// Close();
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