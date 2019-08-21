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

        Health target;
        float timeSinceLastAttack = 0;

        private void Update()
        {
            //always increment time since last attack
            timeSinceLastAttack += Time.deltaTime;

            //if target equals null return (skip the rest of the code)
            if (target == null) { return; }

            //if target is dead return (skip the rest of the code) [no longer targetable]
            if (target.IsDead()) { return; }

            //else there is a target AND if fighter is not within weapon range
            if (!GetIsInRange())
            {
                //move to the target
                GetComponent<Mover>().MoveTo(target.transform.position);
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
            //make sure fighter is looking at target
            transform.LookAt(target.transform);

            //if the the time since last attack is greater than the required time between attacks
            if (timeSinceLastAttack > timeBetweenAttacks)
            {
                //in this function we will trigger the Hit() event
                TriggerAttack();

                //reset time since last attack to 0
                timeSinceLastAttack = 0;
            }
        }

        private void TriggerAttack()
        {
            //before we set the trigger for attack we want to make sure that the stopAttack trigger is reset (not triggered)
            GetComponent<Animator>().ResetTrigger("stopAttack");

            //set the attack trigger in the animator (play the attack animation)
            //THIS WILL TRIGGER THE Hit() EVENT
            GetComponent<Animator>().SetTrigger("attack");
        }

        //Animation Event
        private void Hit()
        {
            //if the target equals null (skip the rest of the code)
            if (target == null) { return; }

            //when the animation triggers hit event, inflict damage to target
            target.TakeDamage(weaponDamage);
        }

        private bool GetIsInRange()
        {
            //return if fighter is within their weapon range of the target
            return Vector3.Distance(transform.position, target.transform.position) < weaponRange;
        }

        //function to check if the enemy that was hit by raycast is targetable
        public bool CanAttack(CombatTarget combatTarget)
        {
            //if the combatTarget passed in is null then return false (we cannot attack a null object)
            if (combatTarget == null) { return false; }

            //get Health component from the passed combatTarget
            Health targetToTest = combatTarget.GetComponent<Health>();

            //return true if target is not null AND target is not dead
            return targetToTest != null && !targetToTest.IsDead();
        }

        public void Attack(CombatTarget combatTarget)
        {
            //call action scheduler for cancelling action if neccessary
            GetComponent<ActionScheduler>().StartAction(this);
            //store the correct combat target into target variable
            target = combatTarget.GetComponent<Health>();
        }

        public void Cancel()
        {
            //trigger the stopAttack variable in the animator with this function
            TriggerStopAttack();
            //set target equal to null (not in combat)
            target = null;
        }

        private void TriggerStopAttack()
        {
            //before we set the trigger for stopAttack we want to make sure that the attack trigger is reset (not triggered)
            GetComponent<Animator>().ResetTrigger("attack");
            //set animator trigger variable for stopping an attack
            GetComponent<Animator>().SetTrigger("stopAttack");
        }
    }
}