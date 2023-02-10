using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework.Runtime;
/// <summary>
/// 主界面模块
/// </summary>
namespace StarForce
{
    public class StartUI : UGuiForm
    {
        private Transform content;
        private Slider slider;
        private Text slider_text;

        [HideInInspector]
        public Button tapBtn;
        Transform Btn1;
        Transform Btn2;


        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
            Name = "StartUI";
            content = transform.Find("Background");
            tapBtn = content.Find("Background").GetComponent<Button>();
            Btn1 = content.Find("Button_1");
            Btn2 = content.Find("Button_2");

            slider = content.Find("Slider").GetComponent<Slider>();
            slider.gameObject.SetActive(false);
            slider.value = 0;
            slider_text = content.Find("Slider/Value").GetComponent<Text>();
            slider_text.text = "0%";
        }

        public void SetSlider(float value)
        {
            Btn1.gameObject.SetActive(false);
            Btn2.gameObject.SetActive(false);
            slider.gameObject.SetActive(true);
            slider.value = 0;
            slider_text.text = "0%";
            Dotween(DotweenType.SliderMove, slider.gameObject, value,0.5f);
            Dotween(DotweenType.TextNumMove, slider_text.gameObject, value, 0.5f);
        }

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
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