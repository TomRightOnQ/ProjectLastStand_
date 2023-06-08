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
        Lock,
        Wondering
        //Spawning
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
    [SerializeField] protected int spawnCount = 0;
    [SerializeField] protected int currentSpawnCount = 0;
    [SerializeField] protected int bombardCount = 0;
    [SerializeField] protected int currentBombardCount = 0;
    protected int extraBombardCount = 0;
    [SerializeField] protected int extraSpawnCount = 0;
    [SerializeField] protected int extraCounter = 0;
    [SerializeField] protected bool halfhprage = false;
    [SerializeField] protected bool wonder = false;
    [SerializeField] protected Vector3 wonderdirection;
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
        stateBehaviors[MonsterState.Wondering] = AIWondering;
        //stateBehaviors[MonsterState.Spawning] = AISpawning;

        // Set waiting times for state transitions
        stateWaitingTimes[MonsterState.MoveTowardBase] = WALKING_TIME;
        stateWaitingTimes[MonsterState.Aiming] = AIMMING_TIME;
        stateWaitingTimes[MonsterState.Lock] = LOCKING_TIME;
        stateWaitingTimes[MonsterState.FireAtPlayer] = FIRING_TIME;
        stateWaitingTimes[MonsterState.Wondering] = 4f;
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
            case MonsterBehaviorType.Hyperion:
                currentBehavior = gameObject.AddComponent<MonsterHyperion>();
                isSetUp = true;
                break;
            case MonsterBehaviorType.Anteater:
                currentBehavior = gameObject.AddComponent<MonsterAnteater>();
                isSetUp = true;
                break;
        }
    }
    public void bombardingAttack(Vector3 pos) { Instantiate(PrefabManager.Instance.BombardingPrefab, pos, Quaternion.identity); }
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

    protected void AIWondering()
    {
        transform.LookAt(wonderdirection);
        Vector3 newPos = wonderdirection * monster.Speed * 3 * Time.deltaTime;
        transform.position += newPos;
        hasTarget = false;
    }

    protected void AIFireAtPlayer()
    {
        // Fire a bullet
        StartCoroutine(AIFireBullet());
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
            currentSpawnCount = spawnCount;
            currentBombardCount = bombardCount;
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

    protected IEnumerator AIFireBullet()//for both fire and spawn
    {
        // if (currentFireCount <= 0)
        // {
        //     yield break;
        // }

        for (int i = 0; i < currentFireCount; i++)
        {
            monster.FireBullet();
            currentFireCount--;
            yield return new WaitForSeconds(0.5f); // Adjust the delay between each bullet firing

            if (currentFireCount <= 0)
            {
                yield break;
            }
        }
        for (int i = 0; i < currentSpawnCount; i++)
        {
            Vector3 position = transform.position;
            position = new Vector3(position.x, 0f, position.z);
            MonsterManager.Instance.spawn(position, 0);
            currentSpawnCount--;
            yield return new WaitForSeconds(0.5f); // Adjust the delay between each spawn
            extraCounter++;
            Vector3 targetPosition = targetPlayer.transform.position;
            bombardingAttack(targetPosition);
            if (currentSpawnCount <= 0)
            {
                yield break;
            }
        }
        for (int i = 0; i < currentBombardCount; i++)
        {
            extraCounter++;
            currentBombardCount--;
            yield return new WaitForSeconds(0.2f); // Adjust the delay between each bullet firing
            Vector3 targetPosition = targetPlayer.transform.position;
            float changeX = (Random.value < 0.5f) ? Random.Range(-8f, -3f) : Random.Range(3f, 8f);
            float changeZ = (Random.value < 0.5f) ? Random.Range(-8f, -3f) : Random.Range(3f, 8f);
            Vector3 newtargetPosition = new Vector3(
                targetPosition.x + changeX,
                targetPosition.y,
                targetPosition.z + changeZ
            );
            bombardingAttack(newtargetPosition);

            if (currentBombardCount <= 0)
            {
                bombardingAttack(targetPosition);
                yield break;
            }
        }
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
        currentFireCount = 0;
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

public class MonsterHyperion : MonsterAI
{
    private void Start()
    {
        searchRadius = 50f;
        fireCount = 0;
        currentFireCount = 0;
        spawnCount = 1;
        currentSpawnCount = 0;
        extraSpawnCount = 4;
        halfhprage = true;
        //never actually fires
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
        
        if ((monster.prevHP < monster.HitPoints/2)&&halfhprage)
        {
            halfhprage = false;
            currentSpawnCount += 12;
        }
    }
}

public class MonsterAnteater : MonsterAI
{
    private void Start()
    {
        searchRadius = 60f;
        fireCount = 0;
        currentFireCount = 0;
        spawnCount = 0;
        currentSpawnCount = 0;
        extraSpawnCount = 0;
        bombardCount = 2;
        currentBombardCount = 0;
        extraBombardCount = 8;
        extraCounter = 10;
        halfhprage = true;
        wonder = false;
        //never actually fires
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
            case MonsterState.Wondering:
                currentState = MonsterState.Aiming;
                break;
            case MonsterState.Aiming:
                float x = Random.Range(-1f, 1f);
                float z = Random.Range(-1f, 1f);
                wonderdirection = new Vector3(x, 0, z).normalized;
                if (hasTarget)
                {
                    currentState = MonsterState.Lock;
                    wonder = true;
                }
                else
                {
                    if (wonder)
                    {
                        currentState = MonsterState.Wondering;
                    }
                    else
                    {
                        currentState = MonsterState.MoveTowardBase;
                    }
                }
                break;
            case MonsterState.Lock:
                currentState = MonsterState.FireAtPlayer;
                if (extraCounter>=15)
                {
                    extraCounter = 0;
                    currentSpawnCount = extraSpawnCount;
                    currentBombardCount = extraBombardCount;
                }
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