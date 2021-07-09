using System;
using UnityEngine;

namespace Local.Util
{    public class RayCaster {

        // public Vector3 Origin;
        // public Vector3 Source;
        // public Vector3 Direction;
        public float RayLength = Mathf.Infinity;
        public int LayerMask = 0;
    
        public event Action<Collider> OnRayEnter;
        public event Action<Collider> OnRayStay;
        public event Action<Collider> OnRayExit;

        Collider previous;
        RaycastHit hit = new RaycastHit();

        public bool CastRay(Vector3 Origin, Vector3 Direction) {
            // Debug.Log("casting ray");
            if(Physics.Raycast(Origin, Direction, out hit, RayLength)){
                Debug.Log(hit.collider.gameObject.name);
            };
            ProcessCollision(hit.collider);
            return hit.collider != null ? true : false;
        }

        public bool CastLine(Vector3 Origin, Vector3 Target) {
            Physics.Linecast(Origin,Target, out hit, LayerMask);
            ProcessCollision(hit.collider);
            return hit.collider != null ? true : false;
        }

        private void ProcessCollision(Collider current) {
            // No collision this frame.
            if (current == null) {
                // But there was an object hit last frame.
                if (previous != null) {
                    DoEvent(OnRayExit, previous);
                }
            }

            // The object is the same as last frame.
            else if (previous == current) {
                DoEvent(OnRayStay, current);
            }

            // The object is different than last frame.
            else if (previous != null) {
                DoEvent(OnRayExit, previous);
                DoEvent(OnRayEnter, current);
            }

            // There was no object hit last frame.
            else {
                DoEvent(OnRayEnter, current);
            }

            // Remember this object for comparing with next frame.
            previous = current;
        }


        private void DoEvent(Action<Collider> action, Collider collider) {
            if (action != null) {
                action(collider);
            }
        }


    }
}