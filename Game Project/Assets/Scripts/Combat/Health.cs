using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Combat
{
    public class Health : MonoBehaviour
    {
        [SerializeField] float healthPoints = 100f;
        bool isDead = false;

        public void TakeDamage(float damage)
        {
            //make health the higher number between health - damage AND 0 (i.e. if health - damage is less than 0, than health = 0)
            healthPoints = Mathf.Max(healthPoints - damage, 0);

            //if the character is not dead and their health is 0
            if (!isDead && healthPoints == 0)
            {
                Die();
            }
        }

        private void Die()
        {
            //if is dead already, skip the rest of the code
            if (isDead) return;

            //else not dead yet, set die trigger in animator
            GetComponent<Animator>().SetTrigger("die");

            //and set is dead to true
            isDead = true;
        }
    }
}