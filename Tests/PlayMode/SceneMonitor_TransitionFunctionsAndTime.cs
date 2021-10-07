using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using U.Universal;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using U.Universal.Scenes;

public class SceneMonitor_TransitionFunctionsAndTime
{

    [OneTimeSetUp]
    public void OneTimeSetUp() { }

    [SetUp]
    public void SetUp()
    {
        // All test must start in intro scene
        SceneManager.LoadSceneAsync("Intro", LoadSceneMode.Single);
        SceneMonitor.RemoveAllSelectors();
        SceneMonitor.RemoveAllTransitions();
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
    public IEnumerator Transition_TransitionFunctionsOrder()
    {

        yield return new WaitForSecondsRealtime(2);

        // Subscribe the transition instance
        var trn = new LocalTransition();
        SceneMonitor.Apply(trn);


        // Start the transition
        SceneMonitor.Transition("Menu", new TransitionData
        {
            startDelay = 3,
            endDelay = 3,
        }).Then(Reject: e => Debug.LogError(e));

        LogAssert.Expect(LogType.Assert, "Set Up");
        LogAssert.Expect(LogType.Assert, "Set Up ready");
        LogAssert.Expect(LogType.Assert, "Tear Down");
        LogAssert.Expect(LogType.Assert, "Tear Down Ready");

        yield return new WaitForSecondsRealtime(6.1f);

        Assert.AreEqual("Menu", SceneManager.GetActiveScene().name);

    }

    class LocalTransitionValues : ISceneTransition
    {
        public bool f1 = false;
        public bool f2 = false;
        public bool f3 = false;
        public bool f4 = false;
        public bool f5 = false;
        public bool f6 = false;
        public bool f7 = false;

        public string CurrentScenePattern() => "*";

        public string NextScenePattern() => "*";

        public void LoadProgres(float p)
        {
            f1 = true;
            Debug.Log("Load P: " + p);
        }

        public void SetUp()
        {
            f2 = true;
            Debug.LogAssertion("Set Up");
        }

        public void SetUpProgress(float p)
        {
            f3 = true;
            Debug.Log("Set Up P: " + p);
        }

        public void SetUpReady()
        {
            f4 = true;
            Debug.LogAssertion("Set Up ready");
        }

        public void TearDown()
        {
            f6 = true;
            Debug.LogAssertion("Tear Down");
        }

        public void TearDownProgress(float p)
        {
            f7 = true;
            Debug.Log("Tear Down P: " + p);
        }

        public void TearDownReady()
        {
            f5 = true;
            Debug.LogAssertion("Tear Down Ready");
        }
    }
    [UnityTest]
    public IEnumerator Transition_TransitionFunctionsThrowsError_AllFuntionsWillRunAndSceneWillBechanged()
    {

        yield return new WaitForSecondsRealtime(2);

        // Subscribe the transition instance
        var t = new LocalTransitionValues();
        SceneMonitor.Apply(t);


        // Start the transition
        SceneMonitor.Transition("Menu", new TransitionData
        {
            startDelay = 3,
            endDelay = 3,
        }).Then(Reject: e => Debug.LogError(e));

        LogAssert.ignoreFailingMessages = true;

        yield return new WaitForSecondsRealtime(6.1f);

        Assert.IsTrue(t.f1 && t.f2 && t.f3 && t.f4 && t.f5 && t.f6 && t.f7);
        Assert.AreEqual("Menu", SceneManager.GetActiveScene().name);

    }

    [UnityTest]
    public IEnumerator Transition_TransitionReload()
    {

        yield return new WaitForSecondsRealtime(2);

        // Subscribe the transition instance
        var trn = new LocalTransition();
        SceneMonitor.Apply(trn);


        // Start the transition
        SceneMonitor.Transition(new TransitionData
        {
            startDelay = 3,
            endDelay = 3,
        }).Then(Reject: e => Debug.LogError(e));


        LogAssert.Expect(LogType.Assert, "Set Up");
        LogAssert.Expect(LogType.Assert, "Set Up ready");
        LogAssert.Expect(LogType.Assert, "Tear Down");
        LogAssert.Expect(LogType.Assert, "Tear Down Ready");

        yield return new WaitForSecondsRealtime(6.1f);

        Assert.AreEqual("Intro", SceneManager.GetActiveScene().name);

    }


    [UnityTest]
    public IEnumerator Transition_TransitionITransitionOrder()
    {

        yield return new WaitForSecondsRealtime(2);

        // Subscribe the transition
        var transition = new LocalTransition();
        SceneMonitor.Apply(transition);

        SceneMonitor.Transition("Menu", new TransitionData
        {
            startDelay = 3f,
            endDelay = 3f,
            transitionMode = SceneMonitor.TransitionMode.Relative,
        }).Then(Reject: e => Debug.LogError(e));

        LogAssert.Expect(LogType.Assert, "Set Up");
        LogAssert.Expect(LogType.Assert, "Set Up ready");
        LogAssert.Expect(LogType.Assert, "Tear Down");
        LogAssert.Expect(LogType.Assert, "Tear Down Ready");

        yield return new WaitForSecondsRealtime(6.1f);

        Assert.AreEqual("Menu", SceneManager.GetActiveScene().name);

    }

    [UnityTest]
    public IEnumerator Transition_TransitionITransitionThrowsError_AllFuntionsWillRunAndSceneWillBechanged()
    {

        yield return new WaitForSecondsRealtime(2);

        // Subscribe the transition
        var transition = new LocalTransitionValues();
        SceneMonitor.Apply(transition);

        SceneMonitor.Transition("Menu", new TransitionData
        {
            startDelay = 3f,
            endDelay = 3f,
            transitionMode = SceneMonitor.TransitionMode.Relative,
        }).Then(Reject: e => Debug.LogError(e));

        LogAssert.ignoreFailingMessages = true;

        yield return new WaitForSecondsRealtime(6.1f);

        Assert.IsTrue(transition.f1 && transition.f2 && transition.f3 && transition.f4 && transition.f5 && transition.f6 && transition.f7);
        Assert.AreEqual("Menu", SceneManager.GetActiveScene().name);

    }

    [UnityTest]
    public IEnumerator Transition_TransitionITransitionReload()
    {

        yield return new WaitForSecondsRealtime(2);

        // Subscribe the transition
        var transition = new LocalTransition();
        SceneMonitor.Apply(transition);

        SceneMonitor.Transition(new TransitionData
        {
            startDelay = 3f,
            endDelay = 3f,
            transitionMode = SceneMonitor.TransitionMode.Relative,
        }).Then(Reject: e => Debug.LogError(e));

        LogAssert.Expect(LogType.Assert, "Set Up");
        LogAssert.Expect(LogType.Assert, "Set Up ready");
        LogAssert.Expect(LogType.Assert, "Tear Down");
        LogAssert.Expect(LogType.Assert, "Tear Down Ready");

        yield return new WaitForSecondsRealtime(6.1f);

        Assert.AreEqual("Intro", SceneManager.GetActiveScene().name);

    }
}
