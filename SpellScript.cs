using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellScript : MonoBehaviour
{
    private Rigidbody2D myRigidBody;

    [SerializeField] float speed;

    //if i want to select target 
    public Transform MyTarget { get; private set; }

    private Transform source;
    private int damage;

   
    

    
    void Start()
    {
        myRigidBody = GetComponent<Rigidbody2D>();
       
    }

    public void Initialize(Transform target, int damage, Transform source)
    {
        this.MyTarget = target;
        this.damage = damage;
        this.source = source;
    }

    
    // Update is called once per frame
    void Update()
    {
        
    }

    //if i want to follow selected target
    private void FixedUpdate()
    {

        if (MyTarget !=null)
        {
            Vector2 direction = MyTarget.position - transform.position;

            myRigidBody.velocity = direction.normalized * speed;

            //Calculates the rotation angle
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            //Rotates the spell towards the target
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "HitBox" && collision.transform == MyTarget)
        {
            Character c = collision.GetComponentInParent<Character>();
            speed = 0;
            c.TakeDamage(damage, source);
            GetComponent<Animator>().SetTrigger("impact");
            myRigidBody.velocity = Vector2.zero;
            MyTarget = null;
        }
    }
}

