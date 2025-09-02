using RovioTest.Services;
using RovioTest.View;
using Urd.Services;
using Urd.Services.EventBus;

namespace RovioTest.Skills
{
    public class CharacterSkill:  ICharacterSkill, IEventBusObservable<IEventBusMessage>
    {
        public bool IsActive { get; protected set; }
        
        protected SkillsModule _skillsModule;
        protected CharacterView _owner;

        public CharacterSkill(ICharacterSkill skill)
        {
            
        }
        
        public virtual void Init(SkillsModule skillsModule)
        {
            _skillsModule = skillsModule;
        }

        public virtual void GetReady(CharacterView owner)
        {
            _owner = owner;
        }
        public virtual void Begin(CharacterView owner)
        {
            StaticServiceLocator.Get<IEventBusService>().Subscribe(this);
            
            IsActive = true;
            _owner = owner;
        }

        public virtual void Finish()
        {
            _owner = null;
            IsActive = false;
            StaticServiceLocator.Get<IEventBusService>()?.Unsubscribe(this);
        }

        public virtual void Dispose()
        {
            _skillsModule = null;
            Finish();
        }
        public void OnNewEvent(IEventBusMessage newEvent) { }
    }
}