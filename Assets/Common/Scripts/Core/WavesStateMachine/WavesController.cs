using System;
using System.Collections.Generic;
using UnityEngine;

public class WavesController : MonoBehaviour
{
    [SerializeField] private float attackWavesCooldown;
    [SerializeField] private float waveDuration;
    [SerializeField] private Spawner spawner;

    public event Action BattleEnd;
    public event Action<float> TimerUpdatedEvent;

    public Transform PlayerTransform;
    public int MaxWavesCount;
    public int CurrentWave;

    private Dictionary<State, BaseWavesState> _allStates = new Dictionary<State, BaseWavesState>();
    private BaseWavesState _currentState;

    private bool _timerStarted;
    private float _timer;


    private void Awake()
    {
        _allStates.Add(State.StartWaveWaiting, new StartWaitingWaveState(attackWavesCooldown, this));
        _allStates.Add(State.WaitingEndWave, new WaitingEndWaveState(spawner, waveDuration, this));
        _allStates.Add(State.EndWaves, new EndWavesState(this));

        foreach (var statesValue in _allStates.Values)
        {
            statesValue.OnTimerChanged += time => TimerUpdatedEvent?.Invoke(time);
        }

        ChangeWaveState(State.StartWaveWaiting);
    }


    private void Update()
    {
        _currentState?.OnTick(Time.deltaTime);
        if (_timerStarted == false) return;

        _timer -= Time.deltaTime;
        if (_timer > 0) return;

        _timerStarted = false;
        _currentState?.OnTimerEnded();
    }

    public void SetPlayer(Transform playerTransform)
    {
        PlayerTransform = playerTransform;
    }

    public void SetTimer(float lastTime)
    {
        _timerStarted = true;
        _timer = lastTime;
        if (_timer <= 0) _currentState.OnTimerEnded();
    }

    public void ChangeWaveState(State nextState)
    {
        _currentState?.OnDisableState();
        _currentState = _allStates[nextState];
        _currentState.OnEnableState();
    }

    public void EndBattle()
    {
        BattleEnd?.Invoke();
    }
}