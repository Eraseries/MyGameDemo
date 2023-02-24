//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2021 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//  UI基类，所有的UI类都要继承这个
//------------------------------------------------------------


using GameFramework.DataTable;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using DG.Tweening;
using UnityGameFramework.Runtime;
using GameFramework;
using GameDevWare.Dynamic.Expressions.CSharp;

namespace StarForce
{
    public enum DotweenType {
        RectMove,
        ColorMove,
        AlphaMove,
        ScaleMove,
        Rotate,
        WidthMove,
        SliderMove,
        TextNumMove,
    }

    public abstract class UGuiForm : UIFormLogic
    {
        public const int DepthFactor = 100;
        private const float FadeTime = 0.3f;
        private static Font s_MainFont = null;
        private Canvas m_CachedCanvas = null;
        private CanvasGroup m_CanvasGroup = null;
        private List<Canvas> m_CachedCanvasContainer = new List<Canvas>();
        private static List<UGuiForm> m_CachedUIFormContainer = new List<UGuiForm>();
        private static Dictionary<string, object> m_CachedPrefabContainer = new Dictionary<string, object>();

        Vector3 click_point;
        GameObject click_effect;

        protected bool SlowShowUI = true;

        public int OriginalDepth
        {
            get;
            protected set;
        }

        public int Depth
        {
            get
            {
                return m_CachedCanvas.sortingOrder;
            }
        }
        /// <summary>
        /// 关闭这个界面，相当于SetActive（false）
        /// </summary>
        public virtual void Close()
        {
            Close(false);
        }

        public void Close(bool ignoreFade)
        {
            StopAllCoroutines();
            if(!transform.gameObject.activeInHierarchy)
            {
                return;
            }
            if (ignoreFade)
            {
                GameEntry.UI.CloseUIForm(this);
            }
            else
            {
                StartCoroutine(CloseCo(FadeTime));
            }
        }

        /// <summary>
        /// 打开这个界面，相当于SetActive（true）
        /// </summary>
        public void Open()
        {
            GameEntry.UI.OpenUIForm(this.UIForm.UIFormAssetName,this.UIForm.UIGroup.Name);
        }

        public void PlayUISound(int uiSoundId)
        {
            GameEntry.Sound.PlayUISound(uiSoundId);
        }

        /// <summary>
        /// 注册按钮方法，传入参数Button组件和Lambda函数
        /// </summary>
        /// <param name="btn">按钮</param>
        /// <param name="action">方法</param>
        public void AddBtnEvent(Button btn,UnityAction action)
        {
            if (btn != null && action != null)
            {
                btn.onClick.AddListener(action);
            }
        }

        public void AddSliderEvent(Slider slider,UnityAction<float> action)
        {
            if (slider != null && action != null)
            {
                slider.onValueChanged.AddListener(action);
            }
        }

        public void AddToggleEvent(Toggle toggle, UnityAction<bool> action)
        {
            if (toggle != null && action != null)
            {
                toggle.onValueChanged.AddListener(action);
            }
        }


        /// <summary>
        /// 获取类，传入参数类名（前提是这个类已经加载了）
        /// </summary>
        /// <param name="uiFormid"></param>
        public UGuiForm GetUIForm(string uiName)
        {
            int temp_index = -1;
            for (int i = 0; i < m_CachedUIFormContainer.Count; i++)
            {
                if(m_CachedUIFormContainer[i] == null)
                {
                    temp_index = i;
                    continue;
                }
                if (m_CachedUIFormContainer[i].Name == uiName)
                {
                    return m_CachedUIFormContainer[i];
                }
            }
            if(temp_index != -1)
            {
                m_CachedUIFormContainer.RemoveAt(temp_index);
            }
            return null;
        }


        /// <summary>
        /// 赋予图片，传入参数Image组件、图片路径(UISprites下的路径)、图片名
        /// </summary>
        /// <param name="image">Image组件</param>
        /// <param name="path">Resource/UISprites下的路径</param>
        /// <param name="image_name">赋值的图片名字</param>
        /// <param name="set_native_size">是否图片原尺寸</param>
        public bool AssignUISprite(Image image, string path, string image_name, bool set_native_size = false)
        {
            Sprite[] sprites = Resources.LoadAll<Sprite>(string.Format("UISprites/{0}", path));
            for (int i = 0; i < sprites.Length; i++)
            {
                if (sprites[i].name == image_name)
                {
                    image.sprite = sprites[i];
                    if (set_native_size)
                    {
                        image.SetNativeSize();
                    }
                    return true;
                }

            }
            Log.Error("请检查路径/名字");
            return false;
        }

