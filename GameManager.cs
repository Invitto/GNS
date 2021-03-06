﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GameManager : MonoBehaviour
{


    [SerializeField]
    private Player player;

    private NPC currentTarget;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        ClickTarget();
    }

    private void ClickTarget()
    {
        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())//If we click the left mouse button
        {
            //Makes a raycast from the mouse position into the game world
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, Mathf.Infinity, 512);

            if (hit.collider != null)//If we hit something
            {
                if (currentTarget != null)//If we have a current target
                {
                    currentTarget.DeSelect(); //deselct the current target
                }

                currentTarget = hit.collider.GetComponent<NPC>(); //Selects the new target

                player.MyTarget = currentTarget.Select(); //Gives the player the new target

                UIManager.MyInstance.ShowTargetFrame(currentTarget);
            }
            else//Deselect the target
            {
                UIManager.MyInstance.HideTargetFrame();

                if (currentTarget != null) //If we have a current target
                {
                    currentTarget.DeSelect(); //We deselct it
                }

                //We remove the references to the target
                currentTarget = null;
                player.MyTarget = null;
            }
        }

    }
}

