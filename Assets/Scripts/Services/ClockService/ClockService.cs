using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Urd.Timer;

namespace Urd.Services
{
    [Serializable]
    public class ClockService : BaseService, IClockService
    {     
        public override int LoadPriority => 20;

        public bool IsInPause { get; private set; }
        public float DeltaTime => Time.deltaTime;

        public DateTime Now => DateTime.UtcNow;
        public float SpeedMod { get; private set; } = 1;

        List<ClockServiceUpdateModel> _updateListeners = new List<ClockServiceUpdateModel>();
        List<ClockServiceUpdateModel> _updatePerSecondListeners = new List<ClockServiceUpdateModel>();
        List<ClockServiceUpdateModel> _fixedUpdateListeners = new List<ClockServiceUpdateModel>();
        List<TimerModel> _delayedCalls = new List<TimerModel>();

        private ICoroutineService _coroutineService;

        private bool _update = true;
        private bool _fixedUpdate = true;

        private float _secondTimestamp = 0;
        public override void Init()
        {
            base.Init();
            _coroutineService = StaticServiceLocator.Get<ICoroutineService>();

            StartUpdate();
            StartFixedUpdate();
        }

        private void StartUpdate()
        {
            _update = true;
            _coroutineService.StartCoroutine(UpdateCoroutineCo());
        }
        private void StartFixedUpdate()
        {
            _update = true;
            _coroutineService.StartCoroutine(FixedUpdateCoroutineCo());
        }

        public void SetPause(bool gamePaused)
        {
            IsInPause = gamePaused;
        }

        public void SetSpeedMod(float speedMod)
        {
            SpeedMod = speedMod;
        }

        public void SubscribeToUpdate(Action<float> listener, bool pausable = true)
        {
            _updateListeners.Add(new ClockServiceUpdateModel(listener, pausable));
        }

        public void UnSubscribeToUpdate(Action<float> listenerToDelete)
        {
            var model = _updateListeners.Find(listener => listener.Listener == listenerToDelete);
            if(model != null)
            {
                _updateListeners.Remove(model);
            }
        }

        public void SubscribeToUpdatePerSecond(Action<float> listener, bool pausable = true)
        {
            _updatePerSecondListeners.Add(new ClockServiceUpdateModel(listener, pausable));
        }

        public void UnSubscribeToUpdatePerSecond(Action<float> listenerToDelete)
        {
            var model = _updatePerSecondListeners.Find(listener => listener.Listener == listenerToDelete);
            if(model != null)
            {
                _updatePerSecondListeners.Remove(model);
            }
        }

        public void SubscribeToFixedUpdate(Action<float> listener, bool pausable = true)
        {
            _fixedUpdateListeners.Add(new ClockServiceUpdateModel(listener, pausable));
        }

        public void UnSubscribeToFixedUpdate(Action<float> listenerToDelete)
        {
            var model = _fixedUpdateListeners.Find(listener => listener.Listener == listenerToDelete);
            if(model != null)
            {
                _fixedUpdateListeners.Remove(model);
            }
        }

        public TimerModel AddDelayCall(float duration, Action finishCallback, bool pausable = true)
        {
            var timerModel = new TimerModel(duration, finishCallback, pausable);
            _delayedCalls.Add(timerModel);
            timerModel.BeginTimer(finishCallback);
            return timerModel;
        }

        private IEnumerator UpdateCoroutineCo()
        {
            while (_update)
            {
                float deltaTime = Time.deltaTime;
                if (SpeedMod != 0)
                {
                    deltaTime *= SpeedMod;
                }
                UpdateUpdates(deltaTime);
                UpdateDelayedCalls(deltaTime);

                UpdatePerSecondUpdates(deltaTime);

                yield return 0;
            }
        }
        
        private IEnumerator FixedUpdateCoroutineCo()
        {
            while (_fixedUpdate)
            {
                float deltaTime = Time.fixedDeltaTime;
                if (SpeedMod != 0)
                {
                    deltaTime *= SpeedMod;
                }

                UpdateFixedUpdates(deltaTime);
                
                yield return new WaitForFixedUpdate();
            }
        }

        private void UpdateFixedUpdates(float deltaTime)
        {
            for (int i = 0; i < _fixedUpdateListeners.Count; i++)
            {
                if (!IsInPause || !_fixedUpdateListeners[i].IsPausable)
                {
                    _fixedUpdateListeners[i].CallListener(deltaTime);
                }
            }
        }

        public void __TestUpdate(float deltaTime)
        {
            UpdateUpdates(deltaTime);
            UpdateDelayedCalls(deltaTime);
        }

        private void UpdateUpdates(float deltaTime)
        {
            for (int i = 0; i < _updateListeners.Count; i++)
            {
                if (!IsInPause || !_updateListeners[i].IsPausable)
                {
                    _updateListeners[i].CallListener(deltaTime);
                }
            }
        }

        private void UpdatePerSecondUpdates(float deltaTime)
        {
            _secondTimestamp += deltaTime;
            if (_secondTimestamp < 1)
            {
                return;
            }

            int seconds = (int)_secondTimestamp;
            for (int i = 0; i < seconds; i++)
            {
                for (int j = 0; j < _updatePerSecondListeners.Count; j++)
                {
                    if (!IsInPause || !_updatePerSecondListeners[j].IsPausable)
                    {
                        _updatePerSecondListeners[j].CallListener(1);
                    }
                }
            }

            _secondTimestamp -= seconds;
        }

        private void UpdateDelayedCalls(float deltaTime)
        {
            for (int i = _delayedCalls.Count - 1; i >= 0; i--)
            {
                if (_delayedCalls[i].Disposed)
                {
                    _delayedCalls.RemoveAt(i);
                    continue;
                }
                
                if (!IsInPause || !_delayedCalls[i].IsPausable)
                {
                    if (_delayedCalls[i].IsInCooldown)
                    {
                        _delayedCalls[i].DeductTime(deltaTime);
                    }
                    else
                    {
                        _delayedCalls.RemoveAt(i);
                    }
                }
            }
        }
        
        protected class ClockServiceUpdateModel
        {
            public bool IsPausable { get; private set; }
            
            public Action<float> Listener { get; private set; }

            public ClockServiceUpdateModel(Action<float> newListener) : this(newListener, true) { }

            public ClockServiceUpdateModel(Action<float> newListener, bool isPausable)
            {
                Listener = newListener;
                IsPausable = isPausable;
            }

            public void CallListener(float deltaTime)
            {
                if (Listener != null && Listener.Target != null)
                {
                    Listener.Invoke(deltaTime);
                }
            }
        }
    }
}