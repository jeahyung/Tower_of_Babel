using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace ParticleEffect.Scripts
{
    public class RotateToMouse : MonoBehaviour
    {
        public Camera cam;
    
        public float maximumLeght;
    
        public Ray rayMouse;
    
        public Vector3 pos;
        public Vector3 direction;
        public Quaternion rotation;
    
        // Update is called once per frame
        void Update()
        {
            if (cam != null)
            {
                RaycastHit hit;
                var mousePos = Input.mousePosition;
                rayMouse = cam.ScreenPointToRay(mousePos);
                if (Physics.Raycast(rayMouse.origin, rayMouse.direction, out hit, maximumLeght))
                {
                    RotateToMouseDirection(gameObject, hit.point);
                }
                else
                {
                    var pos = rayMouse.GetPoint(maximumLeght);
                    RotateToMouseDirection(gameObject, pos);
                }
            }        
        }
    
        private void RotateToMouseDirection(GameObject obj, Vector3 destination)
        {
            direction = destination - obj.transform.position;
            rotation = Quaternion.LookRotation(direction);
            obj.transform.rotation = Quaternion.Lerp(obj.transform.rotation, rotation, 1);
        }
    }

}
