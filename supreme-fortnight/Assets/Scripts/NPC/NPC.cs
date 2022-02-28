using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum EnemyState {
    Idle,
    Patrol,
    Chase,
    Attack,
    Death
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

    public AudioClip walkSound;
    public AudioClip runSound;
    public AudioClip attackSound;
    public float timeSinceAudioClip;
    
    public GameObject model;
    public float speed = 1.5f;
    public Transform objectOfTransforms;
    public float stoppingDist = 0.64f;

    public float sightRadius;
    public float sightAngle;

    public float captureDist;
    

    // Start is called before the first frame update
    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        currCheckpoint = 0;
        checkpointCount = objectOfTransforms.childCount;

        anim = model.GetComponent<Animator>();

        // init the patrol state
        StartPatrol();
        dir = 1;

        navMeshAgent.destination = objectOfTransforms.GetChild(currCheckpoint).position;
    }

    // Update is called once per frame
    void Update()
    {
        timeSinceAudioClip += Time.deltaTime;
        if (timeSinceAudioClip > active.length) {
            source.PlayOneShot(active);
            timeSinceAudioClip = 0;
        }

        switch (status)
        {
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
        }
    }

    void OnPatrol() {
        
        Vector3 currGoal = objectOfTransforms.GetChild(currCheckpoint).position;
        if (Vector3.Distance(transform.position, currGoal) <= stoppingDist) {
            SetNextPoint();
        }
        navMeshAgent.destination = objectOfTransforms.GetChild(currCheckpoint).position;

        // if enemy sees player, chase mode activated
        if (PlayerInFOV()) {
            ExitPatrol();
            StartChase();
        }
    }

    void OnChase() {

        Vector3 currGoal = GameObject.FindGameObjectWithTag("Player").transform.position;
        navMeshAgent.destination = currGoal;

        // if player falls out of FOV, go back to patrol duty
        if (!PlayerInFOV()) {
            ExitChase();
            StartPatrol();
        }

        // if player gets within the capture range, enter Attack
        if (Vector3.Distance(transform.position, currGoal) <= captureDist) {
            ExitChase();
            StartAttack();
        }
    }

    void OnAttack() {
        Vector3 currGoal = GameObject.FindGameObjectWithTag("Player").transform.position;
        navMeshAgent.destination = currGoal;
        FaceTarget(currGoal);

        // if player leaves capture range, go back to Chase
        if (Vector3.Distance(transform.position, currGoal) > captureDist) {
            ExitAttack();
            StartChase();
        }
    }

    void OnDie() {

    }

    // transition functions here:


    void StartPatrol() {
        status = EnemyState.Patrol;
        anim.SetBool("Patrol", true);
        active = walkSound;

        navMeshAgent.speed = 3.5f;

        timeSinceAudioClip = 0;
        source.PlayOneShot(active);
    }

    void ExitPatrol() {
        anim.SetBool("Patrol", false);
    }

    void StartChase() {
        status = EnemyState.Chase;
        anim.SetBool("Chase", true);
        active = runSound;

        navMeshAgent.speed = 7.9f;

        timeSinceAudioClip = 0;
        source.PlayOneShot(active);
    }

    void ExitChase() {
        anim.SetBool("Chase", false);
    }

    void StartAttack() {
        status = EnemyState.Attack;
        anim.SetBool("Capture", true);
        active = attackSound;

        timeSinceAudioClip = 0;
        source.PlayOneShot(active);
    }

    void ExitAttack() {
        anim.SetBool("Capture", false);
    }

    // We are entering helper function territory here, spaghetti code galore

    void SetNextPoint() {
        if (currCheckpoint == checkpointCount - 1) {
            dir = -1;
        }
        if (currCheckpoint == 0) {
            dir = 1;
        }
        currCheckpoint += dir;
        FaceTarget(objectOfTransforms.GetChild(currCheckpoint).position);
    }

    void FaceTarget(Vector3 target) {
        Vector3 directionToTarget = target - transform.position;
        directionToTarget.y = 0;
        Quaternion lookRotation = Quaternion.LookRotation(directionToTarget);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, 10 * Time.deltaTime);
    }

    bool PlayerInFOV() {
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
