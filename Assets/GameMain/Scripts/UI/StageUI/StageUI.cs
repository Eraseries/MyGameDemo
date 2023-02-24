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

        GameObject challange_panel;
        RectTransform challange_panel_rect;
        Vector2 init_anchoredposition;
        bool panel_running = false;
        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
            Name = "StageUI";
            top = transform.Find("Background/Top");
            content_1 = transform.Find("Background/Content1");
            content_2 = transform.Find("Background/Content2");
            content_1.gameObject.SetActive(true);
            content_2.gameObject.SetActive(false);
            challange_panel = content_2.Find("ChallengePanel").gameObject;
            challange_panel_rect = challange_panel.GetComponent<RectTransform>();
            init_anchoredposition = challange_panel_rect.anchoredPosition;
            challange_panel_rect.anchoredPosition = new Vector2(init_anchoredposition.x + 1500, init_anchoredposition.y);
            challange_panel.SetActive(false);
            for (int i = 1; i <= 4; i++)
            {
                AddBtnEvent(content_1.Find("ScrollRect/Content/BigStage_"+ i).GetComponent<Button>(), () =>
                {
                    content_2.gameObject.SetActive(true);
                });
            }

            for (int i = 1; i <= 8; i++)
            {
                GameObject small_stage = content_2.Find("ScrollRect/Content/SmallStage_" + i).gameObject;
                for (int j = 0; j < 3; j++)
                {
                    AddBtnEvent(small_stage.transform.Find("Icon").GetChild(j).GetComponent<Button>(), () =>
                    {
                        ShowChallangePanel();
                    });
                }
            }

            AddBtnEvent(top.Find("BackBtn").GetComponent<Button>(), () =>
            {
                if(content_2.gameObject.activeSelf)
                {
                    content_2.gameObject.SetActive(false);
                    return;
                }
                GameEntry.UI.OpenUIForm(UIFormId.MainUI, this);
                Close(true);
            });

            AddBtnEvent(content_1.Find("PreBtn").GetComponent<Button>(), () => {
                (GameEntry.Procedure.CurrentProcedure as ProcedureDemo).m_GoToBattle = true;
            });
            AddBtnEvent(challange_panel.transform.Find("BackBtn").GetComponent<Button>(), () => {
                ShowChallangePanel(false);
            });
        }

        private void ShowChallangePanel(bool bo = true)
        {
            if(panel_running)
            {
                return;
            }
            if(!bo)
            {
                panel_running = true;
                Dotween(DotweenType.RectMove, challange_panel, new Vector2(init_anchoredposition.x + 1500, init_anchoredposition.y), 0.5f, () => {
                    challange_panel.SetActive(false);
                    panel_running = false;
                });
                return;
            }

            panel_running = true;
            challange_panel_rect.anchoredPosition = new Vector2(init_anchoredposition.x + 1500, init_anchoredposition.y);
            challange_panel.SetActive(true);
            Dotween(DotweenType.RectMove, challange_panel, new Vector2(init_anchoredposition.x, init_anchoredposition.y), 0.5f, () => {
                panel_running = false;
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