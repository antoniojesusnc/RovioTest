using System.Runtime.CompilerServices;
using RovioTest.View;
using UnityEngine;
using Urd.Services.EventBus;

namespace RovioTest.Events
{
    public class OnBallBeingHitEvent : IEventBusMessage
    {
        public CharacterView HitCharacter { get; private set; }
        public ContactPoint ContactPoint { get; private set; }
        public BallHitTypes BallHitType { get; private set; }
        public bool IsServe { get; private set; }
        
        public static OnBallBeingHitEvent CharacterHitBall(CharacterView characterView, BallHitTypes hitType, bool isServe = false)
        {
            return new OnBallBeingHitEvent(characterView, default, hitType, isServe);
        }
        
        public static OnBallBeingHitEvent HitWithCharacter(CharacterView characterView, ContactPoint contactPoint)
        {
            return new OnBallBeingHitEvent(characterView, contactPoint, BallHitTypes.Hit);
        }
        public static OnBallBeingHitEvent HitWithWall(ContactPoint contactPoint)
        {
            return new OnBallBeingHitEvent(null, contactPoint, BallHitTypes.Wall);
        }
        
        public OnBallBeingHitEvent(CharacterView hitCharacter, ContactPoint contactPoint, BallHitTypes ballHitType, bool isServe = false)
        {
            ContactPoint = contactPoint;  
            HitCharacter = hitCharacter;
            BallHitType = ballHitType;
            IsServe = isServe;
        }

    }
}