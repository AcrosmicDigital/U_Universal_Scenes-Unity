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

        public enum TransitionMode
        {
            Relative,  // Transitions waiting are canceled when any transition is performed
            Absolute,  // Transitions are not canceled when a transition in performed
        }


        private static List<string> relativeTransitionsList = new List<string>(); // List of autoLoadids
        private static List<string> absoluteTransitionsList = new List<string>(); // List of autoLoadids
        private static List<ISceneTransition> transitiionsList = new List<ISceneTransition>();  // List of subscribed scene transitiions
        private static bool isInTransition = false;



        // Subscribe a transition
        public static void Apply(ISceneTransition transition)
        {
            if (transition == null)
                return;

            transitiionsList.Add(transition);
        }

        public static void Apply(params ISceneTransition[] transitions)
        {
            if (transitions == null) return;
            foreach (var transition in transitions)
            {
                Apply(transition); // Calling other function can cauuse stackoverflow
            }
        }

        public static void Remove(ISceneTransition transition)
        {
            if (transition != null)
                transitiionsList.RemoveAll(s => s == transition);
        }

        public static void Remove(params ISceneTransition[] transitions)
        {
            if (transitions == null) return;
            foreach (var transition in transitions)
            {
                Remove(transition); // Calling other function can cauuse stackoverflow
            }
        }

        public static void RemoveAllTransitions()
        {
            transitiionsList.Clear();
        }


        public static int CountTransitions => transitiionsList.Count;




        private static async Task<bool> AwitForDelay(float delay, string cancelString, TransitionMode transitionMode, List<string> relativeList, List<string> absoluteList)
        {

            // All Transitions will have a token
            if (string.IsNullOrEmpty(cancelString))
                cancelString = StaticFunctions.NewIdShort() + "";

            // Add the cancel token to the list
            if (transitionMode == TransitionMode.Absolute && !absoluteList.Contains(cancelString))
                absoluteList.Add(cancelString);
            if (transitionMode == TransitionMode.Relative && !relativeList.Contains(cancelString))
                relativeList.Add(cancelString);

            // Await for the delay
            await StaticFunctions.WaitForSecondsRealtime(_host, delay);

            // Check if still the cancell token in the list
            if (transitionMode == TransitionMode.Absolute)
            {
                if (absoluteList.Contains(cancelString))
                    absoluteList.Remove(cancelString);
                else
                    return false;
            }
            else if (transitionMode == TransitionMode.Relative)
            {
                if (relativeList.Contains(cancelString))
                    relativeList.Remove(cancelString);
                else
                    return false;
            }

            return true;

        }

        private static SceneOperation CancelAwaitForDelay(string cancelString, TransitionMode transitionMode, List<string> relativeList, List<string> absoluteList)
        {
            // Create the operation
            var operation = new SceneOperation();

            // Add the cancel token to the list
            if (!string.IsNullOrEmpty(cancelString))
            {
                if (transitionMode == TransitionMode.Absolute)
                {
                    if (absoluteList.Contains(cancelString))
                    {
                        absoluteList.Remove(cancelString);
                        return operation.Successful("1");
                    }
                }
                else if(transitionMode == TransitionMode.Relative)
                {
                    if (relativeList.Contains(cancelString))
                    {
                        relativeList.Remove(cancelString);
                        return operation.Successful("1");
                    }
                }
            }

            return operation.Fails(new Exception("Cant Cancel Transition"));
        }

        private static async Task<SceneOperation> DoTransition(SceneData nextScene, TransitionData def)
        {
            // Create the operation
            var operation = new SceneOperation();

            // Await for the delay
            if (!await AwitForDelay(def.delay, def.cancelString, def.transitionMode, relativeTransitionsList, absoluteTransitionsList))
                return operation.Fails(new Exception("AutoLoadScene canceled"));

            // Check if is other transition in progres
            if (isInTransition)
                return operation.Fails(new Exception("Other transition in in progress"));
            else
                isInTransition = true;

            // Clear the relatives transitions
            relativeTransitionsList.Clear();

            // Search transitions patterns
            var transitions = SearchTransitions(SceneManager.GetActiveScene(), nextScene, transitiionsList);

            // Do the transition
            foreach (var transition in transitions)
                ExecuteDelegate(transition.SetUp, "SetUp");

            try
            {
                // SetUpProgress
                foreach (var transition in transitions)
                {
                    _host.AddComponent<TransitionTimer>().Set(def.startDelay, transition.SetUpProgress, false);
                }
                await StaticFunctions.WaitForSecondsRealtime(_host, def.startDelay);

                // SetUpReady
                // Do the transition
                foreach (var transition in transitions)
                    ExecuteDelegate(transition.SetUpReady, "TransitionSetUpReady");

                // Load the new scene and if load download the prev
                var prevScene = SceneManager.GetActiveScene();

                // LoadProgess, load and unload
                if (!String.IsNullOrEmpty(nextScene.path))
                {
                    await StaticFunctions.LoadSceneAsync(_host, nextScene.path, LoadSceneMode.Additive, (p) =>
                    {
                        foreach (var transition in transitions)
                            ExecuteDelegate(transition.LoadProgres, p, "TransitionLoadProgres");
                    });
                    //Debug.Log("Path: " + SceneManager.GetSceneByName("Menu").path + " vs " + nextScene.path);
                    SceneManager.SetActiveScene(SceneManager.GetSceneByPath("Assets/" + nextScene.path + ".unity"));
                }
                else if (nextScene.buildIndex >= 0)
                {
                    await StaticFunctions.LoadSceneAsync(_host, nextScene.buildIndex, LoadSceneMode.Additive, (p) =>
                    {
                        foreach (var transition in transitions)
                            ExecuteDelegate(transition.LoadProgres, p, "TransitionLoadProgres");
                    });
                    SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex(nextScene.buildIndex));
                }
                else
                {
                    await StaticFunctions.LoadSceneAsync(_host, nextScene.name, LoadSceneMode.Additive, (p) =>
                    {
                        foreach (var transition in transitions)
                            ExecuteDelegate(transition.LoadProgres, p, "TransitionLoadProgres");
                    });
                    SceneManager.SetActiveScene(SceneManager.GetSceneByName(nextScene.name));
                }
                
                // Unload the prev
                await StaticFunctions.UnloadSceneAsync(_host, prevScene.name);

                // TearDown
                foreach (var transition in transitions)
                    ExecuteDelegate(transition.TearDown, "TransitionTearDown");

                // TearDownProgress
                foreach (var transition in transitions)
                {
                    _host.AddComponent<TransitionTimer>().Set(def.startDelay, transition.TearDownProgress, false);
                }
                await StaticFunctions.WaitForSecondsRealtime(_host, def.endDelay);

            }
            catch (Exception e)
            {
                Debug.LogError("Transition Error: " + e);
                return operation.Fails(e);
            }

            isInTransition = false;

            // TearDownReady
            foreach (var transition in transitions)
                ExecuteDelegate(transition.TearDownReady, "TransitionTearDownReady");

            return operation.Successful("1");

        }



        public static Task<SceneOperation> Transition(SceneData nextScene, TransitionData def)
        {
            return DoTransition(nextScene, def);
        }

        public static Task<SceneOperation> Transition(string nextSceneName, TransitionData def)
        {
            return DoTransition(new SceneData(nextSceneName), def);
        }

        public static Task<SceneOperation> Transition(int nextSceneBuildIndex, TransitionData def)
        {
            return DoTransition(new SceneData(nextSceneBuildIndex), def);
        }

        public static Task<SceneOperation> Transition(SceneData nextScene)
        {
            return DoTransition(nextScene, new TransitionData());
        }

        public static Task<SceneOperation> Transition(string nextSceneName)
        {
            return DoTransition(new SceneData(nextSceneName), new TransitionData());
        }

        public static Task<SceneOperation> Transition(int nextSceneBuildIndex)
        {
            return DoTransition(new SceneData(nextSceneBuildIndex), new TransitionData());
        }

        public static Task<SceneOperation> Transition(TransitionData def)
        {
            return DoTransition(new SceneData(SceneManager.GetActiveScene()), def);
        }

        public static Task<SceneOperation> Transition()
        {
            return DoTransition(new SceneData(SceneManager.GetActiveScene()), new TransitionData());
        }



        public static SceneOperation CancelTransition(string cancelString, TransitionMode transitionMode)
        {
            return CancelAwaitForDelay(cancelString, transitionMode, relativeTransitionsList, absoluteTransitionsList);
        }

        public static void CancelAllTransitions(TransitionMode transitionMode)
        {

            if (transitionMode == TransitionMode.Absolute)
                absoluteTransitionsList.Clear();
            else if (transitionMode == TransitionMode.Relative)
                relativeTransitionsList.Clear();

        }




    }
}