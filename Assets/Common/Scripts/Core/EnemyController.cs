using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour, IUnit
{
    [SerializeField] private Transform shotPoint;
    [SerializeField] private int maxDamage;

    public float pathUpdateDelay = 0.2f;

    public Health Health { get; private set; }
    public int Damage => maxDamage;

    private const int PLAYER_LAYER_MASK = 1 << 7;
    private const float ATTACK_DELAY = 3f;

    private Transform _playerTransform;
    private NavMeshAgent _navMeshAgent;

    private bool PlayerIsNull => _playerTransform == null;
    private bool _isNavMeshAgentNull;
    private float _lastAttack;
    private float _pathUpdateDeadLine;
    private float _shootingDistance;

    private Transform _transform;

    private void Awake()
    {
        _transform = transform;
        _navMeshAgent = GetComponent<NavMeshAgent>();
        Health = GetComponent<Health>();

        _isNavMeshAgentNull = _navMeshAgent == null;

        if (_isNavMeshAgentNull is true) return;
        _shootingDistance = _navMeshAgent.stoppingDistance;
        _pathUpdateDeadLine = pathUpdateDelay;

        Health.OnDie += OnDie;
    }


    private void Update()
    {
        if (PlayerIsNull)
            return;

        var inRange = (transform.position - _playerTransform.position).sqrMagnitude <=
                      _shootingDistance * _shootingDistance;

        if (inRange)
        {
            if (LookAtTarget())
            {
                Attack();
            }
        }
        else
        {
            UpdatePathToTarget();
        }
    }

    public void SetTarget(Transform playerTransform)
    {
        _playerTransform = playerTransform;
    }

    private void Attack()
    {
        if (_lastAttack + ATTACK_DELAY > Time.time) return;

        if (Physics.Raycast(shotPoint.position, shotPoint.forward, out RaycastHit hit, _shootingDistance,
            PLAYER_LAYER_MASK,
            QueryTriggerInteraction.Ignore) is false) return;

        if (!hit.collider.TryGetComponent<Health>(out var health)) return;

        health.TakeDamage(maxDamage);
        _lastAttack = Time.time;
    }

    private bool LookAtTarget()
    {
        var lookPosition = _playerTransform.position - this.gameObject.transform.position;
        lookPosition.y = 0;

        var rotation = Quaternion.LookRotation(lookPosition);
        _transform.rotation = Quaternion.Slerp(_transform.rotation, rotation, 0.2f);
        return Vector3.Dot(_transform.forward, lookPosition) > 0;
    }

    private void UpdatePathToTarget()
    {
        if (_isNavMeshAgentNull)
            return;

        if (Time.time >= _pathUpdateDeadLine)
        {
            _pathUpdateDeadLine = Time.time + pathUpdateDelay;
            _navMeshAgent.SetDestination(_playerTransform.position);
        }
    }

    private void OnDie()
    {
        Destroy(gameObject);
    }
}