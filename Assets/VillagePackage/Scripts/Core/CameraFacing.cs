using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Core
{
    public class CameraFacing : MonoBehaviour
    {
        // Update is called once per frame
        void LateUpdate()
        {
            transform.LookAt(transform.position + 
                Camera.main.transform.rotation * Vector3.forward,
                Camera.main.transform.rotation * Vector3.up);
           // transform.forward= Camera.main.transform.forward;
        }
    }
}