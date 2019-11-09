using System.Collections.Generic;
using UnityEngine;

namespace Hirame.Janus
{
    [ExecuteInEditMode]
    public class Waypoint : MonoBehaviour
    {        
        private static Dictionary<WaypointLink, Waypoint> loadedWaypoints = new Dictionary<WaypointLink, Waypoint> ();

        [SerializeField] private WaypointLink waypointLink;
        
        public Vector3 Position => transform.position;

        public static bool TryGetWaypointFromLink (WaypointLink link, out Waypoint waypoint)
        {
            return loadedWaypoints.TryGetValue (link, out waypoint);
        }

        private void Awake ()
        {
            if (!waypointLink) 
                return;
            
            if (loadedWaypoints.ContainsKey (waypointLink))
            {
                waypointLink = null;
            }
            else
            {
                loadedWaypoints.Add (waypointLink, this);
            }
        }

        private void OnDestroy ()
        {
            if (waypointLink)
                loadedWaypoints.Remove (waypointLink);
        }

        private void OnTriggerEnter (Collider other)
        {
            var otherRb = other.attachedRigidbody;
            if (otherRb == false)
                return;

            var traveller = otherRb.GetComponent<WaypointTraveller> ();
            if (traveller == false)
                return;
            
            traveller.OnWaypointReached (waypointLink);
        }

        private void OnValidate ()
        {
            if (waypointLink)
            {
                waypointLink.SetTargetWaypoint (this);
            }
        }
    }

}
