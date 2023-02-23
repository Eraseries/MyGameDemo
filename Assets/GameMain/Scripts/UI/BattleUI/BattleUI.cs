using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GameFramework.DataTable;
using GameFramework.Event;
using UnityGameFramework.Runtime;
using ProcedureOwner = GameFramework.Fsm.IFsm<GameFramework.Procedure.IProcedureManager>;
using System.Reflection;
using UnityEditor.SceneManagement;
using System;
/// <summary>
/// 战斗模块
/// </summary>
namespace StarForce
{
    public class BattleUI : UGuiForm
    {
        private Transform content;
        private Transform top;
        private Transform right;
        private Transform bottom;
        private int index = 5;
        [HideInInspector]
        public Button backBtn;

        bool InitModel = false;
        GameObject pausePanel;
        GameObject roundPanel;
        Dictionary<int, BattleCard> grid_table = new Dictionary<int, BattleCard>();
        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
            Name = "BattleUI";

            content = transform.Find("Background");
            pausePanel = content.Find("PausePanel").gameObject;
            pausePanel.SetActive(false);
            roundPanel = content.Find("RoundPanel").gameObject;
            roundPanel.SetActive(false);
            top = content.Find("Top");
            right = content.Find("Right");
            bottom = content.Find("Bottom");
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
            InitBottomGrid();
        }

        //初始化底部格子
        private void InitBottomGrid()
        {
            for (int i = 1; i <= 4; i++)
            {
                GameObject go = bottom.Find("Slot_" + i).gameObject;
                go.SetActive(true);
                go.AddComponent<BattleCard>();
                grid_table.Add(i, go.GetComponent<BattleCard>());
            }

            foreach (var item in grid_table)
            {
                Log.Error(item.Key);
            }

        }

        //更新底部出战卡牌(传入角色id)
        private void UpdateBottomCard(int role_id)
        {
            //获取该角色的战斗卡组
            int grid_index = 0;
            foreach (var item_1 in GameEntry.PlayerData.GetPlayerData().RoleBag)
            {
                if(item_1.Key == role_id)
                {
                    foreach (var item_2 in item_1.Value.CardBag)
                    {
                        if(item_2.Value.load_battle)
                        {
                            grid_table[grid_index].UpdateCardInfo(item_1.Value);
                        }
                    }
                }
            }
            
        }

        public void RoundStart(Action func = null)
        {
            roundPanel.SetActive(true);
            Timer timer = Timer.Register(1f, () => {
                roundPanel.SetActive(false);
            }, null, false, true);
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