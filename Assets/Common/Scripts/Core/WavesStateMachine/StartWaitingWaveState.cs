using System;
using UnityEngine;

public class StartWaitingWaveState : BaseWavesState
{
    public override event Action<float> OnTimerChanged;
    
    private float _fullTime;
    private float _lastTime;


    public StartWaitingWaveState(float attackWavesCooldown, WavesController wavesController) : base(wavesController)
    {
        _fullTime = attackWavesCooldown;
        _lastTime = _fullTime;
    }

    public override void OnTick(float deltaTime)
    {
        _lastTime -= deltaTime;
        OnTimerChanged?.Invoke(_lastTime);
    }

    public override void OnEnableState()
    {
        wavesController.SetTimer(_fullTime);
    }

    public override void OnDisableState()
    {
        wavesController.CurrentWave++;
        _lastTime = _fullTime;
    }

    public override void OnTimerEnded()
    {
        wavesController.ChangeWaveState(State.WaitingEndWave);
    }
}