using System.Collections;
using UnityEngine;

namespace RTS
{
    public static class ResourceManager
    {
        public static float ScrollSpeed { get { return 50; } }
        public static int ScrollWidth { get { return 30; } }
        public static float MinCameraHeight { get { return 50; } }
        public static float MaxCameraHeight { get { return 500; } }

        private static Vector3 invalidPosition = new Vector3(-99999, -99999, -99999);
        public static Vector3 InvalidPosition { get { return invalidPosition; } }
    }

}
