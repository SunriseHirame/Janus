﻿using Hirame.Pantheon;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Hirame.Janus
{
    [CreateAssetMenu (menuName = "Hirame/Janus/Waypoint Link")]
    public class WaypointLink : ScriptableObject
    {
        [Header ("Progression")]
        [SerializeField] private WaypointLink previous;
        [SerializeField] private WaypointLink next;

        [Header ("Link Info")]
        [ReadOnly]
        [SerializeField] private Suid linkedTriggerId;
        
        [ReadOnly]
        [SerializeField] private string containingSceneGuid;

        public bool IsTargetLoaded => true;
        
        public WaypointLink Next => next;
        public WaypointLink Previous => previous;

        public bool TryGetWaypointPosition (out Vector3 position)
        {
            var found = Waypoint.TryGetWaypointFromLink (this, out var waypoint);
            position = waypoint ? waypoint.Position : Vector3.zero;
            
            return found;
        }

        internal bool PushTrigger (Waypoint trigger)
        {
            if (trigger == false)
            {
                linkedTriggerId = new Suid ();
                containingSceneGuid = string.Empty;
            }
            else
            {
                linkedTriggerId = trigger.Id;

                var scenePath = trigger.gameObject.scene.path;
                var sceneGuid = AssetDatabase.AssetPathToGUID (scenePath);
                containingSceneGuid = sceneGuid;
            }
            
            return !string.IsNullOrEmpty (containingSceneGuid);
        }
    }

}
