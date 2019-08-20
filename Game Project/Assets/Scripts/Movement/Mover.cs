using RPG.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace RPG.Movement
{
    public class Mover : MonoBehaviour, IAction
    {
        [SerializeField] Transform target;

        NavMeshAgent navMeshAgent;

        private void Start()
        {
            //get the nav mesh agent at start and store it in the variable
            navMeshAgent = GetComponent<NavMeshAgent>();
        }

        // Update is called once per frame
        void Update()
        {
            //always update animator variable
            UpdateAnimator();
        }

        public void StartMoveAction(Vector3 destination)
        {
            //call action scheduler for cancelling action if neccessary
            GetComponent<ActionScheduler>().StartAction(this);
            //call move to function (this is from a movement only action [No Combat])
            MoveTo(destination);
        }

        public void MoveTo(Vector3 destination)
        {
            //set destiantion for the NavMeshAgent (location to move to)
            navMeshAgent.destination = destination;
            //allow the NavMeshAgent to move
            navMeshAgent.isStopped = false;
        }

        public void Cancel()
        {
            //stop the NavMeshAgent
            navMeshAgent.isStopped = true;
        }

        private void UpdateAnimator()
        {
            //set velocity to global velocity
            Vector3 velocity = navMeshAgent.velocity;
            //set localVelocity to local velocity from the global velocity
            Vector3 localVelocity = transform.InverseTransformDirection(velocity);
            //set speed to the forward movement of local velocity
            float speed = localVelocity.z;
            //set animator's variable to current speed
            GetComponent<Animator>().SetFloat("forwardSpeed", speed);
        }
    }
}