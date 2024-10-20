using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyPatrolState : BaseState<EnemyStateMachine.EnemyState>
{
    private EnemyStateMachine _enemy;
    private Vector3 center;
    static public Vector3 nextWaypoint;
    private float _walkingSpeed = 2.0f;
    private float walking_radius = 10f;
    public EnemyPatrolState(EnemyStateMachine enemy, EnemyStateMachine.EnemyState key = EnemyStateMachine.EnemyState.PATROL) : base(key)
    {
        _enemy = enemy;
    }

    public override void EnterState()
    {
        // Set a new center after done chasing, attacking etc
        center = _enemy.transform.position;
        _enemy.Agent.isStopped = true;
    }

    public override void UpdateState()
    {
        PatrolCycle();
    }

    public override void ExitState()
    {

    }

    private void PatrolCycle()
    {
        if (_enemy.Agent.isStopped)
        {
            nextWaypoint = GenerateRandomPointIn3DSpace(center, walking_radius);
            _enemy.Agent.speed = _walkingSpeed;
            _enemy.Agent.isStopped = false;
            _enemy._isIdling = false;
            _enemy.Agent.SetDestination(nextWaypoint);
        }
        else
        {
            if (Vector3.Distance(_enemy.transform.position, nextWaypoint) < 0.5 && !_enemy._isIdling)
            {
                _enemy.StartIdle();

            }
        }
    }

    public override EnemyStateMachine.EnemyState GetNextState()
    {
        if (_enemy.PlayerDetected(_enemy.chase_radius))
        {
            return EnemyStateMachine.EnemyState.CHASE;
        }
        return EnemyStateMachine.EnemyState.PATROL;
    }

    public override void OnTriggerEnter(Collider other) { }

    public override void OnTriggerExit(Collider other) { }

    public override void OnTriggerStay(Collider other) { }

    private Vector3 GenerateRandomPointIn3DSpace(Vector3 centerPosition, float radius)
    {
        float angle = Random.Range(0f, 360f); // Random angle in degrees
        float distance = Random.Range(0f, radius);
        NavMeshHit hit;
        Vector3 randomPoint;
        do
        {
            // Calculate the position relative to the center, restricting the Y coordinate to 0
            randomPoint = centerPosition + new Vector3(
                Mathf.Cos(angle * Mathf.Deg2Rad) * distance,
                0,
                Mathf.Sin(angle * Mathf.Deg2Rad) * distance
            );
        } while (!NavMesh.SamplePosition(randomPoint, out hit, radius, NavMesh.AllAreas));
        return new Vector3(hit.position.x, randomPoint.y, hit.position.z); ;
    }

}
