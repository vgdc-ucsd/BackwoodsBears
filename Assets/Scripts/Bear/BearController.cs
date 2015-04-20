using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/*
 * A class used to derive all enemy types from. 
 */
public class BearController : MonoBehaviour
{
    public enum BearState
    {
        Patroling,
        Alerted,
        Cautious,
        HeardPlayer,
        Dead
    };

    public const BearState
        PATROLING = BearState.Patroling,
        ALERTED = BearState.Alerted,
        CAUTIOUS = BearState.Cautious,
        HEARDPLAYER = BearState.HeardPlayer,
        DEAD = BearState.Dead;

    public GameObject Player;
    //public BoxCollider2D AttackHitBox;
    public GameObject BearModel;
    public PatrolDefinition PatrolPath;
    public float Speed = 2.0f;
    public float ChaseSpeed = 2.5f;
    public float AttackRange = 5.5f;
    public float MaxDistanceToPoint = 0.7f;

    public bool IsFacingRight { get; protected set; }

    public BearState CurrentState; // { get; protected set; }

    //protected Animator _animator;
    protected IEnumerator<Transform> _currentPatrolPoint;

    public void Awake()
    {
        //_animator = GetComponent<Animator>();

        CurrentState = PATROLING;
        IsFacingRight = transform.localScale.z > 0;
    }

    public void Start()
    {
        if (PatrolPath == null)
        {
            Debug.LogError("Patrol Path cannot be null", gameObject);
            return;
        }

        _currentPatrolPoint = PatrolPath.GetPathsEnumerator();
        _currentPatrolPoint.MoveNext();

        if (_currentPatrolPoint.Current == null)
            return;

        transform.position = _currentPatrolPoint.Current.position;
    }

    public void Update()
    {
        if (CurrentState == DEAD ||_currentPatrolPoint == null || _currentPatrolPoint.Current == null)
            return;

        switch (CurrentState)
        { 
            case PATROLING:
                Patrol();
                break;
            case ALERTED:
                Debug.Log("I AM ALERTED!");
                if (Mathf.Abs(Player.transform.position.x - transform.position.x) > AttackRange)
                    Chase();
                /*
                 * else if (player.InTree == null)
                 *   AttackPlayer();
                 */
                else
                    AttackTree();
                break;
            case CAUTIOUS:
                Debug.Log("I'm cautious now.");
                StartCoroutine(CautiousCo());
                break;
            case HEARDPLAYER:
                Debug.Log("I HEARD YOU!");
                break;
        }
    }

    public void Kill()
    {
        CurrentState = DEAD;

        gameObject.SetActive(false);
    }

    public void BecomeAlerted()
    {
        CurrentState = ALERTED;
    }

    public void BecomeCautious()
    {
        if (CurrentState == ALERTED)
            CurrentState = CAUTIOUS;
    }

    public void HeardPlayer(float playerX)
    {
        if (CurrentState != HEARDPLAYER && CurrentState != ALERTED)
        {
            CurrentState = HEARDPLAYER;

            // Turn towards the direction of the sound.
            if ((playerX > transform.position.x && !IsFacingRight) || (playerX < transform.position.x && IsFacingRight))
                Flip();

            // Return CurrentState back to patrolling after 3 seconds.
            StartCoroutine(HeardPlayerCo());
        }
    }

    public void Flip()
    { 
        transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, -transform.localScale.z);
        IsFacingRight = transform.localScale.z > 0;
    }

    protected IEnumerator HeardPlayerCo()
    {
        yield return new WaitForSeconds(3.0f);

        if (CurrentState != ALERTED)
            CurrentState = PATROLING;
    }

    protected IEnumerator CautiousCo()
    {
        yield return new WaitForSeconds(3.0f);

        if (CurrentState != ALERTED)
        {
            CurrentState = PATROLING;
        }
    }

    protected void Patrol()
    {
        transform.position = Vector3.MoveTowards(transform.position, _currentPatrolPoint.Current.position, Time.deltaTime * Speed);

        if ((_currentPatrolPoint.Current.position.x > transform.position.x && !IsFacingRight) || (_currentPatrolPoint.Current.position.x < transform.position.x && IsFacingRight))
            Flip();

        if (Mathf.Abs(transform.position.x - _currentPatrolPoint.Current.position.x) < MaxDistanceToPoint)
            _currentPatrolPoint.MoveNext();
    }

    protected void Chase()
    { 
        if ((Player.transform.position.x > transform.position.x && !IsFacingRight) || (Player.transform.position.x < transform.position.x && IsFacingRight))
            Flip();

        transform.position = new Vector3(Mathf.Lerp(transform.position.x, Player.transform.position.x, ChaseSpeed * Time.deltaTime), transform.position.y, transform.position.z);
    }

    // Regular bear will attack. Laser bear will attack with laser.
    virtual protected void AttackPlayer()
    {
        Debug.Log("I try to hit you.");
        
        // regular: _animator.Play("Swipe");
        // laser: _animator.Play("Laser");

        // Set the attack hitbox on and off in the animation curve.
    }

    // Regular bear will shake the tree. Laser bear will vaporize the tree.
    virtual protected void AttackTree()
    {
        Debug.Log("I go to tree.");
        // var bearX= transform.position.x;
        // var treeX = Player.Intree.transform.position.x
        // if(Mathf.Abs(bearX - treeX) > MaxDistanceToPoint
        //  transform.position = new Vector3(Mathf.Lerp(bearX, treeX, ChaseSpeed * Time.deltaTime), transform.position.y, transform.position.z);
        // else
        //  regular: _animator.Play("Shake");
        //  laser: _animator.Play("Laser");

        // Set the attack hitbox on and off in the animation curve.
    }
}