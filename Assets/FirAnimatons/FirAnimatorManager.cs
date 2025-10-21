using System;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace FirAnimations
{
    public class FirAnimationsManager : MonoBehaviour
    {
        [SerializeField, Range(0, 1)]
        private float _time;
        public float _timeLimit;
        [SerializeField] 
        private List<FirAnimationManagerComponent> animations_and_startTime;
        
        public Action OnEndAllAnimations;

        public void Initialize()
        {
            Stop();
            ToStartPoint();
        }
        
        private void OnValidate()
        {
            MoveByDelta();
        }

        public void StartAnimations()
        {
            _time = 0;
            enabled = true;
        }
        public void Stop()
        {
            enabled = false;
            ToEndPoint();
        }

        public void Update()
        {
            if (_time >= 1)
            {
#if UNITY_EDITOR
                _time %= 1;
#else
                _time = 1;
                enabled = false;
                OnEndAllAnimations?.Invoke();
                return;
#endif
            }
            
            _time += Time.unscaledDeltaTime/_timeLimit;
            MoveByDelta();
#if UNITY_EDITOR
            EditorApplication.QueuePlayerLoopUpdate();
#endif
        }

        private void MoveByDelta()
        {
            foreach (var animation in animations_and_startTime)
            {
                if (_time * _timeLimit < animation.StartTime) 
                    continue;

                float animationTime = _time * _timeLimit - animation.StartTime;
                animation.Animation.Time = animationTime;
                animation.Animation.Update();
            }
        }
        
        [ContextMenu("ToStartPoint")]
        public void ToStartPoint()
        {
            foreach (var animation in animations_and_startTime)
            { 
                animation.Animation.ToStartPoint();
            }
            
            _time = 0;
        }
        [ContextMenu("ToEndPoint")]
        public void ToEndPoint()
        {
            foreach (var animation in animations_and_startTime)
            {
                animation.Animation.ToEndPoint();
            }

            _time = 1;
        }
    }
    [Serializable]
    public struct FirAnimationManagerComponent
    {
        public FirAnimation Animation;
        public float StartTime;
    }
}