using DG.Tweening;
using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework.Runtime;
using static GameFramework.Utility;

/// <summary>
/// 主界面模块
/// </summary>
namespace StarForce
{
    public class SettingUI : UGuiForm
    {
        private Transform content;
        Transform panel_1;
        Transform panel_2;

        Slider soundSlider;
        Slider musicSlider;
        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
            Name = "SettingUI";
            SlowShowUI = false;


            content = transform.Find("Background");
            panel_1 = content.Find("Panel1");
            panel_2 = content.Find("Panel2");
            panel_1.gameObject.SetActive(true);
            panel_2.gameObject.SetActive(false);

            AddBtnEvent(panel_1.Find("Group_Right/SaveBtn").GetComponent<Button>(), () =>
            {
                panel_2.gameObject.SetActive(true);
                panel_1.gameObject.SetActive(false);
            });

            AddBtnEvent(panel_1.Find("Group_Right/ExitBtn").GetComponent<Button>(), () =>
            {
                GameEntry.PlayerData.Operate("save");
                UnityGameFramework.Runtime.GameEntry.Shutdown(ShutdownType.Quit);
            });

            AddBtnEvent(content.Find("back/CloseBtn").GetComponent<Button>(), () => { 
                if (panel_2.gameObject.activeInHierarchy)
                {
                    panel_2.gameObject.SetActive(false);
                    panel_1.gameObject.SetActive(true);
                }
                else
                {
                    Close(true);
                }});

            soundSlider = panel_1.Find("Group_Left/Sound/Slider").GetComponent<Slider>();
            AddSliderEvent(soundSlider, (float value) => {
                GameEntry.Sound.SetVolume("Sound", value);
                bool isOn = value == 0;
                GameEntry.Sound.Mute("Sound", isOn);

                string str = "";
                if (isOn)
                {
                    str = "true";
                }
                else
                {
                    str = "false";
                }
                PlayerPrefs.SetString("MuteSound", str);
                PlayerPrefs.SetFloat("SoundValue",value);
            });

            musicSlider = panel_1.Find("Group_Left/Music/Slider").GetComponent<Slider>();
            AddSliderEvent(musicSlider, (float value) => {
                GameEntry.Sound.SetVolume("Music", value);
                bool isOn = value == 0;
                GameEntry.Sound.Mute("Music", isOn);

                string str = "";
                if (isOn)
                {
                    str = "true";
                }
                else
                {
                    str = "false";
                }
                PlayerPrefs.SetString("MuteMusic", str);
                PlayerPrefs.SetFloat("MusicValue", value);
            });
            //for (int i = 0; i < 4; i++)
            //{
            //    int index = i + 1;

            //    AddBtnEvent(panel_2.Find("Slot" + index + "/LoadBtn").GetComponent<Button>(),()=>{
            //        GameEntry.PlayerData.Operate("load",index);
            //    });
            //    AddBtnEvent(panel_2.Find("Slot" + index + "/SaveBtn").GetComponent<Button>(), () => {
            //        GameEntry.PlayerData.Operate("save", index);
            //    });
            //}
            InitSetting();
        }

        void InitSetting()
        {
            musicSlider.value = PlayerPrefs.GetFloat("MusicValue");
            soundSlider.value = PlayerPrefs.GetFloat("SoundValue");
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