﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : NPC
{
    [SerializeField]
    private CanvasGroup healthGroup;

    

    private IState currentState;

    public float MyAttackRange { get; set; }

    public float MyAttackTime { get; set; }

    public Vector3 MyStartPostion;

    [SerializeField]
    private float initAggroRange;

    public float MyAggroRange { get; set; }

    public bool InRange
    {
        get
        {
            return Vector2.Distance(transform.position, MyTarget.position) < (MyAggroRange);
        }
    }

    protected void Awake()
    {
        MyStartPostion = transform.position;
        MyAggroRange = initAggroRange;
        MyAttackRange =5;
        ChangeState(new IdleState());
    }

    protected override void Update()
    {
        if (IsAlive)
        {
            if (!IsAttacking)
            {
                MyAttackTime += Time.deltaTime;
            }
            currentState.Update();

        }
        base.Update();
    }

    /// <summary>
    /// When the enemy is selected
    /// </summary>
    /// <returns></returns>
    public override Transform Select()
    {
        //Shows the health bar
        healthGroup.alpha = 1;

        return base.Select();
    }

    /// <summary>
    /// When we deselect our enemy
    /// </summary>
    public override void DeSelect()
    {
        //Hides the healthbar
        healthGroup.alpha = 0;

        base.DeSelect();
    }

    public override void TakeDamage(float damage, Transform source)
    {
        if (!(currentState is EvadeState))
        {

            SetTarget(source);


            base.TakeDamage(damage, source);

            OnHealthChanged(health.MyCurrentValue);
        }
    }

 

    public void ChangeState(IState newState)
    {
        if (currentState != null)
        {
            currentState.Exit();
        }

        currentState = newState;
        currentState.Enter(this);
    }

    public void SetTarget(Transform target)
    {
        if (!(MyTarget == null && currentState is EvadeState))
        {
            float distance = Vector2.Distance(transform.position, target.position);
            MyAggroRange += distance;
            MyTarget = target;
        }
    }

    public void Reset()
    {
        this.MyTarget = null;
        this.MyAggroRange = initAggroRange;
        this.MyHealth.MyCurrentValue = this.MyHealth.MyMaxValue;
        OnHealthChanged(health.MyCurrentValue);

    }
}
