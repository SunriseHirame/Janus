using System;
using System.Collections.Generic;
using Hirame.Pantheon;
using UnityEngine;

namespace Hirame.Janus
{
    public class Waypoint : MonoBehaviour
    {        
        private static Dictionary<WaypointLink, Waypoint> waypoints = new Dictionary<WaypointLink, Waypoint> ();

        [SerializeField] private WaypointLink waypointLink;
        
        [ReadOnly]
        [SerializeField] private Suid triggerId;

        public Vector3 Position => transform.position;

        internal ref readonly Suid Id
        {
            get
            {
                if (!triggerId.IsValid ())
                    triggerId = Suid.CreateNew ();

                return ref triggerId;
            }
        }

        public static bool TryGetWaypointFromLink (WaypointLink link, out Waypoint waypoint)
        {
            return waypoints.TryGetValue (link, out waypoint);
        }

        private void Awake ()
        {
            if (waypointLink)
                waypoints.Add (waypointLink, this);
        }

        private void OnDestroy ()
        {
            if (waypointLink)
                waypoints.Remove (waypointLink);
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
                waypointLink.PushTrigger (this);
            }
        }
    }

}
