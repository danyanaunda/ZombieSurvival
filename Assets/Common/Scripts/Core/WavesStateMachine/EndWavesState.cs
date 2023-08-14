using System;

public class EndWavesState : BaseWavesState
{
    public override event Action<float> OnTimerChanged;

    
    public EndWavesState(WavesController wavesController) : base(wavesController)
    {
    }
    
    public override void OnEnableState()
    {
        wavesController.EndBattle();
    }
}