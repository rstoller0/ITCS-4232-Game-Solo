using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Core
{
    public class CameraFollow : MonoBehaviour
    {
        [SerializeField] Transform target;

        // Update is called once per frame
        void LateUpdate()
        {
            //track the target's position
            transform.position = target.position;
        }
    }
}