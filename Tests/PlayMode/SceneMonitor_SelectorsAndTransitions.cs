using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using U.Universal;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using U.Universal.Scenes;
using System.Text.RegularExpressions;

public class SceneMonitor_SelectorsAndTransitions
{


    [OneTimeSetUp]
    public void OneTimeSetUp() { }


    [SetUp]
    public void SetUp()
    {
        SceneManager.LoadSceneAsync("Intro", LoadSceneMode.Single);
        SceneMonitor.RemoveAllSelectors();
        SceneMonitor.RemoveAllJumps();
    }

    class LocalTransition : ISceneTransition
    {
        public string CurrentScenePattern() => "*";

        public string NextScenePattern() => "*";

        public void LoadProgres(float p)
        {
            Debug.Log("Load P: " + p);
        }

        public void SetUp()
        {
            Debug.LogAssertion("Set Up");
        }

        public void SetUpProgress(float p)
        {
            Debug.Log("Set Up P: " + p);
        }

        public void SetUpReady()
        {
            Debug.LogAssertion("Set Up ready");
        }

        public void TearDown()
        {
            Debug.LogAssertion("Tear Down");
        }

        public void TearDownProgress(float p)
        {
            Debug.Log("Tear Down P: " + p);
        }

        public void TearDownReady()
        {
            Debug.LogAssertion("Tear Down Ready");
        }
    }


    [UnityTest]
    public IEnumerator Selectors_TransitionsInsideSelectors()
    {
        yield return new WaitForSecondsRealtime(1);

        // Apply the transitions
        var t = new LocalTransition();
        SceneMonitor.Apply(t);



        // Build the scene selectors
        SceneSelector[] config = new SceneSelector[]
        {
            new SceneSelector
            {
                pattern = "Intro",
                onSetActive = (s) => {
                    Debug.LogAssertion("Intro Set Active");

                    // Transition
                    SceneMonitor.Jump("Menu", new TransitionData
                    {
                        delay = 2f,
                        startDelay = 3f,
                        endDelay = 3f,
                    }).Then(Reject: e => Debug.LogError(e), Resolve: o => Debug.Log(o + ""));

                },
                onLoad = (s) =>  {
                    Debug.LogAssertion("Intro Load");
                },
                onSetInactive = (s) =>  {
                    Debug.LogAssertion("Intro SetInactive");
                },
                onUnload = (s) =>  {
                    Debug.LogAssertion("Intro Unload");
                },
            },
            new SceneSelector
            {
                pattern = "Menu",
                onSetActive = (s) => {
                    Debug.LogAssertion("Menu Set Active");
                },
                onLoad = (s) =>  {
                    Debug.LogAssertion("Menu Load");
                },
                onSetInactive = (s) =>  {
                    Debug.LogAssertion("Menu SetInactive");
                },
                onUnload = (s) =>  {
                    Debug.LogAssertion("Menu Unload");
                },
            },
        };
        SceneMonitor.Apply(config);


        // Initial load
        LogAssert.Expect(LogType.Assert, new Regex("Intro Load"));  // -> Sel
        LogAssert.Expect(LogType.Assert, new Regex("Intro Set Active"));  // -> Sel

        // Transition
        LogAssert.Expect(LogType.Assert, new Regex("Set Up"));  // -> Trn
        LogAssert.Expect(LogType.Assert, new Regex("Set Up ready"));  // -> Trn
        LogAssert.Expect(LogType.Assert, new Regex("Menu Load"));  // -> Sel
        LogAssert.Expect(LogType.Assert, new Regex("Intro SetInactive"));  // -> Sel
        LogAssert.Expect(LogType.Assert, new Regex("Menu Set Active"));  // -> Sel
        LogAssert.Expect(LogType.Assert, new Regex("Intro Unload"));  // -> Sel
        LogAssert.Expect(LogType.Assert, new Regex("Tear Down"));  // -> Trn
        LogAssert.Expect(LogType.Assert, new Regex("Tear Down Ready"));  // -> Trn



        // Enable the monitor
        SceneMonitor.Enable();

        

        yield return new WaitForSecondsRealtime(10);
        Assert.AreEqual("Menu", SceneManager.GetActiveScene().name);


    }

}
