using System;
using UnityEngine;

public class WaitingEndWaveState : BaseWavesState
{
    public override event Action<float> OnTimerChanged;

    private Spawner _spawner;
    private Transform _playerTransform;
    
    private int _spawnedCount;
    private float _timerInterval;
    private float _currentTimerInterval;
    private float _lastTimeSpawn;
    private float _spawnInterval = 1f;
    private bool _waitAllEnemiesDied;


    public WaitingEndWaveState(Spawner spawner, float timerInterval,
        WavesController wavesController) : base(wavesController)
    {
        _spawner = spawner;
        _timerInterval = timerInterval;
        _currentTimerInterval = _timerInterval;
    }

    public override void OnEnableState()
    {
        _playerTransform = wavesController.PlayerTransform;
        _waitAllEnemiesDied = false;
        _spawner.Init(_playerTransform);
        wavesController.SetTimer(_timerInterval);
    }

    public override void OnDisableState()
    {
        _currentTimerInterval = _timerInterval;
    }


    public override void OnTick(float deltaTime)
    {
        if (_waitAllEnemiesDied) return;

        _currentTimerInterval -= deltaTime;
        _lastTimeSpawn -= deltaTime;
        OnTimerChanged?.Invoke(_currentTimerInterval);

        if (_lastTimeSpawn <= 0 && _currentTimerInterval >= 0)
        {
            _lastTimeSpawn = _spawnInterval;
            SpawnEnemy();
            _spawnedCount++;
        }
    }

    private void SpawnEnemy()
    {
        var enemy = _spawner.Spawn();
        enemy.Health.OnDie += OnUnitDie;
    }

    private void OnUnitDie()
    {
        _spawnedCount--;
        if (_spawnedCount <= 0 && _waitAllEnemiesDied) WaveEnded();
    }

    public override void OnTimerEnded()
    {
        _waitAllEnemiesDied = true;

        if (_spawnedCount <= 0) WaveEnded();
    }

    private void WaveEnded()
    {
        if (wavesController.CurrentWave >= wavesController.MaxWavesCount)
        {
            wavesController.ChangeWaveState(State.EndWaves);
        }
        else
        {
            wavesController.ChangeWaveState(State.StartWaveWaiting);
        }
    }
}