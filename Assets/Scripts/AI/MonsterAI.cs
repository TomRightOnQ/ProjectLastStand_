using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static MonsterConfigs;

// Monster behaviours
public class MonsterAI : MonoBehaviour
{
    // States
    protected enum MonsterState
    {
        MoveTowardBase,
        Aimming,
        FireAtPlayer
    }

    // Data
    [SerializeField] protected Monsters monster;
    protected Vector3 targetPosition;
    protected MonsterState currentState;
    private MonsterAI currentBehavior;
    private bool isSetUp = false;

    // Map state and data
    protected Dictionary<MonsterState, System.Action> stateBehaviors = new Dictionary<MonsterState, System.Action>();
    protected Dictionary<MonsterState, float> stateWaitingTimes = new Dictionary<MonsterState, float>();
    protected float currentStateStartTime;
    protected float behaviorTimer;


    // Constants
    protected const float WALKING_TIME = 3f;
    protected const float AIMMING_TIME = 0.5f;
    protected const float FIRING_TIME = 0.5f;

    private void Awake()
    {
        monster = GetComponent<Monsters>();
        targetPosition = new Vector3(0, 0.1f, 0);
        currentState = MonsterState.MoveTowardBase;

        // Map state to behavior methods
        stateBehaviors[MonsterState.MoveTowardBase] = AIWalkTowardBase;
        stateBehaviors[MonsterState.FireAtPlayer] = AIFireAtPlayer;

        // Set waiting times for state transitions
        stateWaitingTimes[MonsterState.MoveTowardBase] = WALKING_TIME;
        stateWaitingTimes[MonsterState.Aimming] = AIMMING_TIME;
        stateWaitingTimes[MonsterState.FireAtPlayer] = FIRING_TIME;
        currentStateStartTime = Time.time;
    }

    // SetUp
    public void SetUp()
    {
        if (isSetUp)
            return;
        switch (monster.BehaviorType)
        {
            case MonsterBehaviorType.Walker:
                currentBehavior = gameObject.AddComponent<MonsterWalker>();
                isSetUp = true;
                break;
            case MonsterBehaviorType.Shooter:
                currentBehavior = gameObject.AddComponent<MonsterShooter>();
                isSetUp = true;
                break;
        }
    }
    // Destroy
    public void RemoveAI()
    {
        if (currentBehavior != null) {
            Destroy(currentBehavior);
            isSetUp = false;
        }
    }


    // Transitions
    protected void TransitionToNextState()
    {
        // Implement state transition logic here
        // For example, move to the next state based on a specific condition or a predefined sequence

        switch (currentState)
        {
            case MonsterState.MoveTowardBase:
                currentState = MonsterState.FireAtPlayer;
                break;
            case MonsterState.FireAtPlayer:
                currentState = MonsterState.MoveTowardBase;
                break;
        }

        // Reset current state start time
        currentStateStartTime = Time.time;
    }

    // Behaviours
    protected void AIWalkTowardBase()
    {
        Vector3 direction = targetPosition - transform.position;
        direction.y = 0;
        direction.Normalize();
        transform.LookAt(targetPosition);
        Vector3 newPos = direction * monster.Speed * 3 * Time.deltaTime;
        transform.position += newPos;
    }

    protected void AIFireAtPlayer()
    {
        // ...
    }
}

public class MonsterWalker : MonsterAI
{
    private void Update()
    {
        stateBehaviors[currentState]();
    }
}

public class MonsterShooter : MonsterAI
{
    private void Update()
    {
        stateBehaviors[currentState]();

        if (behaviorTimer <= 0f)
        {
            TransitionToNextState();
            behaviorTimer = stateWaitingTimes[currentState];
        }

        behaviorTimer -= Time.deltaTime;
    }
}
