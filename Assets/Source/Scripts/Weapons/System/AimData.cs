using UnityEngine;

namespace LastTrain.Weapons.System
{
    public struct AimData
    {
        public Ray CamRay;
        public bool HasHit;
        public Vector3 WorldPoint;
    }
}
