using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private List<EnemyController> enemies;
    [SerializeField] private float radiusSpawner;
    [SerializeField] private float minCloseDistance;
    
    private Transform _playerTransform;
    
    private float _timer;
    private bool _needSpawn;
    private int _unitsCount;


    public void Init(Transform playerTransform)
    {
        _playerTransform = playerTransform;
    }
    
    public EnemyController Spawn()
    {
        var position = CalculatePosition();
        var rotationToTarget = _playerTransform.position - position;

        var enemy = enemies[0];
        enemy.name = $"Zombie_{_unitsCount++}";

        var enemyController = Instantiate(enemy, position, Quaternion.Euler(rotationToTarget));

        enemyController.SetTarget(_playerTransform);
        return enemyController;
    }

    private Vector3 CalculatePosition()
    {
        var localPosition = Random.insideUnitCircle * radiusSpawner;

        var r = Random.Range(minCloseDistance, radiusSpawner);
        var angle = Random.Range(0f, 360f);

        var x = r * Mathf.Cos(angle);
        var y = r * Mathf.Sin(angle);

        localPosition += new Vector2(x, y);

        return new Vector3(localPosition.x, 0.82f, localPosition.y) + _playerTransform.position;
    }
}