using RPG.Movement;
using RPG.Combat;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace RPG.Control
{
    public class PlayerController : MonoBehaviour
    {

        // Update is called once per frame
        void Update()
        {
            //if we are interacting with combat skip the rest of the code
            if (InteractWithCombat()) return;
            //else if we are interacting with movement skip the rest of the code
            if (InteractWithMovement()) return;
            //else nothing to do (print) [change later]
            print("Nothing to do.");
        }

        private bool InteractWithCombat()
        {
            //collect all hits along raycast
            RaycastHit[] hits = Physics.RaycastAll(GetMouseRay());

            //iterate through all hits from raycast
            foreach (RaycastHit hit in hits)
            {
                CombatTarget target = hit.transform.GetComponent<CombatTarget>();

                //if target is equal to null skip the rest of the code and go on to the next hit
                if (target == null) continue;

                //else taget != null
                //if left mouse buttin is clicked
                if (Input.GetMouseButtonDown(0))
                {
                    //trigger attack function with correct target parameter
                    GetComponent<Fighter>().Attack(target);
                }

                //return true that we are interacting with combat
                return true;
            }

            //return false that we are not interacting with combat
            return false;
        }

        private bool InteractWithMovement()
        {
            //variable for raycast hit
            RaycastHit hit;

            //variable for if there is a hit or not
            bool hasHit = Physics.Raycast(GetMouseRay(), out hit);

            //if there is a hit
            if (hasHit)
            {
                //if left mouse button is clicked/held down
                if (Input.GetMouseButton(0))
                {
                    //trigger start move action function
                    GetComponent<Mover>().StartMoveAction(hit.point);
                }

                //return true that we are interacting with movement (can move)
                return true;
            }

            //return false that we are not interacting with movement (cannot move)
            return false;
        }

        private static Ray GetMouseRay()
        {
            //return a ray from screen to mouse position in world
            return Camera.main.ScreenPointToRay(Input.mousePosition);
        }
    }

}