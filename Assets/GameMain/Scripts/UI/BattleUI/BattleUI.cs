using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GameFramework.DataTable;
using GameFramework.Event;
using UnityGameFramework.Runtime;
using ProcedureOwner = GameFramework.Fsm.IFsm<GameFramework.Procedure.IProcedureManager>;
using System.Reflection;
/// <summary>
/// 主界面模块
/// </summary>
namespace StarForce
{
    public class BattleUI : UGuiForm
    {
        private Transform content;
        private Transform top;
        private Transform right;
        private int index = 5;
        [HideInInspector]
        public Button backBtn;

        bool InitModel = false;
        GameObject pausePanel;
        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
            Name = "BattleUI";
            content = transform.Find("Background");
            pausePanel = content.Find("PausePanel").gameObject;
            pausePanel.SetActive(false);
            top = content.Find("Top");
            right = content.Find("Right");
            AddBtnEvent(right.Find("PauseBtn").GetComponent<Button>(), () =>
            {
                OperateGame("pause");
            });
            AddBtnEvent(right.Find("AutoBtn").GetComponent<Button>(), () =>
            {
                if(index > 11)
                {
                    index = 5;
                }
                BattlePanel.Instance.TestOperate(index);
                index++;
            });
            AddBtnEvent(right.Find("SpeedBtn").GetComponent<Button>(), () =>
            {
                if(GameEntry.Base.GameSpeed == 1)
                {
                    GameEntry.Base.GameSpeed = 2;
                }
                else if(GameEntry.Base.GameSpeed == 2)
                {
                    GameEntry.Base.GameSpeed = 4;
                }
                else
                {
                    GameEntry.Base.GameSpeed = 1;
                }
                
            });
            AddBtnEvent(pausePanel.transform.Find("MenuFrame/Group_Menu/ContinueBtn").GetComponent<Button>(), () =>
            {
                OperateGame("continue");
            });

            AddBtnEvent(pausePanel.transform.Find("MenuFrame/Group_Menu/RestartBtn").GetComponent<Button>(), () =>
            {
                OperateGame("restart");
            });

            AddBtnEvent(pausePanel.transform.Find("MenuFrame/Group_Menu/ExitBtn").GetComponent<Button>(), () =>
            {
                OperateGame("exit");
            });
        }


        private void OperateGame(string operate)
        {
            if(operate == "continue")
            {
                GameEntry.Base.ResumeGame();
                GameEntry.Sound.Mute("Music", false);
                pausePanel.SetActive(false);
            }
            else if(operate == "pause")
            {
                GameEntry.Base.PauseGame();
                GameEntry.Sound.Mute("Music", true);
                pausePanel.SetActive(true);
            }
            else if(operate == "restart")
            {
                Close(true);
                BattlePanel.Instance.Reset();
                GameEntry.Base.ResetNormalGameSpeed();
                GameEntry.Base.ResumeGame();
                GameEntry.Sound.StopMusic();
                pausePanel.SetActive(false);
            }
            else if(operate == "exit")
            {
                (GameEntry.Procedure.CurrentProcedure as ProcedureBattle1).m_ExitBattle = true;
                Close(true);
            }
        }

        protected override void OnOpen(object userData)
        {
            BattlePanel.Instance.Show();
            base.OnOpen(userData);
        }

        protected override void OnClose(bool isShutdown, object userData)
        {
            BattlePanel.Instance.Close();
            pausePanel.SetActive(false);
            base.OnClose(isShutdown, userData);
        }

        protected override void OnDepthChanged(int uiGroupDepth, int depthInUIGroup)
        {
            base.OnDepthChanged(uiGroupDepth, depthInUIGroup);
        }

        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            BattlePanel.Instance.Update();
            base.OnUpdate(elapseSeconds, realElapseSeconds);
        }

    }

}