        public object LoadAsset(string prefab_name)
        {
            if (m_CachedPrefabContainer.ContainsKey(prefab_name))
            {
                return m_CachedPrefabContainer[prefab_name];
            }

            string prefab_path = string.Format("Assets/GameMain/Entities/{0}{1}", prefab_name, ".prefab");
            object go = null;
            try
            {
                GameEntry.Resource.LoadAsset(prefab_path, new GameFramework.Resource.LoadAssetCallbacks(
                (assetName, asset, duration, userData) =>
                {
                    go = asset;
                    m_CachedPrefabContainer.Add(prefab_name, asset);
                }));
            }
            catch (Exception e)
            {
                Log.Error(e.Message);
            }
            return null;
        }

        public static void SetMainFont(Font mainFont)
        {
            if (mainFont == null)
            {
                Log.Error("Main font is invalid.");
                return;
            }

            s_MainFont = mainFont;
        }

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);

            m_CachedCanvas = gameObject.GetOrAddComponent<Canvas>();
            m_CachedCanvas.overrideSorting = true;
            OriginalDepth = m_CachedCanvas.sortingOrder;

            m_CanvasGroup = gameObject.GetOrAddComponent<CanvasGroup>();

            RectTransform transform = GetComponent<RectTransform>();
            transform.anchorMin = Vector2.zero;
            transform.anchorMax = Vector2.one;
            transform.anchoredPosition = Vector2.zero;
            transform.sizeDelta = Vector2.zero;
            transform.localPosition = Vector3.zero;
            gameObject.GetOrAddComponent<GraphicRaycaster>();

            //Text[] texts = GetComponentsInChildren<Text>(true);
            //for (int i = 0; i < texts.Length; i++)
            //{
            //    texts[i].font = s_MainFont;
            //    if (!string.IsNullOrEmpty(texts[i].text))
            //    {
            //        texts[i].text = GameEntry.Localization.GetString(texts[i].text);
            //    }
            //}
            string class_name = this.Name;
            if (class_name.IndexOf("UI")>0)
            {
                m_CachedUIFormContainer.Add(this);
            }


