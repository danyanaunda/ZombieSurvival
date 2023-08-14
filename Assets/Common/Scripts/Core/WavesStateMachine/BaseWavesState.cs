using System;

public abstract class BaseWavesState
{
    public abstract event Action<float> OnTimerChanged;

    protected WavesController wavesController;

    protected BaseWavesState(WavesController wavesController)
    {
        this.wavesController = wavesController;
    }
    
    public abstract void OnEnableState();
    public virtual void OnTick(float deltaTime){}
    public virtual void OnDisableState(){}
    public virtual void OnTimerEnded(){}
}