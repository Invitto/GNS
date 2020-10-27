using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : IState
{

    private Enemy parent;

    private float attackCooldown = 3;

    private float extraRange = .1f;
    public void Enter(Enemy parent)
    {
        this.parent = parent;
    }

    public void Exit()
    {
       
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void IState.Update()
    {

        if (parent.MyAttackTime >= attackCooldown && !parent.IsAttacking)
        {
            parent.MyAttackTime = 0;
            parent.StartCoroutine(Attack());
        }
        if (parent.MyTarget !=null)
        {
            float distance = Vector2.Distance(parent.MyTarget.position, parent.transform.position);

            if (distance >= parent.MyAttackRange + extraRange && !parent.IsAttacking)
            {
                parent.ChangeState(new FollowState());
            }

        }
        else
        {
            parent.ChangeState(new IdleState());
        }
    }

    public IEnumerator Attack()
    {
        parent.IsAttacking = true;

        parent.MyAnimator.SetTrigger("Attack");

        yield return new WaitForSeconds(parent.MyAnimator.GetCurrentAnimatorStateInfo(2).length);

        parent.IsAttacking = false;
    }



}
