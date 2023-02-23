/*
 * Unity Timer
 *
 * Version: 1.0
 * By: Alexander Biggs + Adam Robinson-Yu
 */

using UnityEngine;
using System;
using System.Linq;
using System.Collections.Generic;
using JetBrains.Annotations;
using Object = UnityEngine.Object;

/// <summary>
/// Allows you to run events on a delay without the use of <see cref="Coroutine"/>s
/// or <see cref="MonoBehaviour"/>s.
///
/// To create and start a Timer, use the <see cref="Register"/> method.
/// </summary>
namespace StarForce
{
    public class Timer
    {
        #region Public Properties/Fields

        /// <summary>
        /// ��ʱ���ӿ�ʼ��������Ҫ�೤ʱ�䡣
        /// </summary>
        public float duration { get; private set; }

        /// <summary>
        /// ��ʱ���Ƿ�����ɺ��ٴ����С�
        /// </summary>
        public bool isLooped { get; set; }

        /// <summary>
        /// ��ʱ���Ƿ���������С������ʱ����ȡ������Ϊfalse��
        /// </summary>
        public bool isCompleted { get; private set; }

        /// <summary>
        /// ��ʱ���Ƿ�ʹ��ʵʱ����Ϸʱ�䡣
        /// ʵʱ�Բ�����Ϸʱ��̶ȱ仯��Ӱ�죨����ͣ�����ƶ���������Ϸʱ����Ӱ�졣
        /// </summary>
        public bool usesRealTime { get; private set; }

        /// <summary>
        /// ��ʱ����ǰ�Ƿ�����ͣ��
        /// </summary>
        public bool isPaused
        {
            get { return this._timeElapsedBeforePause.HasValue; }
        }

        /// <summary>
        /// ��ʱ���Ƿ���ȡ����
        /// </summary>
        public bool isCancelled
        {
            get { return this._timeElapsedBeforeCancel.HasValue; }
        }

        /// <summary>
        /// ��ȡ��ʱ���Ƿ����κ�ԭ����������С�
        /// </summary>
        public bool isDone
        {
            get { return this.isCompleted || this.isCancelled || this.isOwnerDestroyed; }
        }

        #endregion

        #region Public Static Methods

        /// <summary>
        /// ע��һ���µļ�ʱ�����ü�ʱ��Ӧ�ھ���һ��ʱ��󴥷��¼���
        ///
        /// ��������ʱ��ע��ļ�ʱ���������١�
        /// </summary>
        /// <param name="duration">��ʱ������ǰ�ȴ���ʱ�䣬����Ϊ��λ��</param>
        /// <param name="onComplete">��ʱ�����ʱҪ�����Ĳ�����</param>
        /// <param name="onUpdate">ÿ�θ��¼�ʱ��ʱ��Ӧ�ô����Ķ�������ȡ��ʱ����ǰѭ����ʼ�󾭹���ʱ�䣨����Ϊ��λ����</param>
        /// <param name="isLooped">��ʱ���Ƿ�Ӧ��ִ�к��ظ���</param>
        /// <param name="useRealTime">��ʱ���Ƿ�ʹ��ʵʱ����������ͣ����/�춯����Ӱ�죩����Ϸʱ�䣨������ͣ����/���˶���Ӱ�죩��</param>
        /// <param name="autoDestroyOwner">Ҫ���˼�ʱ�����ӵ��Ķ����ڸö������ٺ󣬼�ʱ�������ڶ���ִ�С���������ͨ����ֹ��ʱ���ڸ������ƻ������кͷ����丸������������ⷳ�˵�</param>
        /// <returns>���ؼ�ʱ���������������ͳ�����ݲ�ֹͣ/�ָ����ȡ�</returns>
        public static Timer Register(float duration, Action onComplete, Action<float> onUpdate = null,
        bool isLooped = false, bool useRealTime = false, MonoBehaviour autoDestroyOwner = null)
        {
            // ����һ���������������������м�ʱ��������������ڣ���
            if (Timer._manager == null)
            {
                TimerManager managerInScene = Object.FindObjectOfType<TimerManager>();
                if (managerInScene != null)
                {
                    Timer._manager = managerInScene;
                }
                else
                {
                    GameObject managerObject = new GameObject { name = "TimerManager" };
                    Timer._manager = managerObject.AddComponent<TimerManager>();
                }
            }

            Timer timer = new Timer(duration, onComplete, onUpdate, isLooped, useRealTime, autoDestroyOwner);
            Timer._manager.RegisterTimer(timer);
            return timer;
        }

