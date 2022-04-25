using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum EnemyState
{
    Idle,
    Patrol,
    Chase,
    Attack,
    Death,
    ChasePuck
}

public class NPC : MonoBehaviour
{
    // Everything in this block that's public is for debug/test purposes
    private NavMeshAgent navMeshAgent;
    public int currCheckpoint;
    private int checkpointCount;
    public EnemyState status;
    private int dir;
    private Animator anim;
    public AudioSource source;
    public AudioClip active;

    public AudioClip idleSound;
    public AudioClip walkSound;
    public AudioClip runSound;
    public AudioClip attackSound;
    public AudioClip punchSound;
    public float timeSinceAudioClip;

    public GameObject model;
    public float speed = 1.5f;
    public Transform objectOfTransforms;
    public float stoppingDist = 0.64f;

    public float sightRadius;
    public float sightAngle;

    public float captureDist;

    public Vector3 idleDirection;

    public float timeSinceAttacking = 0.0f;
    public float attackCaptureTime = 0.75f;


    //Hectors additional fields
    public int distractionPuckDetectionDistance = 5;
    public GameObject puckPrefab;
    float distanceToPuck;

    // Start is called before the first frame update
    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        currCheckpoint = 0;
        checkpointCount = objectOfTransforms.childCount;

        anim = model.GetComponent<Animator>();

