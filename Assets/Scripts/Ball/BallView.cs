using DG.Tweening;
using MyBox;
using RovioTest.Config;
using RovioTest.Events;
using RovioTest.Models;
using RovioTest.Services;
using TMPro;
using UnityEngine;
using Urd;
using Urd.Services;

namespace RovioTest.View
{
    public class BallView : MonoBehaviourEventObservable, 
        IEventBusObservable<OnBallChangeObjectiveEvent>,
        IEventBusObservable<OnCharacterSmashedEvent>,
        IEventBusObservable<OnCharacterHitBallEvent>,
        IEventBusObservable<OnGameOverEvent>,
        IEventBusObservable<OnBeginServeEvent>,
        IEventBusObservable<OnFinishServeEvent>
    {
        [SerializeField]
        private Rigidbody _rigidBody;
        [SerializeField]
        private MeshRenderer _meshRenderer;
        [SerializeField]
        private TrailRenderer _trailRenderer;
        [Header("HitEffects")]
        [SerializeField]
        private DOTweenAnimation _onHitAnimation;
        [SerializeField]
        private VFXWhiteColorEffect _hitBallColorEffect;
       
        [Header("Text")]
        [SerializeField]
        private TextMeshPro _text;
        [SerializeField]
        private Transform _ballTextPivot;
        
        private CharacterView _objective;
        private BallModuleConfig _ballConfig;
        private CharacterModel _characterModel;

        private float _defaultStellaTime;

        public BallModel Model { get; private set; }
        public bool IsMoving { get; private set; }

        private void Awake()
        {
            _rigidBody.detectCollisions = false;
            _defaultStellaTime = _trailRenderer.time;
        }

        protected override void Start()
        {
            base.Start();
            StaticServiceLocator.Get<IClockService>().SubscribeToUpdate(CustomUpdate);
            _ballConfig = StaticServiceLocator.Get<IGamePlayService>().GetModule<BallModule>().Config;
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            StaticServiceLocator.Get<IClockService>().UnSubscribeToUpdate(CustomUpdate);
        }

        public void SetModel(BallModel model)
        {
            Model = model;
        }
        
        private void CustomUpdate(float deltaTime)
        {
            Move(deltaTime);
            MoveTextBillBoard();
        }

        private void MoveTextBillBoard()
        {
            _ballTextPivot.localRotation = Quaternion.Euler(-transform.rotation.eulerAngles);
        }

        private void BeginMovement(CharacterView objective, Vector3 direction)
        {
            _objective = objective;
            transform.LookAt(transform.position + direction.SetY(0));
            _rigidBody.rotation = transform.rotation;
            
            IsMoving = true;
        }
        public void Move(float deltaTime)
        {
            if (!IsMoving)
            {
                return;
            }
            
            var direction = (_objective.transform.position - _rigidBody.position).normalized.SetY(0);
            var step = Model.MaxTurnDegreesAngle * Mathf.Deg2Rad * Time.deltaTime;
            var newDirection = Vector3.RotateTowards(transform.forward, direction, step, 0);
            transform.LookAt(transform.position + newDirection.SetY(0));
            _rigidBody.rotation = transform.rotation;
            
            var movement = transform.forward.normalized * Model.Speed* deltaTime;
            _rigidBody.MovePosition(transform.position + movement);
        }
        
        private void Stop()
        {
            _trailRenderer.Clear();
            
            IsMoving = false;
            _rigidBody.ResetInertiaTensor();
            _rigidBody.velocity = Vector3.zero;
            _rigidBody.angularVelocity =Vector3.zero ;
            _trailRenderer.Clear();
            _trailRenderer.time = 0;
        }

        private void SetScore()
        {
            _text.SetText(Model.CurrentScore.ToString("#0"));
        }
        
        private void DoEffectOfPlayerHitingBall()
        {
            _onHitAnimation.tween.Restart();
            _hitBallColorEffect.DoEffect(_meshRenderer);
            _trailRenderer.Clear();
        }
        
        public void ChangeScale(float scaleFactor)
        {
            transform.localScale *= scaleFactor;
        }
        
        public void OnNewEvent(OnBallChangeObjectiveEvent newEvent)
        {
            SetScore();
            BeginMovement(newEvent.Objetive, newEvent.Direction);
        }

        private void ChangeColor(Color color)
        {
            _meshRenderer.material.color = color;
            _trailRenderer.startColor = color;
            _trailRenderer.endColor = color;
        }

        public void OnNewEvent(OnCharacterSmashedEvent newEvent)
        {
            Stop();
        }

        public void OnNewEvent(OnGameOverEvent newEvent)
        {
            Stop();
        }

        public void OnNewEvent(OnFinishServeEvent newEvent)
        {
            _rigidBody.detectCollisions = true;
            _trailRenderer.time = _defaultStellaTime;
        }

        public void OnNewEvent(OnBeginServeEvent newEvent)
        {
            ChangeColor(_ballConfig.StandardColor);
        }

        public void OnNewEvent(OnCharacterHitBallEvent newEvent)
        {
            var color = newEvent.Character.Model.IsPlayer 
                ? _ballConfig.BallColorWhenPlayerHit
                : _ballConfig.BallColorWhenEnemyHit;
            ChangeColor(color);
            DoEffectOfPlayerHitingBall();
        }
    }
}