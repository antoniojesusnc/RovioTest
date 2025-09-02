namespace RovioTest.Config
{
    public interface IVFXEffectBase<T>
    {
        void DoEffect(T classType);
        void Cancel();
    }
}