using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public  enum EnemyState { Patrolling , Chasing}
public class EnemyAI : MonoBehaviour
{
    [SerializeField] private EnemyState _currentState = EnemyState.Patrolling;
    [SerializeField] private Transform[] _wayPoints;
    [SerializeField] private Transform _player;
    private int _currentWaypoint = 0;
    private NavMeshAgent _agent;
    [SerializeField] private float _watchDistance = 5f;
    [SerializeField] private float _watchAngle = 90f;
    [SerializeField] private LayerMask _layerMask;
    [SerializeField] private float _rotationSpeed = 40f;

    private void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        if (_wayPoints.Length > 0 )
        {
            SetWaypoint();
        }
    }

    private void Update()
    {
        switch (_currentState)
        {
            case EnemyState.Patrolling:
                Patrol();
                break;
            case EnemyState.Chasing:
                _agent.SetDestination(_player.position);
                if (!CheckPlayer())
                {
                    _currentState = EnemyState.Patrolling;
                }
                break;
        }
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            GameManager.Instance.GameOver();
        }
    }

    private void Patrol()
    {
        if (_wayPoints == null || _wayPoints.Length == 0)
        {
            transform.Rotate(Vector3.up * _rotationSpeed * Time.deltaTime);
            return;
        }
        else if (!_agent.pathPending && _agent.remainingDistance <= _agent.stoppingDistance)
        {
            GoToWaypoint();
        }
        if (CheckPlayer())
        {
            _currentState = EnemyState.Chasing;
        }

    }

    private bool CheckPlayer()
    {
        Vector3 directionToPlayer = (_player.position - transform.position).normalized;
        float distamceToPlayer = Vector3.Distance(transform.position, _player.position);
        if (distamceToPlayer < _watchDistance )
        {
            if (Vector3.Angle(transform.forward, directionToPlayer) < _watchAngle/2)
            {
                if (!Physics.Raycast(transform.position + Vector3.up , directionToPlayer , distamceToPlayer , _layerMask))
                {
                    return true;
                }
            }
        }
        return false;
    }

    

    private void GoToWaypoint()
    {
        if (_wayPoints.Length == 0) return;
        _currentWaypoint =(_currentWaypoint +1) % _wayPoints.Length;
        SetWaypoint();
    }

    private void SetWaypoint()
    {
        _agent.SetDestination(_wayPoints[_currentWaypoint].position);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, _watchDistance);
        Vector3 left = Quaternion.Euler(0, -_watchAngle / 2, 0) * transform.forward;
        Vector3 right = Quaternion.Euler(0, _watchAngle / 2, 0) * transform.forward;
        Gizmos.DrawRay(transform.position, left * _watchDistance);
        Gizmos.DrawRay(transform.position, right * _watchDistance);
    }
}
