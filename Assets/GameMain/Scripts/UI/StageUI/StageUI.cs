using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework.Runtime;

/// <summary>
/// 关卡模块
/// </summary>
namespace StarForce
{
    public class StageUI : UGuiForm
    {
        private Transform content_1;
        private Transform content_2;
        private Transform top;

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
            Name = "StageUI";
            top = transform.Find("Background/Top");
            content_1 = transform.Find("Background/Content1");
            content_2 = transform.Find("Background/Content2");

            for (int i = 1; i <= 4; i++)
            {
                AddBtnEvent(content_1.Find("ScrollRect/Content/Stage_"+ i).GetComponent<Button>(), () =>
                {
                    content_2.gameObject.SetActive(true);
                });
            }


            AddBtnEvent(content_1.Find("Top/BackBtn").GetComponent<Button>(), () =>
            {
                GameEntry.UI.OpenUIForm(UIFormId.MainUI, this);
                Close(true);
            });

            AddBtnEvent(content_2.Find("Top/BackBtn").GetComponent<Button>(), () =>
            {
                content_2.gameObject.SetActive(false);
            });

            AddBtnEvent(content_1.Find("PreBtn").GetComponent<Button>(), () => {
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