        // init the patrol state
        if (checkpointCount == 1)
        {
            StartIdle();
        }
        else
        {
            StartPatrol();
        }
        dir = 1;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameObject.FindGameObjectWithTag("Distraction") != null)
        {
            distanceToPuck = Vector3.Distance(transform.position, GameObject.FindGameObjectWithTag("Distraction").transform.position);
        }


        timeSinceAudioClip += Time.deltaTime;
        if (timeSinceAudioClip > active.length)
        {
            source.PlayOneShot(active);
            timeSinceAudioClip = 0;
        }

        switch (status)
        {
            case EnemyState.Idle:
                OnIdle();
                break;
            case EnemyState.Patrol:
                OnPatrol();
                break;
            case EnemyState.Chase:
                OnChase();
                break;
            case EnemyState.Attack:
                OnAttack();
                break;
            case EnemyState.Death:
                OnDie();
                break;

            case EnemyState.ChasePuck:
                OnChasePuck();
                break;
        }
    }

    void OnIdle()
    {
        transform.rotation = Quaternion.Euler(idleDirection);
        // if enemy sees player, chase mode activated
        if (PlayerInFOV())
        {
            ExitIdle();
            StartChase();
        }

        //if the distraction puck is in the robots range then chase the puck
        else if (distanceToPuck <= distractionPuckDetectionDistance)
        {
            ExitIdle();
            StartChasePuck();

        }


    }

    void OnPatrol()
    {

        Vector3 currGoal = objectOfTransforms.GetChild(currCheckpoint).position;
        if (Vector3.Distance(transform.position, currGoal) <= stoppingDist)
        {
            if (checkpointCount == 1)
            {
                ExitPatrol();
                StartIdle();
            }
            else
            {
                SetNextPoint();
            }
        }
        navMeshAgent.destination = currGoal;
        FaceTarget(currGoal);


        // if enemy sees player, chase mode activated
        if (PlayerInFOV())
        {
            ExitPatrol();
            StartChase();
        }

        //if the distraction puck is in the robots range then chase the puck
        else if (distanceToPuck <= distractionPuckDetectionDistance && GameObject.FindGameObjectWithTag("Distraction") != null)
        {
            ExitPatrol();
            StartChasePuck();

        }


    }

    //changes the robots state to chasing the puck
    void StartChasePuck()
    {
        status = EnemyState.ChasePuck;
        anim.SetBool("Chase", true);
        active = runSound;

        navMeshAgent.speed = 7.9f;

        timeSinceAudioClip = 0;
        source.PlayOneShot(active);

    }

    //sets the robots target to the puck and chases the puck instead
    void OnChasePuck()
    {
        if (GameObject.FindGameObjectWithTag("Distraction") != null)
        {
            Vector3 currGoal = GameObject.FindGameObjectWithTag("Distraction").transform.position;
            navMeshAgent.destination = currGoal;
            FaceTarget(currGoal);
        }

        else
        {
            Debug.Log("puck doesnt exist");
            ExitChase();
            if (checkpointCount == 0)
            {
                StartIdle();
            }
            else
            {
                StartPatrol();
            }
        }




    }


    void OnChase()
    {

        Vector3 currGoal = GameObject.FindGameObjectWithTag("Player").transform.position;
        navMeshAgent.destination = currGoal;
        FaceTarget(currGoal);

        // if player falls out of FOV, go back to patrol/idle duty
        if (!PlayerInFOV())
        {
            ExitChase();
            if (checkpointCount == 0)
            {
                StartIdle();
            }
            else
            {
                StartPatrol();
            }
        }

        // if player gets within the capture range, enter Attack
        if (Vector3.Distance(transform.position, currGoal) <= captureDist)
        {
            ExitChase();
            StartAttack();
        }
    }

    void OnAttack()
    {
        Vector3 currGoal = GameObject.FindGameObjectWithTag("Player").transform.position;
        FPSController controller = GameObject.FindGameObjectWithTag("Player").GetComponent<FPSController>();
        navMeshAgent.destination = currGoal;
        FaceTarget(currGoal);

        timeSinceAttacking += Time.deltaTime;
        if (timeSinceAttacking >= attackCaptureTime)
        {
            controller.FreezePlayer();
            controller.ScreenFadeToDie(timeSinceAttacking - attackCaptureTime);
        }
        if (timeSinceAttacking >= attackCaptureTime + controller.deathTransitionTime)
        {
            controller.RespawnPlayer();
        }

        // if player leaves capture range or no longer in sight, go back to Chase
        if (Vector3.Distance(transform.position, currGoal) > captureDist)
        {
            ExitAttack();
            StartChase();
            controller.UnfreezePlayer();
            controller.ResetDeathScreen();
        }
    }

    void OnDie()
    {

    }

    // transition functions here:

    void StartIdle()
    {
        status = EnemyState.Idle;
        anim.SetBool("Idle", true);
        active = idleSound;

        timeSinceAudioClip = 0;
        source.PlayOneShot(active);
    }

    void ExitIdle()
    {
        anim.SetBool("Idle", false);
    }

    void StartPatrol()
    {
        status = EnemyState.Patrol;
        anim.SetBool("Patrol", true);
        active = walkSound;

        navMeshAgent.speed = 3.5f;

        timeSinceAudioClip = 0;
        source.PlayOneShot(active);
    }

    void ExitPatrol()
    {
        anim.SetBool("Patrol", false);
    }

    void StartChase()
    {
        status = EnemyState.Chase;
        anim.SetBool("Chase", true);
        active = runSound;

        navMeshAgent.speed = 12.5f;

        timeSinceAudioClip = 0;
        source.PlayOneShot(active);
    }

    void ExitChase()
    {
        anim.SetBool("Chase", false);
    }

    void StartAttack()
    {
        status = EnemyState.Attack;
        anim.SetBool("Capture", true);
        active = attackSound;

        timeSinceAudioClip = 0;
        source.PlayOneShot(active);
        source.PlayOneShot(punchSound);
        timeSinceAttacking = 0.0f;
    }

    void ExitAttack()
    {
        anim.SetBool("Capture", false);
        timeSinceAttacking = 0.0f;
    }

    // We are entering helper function territory here, spaghetti code galore

    void SetNextPoint()
    {
        if (checkpointCount == 1)
        {
            return;
        }
        if (currCheckpoint == checkpointCount - 1)
        {
            dir = -1;
        }
        if (currCheckpoint == 0)
        {
            dir = 1;
        }
        currCheckpoint += dir;
        FaceTarget(objectOfTransforms.GetChild(currCheckpoint).position);
    }

    void FaceTarget(Vector3 target)
    {
        Vector3 directionToTarget = target - transform.position;
        directionToTarget.y = 0;
        Quaternion lookRotation = Quaternion.LookRotation(directionToTarget);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, 10 * Time.deltaTime);
    }

    bool PlayerInFOV()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        Vector3 directionToPlayer = player.transform.position - transform.position;
        if (Vector3.Angle(directionToPlayer, transform.forward) <= sightAngle)
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, directionToPlayer, out hit, sightRadius))
            {
                if (hit.collider.CompareTag("Player"))
                {
                    return true;
                }
            }
        }
        return false;
    }
}
