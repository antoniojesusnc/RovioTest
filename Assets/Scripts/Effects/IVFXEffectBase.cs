namespace RovioTest.Config
{
    public interface IVFXEffectBaseT2<T1, T2> : IVFXEffectBase
    {
        void DoEffect(T1 classType1, T2 classType2);
    }
    
    public interface IVFXEffectBaseT1<T> : IVFXEffectBase
    {
        void DoEffect(T classType);
    }
    
    public interface IVFXEffectBaseT0 : IVFXEffectBase
    {
        void DoEffect();
    }
    
    public interface IVFXEffectBase
    {
        void Cancel();
    }
}