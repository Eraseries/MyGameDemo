using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GameFramework.DataTable;
using GameFramework.Event;
using UnityGameFramework.Runtime;
using ProcedureOwner = GameFramework.Fsm.IFsm<GameFramework.Procedure.IProcedureManager>;
using LitJson;
using System.IO;
using GameFramework;
using UnityEngine.PlayerLoop;
/// <summary>
/// 主界面模块
/// </summary>
namespace StarForce
{
    public class MainUI : UGuiForm
    {
        private Transform content;
        private Transform bottom;
        private Transform right;
        private Transform left;


        Button houseBtn;
        Button bagBtn;
        Button skillBtn;
        Button equipBtn;
        Button stageBtn;
        Button playerInfoBtn;
        Button roleBtn;

        Text playerExpText;
        Text playerLevelText;
        Text playerNameText;

        Slider expSlider;

        string[] rightBtnGroup =
        {
            "RankBtn",
            "FriendBtn",
            "EmailBtn",
            "FriendBtn",
            "TaskBtn",
            "ChatBtn",
            "SettingBtn",
            "NoticeBtn",
            "ActivityBtn",
        };

        private void GoToUI(string str)
        {
            UIFormId uIFormId;
            switch (str)
            {
                case "RankBtn":
                    uIFormId = UIFormId.PlayerInfoUI;
                    break;
                case "EmailBtn":
                    uIFormId = UIFormId.PlayerInfoUI;
                    break;
                case "FriendBtn":
                    uIFormId = UIFormId.PlayerInfoUI;
                    break;
                case "TaskBtn":
                    uIFormId = UIFormId.TaskUI;
                    break;
                case "ChatBtn":
                    uIFormId = UIFormId.ChatUI;
                    break;
                case "SettingBtn":
                    uIFormId = UIFormId.SettingUI;
                    break;
                case "NoticeBtn":
                    uIFormId = UIFormId.LuckyDrawUI;
                    break;
                case "ActivityBtn":
                    uIFormId = UIFormId.ActivityUI;
                    break;
                default:
                    uIFormId = UIFormId.Undefined;
                    break;
            }
            GameEntry.UI.OpenUIForm(uIFormId);
        }



        [HideInInspector]
        public Button battleBtn;


        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
            Name = "MainUI";


            content = transform.Find("Background");

            bottom = content.Find("Bottom");
            roleBtn = bottom.Find("RoleBtn").GetComponent<Button>();
            bagBtn = bottom.Find("BagBtn").GetComponent<Button>();
            skillBtn = bottom.Find("SkillBtn").GetComponent<Button>();
            equipBtn = bottom.Find("EquipBtn").GetComponent<Button>();
            stageBtn = bottom.Find("StageBtn").GetComponent<Button>();
            battleBtn = bottom.Find("BattleBtn").GetComponent<Button>();


            left = content.Find("Left");
            expSlider = left.Find("PlayerInfo/Exp").GetComponent<Slider>();
            playerExpText = left.Find("PlayerInfo/Exp/Text").GetComponent<Text>();
            playerLevelText = left.Find("PlayerInfo/Level/Text").GetComponent<Text>();
            playerNameText = left.Find("PlayerInfo/Name").GetComponent<Text>();
            playerInfoBtn = left.Find("PlayerInfo/Level").GetComponent<Button>();

            right = content.Find("Right");
            foreach (var btn_name in rightBtnGroup)
            {
                AddBtnEvent(right.Find(btn_name).GetComponent<Button>(), () => { GoToUI(btn_name); });
            }



            AddBtnEvent(roleBtn, () =>
            {
                //AssignUISprite("Image.spriteatlas", "", bottom.Find("StageBtn/Icon").GetComponent<Image>()); 
                GameEntry.UI.OpenUIForm(UIFormId.RoleUI);
            });
            AddBtnEvent(bagBtn, () => { GameEntry.UI.OpenUIForm(UIFormId.BagUI); });
            AddBtnEvent(skillBtn, () =>
            {
                //if (GetUIForm("BagUI") != null)
                //{
                //    GetUIForm("BagUI").Open();
                //}


                //TODO  解析Json文件

                //ReadData();
            });
            AddBtnEvent(equipBtn, () => {
                GameEntry.Event.Fire(this, ReferencePool.Acquire<PlayerDefineEventArgs>().DefineEvent(PlayerDefineEventArgs.EventType.UpdatePlayerData));
            });
            AddBtnEvent(stageBtn, () =>
            {
                GameEntry.UI.OpenUIForm(UIFormId.BigStageUI);
            });
            AddBtnEvent(battleBtn, () =>
            {
                GameEntry.UI.OpenUIForm(UIFormId.BattleUI);
            });
            AddBtnEvent(playerInfoBtn, () => { GameEntry.UI.OpenUIForm(UIFormId.PlayerInfoUI); });
        }


        void UpdatePlayerInfo(object sender, GameEventArgs e)
        {
            PlayerDataConfig playerData = GameEntry.PlayerData.GetPlayerData();
            IDataTable<DRPlayer> dtPlayer = GameEntry.DataTable.GetDataTable<DRPlayer>();
            DRPlayer data = dtPlayer.GetDataRow(playerData.level);



            playerExpText.text = playerData.exp.ToString() + "/" + data.Exp;
            playerLevelText.text = playerData.level.ToString();
            playerNameText.text = playerData.playerName;

            expSlider.value = (float)playerData.exp / (float)data.Exp;
        }


        public void ReadData()
        {
            string FileName = "Assets/GameMain/ExcelTable/JsonTable/pet_config_new.json";
            StreamReader json = File.OpenText(FileName);
            string input = json.ReadToEnd();
            Dictionary<string,Dictionary<string, object>> jsonObject = JsonMapper.ToObject<Dictionary<string,Dictionary<string, object>>>(input);
            Debug.LogError(jsonObject.Count);
            foreach (var item in jsonObject)
            {
                if (item.Key == "0")
                {
                    foreach (var list in item.Value)
                    {
                        string temp_key = list.Key;
                        object temp_value_type = list.Value;
                        string[] str = temp_key.Split('#');
                        Debug.LogError(str[0]);
                    }
                }
                else
                {

                    //foreach (var list in item.Value)
                    //{
                    //    Debug.LogError(list);
                    //}
                }
            }
        }

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            GameEntry.Event.Subscribe(PlayerDefineEventArgs.EventId, UpdatePlayerInfo);
        }

        protected override void OnResume()
        {
            base.OnResume();
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