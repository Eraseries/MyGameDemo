using GameFramework;
using GameFramework.DataTable;
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
    public class MoneyUI : UGuiForm
    {
        private Transform diamond;
        private Transform coin;
        private Transform energy;

        RectTransform rectTransform;

        Text diamond_text;
        Text coin_text;
        Text energy_text;
        Canvas canvas;
        Vector2 init_pos = new Vector2(-219, -83);
        Vector2 init_delta = new Vector2(994, 100);
        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
            Name = "MoneyUI";
            rectTransform = transform.GetComponent<RectTransform>();
            canvas = gameObject.GetOrAddComponent<Canvas>();
            rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
            rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
            rectTransform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Top, 0, 0);
            rectTransform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Right, 0, 0);
            rectTransform.sizeDelta = init_delta;
            rectTransform.pivot = new Vector2(1, 0.5f);
            rectTransform.anchoredPosition = init_pos;
        }

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

            PlayerDataConfig playerData = GameEntry.PlayerData.GetPlayerData();
            IDataTable<DRPlayer> dtPlayer = GameEntry.DataTable.GetDataTable<DRPlayer>();
            DRPlayer data = dtPlayer.GetDataRow(playerData.level);

            energy_text.text = playerData.energy.ToString() + "/" + data.MaxEnergy;
            coin_text.text = playerData.coin.ToString();
            diamond_text.text = playerData.diamond.ToString();
        }

        private void OnEnable()
        {
            GameEntry.Event.Subscribe(PlayerDefineEventArgs.EventId, UpdateUI);
        }

        private void OnDisable()
        {
            GameEntry.Event.Unsubscribe(PlayerDefineEventArgs.EventId, UpdateUI);
        }

        protected override void OnDepthChanged(int uiGroupDepth, int depthInUIGroup)
        {
            base.OnDepthChanged(uiGroupDepth, depthInUIGroup);
            canvas.sortingOrder = 32767;
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