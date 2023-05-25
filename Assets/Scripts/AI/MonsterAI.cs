using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static MonsterConfigs;

// Monster behaviours
public class MonsterAI : MonoBehaviour
{
    // States
    protected enum MonsterState
    {
        MoveTowardBase,
        Aiming,
        FireAtPlayer,
        Lock
    }

    [SerializeField] protected bool hasTarget;
    [SerializeField] protected GameObject targetPlayer;
    protected float searchRadius = 10f;

    // Data
    [SerializeField] protected Monsters monster;
    protected Vector3 targetPosition;
    [SerializeField] protected MonsterState currentState;
    private MonsterAI currentBehavior;
    private bool isSetUp = false;
    protected int fireCount = 0;
    protected int currentFireCount = 0;

    // Map state and data
    protected Dictionary<MonsterState, System.Action> stateBehaviors = new Dictionary<MonsterState, System.Action>();
    protected Dictionary<MonsterState, float> stateWaitingTimes = new Dictionary<MonsterState, float>();
    protected float currentStateStartTime;
    protected float behaviorTimer;


    // Constants
    protected const float WALKING_TIME = 3f;
    protected const float AIMMING_TIME = 0.1f;
    protected const float FIRING_TIME = 0.3f;
    protected const float LOCKING_TIME = 2f;

    private void Awake()
    {
        monster = GetComponent<Monsters>();
        targetPosition = new Vector3(0, 0.1f, 0);
        currentState = MonsterState.MoveTowardBase;

        // Map state to behavior methods
        stateBehaviors[MonsterState.MoveTowardBase] = AIWalkTowardBase;
        stateBehaviors[MonsterState.Aiming] = AISearchForPlayer;
        stateBehaviors[MonsterState.Lock] = AILookingAt;
        stateBehaviors[MonsterState.FireAtPlayer] = AIFireAtPlayer;

        // Set waiting times for state transitions
        stateWaitingTimes[MonsterState.MoveTowardBase] = WALKING_TIME;
        stateWaitingTimes[MonsterState.Aiming] = AIMMING_TIME;
        stateWaitingTimes[MonsterState.Lock] = LOCKING_TIME;
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
    protected virtual void TransitionToNextState()
    {
        // Implement state transition logic here

        switch (currentState)
        {
            case MonsterState.MoveTowardBase:
                currentState = MonsterState.MoveTowardBase;
                behaviorTimer = stateWaitingTimes[currentState];
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
        hasTarget = false;
    }

    protected void AIFireAtPlayer()
    {
        // Fire a bullet
        AIFireBullet();
    }

    protected void AISearchForPlayer()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, searchRadius, LayerMask.GetMask("Player"));

        if (colliders.Length > 0)
        {
            // Found a player, set it as the target
            targetPlayer = colliders[0].gameObject;
            hasTarget = true;
            currentFireCount = fireCount;
        }
        else {
            hasTarget = false;
        }
    }

    protected void AILookingAt()
    {
        Vector3 targetPosition = targetPlayer.transform.position;
        transform.LookAt(new Vector3(targetPosition.x, transform.position.y, targetPosition.z));
    }

    protected void AIFireBullet()
    {
        if (currentFireCount <= 0) {
            return;
        }
        for (int i = currentFireCount; i > 0; i--)
        {
            currentFireCount--;
            monster.FireBullet();
            StartCoroutine(Reload());
        }
    }

    protected IEnumerator Reload()
    {
        yield return new WaitForSeconds(0.2f);
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
    private void Start()
    {
        searchRadius = 25f;
        fireCount = 2;
    }

    // Transitions
    protected override void TransitionToNextState()
    {
        // Implement state transition logic here

        switch (currentState)
        {
            case MonsterState.MoveTowardBase:
                currentState = MonsterState.Aiming;
                break;
            case MonsterState.Aiming:
                if (hasTarget)
                {
                    currentState = MonsterState.Lock;
                }
                else
                {
                    currentState = MonsterState.MoveTowardBase;
                }
                break;
            case MonsterState.Lock:
                currentState = MonsterState.FireAtPlayer;
                break;
            case MonsterState.FireAtPlayer:
                currentState = MonsterState.Aiming;
                break;
        }

        // Reset current state start time
        currentStateStartTime = Time.time;
    }

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
