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
        /// 计时器从开始到结束需要多长时间。
        /// </summary>
        public float duration { get; private set; }

        /// <summary>
        /// 计时器是否在完成后再次运行。
        /// </summary>
        public bool isLooped { get; set; }

        /// <summary>
        /// 计时器是否已完成运行。如果计时器被取消，则为false。
        /// </summary>
        public bool isCompleted { get; private set; }

        /// <summary>
        /// 计时器是否使用实时或游戏时间。
        /// 实时性不受游戏时间刻度变化的影响（如暂停、慢移动），而游戏时间受影响。
        /// </summary>
        public bool usesRealTime { get; private set; }

        /// <summary>
        /// 计时器当前是否已暂停。
        /// </summary>
        public bool isPaused
        {
            get { return this._timeElapsedBeforePause.HasValue; }
        }

        /// <summary>
        /// 计时器是否已取消。
        /// </summary>
        public bool isCancelled
        {
            get { return this._timeElapsedBeforeCancel.HasValue; }
        }

        /// <summary>
        /// 获取计时器是否因任何原因已完成运行。
        /// </summary>
        public bool isDone
        {
            get { return this.isCompleted || this.isCancelled || this.isOwnerDestroyed; }
        }

        #endregion

        #region Public Static Methods

        /// <summary>
        /// 注册一个新的计时器，该计时器应在经过一定时间后触发事件。
        ///
        /// 场景更改时，注册的计时器将被销毁。
        /// </summary>
        /// <param name="duration">计时器启动前等待的时间，以秒为单位。</param>
        /// <param name="onComplete">计时器完成时要启动的操作。</param>
        /// <param name="onUpdate">每次更新计时器时都应该触发的动作。获取计时器当前循环开始后经过的时间（以秒为单位）。</param>
        /// <param name="isLooped">计时器是否应在执行后重复。</param>
        /// <param name="useRealTime">计时器是否使用实时（即不受暂停、慢/快动作的影响）或游戏时间（将受暂停和慢/快运动的影响）。</param>
        /// <param name="autoDestroyOwner">要将此计时器附加到的对象。在该对象被销毁后，计时器将过期而不执行。这允许您通过防止计时器在父级被破坏后运行和访问其父级的组件来避免烦人的</param>
        /// <returns>返回计时器对象，允许您检查统计数据并停止/恢复进度。</returns>
        public static Timer Register(float duration, Action onComplete, Action<float> onUpdate = null,
        bool isLooped = false, bool useRealTime = false, MonoBehaviour autoDestroyOwner = null)
        {
            // 创建一个管理器对象来更新所有计时器（如果还不存在）。
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

        // 取消计时器。
        public static void Cancel(Timer timer)
        {
            if (timer != null)
            {
                timer.Cancel();
            }
        }

        // 暂停计时器。
        public static void Pause(Timer timer)
        {
            if (timer != null)
            {
                timer.Pause();
            }
        }

        // 恢复计时器。
        public static void Resume(Timer timer)
        {
            if (timer != null)
            {
                timer.Resume();
            }
        }

        //取消所有的计时器
        public static void CancelAllRegisteredTimers()
        {
            if (Timer._manager != null)
            {
                Timer._manager.CancelAllTimers();
            }
        }

        //暂停所有的计时器
        public static void PauseAllRegisteredTimers()
        {
            if (Timer._manager != null)
            {
                Timer._manager.PauseAllTimers();
            }
        }

        //恢复所有的计时器
        public static void ResumeAllRegisteredTimers()
        {
            if (Timer._manager != null)
            {
                Timer._manager.ResumeAllTimers();
            }
        }

        #endregion

        #region Public Methods

        //停止正在进行或暂停的计时器。不会调用计时器的完成时回调。
        public void Cancel()
        {
            if (this.isDone)
            {
                return;
            }

            this._timeElapsedBeforeCancel = this.GetTimeElapsed();
            this._timeElapsedBeforePause = null;
        }

        //暂停正在运行的计时器。可以从暂停的同一点恢复暂停的计时器。
        public void Pause()
        {
            if (this.isPaused || this.isDone)
            {
                return;
            }

            this._timeElapsedBeforePause = this.GetTimeElapsed();
        }

        //恢复计时器。如果计时器未暂停，则不执行任何操作。
        public void Resume()
        {
            if (!this.isPaused || this.isDone)
            {
                return;
            }

            this._timeElapsedBeforePause = null;
        }

        /// <summary>
        /// 获取此计时器当前周期开始后已过的秒数。
        /// </summary>
        /// <returns>自计时器当前循环开始以来所经过的秒数，即，如果计时器循环，则为当前循环，如果计时器没有循环，则是启动。
        ///
        /// 如果计时器已完成运行，则等于持续时间。
        ///
        /// 如果计时器被取消/暂停，则等于计时器启动和取消/暂停之间经过的秒数</returns>
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
        /// 获取计时器完成前还剩多少秒。
        /// </summary>
        /// <returns>计时器完成之前剩余的秒数。计时器只有在没有暂停、取消或完成的情况下才经过时间。如果计时器完成，这将等于零。</returns>
        public float GetTimeRemaining()
        {
            return this.duration - this.GetTimeElapsed();
        }

        /// <summary>
        /// 获取计时器从开始到结束的进度比例。
        /// </summary>
        /// <returns>一个从0到1的值，指示计时器的持续时间已过。</returns>
        public float GetRatioComplete()
        {
            return this.GetTimeElapsed() / this.duration;
        }

        /// <summary>
        /// 获取计时器剩余的进度作为比率。
        /// </summary>
        /// <returns>一个从0到1的值，指示计时器的持续时间还有多长。</returns>
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
