
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

        // States
        private static List<ISceneSelector> selectorsList = new List<ISceneSelector>();
        //private static bool isFirstTime



        public static void Apply(ISceneSelector selector)
        {
            if (selector == null)
                return;

            selectorsList.Add(selector);
        }

        public static void Apply(params ISceneSelector[] selectors)
        {
            if (selectors == null) return;
            foreach (var selector in selectors)
            {
                Apply(selector); // Calling other function can cauuse stackoverflow
            }
        }

        public static void Apply(IEnumerable<ISceneSelector> selectors)
        {
            if (selectors == null) return;
            foreach (var selector in selectors)
            {
                Apply(selector);
            }
        }

        public static void  Apply(params IEnumerable<ISceneSelector>[] selectors)
        {
            if (selectors == null) return;
            foreach (var selector in selectors)
            {
                Apply(selector);
            }
        }



        public static void Remove(ISceneSelector selector)
        {
            if (selector != null)
                selectorsList.RemoveAll(s => s == selector);
        }

        public static void Remove(params ISceneSelector[] selectors)
        {
            if (selectors == null) return;
            foreach (var selector in selectors)
            {
                Remove(selector); // Calling other function can cauuse stackoverflow
            }
        }

        public static void Remove(IEnumerable<ISceneSelector> selectors)
        {
            if (selectors == null) return;
            foreach (var selector in selectors)
            {
                Remove(selector);
            }
        }

        public static void Remove(params IEnumerable<ISceneSelector>[] selectors)
        {
            if (selectors == null) return;
            foreach (var selector in selectors)
            {
                Remove(selector);
            }
        }

        public static void Remove(string pattern)
        {
            selectorsList.RemoveAll(s => s.Pattern() == pattern);
        }

        public static void Remove(params string[] patterns)
        {
            if (patterns == null) return;

            selectorsList.RemoveAll(s =>
            {

                foreach (var pattern in patterns)
                    if (s.Pattern() == pattern) return true;

                return false;

            });
        }

        public static void Remove(IEnumerable<string> patterns)
        {
            if (patterns == null) return;

            selectorsList.RemoveAll(s =>
            {

                foreach (var pattern in patterns)
                    if (s.Pattern() == pattern) return true;

                return false;

            });
        }

        public static void RemoveAllSelectors()
        {
            selectorsList.Clear();
        }


        public static int CountSelectors => selectorsList.Count;


        public static void Enable()
        {
            // Unsuscribe from scene events
            Disable();

            // Excecute on initialize delegates and scene loaded ans active
            var firstScene = SceneManager.GetActiveScene();
            OnSceneLoad(firstScene, LoadSceneMode.Single);
            OnActiveSceneChangedNullable(null, firstScene);

            // Subscribe to the events
            SceneManager.sceneLoaded += OnSceneLoad;
            SceneManager.sceneUnloaded += OnSceneUnload;
            SceneManager.activeSceneChanged += OnActiveSceneChanged;

        }

        public static void Disable()  // Disable all selectors
        {
            // Unsuscribe from scene events
            SceneManager.sceneLoaded -= OnSceneLoad;
            SceneManager.sceneUnloaded -= OnSceneUnload;
            SceneManager.activeSceneChanged -= OnActiveSceneChanged;
        }



        private static void OnSceneLoad(Scene scene, LoadSceneMode loadMode)
        {
            // If is the transition scene just return
            if (scene.path == TransitionScenePath) return;

            // Search for the scene in scenes def list
            var selectors = SearchSelectors(scene, selectorsList);
            if (selectors == null)
                return;

            // Execute afterLoad delagate
            foreach (var usene in selectors)
            {
                if (usene == null)
                    continue;

                ExecuteDelegate(usene.OnLoad, scene, "onLoad of Scene: " + usene.Pattern());

            }
            
        }

        private static void OnSceneUnload(Scene scene)
        {
            // If is the transition scene just return
            if (scene.path == TransitionScenePath) return;

            // Search for the scene in scenes def list
            var selectors = SearchSelectors(scene, selectorsList);
            if (selectors == null) 
                return;

            // Execute beforeUnload delagate
            foreach (var usene in selectors)
            {
                if (usene == null)
                    continue;

                ExecuteDelegate(usene.OnUnload, scene, "onUnload of Scene: " + usene.Pattern());

            }

        }

        private static void OnActiveSceneChanged(Scene currentScene, Scene nextScene)
        {
            OnActiveSceneChangedNullable(currentScene, nextScene);
        }
        private static void OnActiveSceneChangedNullable(Scene? currentScene, Scene nextScene)
        {

            // Search for the scene in scenes def list
            var selectorsNexts = SearchSelectors(nextScene, selectorsList);
            var selectorsCurrents = new ISceneSelector[0];
            if (currentScene != null)
                selectorsCurrents = SearchSelectors(((Scene)currentScene), selectorsList);

            // Execute afterLoad delagate from the scene that will be inactivated, can be null if is the first scene
            foreach (var usene in selectorsCurrents)
            {
                if (usene == null)
                    continue;

                // If is the transition scene just dont execute
                if (((Scene)currentScene).path == TransitionScenePath) continue;

                ExecuteDelegate(usene.OnSetInactive, (Scene)currentScene, "onSetAsInactive of Scene: " + usene.Pattern());

            }

            // Execute afterLoad delagate from the scene, not from the route, becouse you are only unloading a scene without a route
            foreach (var usene in selectorsNexts)
            {
                if (usene == null)
                    continue;

                // If is the transition scene just dont execute
                if (nextScene.path == TransitionScenePath) continue;

                ExecuteDelegate(usene.OnSetActive, nextScene, "onSetAsActive of Scene: " + usene.Pattern());

            }

        }


    }

}


