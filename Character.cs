using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using UnityEngine;


[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]


public abstract class Character : MonoBehaviour
{
    [SerializeField] private float speed = 1f;


    
    public Animator MyAnimator { get; set; }


    public Vector2 direction;

    private Rigidbody2D myRigidBody;

    

    public bool IsAttacking { get; set; }

    [SerializeField]
    protected Transform hitBox;

    [SerializeField]
    protected Stat health;

    public Transform MyTarget { get; set; }

    public Stat MyHealth
    {
        get { return health; }
    }

    [SerializeField]
    private float initHealth;

    protected Coroutine attackRoutine;

    public bool IsMoving
    {
        get
        {
            return Direction.x != 0 || Direction.y != 0;
        }
    }

    public Vector2 Direction { get => direction; set => direction = value; }
    public float Speed { get => speed; set => speed = value; }

    public bool IsAlive
    {
        get 
        {
            return health.MyCurrentValue > 0;
        }
    }

    protected virtual void Start()
    {
        health.Intitialize(initHealth, initHealth);

        myRigidBody = GetComponent<Rigidbody2D>();
        MyAnimator = GetComponent<Animator>();


       
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        HandleLayers();
    }

    private void FixedUpdate()
    {
        Move();
    }
    public void Move()
    {
        if (IsAlive)
        {
            myRigidBody.velocity = Direction.normalized * Speed;
        }
        

        
    }

    public void HandleLayers()
    {
        if (IsAlive)
        {
            if (IsMoving)
            {

                ActivateLayer("Walk");




                MyAnimator.SetFloat("X", Direction.x);
                MyAnimator.SetFloat("Y", Direction.y);


            }
            else if (IsAttacking)
            {
                ActivateLayer("Attack");
            }
            else
            {
                ActivateLayer("Idle");
            }
        }
        else
        {
            ActivateLayer("Death");
        }
    }
   

    public void ActivateLayer(string layerName)
    {
        for (int i = 0; i < MyAnimator.layerCount; i++)
        {
            MyAnimator.SetLayerWeight(i, 0);
        }

        MyAnimator.SetLayerWeight(MyAnimator.GetLayerIndex(layerName),1);
        

    }

    

    public virtual void TakeDamage(float damage, Transform source)
    {
        

        health.MyCurrentValue -= damage;

        if (health.MyCurrentValue <= 0)
        {
            Direction = Vector2.zero;
            myRigidBody.velocity = Direction;
            MyAnimator.SetTrigger("Die");
        }
    }

}
