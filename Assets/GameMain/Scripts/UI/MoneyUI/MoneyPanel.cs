using GameFramework;
using GameFramework.Event;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework.Runtime;

/// <summary>
/// 主界面模块
/// </summary>
namespace StarForce
{
    public class MoneyPanel : UGuiForm
    {
        private Transform diamond;
        private Transform coin;
        private Transform energy;

        Text diamond_text;
        Text coin_text;
        Text energy_text;
        private void Awake()
        {
            diamond = transform.Find("Diamond");
            diamond_text = diamond.Find("Value").GetComponent<Text>();

            coin = transform.Find("Coin");
            coin_text = coin.Find("Value").GetComponent<Text>();

            energy = transform.Find("Energy");
            energy_text = energy.Find("Value").GetComponent<Text>();

            AddBtnEvent(diamond.Find("Add").GetComponent<Button>(), () => { OpenShopUI(3); });
            AddBtnEvent(coin.Find("Add").GetComponent<Button>(), () => { OpenShopUI(2); });
            AddBtnEvent(energy.Find("Add").GetComponent<Button>(), () => { OpenShopUI(1); });
            GameEntry.Event.Fire(this, ReferencePool.Acquire<PlayerDefineEventArgs>().DefineEvent(PlayerDefineEventArgs.EventType.UpdatePlayerData));
        }

        private void Start()
        {
            //这里会不会存在没有销毁订阅的一个bug??????
            GameEntry.Event.Subscribe(PlayerDefineEventArgs.EventId, UpdateUI);
        }

        private void OpenShopUI(int panel_type)
        {
            //GameEntry.UI.OpenUIForm(UIFormId.ShopUI);
            StopCoroutine("OpenShopCallBack");
            StartCoroutine("OpenShopCallBack", panel_type);
        }

        //更新UI
        void UpdateUI(object sender, GameEventArgs e)
        {
            //Debug.LogError("UI更新");
            Debug.LogError(GameEntry.PlayerData);
            PlayerDataConfig playerDataConfig = GameEntry.PlayerData.GetPlayerData();
            energy_text.text = playerDataConfig.energy.ToString();
            coin_text.text = playerDataConfig.coin.ToString();
            diamond_text.text = playerDataConfig.diamond.ToString();
        }

        IEnumerator OpenShopCallBack(int panel_type)
        {
            GameEntry.UI.OpenUIForm(UIFormId.ShopUI, this);
            yield return null;
            ShopUI shopUI = (ShopUI)GetUIForm("ShopUI");
            shopUI.SelectPanel(panel_type);
        }
    }
}