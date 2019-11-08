using UnityEngine;

namespace Hirame.Janus
{
    public class WaypointTraveller : MonoBehaviour
    {
        [SerializeField] private WaypointLink lastWaypointLink;

        [Header ("Controls")]
        [SerializeField] private KeyCode returnToLast = KeyCode.R;
        [SerializeField] private KeyCode goToNext = KeyCode.PageUp;
        [SerializeField] private KeyCode goToPrevious = KeyCode.PageDown;

        public void OnWaypointReached (WaypointLink waypointLink)
        {
            Debug.Log (waypointLink);
            lastWaypointLink = waypointLink;
        }

        private void Update ()
        {
            if (lastWaypointLink == false)
                return;

            if (Input.GetKeyDown (returnToLast))
                GoToWaypoint (lastWaypointLink);
            
            if (Input.GetKeyDown (goToNext))
                GoToWaypoint (lastWaypointLink.Next);
            
            if (Input.GetKeyDown (goToPrevious))
                GoToWaypoint (lastWaypointLink.Previous);
        }

        private void GoToWaypoint (WaypointLink waypointLink)
        {
            if (waypointLink == false || waypointLink.IsTargetLoaded == false)
                return;

            if (Waypoint.TryGetWaypointFromLink (waypointLink, out var waypoint))
            {
                lastWaypointLink = waypointLink;
                transform.position = waypoint.Position;
            }
        }
    }

}