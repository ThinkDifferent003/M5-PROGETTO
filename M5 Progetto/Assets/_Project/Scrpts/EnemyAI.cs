using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public  enum EnemyState { Idle , Patrolling , Chasing , Searching , Returning}
public class EnemyAI : MonoBehaviour
{
    [SerializeField] private EnemyState _currentState;
    [SerializeField] private bool _isStaticEnemy;
    [SerializeField] Transform[] _waypoints;
    [SerializeField] private float _visionRange = 10f;
    [SerializeField] private float _visionAngle = 45f;
    [SerializeField] private Transform _player;
    [SerializeField] private Transform _eye;
    [SerializeField] private float _chaseTimeOut = 1.5f;
    private float _chaseTimer;
    private NavMeshAgent _agent;
    private int _currentWayPoint;
    private Vector3 _lastPosition;
    private Vector3 _startPosition;
    void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        _startPosition = transform.position;

        if (_isStaticEnemy)
        {
            _currentState = EnemyState.Idle;
            StartCoroutine(RotationRoutine());
        }
        else
        {
            _currentState = EnemyState.Patrolling;
        }
    }

    
    void Update()
    {
        LookPlayer();
        switch (_currentState)
        {
            case EnemyState.Idle:
                break;
            case EnemyState.Patrolling:
                Patrol();
                break;
            case EnemyState.Chasing:
                Chase();
                break;
            case EnemyState.Searching:
                Search(); 
                break;
            case EnemyState.Returning:
                Return();
                break;
        }
        
    }

    private void LookPlayer()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, _player.position);
        Vector3 direction = _player.position - _eye.position;
        float angle = Vector3.Angle(_eye.forward, direction);

        bool canSee = false;

        if (angle < _visionAngle && direction.magnitude < _visionRange)
        {
            RaycastHit hit;
            if (Physics.Raycast(_eye.position + Vector3.up, direction.normalized , out hit , _visionRange ))
            {
                Debug.Log("Il raggio vede: " + hit.collider.name);
                if (hit.collider.CompareTag("Player"))
                {
                   canSee = true;
                }
            }
        }

        if (canSee)
        {
            _currentState = EnemyState.Chasing;
            _lastPosition = _player.position;
            _chaseTimer = _chaseTimeOut;
        }
        else if (_currentState == EnemyState.Chasing)
        {
            _chaseTimer -= Time.deltaTime;
            if (_chaseTimer <= 0)
            {
                _currentState = EnemyState.Searching;
            }
        }
       
    }

    private void Patrol()
    {
        if (_waypoints.Length == 0) return;
        
        if (!_agent.pathPending && _agent.remainingDistance < 0.5f)
        {
            _currentWayPoint = (_currentWayPoint + 1) % _waypoints.Length;
            _agent.SetDestination(_waypoints[_currentWayPoint].position);
        }
    }

    private void Chase()
    {
        _agent.SetDestination(_player.position);
    }

    private void Search()
    {
        _agent.SetDestination(_lastPosition);
        if (_agent.remainingDistance < 0.5f)
        {
            _currentState = EnemyState.Returning;
        }
    }

    private void Return()
    {
        _agent.SetDestination(_startPosition);
        if (!_agent.pathPending && _agent.remainingDistance < 0.5f)
        {
            if (_isStaticEnemy)
            {
                _currentState = EnemyState.Idle;
            }
            else
            {
                _currentState = EnemyState.Patrolling;
                if (_waypoints.Length > 0)
                {
                    _agent.SetDestination(_waypoints[_currentWayPoint].position);
                }
            }
        }
    }

    IEnumerator RotationRoutine()
    {
        while (true)
        {
            if (_currentState == EnemyState.Idle)
            {
                yield return new WaitForSeconds(3f);
                transform.Rotate(0, 100, 0);
            }
            yield return null;
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (_eye == null) return;
        Gizmos.color = Color.yellow;
        Gizmos.DrawRay(_eye.position , _eye.forward * _visionRange);

        Gizmos.color = Color.red;
        Vector3 left = Quaternion.Euler(0,- _visionAngle,0) * _eye.forward;
        Vector3 right = Quaternion.Euler(0, _visionAngle, 0) * _eye.forward;
        Gizmos.DrawRay(_eye.position , left * _visionRange);
        Gizmos.DrawRay(_eye.position, right * _visionRange);
    }
}
