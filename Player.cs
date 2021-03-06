﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This is the player script, it contains functionality that is specific to the Player
/// </summary>
public class Player : Character
{
    private static Player instance;

    public static Player MyInstance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<Player>();
            }

            return instance;
        }
    }

    /// <summary>
    /// The player's mana
    /// </summary>
    [SerializeField]
    private Stat mana;

    /// <summary>
    /// The player's initial mana
    /// </summary>
    private float initMana = 50;

    /// <summary>
    /// An array of blocks used for blocking the player's sight
    /// </summary>
    [SerializeField]
    private Block[] blocks;

    /// <summary>
    /// Exit points for the spells
    /// </summary>
    [SerializeField]
    private Transform[] exitPoints;

    /// <summary>
    /// Index that keeps track of which exit point to use, 2 is default down
    /// </summary>
    private int exitIndex = 2;

    private Vector3 min, max;

    protected override void Start()
    {
        mana.Intitialize(initMana, initMana);

        base.Start();
    }

    /// <summary>
    /// We are overriding the characters update function, so that we can execute our own functions
    /// </summary>
    protected override void Update()
    {
        //Executes the GetInput function
        GetInput();

        //Clamps the player inside the tilemap
        
        base.Update();
    }

    /// <summary>
    /// Listen's to the players input
    /// </summary>
    private void GetInput()
    {
        Direction = Vector2.zero;

        ///THIS IS USED FOR DEBUGGING ONLY
        if (Input.GetKeyDown(KeyCode.I))
        {
            health.MyCurrentValue -= 10;
            mana.MyCurrentValue -= 10;
        }
        if (Input.GetKeyDown(KeyCode.O))
        {
            health.MyCurrentValue += 10;
            mana.MyCurrentValue += 10;
        }

        if (Input.GetKey(KeybindManager.MyInstance.Keybinds["UP"])) //Moves up
        {
            exitIndex = 0;
            Direction += Vector2.up;
        }
        if (Input.GetKey(KeybindManager.MyInstance.Keybinds["LEFT"])) //Moves left
        {
            exitIndex = 3;
            Direction += Vector2.left;
        }
        if (Input.GetKey(KeybindManager.MyInstance.Keybinds["DOWN"]))
        {
            exitIndex = 2;
            Direction += Vector2.down;
        }
        if (Input.GetKey(KeybindManager.MyInstance.Keybinds["RIGHT"])) //Moves right
        {
            exitIndex = 1;
            Direction += Vector2.right;
        }
        if (IsMoving)
        {
            StopAttack();
        }

        foreach (string action in KeybindManager.MyInstance.ActionBinds.Keys)
        {
            if (Input.GetKeyDown(KeybindManager.MyInstance.ActionBinds[action]))
            {
                UIManager.MyInstance.ClickActionButton(action);

            }
        }


    }

    /// <summary>
    /// Set's the player's limits so that he can't leave the game world
    /// </summary>
    /// <param name="min">The minimum position of the player</param>
    /// <param name="max">The maximum postion of the player</param>
    public void SetLimits(Vector3 min, Vector3 max)
    {
        this.min = min;
        this.max = max;
    }

    /// <summary>
    /// A co routine for attacking
    /// </summary>
    /// <returns></returns>
    private IEnumerator Attack(string spellName)
    {
        Transform currentTarget = MyTarget;

        //Creates a new spell, so that we can use the information form it to cast it in the game
        Spell newSpell = SpellBooks.MyInstance.CastSpell(spellName);

        IsAttacking = true; //Indicates if we are attacking

        MyAnimator.SetBool("Attack", IsAttacking); //Starts the attack animation

        yield return new WaitForSeconds(newSpell.MyCastTime); //This is a hardcoded cast time, for debugging

        if (currentTarget != null && InLineOfSight())
        {
            SpellScript s = Instantiate(newSpell.MySpellPrefab, exitPoints[exitIndex].position, Quaternion.identity).GetComponent<SpellScript>();

            s.Initialize(currentTarget, newSpell.MyDamage, transform);
        }

        StopAttack(); //Ends the attack
    }

    /// <summary>
    /// Casts a spell
    /// </summary>
    public void CastSpell(string spellName)
    {
        Block();

        if (MyTarget != null && MyTarget.GetComponentInParent<Character>().IsAlive && !IsAttacking && !IsMoving && InLineOfSight()) //Chcks if we are able to attack
        {
            attackRoutine = StartCoroutine(Attack(spellName));
        }
    }

    /// <summary>
    /// Checks if the target is in line of sight
    /// </summary>
    /// <returns></returns>
    private bool InLineOfSight()
    {
        if (MyTarget != null)
        {
            //Calculates the target's direction
            Vector3 targetDirection = (MyTarget.transform.position - transform.position).normalized;

            //Thorws a raycast in the direction of the target
            RaycastHit2D hit = Physics2D.Raycast(transform.position, targetDirection, Vector2.Distance(transform.position, MyTarget.transform.position), 256);

            //If we didn't hit the block, then we can cast a spell
            if (hit.collider == null)
            {
                return true;
            }

        }

        //If we hit the block we can't cast a spell
        return false;
    }

    /// <summary>
    /// Changes the blocks based on the players direction
    /// </summary>
    private void Block()
    {
        foreach (Block b in blocks)
        {
            b.Deactivate();
        }

        blocks[exitIndex].Activate();
    }

    /// <summary>
    /// Stops the attack
    /// </summary>
    public void StopAttack()
    {
        //Stop the spellbook from casting

        IsAttacking = false; //Makes sure that we are not attacking

        MyAnimator.SetBool("Attack", IsAttacking); //Stops the attack animation

        if (attackRoutine != null) //Checks if we have a reference to an co routine
        {
            StopCoroutine(attackRoutine);
        }
    }
}
