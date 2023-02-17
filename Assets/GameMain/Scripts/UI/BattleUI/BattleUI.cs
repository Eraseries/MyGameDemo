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
        [HideInInspector]
        public Button backBtn;

        bool InitModel = false;
        BattlePanel battlePanel;
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
                ResumeGame(false);
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
                ResumeGame(true);
            });

            AddBtnEvent(pausePanel.transform.Find("MenuFrame/Group_Menu/ExitBtn").GetComponent<Button>(), () =>
            {
                Close(true);
            });
        }


        private void ResumeGame(bool bo)
        {
            if(bo)
            {
                GameEntry.Base.ResumeGame();
                GameEntry.Sound.Mute("Music", false);
                pausePanel.SetActive(false);
            }
            else
            {
                GameEntry.Base.PauseGame();
                GameEntry.Sound.Mute("Music", true);
                pausePanel.SetActive(true);
            }
        }

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            ResumeGame(true);
            InitModel = false;
        }

        void SetPlayer()
        {
            for (int i = 1; i <= 11; i++)
            {
                int index = i;
                if (GameEntry.Entity.HasEntity(index))
                {
                    IDataTable<DRBattleScene1> dtPlayer = GameEntry.DataTable.GetDataTable<DRBattleScene1>();
                    DRBattleScene1 data = dtPlayer.GetDataRow(index);
                    (GameEntry.Entity.GetEntity(index).Logic as Model).SetPos(new Vector3(data.X,data.Y,data.Z));
                    (GameEntry.Entity.GetEntity(index).Logic as Model).SetDirection(data.Type);
                    GameEntry.Entity.GetEntity(index).gameObject.SetActive(true);
                    if (index == 9)
                    {
                        InitModel = true;
                        break;
                    }
                }
                else
                {
                    GameEntry.Entity.ShowModel(new ModelData(index, 100000 + index));
                    IDataTable<DRBattleScene1> dtPlayer = GameEntry.DataTable.GetDataTable<DRBattleScene1>();
                    DRBattleScene1 data = dtPlayer.GetDataRow(index);
                    (GameEntry.Entity.GetEntity(index).Logic as Model).SetPos(new Vector3(data.X, data.Y, data.Z));
                    (GameEntry.Entity.GetEntity(index).Logic as Model).SetDirection(data.Type);
                    GameEntry.Entity.GetEntity(index).gameObject.SetActive(true);
                    if (index == 9)
                    {
                        InitModel = true;
                        break;
                    }
                }

            }
        }

        protected override void OnClose(bool isShutdown, object userData)
        {
            (GameEntry.Procedure.CurrentProcedure as ProcedureBattle1).m_ExitBattle = true;
            base.OnClose(isShutdown, userData);
        }

        protected override void OnDepthChanged(int uiGroupDepth, int depthInUIGroup)
        {
            base.OnDepthChanged(uiGroupDepth, depthInUIGroup);
        }

        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);
            if(!InitModel)
            {
                //SetPlayer();
            }
        }

    }

}