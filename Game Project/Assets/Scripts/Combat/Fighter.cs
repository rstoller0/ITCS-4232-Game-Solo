using RPG.Core;
using RPG.Movement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Combat
{
    public class Fighter : MonoBehaviour, IAction
    {
        [SerializeField] float weaponRange = 2f;
        [SerializeField] float weaponDamage = 5f;
        [SerializeField] float timeBetweenAttacks = 1.25f;

        Transform target;
        float timeSinceLastAttack = 0;

        private void Update()
        {
            //always increment time since last attack
            timeSinceLastAttack += Time.deltaTime;

            //if target equals null return (skip the rest of the code)
            if (target == null) return;

            //else there is a target AND if fighter is not within weapon range
            if (!GetIsInRange())
            {
                //move to the target
                GetComponent<Mover>().MoveTo(target.position);
            }
            else
            {
                //else the fighter is within weapon range
                //so stop the fighter
                GetComponent<Mover>().Cancel();

                //trigger attack animation
                AttackBehaviour();
            }
        }

        private void AttackBehaviour()
        {
            //if the the time since last attack is greater than the required time between attacks
            if (timeSinceLastAttack > timeBetweenAttacks)
            {
                //set the attack trigger in the animator (play the attack animation)
                //THIS WILL TRIGGER THE Hit() EVENT
                GetComponent<Animator>().SetTrigger("attack");

                //reset time since last attack to 0
                timeSinceLastAttack = 0;
            }
        }

        //Animation Event
        private void Hit()
        {
            Health healthComponent = target.GetComponent<Health>();
            healthComponent.TakeDamage(weaponDamage);
        }

        private bool GetIsInRange()
        {
            //return if fighter is within their weapon range of the target
            return Vector3.Distance(transform.position, target.position) < weaponRange;
        }

        public void Attack(CombatTarget combatTarget)
        {
            //call action scheduler for cancelling action if neccessary
            GetComponent<ActionScheduler>().StartAction(this);
            //store the correct combat target into target variable
            target = combatTarget.transform;
        }

        public void Cancel()
        {
            //set target equal to null (not in combat)
            target = null;
        }
    }
}