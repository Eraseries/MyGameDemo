using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// 主界面模块
/// </summary>
namespace StarForce
{
    public class LoadingUI : UGuiForm
    {
        private Transform content;
        private Slider slider;
        private Text slider_text;

        [HideInInspector]
        public Button tapBtn;
        Tween tween_1 = null;
        Tween tween_2 = null;
        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
            Name = "LoadingUI";

            content = transform.Find("Background");
            tapBtn = content.Find("Background").GetComponent<Button>();
            slider = content.Find("Slider").GetComponent<Slider>();
            slider.value = 0;
            slider_text = content.Find("Slider/Value").GetComponent<Text>();
            slider_text.text = "0%";
        }

        public void SetSlider(float value,bool directory = true)
        {
            if (directory)
            {
                if(tween_1 != null)
                {
                    tween_1.Kill(true);
                }
                if (tween_2 != null)
                {
                    tween_2.Kill(true);
                }
                slider.value = 1;
                slider_text.text = "100%";
            }
            else
            {
                tween_1 = Dotween(DotweenType.SliderMove, slider.gameObject, value, 0.5f);
                tween_2 = Dotween(DotweenType.TextNumMove, slider_text.gameObject, value, 0.5f);
            }
        }

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            SetSlider(0.9f,false);
        }

        protected override void OnClose(bool isShutdown, object userData)
        {
            slider.value = 0;
            slider_text.text = "0%";
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