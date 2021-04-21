using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;


public class SpookScript : EnemyBase
{
    private Weapon weapon;
    private Transform firePoint;
    private float timeElapsed = 0;

    [SerializeField] private float timeBetweenShoots = 2;
    
    protected override void Start()
    {
        weapon = this.GetComponent<Weapon>();
        rb = this.GetComponent<Rigidbody2D>();
        firePoint = gameObject.transform.Find("FirePoint");
        moveSpeedPatrolling = 5f;
        moveSpeedChasing = 7f;

        chasingRange = 50f;
        attackingRange = 30f;
        currentState = enemyState.Patrolling;

        health = 100f;

        CreatePatrolArea();
        SetTarget();
        GetNextPatrolPosition();
    }

    [Server]
    protected override void Update()
    {
        CheckDistanceToTarget();

        switch (currentState)
        {
            case enemyState.Patrolling:
                UpdateRotationPatrolling();
                UpdateMovementPatrolling();
                base.CheckForFlip();
                break;
            case enemyState.Chasing:
                UpdateRotationChasing();
                UpdateMovementChasing();
                base.CheckForFlip();
                break;
            case enemyState.Attacking:
                UpdateRotationAttacking();
                UpdateFirePointRotation();
                timeElapsed += Time.deltaTime;
                if (timeElapsed >= timeBetweenShoots)
                {
                    timeElapsed -= timeBetweenShoots;
                    weapon.shoot();
                }
                break;
        }


    }

    protected void UpdateMovementChasing()
    {
        Vector2 direction = (targetGameObject.transform.position - transform.position).normalized;
        Vector2 movement = direction * movementSpeed * Time.deltaTime;
        rb.AddForce(movement);
    }
    protected void UpdateRotationChasing()
    {
        Vector2 direction = targetGameObject.transform.position - transform.position;
        float angle = Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.AngleAxis(-angle, Vector3.forward);
        transform.rotation = (Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * 5f));

    }

    protected void UpdateRotationPatrolling()
    {
        //Vector till target
        Vector2 direction = currentPatrolTarget - transform.position;
        //vinkel till target
        float angle = Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg;
        //rotationen som kr�vs till target som en quaternion runt z axlen
        Quaternion rotation = Quaternion.AngleAxis(-angle, Vector3.forward);
        //Mindre del av rotationen till target (slerp)
        transform.rotation = (Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * 5f));
    }


    protected void UpdateMovementPatrolling()
    {
        Vector3 direction = (currentPatrolTarget - transform.position);
        rb.MovePosition((Vector3)transform.position + (direction * moveSpeedPatrolling * Time.deltaTime));

        if (Vector3.Distance(currentPatrolTarget, transform.position) < 1f)
        {
            base.GetNextPatrolPosition();
        }
    }

    protected void UpdateRotationAttacking()
    {
        //Utr�ckning av direction �r omv�nd f�r att fisken ska rotera 180 grader s� att baksidan �r roterad mot submarine;
        Vector2 direction = transform.position - targetGameObject.transform.position;
        float angle = Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.AngleAxis(-angle, Vector3.forward);
        transform.rotation = (Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * 5f));
    }

    protected void UpdateFirePointRotation()
    {
        Vector2 direction = targetGameObject.transform.position - firePoint.transform.position;
        float angle = Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.AngleAxis(-angle, Vector3.forward);
        firePoint.rotation = (Quaternion.Slerp(firePoint.transform.rotation, rotation, Time.deltaTime * 5f));
    }
   
    private void CheckDistanceToTarget()
    {
        float distance = Vector3.Distance(targetGameObject.transform.position, transform.position);
        if (distance < attackingRange) currentState = enemyState.Attacking;
        else if (distance < chasingRange) currentState = enemyState.Chasing;
        else currentState = enemyState.Patrolling;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "AllyBullet")
        {
             health -= collision.gameObject.GetComponent<Bullet>().Damage;
        } 
        base.CheckIfAlive();
    }
}
