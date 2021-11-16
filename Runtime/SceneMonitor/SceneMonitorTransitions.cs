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

        public enum JumpMode
        {
            Relative,  // Transitions waiting are canceled when any transition is performed
            Absolute,  // Transitions are not canceled when a transition in performed
        }


        private static List<string> relativeJumpsList = new List<string>(); // List of autoLoadids
        private static List<string> absoluteJumpsList = new List<string>(); // List of autoLoadids
        private static List<ISceneTransition> transitionsList = new List<ISceneTransition>();  // List of subscribed scene transitiions
        private static bool isInTransition = false;



        // Subscribe a transition
        public static void Apply(ISceneTransition transition)
        {
            if (transition == null)
                return;

            transitionsList.Add(transition);
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
                transitionsList.RemoveAll(s => s == transition);
        }

        public static void Remove(params ISceneTransition[] transitions)
        {
            if (transitions == null) return;
            foreach (var transition in transitions)
            {
                Remove(transition); // Calling other function can cauuse stackoverflow
            }
        }

        public static void RemoveAllJumps()
        {
            transitionsList.Clear();
        }


        public static int CountTransitions => transitionsList.Count;




        private static async Task<bool> AwitForDelay(float delay, string cancelString, JumpMode transitionMode, List<string> relativeList, List<string> absoluteList)
        {

            // All Transitions will have a token
            if (string.IsNullOrEmpty(cancelString))
                cancelString = StaticFunctions.NewIdShort() + "";

            // Add the cancel token to the list
            if (transitionMode == JumpMode.Absolute && !absoluteList.Contains(cancelString))
                absoluteList.Add(cancelString);
            if (transitionMode == JumpMode.Relative && !relativeList.Contains(cancelString))
                relativeList.Add(cancelString);

            // Await for the delay
            await StaticFunctions.WaitForSecondsRealtime(_host, delay);

            // Check if still the cancell token in the list
            if (transitionMode == JumpMode.Absolute)
            {
                if (absoluteList.Contains(cancelString))
                    absoluteList.Remove(cancelString);
                else
                    return false;
            }
            else if (transitionMode == JumpMode.Relative)
            {
                if (relativeList.Contains(cancelString))
                    relativeList.Remove(cancelString);
                else
                    return false;
            }

            return true;

        }

        private static SceneOperation CancelAwaitForDelay(string cancelString, JumpMode transitionMode, List<string> relativeList, List<string> absoluteList)
        {
            // Create the operation
            var operation = new SceneOperation();

            // Add the cancel token to the list
            if (!string.IsNullOrEmpty(cancelString))
            {
                if (transitionMode == JumpMode.Absolute)
                {
                    if (absoluteList.Contains(cancelString))
                    {
                        absoluteList.Remove(cancelString);
                        return operation.Successful("1");
                    }
                }
                else if(transitionMode == JumpMode.Relative)
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

        private static async Task<SceneOperation> DoJump(SceneData nextScene, TransitionData def)
        {
            // Create the operation
            var operation = new SceneOperation();

            // Await for the delay
            if (!await AwitForDelay(def.delay, def.cancelString, def.transitionMode, relativeJumpsList, absoluteJumpsList))
                return operation.Fails(new Exception("AutoLoadScene canceled"));

            // Check if is other transition in progres
            if (isInTransition)
                return operation.Fails(new Exception("Other transition in in progress"));
            else
                isInTransition = true;

            // Clear the relatives transitions
            relativeJumpsList.Clear();

            // Search transitions patterns
            var transitions = SearchTransitions(SceneManager.GetActiveScene(), nextScene, transitionsList);

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



        public static Task<SceneOperation> Jump(SceneData nextScene, TransitionData def)
        {
            return DoJump(nextScene, def);
        }

        public static Task<SceneOperation> Jump(string nextSceneName, TransitionData def)
        {
            return DoJump(new SceneData(nextSceneName), def);
        }

        public static Task<SceneOperation> Jump(int nextSceneBuildIndex, TransitionData def)
        {
            return DoJump(new SceneData(nextSceneBuildIndex), def);
        }

        public static Task<SceneOperation> Jump(SceneData nextScene)
        {
            return DoJump(nextScene, new TransitionData());
        }

        public static Task<SceneOperation> Jump(string nextSceneName)
        {
            return DoJump(new SceneData(nextSceneName), new TransitionData());
        }

        public static Task<SceneOperation> Jump(int nextSceneBuildIndex)
        {
            return DoJump(new SceneData(nextSceneBuildIndex), new TransitionData());
        }

        public static Task<SceneOperation> Jump(TransitionData def)
        {
            return DoJump(new SceneData(SceneManager.GetActiveScene()), def);
        }

        public static Task<SceneOperation> Jump()
        {
            return DoJump(new SceneData(SceneManager.GetActiveScene()), new TransitionData());
        }



        public static SceneOperation CancelJump(string cancelString, JumpMode transitionMode)
        {
            return CancelAwaitForDelay(cancelString, transitionMode, relativeJumpsList, absoluteJumpsList);
        }

        public static void CancelAllJumps(JumpMode transitionMode)
        {

            if (transitionMode == JumpMode.Absolute)
                absoluteJumpsList.Clear();
            else if (transitionMode == JumpMode.Relative)
                relativeJumpsList.Clear();

        }




    }
}