        // ȡ����ʱ����
        public static void Cancel(Timer timer)
        {
            if (timer != null)
            {
                timer.Cancel();
            }
        }

        // ��ͣ��ʱ����
        public static void Pause(Timer timer)
        {
            if (timer != null)
            {
                timer.Pause();
            }
        }

        // �ָ���ʱ����
        public static void Resume(Timer timer)
        {
            if (timer != null)
            {
                timer.Resume();
            }
        }

        //ȡ�����еļ�ʱ��
        public static void CancelAllRegisteredTimers()
        {
            if (Timer._manager != null)
            {
                Timer._manager.CancelAllTimers();
            }
        }

        //��ͣ���еļ�ʱ��
        public static void PauseAllRegisteredTimers()
        {
            if (Timer._manager != null)
            {
                Timer._manager.PauseAllTimers();
            }
        }

        //�ָ����еļ�ʱ��
        public static void ResumeAllRegisteredTimers()
        {
            if (Timer._manager != null)
            {
                Timer._manager.ResumeAllTimers();
            }
        }

        #endregion

        #region Public Methods

        //ֹͣ���ڽ��л���ͣ�ļ�ʱ����������ü�ʱ�������ʱ�ص���
        public void Cancel()
        {
            if (this.isDone)
            {
                return;
            }

            this._timeElapsedBeforeCancel = this.GetTimeElapsed();
            this._timeElapsedBeforePause = null;
        }

        //��ͣ�������еļ�ʱ�������Դ���ͣ��ͬһ��ָ���ͣ�ļ�ʱ����
        public void Pause()
        {
            if (this.isPaused || this.isDone)
            {
                return;
            }

            this._timeElapsedBeforePause = this.GetTimeElapsed();
        }

        //�ָ���ʱ���������ʱ��δ��ͣ����ִ���κβ�����
        public void Resume()
        {
            if (!this.isPaused || this.isDone)
            {
                return;
            }

            this._timeElapsedBeforePause = null;
        }

        /// <summary>
        /// ��ȡ�˼�ʱ����ǰ���ڿ�ʼ���ѹ���������
        /// </summary>
        /// <returns>�Լ�ʱ����ǰѭ����ʼ���������������������������ʱ��ѭ������Ϊ��ǰѭ���������ʱ��û��ѭ��������������
        ///
        /// �����ʱ����������У�����ڳ���ʱ�䡣
        ///
        /// �����ʱ����ȡ��/��ͣ������ڼ�ʱ��������ȡ��/��֮ͣ�侭��������</returns>
        public float GetTimeElapsed()
        {
            if (this.isCompleted || this.GetWorldTime() >= this.GetFireTime())
            {
                return this.duration;
            }

            return this._timeElapsedBeforeCancel ??
                   this._timeElapsedBeforePause ??
                   this.GetWorldTime() - this._startTime;
        }

        /// <summary>
        /// ��ȡ��ʱ�����ǰ��ʣ�����롣
        /// </summary>
        /// <returns>��ʱ�����֮ǰʣ�����������ʱ��ֻ����û����ͣ��ȡ������ɵ�����²ž���ʱ�䡣�����ʱ����ɣ��⽫�����㡣</returns>
        public float GetTimeRemaining()
        {
            return this.duration - this.GetTimeElapsed();
        }

        /// <summary>
        /// ��ȡ��ʱ���ӿ�ʼ�������Ľ��ȱ�����
        /// </summary>
        /// <returns>һ����0��1��ֵ��ָʾ��ʱ���ĳ���ʱ���ѹ���</returns>
        public float GetRatioComplete()
        {
            return this.GetTimeElapsed() / this.duration;
        }

        /// <summary>
        /// ��ȡ��ʱ��ʣ��Ľ�����Ϊ���ʡ�
        /// </summary>
        /// <returns>һ����0��1��ֵ��ָʾ��ʱ���ĳ���ʱ�仹�ж೤��</returns>
        public float GetRatioRemaining()
        {
            return this.GetTimeRemaining() / this.duration;
        }

        #endregion

        #region Private Static Properties/Fields

        // responsible for updating all registered timers
        private static TimerManager _manager;

