
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace U.Universal.Scenes
{
    public sealed partial class SceneMonitor
    {

        // <Host>
        private static GameObject hostInstance; // Go to host all coroutines used here
        private static GameObject _host
        {
            get
            {
                if (hostInstance == null)
                {
                    hostInstance = new GameObject("SceneMonitor-Host");
                    UnityEngine.Object.DontDestroyOnLoad(hostInstance);
                }

                return hostInstance;

            }
        } // Go to host all coroutines used here

        // </Host>


        // Trsnsition scene path
        public static string TransitionScenePath => "Assets/Scenes/_Transition.unity";


        private static void ExecuteDelegate(Action action, string name)
        {
            try { action?.Invoke(); }
            catch (Exception e) { Debug.LogError("SceneMonitor: Error executing delegate " + name + ", " + e); }
        }

        private static void ExecuteDelegate(Action<Scene> action, Scene scene, string name)
        {
            try { action?.Invoke(scene); }
            catch (Exception e) { Debug.LogError("SceneMonitor: Error executing delegate " + name + ", " + e); }
        }

        private static void ExecuteDelegate(Action<float> action, float parameter, string name)
        {
            try { action?.Invoke(parameter); }
            catch (Exception e) { Debug.LogError("SceneMonitor: Error executing delegate " + name + ", " + e); }
        }



    }
}
