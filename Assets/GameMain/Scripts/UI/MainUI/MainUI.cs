using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GameFramework.DataTable;
using GameFramework.Event;
using LitJson;
using System.IO;
using UnityGameFramework.Runtime;
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


        Button skillBtn;
        Button equipBtn;

        Text playerExpText;
        Text playerLevelText;
        Text playerNameText;

        Slider expSlider;

        string[] BtnGroup =
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
            "MapBtn",
            "StageBtn",
            "BagBtn",
            "PlayerInfoBtn",
            "RoleBtn",
        };

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
            Name = "MainUI";


            content = transform.Find("Background");

            bottom = content.Find("Bottom");
            skillBtn = bottom.Find("SkillBtn").GetComponent<Button>();
            equipBtn = bottom.Find("EquipBtn").GetComponent<Button>();

            left = content.Find("Left");
            expSlider = left.Find("PlayerInfo/Exp").GetComponent<Slider>();
            playerExpText = left.Find("PlayerInfo/Exp/Text").GetComponent<Text>();
            playerLevelText = left.Find("PlayerInfo/Level/Text").GetComponent<Text>();
            playerNameText = left.Find("PlayerInfo/Name").GetComponent<Text>();

            right = content.Find("Right");
            foreach (var btn_name in BtnGroup)
            {
                AddBtnEvent(GameObject.Find(btn_name).GetComponent<Button>(), () => { GoToUI(btn_name); });
            }

            AddBtnEvent(skillBtn, () =>
            {
                //TODO  解析Json文件
                //ReadData();
            });
            AddBtnEvent(equipBtn, () => {
                UpdatePlayerData();
            });
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

            IDataTable<DREntity> dtEntity = GameEntry.DataTable.GetDataTable<DREntity>();
            DREntity drEntity = dtEntity.GetDataRow(100001);
            PlayerDataConfig playerDataConfig = GameEntry.PlayerData.GetPlayerData();
            Log.Error(ExpressionDeal(string.Format(drEntity.Exp,5,playerDataConfig.level)));
        }

        protected override void OnResume()
        {
            base.OnResume();
        }

        protected override void OnClose(bool isShutdown, object userData)
        { 
            base.OnClose(isShutdown, userData);
            GameEntry.Event.Unsubscribe(PlayerDefineEventArgs.EventId, UpdatePlayerInfo);
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