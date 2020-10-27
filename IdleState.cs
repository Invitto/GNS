using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class IdleState : IState
{

    private Enemy parent;
    public void Enter(Enemy parent)
    {
        this.parent = parent;
        this.parent.Reset();
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
        if (parent.MyTarget != null)
        {
            parent.ChangeState(new FollowState());


        }
    }
}
