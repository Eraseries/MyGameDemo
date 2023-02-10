using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
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

        private void Awake()
        {
            diamond = transform.Find("Diamond");
            coin = transform.Find("Coin");
            energy = transform.Find("Energy");
            AddBtnEvent(diamond.Find("Add").GetComponent<Button>(), () => { OpenShopUI(3); });
            AddBtnEvent(coin.Find("Add").GetComponent<Button>(), () => { OpenShopUI(2); });
            AddBtnEvent(energy.Find("Add").GetComponent<Button>(), () => { OpenShopUI(1); });
        }

        private void Start()
        {

        }

        private void OpenShopUI(int panel_type)
        {
            //GameEntry.UI.OpenUIForm(UIFormId.ShopUI);
            StopCoroutine("OpenShopCallBack");
            StartCoroutine("OpenShopCallBack", panel_type);
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