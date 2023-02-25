using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;
using Newtonsoft.Json.Serialization;
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
        private int index = 2;
        private Text time_text;
        Text useCardText;
        [HideInInspector]
        public Button backBtn;

        GameObject pausePanel;
        GameObject roundPanel;
        Dictionary<int, BattleCard> grid_table = new Dictionary<int, BattleCard>();

        Timer top_timer;
        int time_index = 0;
        int round_use_card_count = 1;
        public int cur_use_card_count = 0;
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
            useCardText = top.Find("UseCardText").GetComponent<Text>();
            AddBtnEvent(right.Find("PauseBtn").GetComponent<Button>(), () =>
            {
                OperateGame("pause");
            });
            AddBtnEvent(right.Find("AutoBtn").GetComponent<Button>(), () =>
            {
                if(index > 4)
                {
                    index = 2;
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

            time_text = top.Find("Time/Time").GetComponent<Text>();
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
                grid_table[i].InitGrid(this);
            }
        }

        //更新底部出战卡牌(传入角色id)
        public void UpdateBottomCard(int role_id)
        {
            //获取该角色的战斗卡组
            int grid_index = 0;
            foreach (var item_1 in GameEntry.PlayerData.GetPlayerData().RoleBag)
            {
                if (item_1.Key == role_id)
                {
                    foreach (var item_2 in item_1.Value.CardBag)
                    {
                        if (item_2.Value.load_battle)
                        {
                            grid_table[grid_index].UpdateCardInfo(item_1.Value);
                        }
                    }
                    round_use_card_count = item_1.Value.round_use_card_count;
                }
            }

        }

        //更新回合使用卡个数
        private void UpdateUseCardText(bool reset = false)
        {
            if(reset)
            {
                useCardText.text = String.Format("(卡牌使用次数:(<color=#ff0000>0</color>/{0})",round_use_card_count);
                return;
            }
            useCardText.text = String.Format("(卡牌使用次数:(<color=#ff0000>{0}</color>/{1})",cur_use_card_count,round_use_card_count);
        }

        //设置回合使用卡个数
        public void SetUseCardCount()
        {
            cur_use_card_count = cur_use_card_count + 1;
            UpdateUseCardText();
        }

        //判断能否继续使用卡牌
        public bool CheckCanUseCard()
        {
            return cur_use_card_count < round_use_card_count;
        }

        //更新顶部时间
        private void UpdateTopTime(bool pause = false)
        {
            if(pause)
            {
                if(top_timer != null)
                {
                    top_timer.Pause();
                }
                return;
            }
            if(top_timer == null)
            {
                top_timer = Timer.Register(1, () => {
                    time_text.text = (time_index++).ToString();
                }, null, true, true, this);
            }
            else
            {
                top_timer.Resume();
            }
        }

        public void RoundStart(System.Action action_1 = null,System.Action action_2 = null)
        {
            roundPanel.SetActive(true);
            if (action_1 != null)
            {
                action_1();
            }
            if (action_2 != null)
            {
                action_2();
            }
            Timer.Register(1.52f, () => {
                roundPanel.SetActive(false);
                UpdateTopTime();
            }, null, false, true,this);
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
            UpdateUseCardText(true);
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