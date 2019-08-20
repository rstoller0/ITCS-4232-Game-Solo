using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Core
{
    public class ActionScheduler : MonoBehaviour
    {
        IAction currentAction;

        public void StartAction(IAction action)
        {
            //if the current action is equal to the new action return (skip the rest of the code)
            if (currentAction == action) return;

            //if current action is not equal to null
            if (currentAction != null)
            {
                //print that the current action is being cancelled
                currentAction.Cancel();
            }
            
            //store new action into current action
            currentAction = action;
        }
    }
}