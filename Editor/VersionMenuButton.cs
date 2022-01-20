using UnityEngine;
using UnityEditor;

namespace U.Universal.Scenes.Editor
{
    public class VersionMenuButton : EditorWindow
    {

        [MenuItem("Universal/Scenes/Version")]
        public static void PrintVersion()
        {

            Debug.Log(" U Framework: Universal Scenes v1.0.0 for Unity");

        }
    }
}