        #endregion

        #region Private Properties/Fields

        private bool isOwnerDestroyed
        {
            get { return this._hasAutoDestroyOwner && this._autoDestroyOwner == null; }
        }

        private readonly Action _onComplete;
        private readonly Action<float> _onUpdate;
        private float _startTime;
        private float _lastUpdateTime;

        // for pausing, we push the start time forward by the amount of time that has passed.
        // this will mess with the amount of time that elapsed when we're cancelled or paused if we just
        // check the start time versus the current world time, so we need to cache the time that was elapsed
        // before we paused/cancelled
        private float? _timeElapsedBeforeCancel;
        private float? _timeElapsedBeforePause;

        // after the auto destroy owner is destroyed, the timer will expire
        // this way you don't run into any annoying bugs with timers running and accessing objects
        // after they have been destroyed
        private readonly MonoBehaviour _autoDestroyOwner;
        private readonly bool _hasAutoDestroyOwner;

        #endregion

        #region Private Constructor (use static Register method to create new timer)

        private Timer(float duration, Action onComplete, Action<float> onUpdate,
            bool isLooped, bool usesRealTime, MonoBehaviour autoDestroyOwner)
        {
            this.duration = duration;
            this._onComplete = onComplete;
            this._onUpdate = onUpdate;

            this.isLooped = isLooped;
            this.usesRealTime = usesRealTime;

            this._autoDestroyOwner = autoDestroyOwner;
            this._hasAutoDestroyOwner = autoDestroyOwner != null;

            this._startTime = this.GetWorldTime();
            this._lastUpdateTime = this._startTime;
        }

        #endregion

        #region Private Methods

        private float GetWorldTime()
        {
            return this.usesRealTime ? Time.realtimeSinceStartup : Time.time;
        }

        private float GetFireTime()
        {
            return this._startTime + this.duration;
        }

        private float GetTimeDelta()
        {
            return this.GetWorldTime() - this._lastUpdateTime;
        }

        private void Update()
        {
            if (this.isDone)
            {
                return;
            }

            if (this.isPaused)
            {
                this._startTime += this.GetTimeDelta();
                this._lastUpdateTime = this.GetWorldTime();
                return;
            }

            this._lastUpdateTime = this.GetWorldTime();

            if (this._onUpdate != null)
            {
                this._onUpdate(this.GetTimeElapsed());
            }

            if (this.GetWorldTime() >= this.GetFireTime())
            {

                if (this._onComplete != null)
                {
                    this._onComplete();
                }

                if (this.isLooped)
                {
                    this._startTime = this.GetWorldTime();
                }
                else
                {
                    this.isCompleted = true;
                }
            }
        }

        #endregion

        #region Manager Class (implementation detail, spawned automatically and updates all registered timers)

        /// <summary>
        /// Manages updating all the <see cref="Timer"/>s that are running in the application.
        /// This will be instantiated the first time you create a timer -- you do not need to add it into the
        /// scene manually.
        /// </summary>
        private class TimerManager : MonoBehaviour
        {
            private List<Timer> _timers = new List<Timer>();

            // buffer adding timers so we don't edit a collection during iteration
            private List<Timer> _timersToAdd = new List<Timer>();

            public void RegisterTimer(Timer timer)
            {
                this._timersToAdd.Add(timer);
            }

            public void CancelAllTimers()
            {
                foreach (Timer timer in this._timers)
                {
                    timer.Cancel();
                }

                this._timers = new List<Timer>();
                this._timersToAdd = new List<Timer>();
            }

            public void PauseAllTimers()
            {
                foreach (Timer timer in this._timers)
                {
                    timer.Pause();
                }
            }

            public void ResumeAllTimers()
            {
                foreach (Timer timer in this._timers)
                {
                    timer.Resume();
                }
            }

            // update all the registered timers on every frame
            [UsedImplicitly]
            private void Update()
            {
                this.UpdateAllTimers();
            }

            private void UpdateAllTimers()
            {
                if (this._timersToAdd.Count > 0)
                {
                    this._timers.AddRange(this._timersToAdd);
                    this._timersToAdd.Clear();
                }

                foreach (Timer timer in this._timers)
                {
                    timer.Update();
                }

                this._timers.RemoveAll(t => t.isDone);
            }
        }

        #endregion

    }
}