            //Log.Error("11111111");
            //GameEntry.Resource.LoadAsset("Assets/GameMain/Entities/ClickEffect.prefab", new GameFramework.Resource.LoadAssetCallbacks(
            //    (assetName, asset, duration, a) =>
            //    {
            //        Log.Error(assetName);
            //        click_effect = (GameObject)asset;
            //    }));
        }

        protected override void OnRecycle()
        {
            base.OnRecycle();
        }

        //这个Onpen是有个动画过程的
        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            if(SlowShowUI)
            {
                m_CanvasGroup.alpha = 0f;
                StopAllCoroutines();
                StartCoroutine(m_CanvasGroup.FadeToAlpha(1f, FadeTime));
            }
            else
            {
                StopAllCoroutines();
                m_CanvasGroup.alpha = 1;
            }
        }

        protected override void OnClose(bool isShutdown, object userData)
        {
            base.OnClose(isShutdown, userData);
        }

        protected override void OnPause()
        {
            base.OnPause();
        }

        protected override void OnResume()
        {
            base.OnResume();
            if (!transform.gameObject.activeInHierarchy)
            {
                return;
            }
            if (SlowShowUI)
            {
                m_CanvasGroup.alpha = 0f;
                StopAllCoroutines();
                StartCoroutine(m_CanvasGroup.FadeToAlpha(1f, FadeTime));
            }
            else
            {
                m_CanvasGroup.alpha = 1;
            }
        }

        protected override void OnCover()
        {
            base.OnCover();
        }

        protected override void OnReveal()
        {
            base.OnReveal();
        }

        protected override void OnRefocus(object userData)
        {
            base.OnRefocus(userData);
        }

        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);
            ///if (Input.GetMouseButtonDown(0))
            ///{
            ///    click_point = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 2f);//获得鼠标点击点
            ///    click_point = Camera.main.ScreenToWorldPoint(click_point);//从屏幕空间转换到世界空间
            ///    GameObject go = Instantiate(click_effect,transform);//生成特效
            ///    go.transform.position = click_point;
            ///    //Destroy(go, 10f);
            ///}
        }

        protected override void OnDepthChanged(int uiGroupDepth, int depthInUIGroup)
        {
            int oldDepth = Depth;
            base.OnDepthChanged(uiGroupDepth, depthInUIGroup);
            int deltaDepth =(int) Mathf.Clamp(UGuiGroupHelper.DepthFactor * uiGroupDepth + DepthFactor * depthInUIGroup - oldDepth + OriginalDepth,-int.MaxValue,int.MaxValue);
            GetComponentsInChildren(true, m_CachedCanvasContainer);
            for (int i = 0; i < m_CachedCanvasContainer.Count; i++)
            {
                m_CachedCanvasContainer[i].sortingOrder += deltaDepth;
            }

            m_CachedCanvasContainer.Clear();
        }

        private IEnumerator CloseCo(float duration)
        {
            yield return m_CanvasGroup.FadeToAlpha(0f, duration);
            GameEntry.UI.CloseUIForm(this);
        }

        public Tween Dotween(DotweenType dotweenType,GameObject gameObject,object target_property,float time,Action func = null,Ease ease = Ease.Linear)
        {
            switch (dotweenType)
            {
                case DotweenType.RectMove:
                    RectTransform rect = gameObject.GetComponent<RectTransform>();
                    Tween tween_1 = rect.DOAnchorPos((Vector2)target_property, time).SetEase(ease);
                    if (func != null)
                    {
                        tween_1.OnComplete(() => { func(); });
                    }
                    return tween_1;
                case DotweenType.Rotate:
                    return null;
                case DotweenType.ColorMove:
                    return null;
                case DotweenType.AlphaMove:
                    CanvasGroup canvasGroup = gameObject.GetComponent<CanvasGroup>();
                    Tween tween_2 = canvasGroup.DOFade((float)target_property, time);
                    if (func != null)
                    {
                        tween_2.OnComplete(() => { func(); });
                    }
                    return tween_2;
                case DotweenType.ScaleMove:
                    Transform transform = gameObject.GetComponent<Transform>();
                    Tween tween_3 = transform.DOScale((Vector3)target_property, time).SetEase(ease);
                    if (func != null)
                    {
                        tween_3.OnComplete(() => { func(); });
                    }
                    return tween_3;
                case DotweenType.WidthMove:
                    RectTransform rect_1 = gameObject.GetComponent<RectTransform>();
                    Tween tween_4 = rect_1.DOScale((Vector3)target_property, time).SetEase(ease);
                    if (func != null)
                    {
                        tween_4.OnComplete(() => { func(); });
                    }
                    return tween_4;
                case DotweenType.SliderMove:
                    Slider slider = gameObject.GetComponent<Slider>();
                    Tween tween_5 = slider.DOValue((float)target_property,time).SetEase(ease);
                    if (func != null)
                    {
                        tween_5.OnComplete(() => { func(); });
                    }
                    return tween_5;
                case DotweenType.TextNumMove:
                    Text text = gameObject.GetComponent<Text>();
                    Tween tween_6 = text.DOText(String.Format("{0}%",(float)target_property * 100), time).SetEase(ease);
                    if (func != null)
                    {
                        tween_6.OnComplete(() => { func(); });
                    }
                    return tween_6;
                default:
                    return null;
            }
        }

        /// <summary>
        /// 字符串表达式操作
        /// </summary>
        /// <param name="expression_str"> 传入要操作的字符串表达式，例如"2 * (2 + 3)"</param>
        /// <returns></returns>
        public int ExpressionDeal(string expression_str)
        {
            int result = CSharpExpression.Evaluate<int>(expression_str);
            return result;
        }

        public void ShowMsgBox()
        {
            GameEntry.UI.OpenUIForm(UIFormId.MessageBoxUI,this);
        }

        //更新玩家数据接口
        public void UpdatePlayerData()
        {
            GameEntry.Event.Fire(this, ReferencePool.Acquire<PlayerDefineEventArgs>().DefineEvent(PlayerDefineEventArgs.EventType.UpdatePlayerData));
        }

        public void GoToUI(string str)
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
                case "StageBtn":
                    uIFormId = UIFormId.StageUI;
                    break;
                case "BagBtn":
                    uIFormId = UIFormId.BagUI;
                    break;
                case "PlayerInfoBtn":
                    uIFormId = UIFormId.PlayerInfoUI;
                    break;
                case "RoleBtn":
                    uIFormId = UIFormId.RoleUI;
                    break;
                default:
                    uIFormId = UIFormId.Undefined;
                    break;
            }
            if(uIFormId == UIFormId.Undefined)
            {
                return;
            }
            GameEntry.UI.OpenUIForm(uIFormId);
        }

    }
}
