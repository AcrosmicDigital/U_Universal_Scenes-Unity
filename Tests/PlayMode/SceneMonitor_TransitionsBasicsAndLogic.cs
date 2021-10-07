using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using U.Universal;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using U.Universal.Scenes;

public class SceneMonitor_TransitionsBasicsAndLogic
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





    [UnityTest]
    public IEnumerator Transition_LoadOtherSceneInmediately()
    {
        
        yield return new WaitForSecondsRealtime(2);

        SceneMonitor.Transition("Menu").Then(Reject: e => Debug.LogError(e));

        yield return new WaitForSecondsRealtime(2);

        Assert.AreEqual("Menu", SceneManager.GetActiveScene().name);

    }


    [UnityTest]
    public IEnumerator Transition_LoadOtherSceneWithDelay()
    {
        yield return new WaitForSecondsRealtime(2);
        
        SceneMonitor.Transition("Menu", new TransitionData { delay = 3, }).Then(Reject: e => Debug.LogError(e));
        
        yield return new WaitForSecondsRealtime(2);

        Assert.AreEqual("Intro", SceneManager.GetActiveScene().name);

        yield return new WaitForSecondsRealtime(2);

        Assert.AreEqual("Menu", SceneManager.GetActiveScene().name);

    }


    [UnityTest]
    public IEnumerator Transition_LoadOtherSceneWithDelay_ButCalcelIt()
    {
        
        yield return new WaitForSecondsRealtime(2);

        SceneMonitor.Transition("Menu", new TransitionData
        {
            delay = 3f,
            transitionMode = SceneMonitor.TransitionMode.Relative,
            cancelString = "cargaMenu",
        }).Then(Reject: e => Debug.LogError(e));

        yield return new WaitForSecondsRealtime(2);

        Assert.AreEqual("Intro", SceneManager.GetActiveScene().name);

        SceneMonitor.CancelTransition("cargaMenu", SceneMonitor.TransitionMode.Relative);

        yield return new WaitForSecondsRealtime(2);
        
        Assert.AreEqual("Intro", SceneManager.GetActiveScene().name);

    }


    [UnityTest]
    public IEnumerator Transition_LoadOtherSceneWithDelay_ButCalcelItLoadingOtherScene()
    {
        
        yield return new WaitForSecondsRealtime(2);

        SceneMonitor.Transition("Menu", new TransitionData
        {
            delay = 3f,
            transitionMode = SceneMonitor.TransitionMode.Relative,
            cancelString = "cargaMenu"
        }).Then(Reject: e => Debug.LogError(e));

        yield return new WaitForSecondsRealtime(2);

        SceneMonitor.Transition("Level1").Then(Reject: e => Debug.LogError(e));

        yield return new WaitForSecondsRealtime(2);

        Assert.AreEqual("Level1", SceneManager.GetActiveScene().name);

    }


    [UnityTest]
    public IEnumerator Transition_LoadOtherSceneWithDelayButAbsolute_LoadingOtherSceneCantCancelIt()
    {

        yield return new WaitForSecondsRealtime(2);

        SceneMonitor.Transition("Menu", new TransitionData
        {
            delay = 3f,
            transitionMode = SceneMonitor.TransitionMode.Absolute,
            cancelString = "cargaMenu"
        }).Then(Reject: e => Debug.LogError(e));

        yield return new WaitForSecondsRealtime(2);

        SceneMonitor.Transition("Level1").Then(Reject: e => Debug.LogError(e));

        yield return new WaitForSecondsRealtime(2);
        Assert.AreEqual("Menu", SceneManager.GetActiveScene().name);

    }


    [UnityTest]
    public IEnumerator Transition_LoadOtherSceneWithDelayButAbsolute_ButCancelIt()
    {

        yield return new WaitForSecondsRealtime(2);

        SceneMonitor.Transition("Menu", new TransitionData
        {
            delay = 3f,
            transitionMode = SceneMonitor.TransitionMode.Absolute,
            cancelString = "cargaMenu"
        }).Then(Reject: e => Debug.LogError(e));

        yield return new WaitForSecondsRealtime(2);

        SceneMonitor.CancelTransition("cargaMenu", SceneMonitor.TransitionMode.Absolute);

        yield return new WaitForSecondsRealtime(2);
        Assert.AreEqual("Intro", SceneManager.GetActiveScene().name);

    }


    [UnityTest]
    public IEnumerator Transition_LoadOtherSceneWithDelay_ButCalcelAllTransitions()
    {

        yield return new WaitForSecondsRealtime(2);

        SceneMonitor.Transition("Menu", new TransitionData
        {
            delay = 3f,
            transitionMode = SceneMonitor.TransitionMode.Relative,
            cancelString = "cargaMenu"
        }).Then(Reject: e => Debug.LogError(e));

        yield return new WaitForSecondsRealtime(2);

        Assert.AreEqual("Intro", SceneManager.GetActiveScene().name);

        SceneMonitor.CancelAllTransitions( SceneMonitor.TransitionMode.Relative);

        yield return new WaitForSecondsRealtime(2);

        Assert.AreEqual("Intro", SceneManager.GetActiveScene().name);

    }


    [UnityTest]
    public IEnumerator Transition_LoadOtherSceneWithDelayButAbsolute_CalcelAllTransitionsDontCancelIt()
    {

        yield return new WaitForSecondsRealtime(2);

        SceneMonitor.Transition("Menu", new TransitionData
        {
            delay = 3f,
            transitionMode = SceneMonitor.TransitionMode.Absolute,
            cancelString = "cargaMenu"
        }).Then(Reject: e => Debug.LogError(e));

        yield return new WaitForSecondsRealtime(2);

        Assert.AreEqual("Intro", SceneManager.GetActiveScene().name);

        SceneMonitor.CancelAllTransitions( SceneMonitor.TransitionMode.Relative);

        yield return new WaitForSecondsRealtime(2);

        Assert.AreEqual("Menu", SceneManager.GetActiveScene().name);

    }


    [UnityTest]
    public IEnumerator Transition_LoadOtherSceneWithDelayButAbsolute_CalcelAllTransitionsAbsoluteCanCancel()
    {

        yield return new WaitForSecondsRealtime(2);

        SceneMonitor.Transition("Menu", new TransitionData
        {
            delay = 3f,
            transitionMode = SceneMonitor.TransitionMode.Absolute,
            cancelString = "cargaMenu"
        }).Then(Reject: e => Debug.LogError(e));

        yield return new WaitForSecondsRealtime(2);

        Assert.AreEqual("Intro", SceneManager.GetActiveScene().name);

        SceneMonitor.CancelAllTransitions( SceneMonitor.TransitionMode.Absolute);

        yield return new WaitForSecondsRealtime(2);

        Assert.AreEqual("Intro", SceneManager.GetActiveScene().name);

    }


    [UnityTest]
    public IEnumerator Transition_IfManyTransitionsOnlyFirstWillBePerformed()
    {
        yield return new WaitForSecondsRealtime(2);
        
        SceneMonitor.Transition("Menu", new TransitionData
        {
            delay = 3f,
        }).Then(Reject: e => Debug.LogError(e));
        SceneMonitor.Transition("Menu1", new TransitionData
        {
            delay = 4f,
        }).Then(Reject: e => Debug.LogError(e));
        SceneMonitor.Transition("Level1", new TransitionData
        {
            delay = 5f,
        }).Then(Reject: e => Debug.LogError(e));

        yield return new WaitForSecondsRealtime(3.1f);

        Assert.AreEqual("Menu", SceneManager.GetActiveScene().name);

        yield return new WaitForSecondsRealtime(1);

        Assert.AreEqual("Menu", SceneManager.GetActiveScene().name);

        yield return new WaitForSecondsRealtime(1);

        Assert.AreEqual("Menu", SceneManager.GetActiveScene().name);

    }


    [UnityTest]
    public IEnumerator Transition_IfManyTransitionsModeAbsoluteAllWillBePerformed()
    {
        yield return new WaitForSecondsRealtime(2);

        SceneMonitor.Transition("Menu", new TransitionData
        {
            delay = 3f,
            transitionMode = SceneMonitor.TransitionMode.Absolute,
        }).Then(Reject: e => Debug.LogError(e));
        SceneMonitor.Transition("Menu1", new TransitionData
        {
            delay = 4f,
            transitionMode = SceneMonitor.TransitionMode.Absolute,
        }).Then(Reject: e => Debug.LogError(e));
        SceneMonitor.Transition("Level1", new TransitionData
        {
            delay = 5f,
            transitionMode = SceneMonitor.TransitionMode.Absolute,
        }).Then(Reject: e => Debug.LogError(e));

        yield return new WaitForSecondsRealtime(3.1f);

        Assert.AreEqual("Menu", SceneManager.GetActiveScene().name);

        yield return new WaitForSecondsRealtime(1);

        Assert.AreEqual("Menu1", SceneManager.GetActiveScene().name);

        yield return new WaitForSecondsRealtime(1);

        Assert.AreEqual("Level1", SceneManager.GetActiveScene().name);

    }


    [UnityTest]
    public IEnumerator Transition_AnAbsoluteTransitionIsNotCanceled()
    {
        yield return new WaitForSecondsRealtime(2);

        SceneMonitor.Transition("Menu", new TransitionData
        {
            delay = 3f,
        }).Then(Reject: e => Debug.LogError(e));
        SceneMonitor.Transition("Menu1", new TransitionData
        {
            delay = 4f,
            transitionMode = SceneMonitor.TransitionMode.Absolute
        }).Then(Reject: e => Debug.LogError(e));
        SceneMonitor.Transition("Level1", new TransitionData
        {
            delay = 5f,
        }).Then(Reject: e => Debug.LogError(e));

        yield return new WaitForSecondsRealtime(3.1f);

        Assert.AreEqual("Menu", SceneManager.GetActiveScene().name);

        yield return new WaitForSecondsRealtime(1);

        Assert.AreEqual("Menu1", SceneManager.GetActiveScene().name);

        yield return new WaitForSecondsRealtime(1);

        Assert.AreEqual("Menu1", SceneManager.GetActiveScene().name);

    }


    [UnityTest]
    public IEnumerator Transition_ManyTransitionsCanBeCanceled()
    {
        yield return new WaitForSecondsRealtime(2);

        SceneMonitor.Transition("Menu", new TransitionData
        {
            delay = 3f,
            transitionMode = SceneMonitor.TransitionMode.Relative,
        }).Then(Reject: e => Debug.LogError(e));
        SceneMonitor.Transition("Menu1", new TransitionData
        {
            delay = 4f,
            transitionMode = SceneMonitor.TransitionMode.Relative,
        }).Then(Reject: e => Debug.LogError(e));
        SceneMonitor.Transition("Level1", new TransitionData
        {
            delay = 5f,
            transitionMode = SceneMonitor.TransitionMode.Relative,
        }).Then(Reject: e => Debug.LogError(e));

        SceneMonitor.CancelAllTransitions( SceneMonitor.TransitionMode.Relative);

        yield return new WaitForSecondsRealtime(3.1f);

        Assert.AreEqual("Intro", SceneManager.GetActiveScene().name);

        yield return new WaitForSecondsRealtime(1);

        Assert.AreEqual("Intro", SceneManager.GetActiveScene().name);

        yield return new WaitForSecondsRealtime(1);

        Assert.AreEqual("Intro", SceneManager.GetActiveScene().name);

    }


    [UnityTest]
    public IEnumerator Transition_ManyAbsoluteTransitionsCantBeCanceledLikeRelativeTransitions()
    {
        yield return new WaitForSecondsRealtime(2);

        SceneMonitor.Transition("Menu", new TransitionData
        {
            delay = 3f,
            transitionMode = SceneMonitor.TransitionMode.Absolute,
        }).Then(Reject: e => Debug.LogError(e));
        SceneMonitor.Transition("Menu1", new TransitionData
        {
            delay = 4f,
            transitionMode = SceneMonitor.TransitionMode.Absolute,
        }).Then(Reject: e => Debug.LogError(e));
        SceneMonitor.Transition("Level1", new TransitionData
        {
            delay = 5f,
            transitionMode = SceneMonitor.TransitionMode.Absolute,
        }).Then(Reject: e => Debug.LogError(e));

        SceneMonitor.CancelAllTransitions( SceneMonitor.TransitionMode.Relative);

        yield return new WaitForSecondsRealtime(3.1f);

        Assert.AreEqual("Menu", SceneManager.GetActiveScene().name);

        yield return new WaitForSecondsRealtime(1);

        Assert.AreEqual("Menu1", SceneManager.GetActiveScene().name);

        yield return new WaitForSecondsRealtime(1);

        Assert.AreEqual("Level1", SceneManager.GetActiveScene().name);

    }


    [UnityTest]
    public IEnumerator Transition_ManyAbsoluteTransitionsCanBeCancel()
    {
        yield return new WaitForSecondsRealtime(2);

        SceneMonitor.Transition("Menu", new TransitionData
        {
            delay = 3f,
            transitionMode = SceneMonitor.TransitionMode.Absolute,
        }).Then(Reject: e => Debug.LogError(e));
        SceneMonitor.Transition("Menu1", new TransitionData
        {
            delay = 4f,
            transitionMode = SceneMonitor.TransitionMode.Absolute,
        }).Then(Reject: e => Debug.LogError(e));
        SceneMonitor.Transition("Level1", new TransitionData
        {
            delay = 5f,
            transitionMode = SceneMonitor.TransitionMode.Absolute,
        }).Then(Reject: e => Debug.LogError(e));

        SceneMonitor.CancelAllTransitions( SceneMonitor.TransitionMode.Absolute);

        yield return new WaitForSecondsRealtime(3.1f);

        Assert.AreEqual("Intro", SceneManager.GetActiveScene().name);

        yield return new WaitForSecondsRealtime(1);

        Assert.AreEqual("Intro", SceneManager.GetActiveScene().name);

        yield return new WaitForSecondsRealtime(1);

        Assert.AreEqual("Intro", SceneManager.GetActiveScene().name);

    }


    [UnityTest]
    public IEnumerator Transition_ManyTransitionsCanBeCanceledWithCancelString()
    {
        yield return new WaitForSecondsRealtime(2);

        SceneMonitor.Transition("Menu", new TransitionData
        {
            delay = 2f,
            transitionMode = SceneMonitor.TransitionMode.Relative,
            cancelString = "cancelString",
        }).Then(Reject: e => Debug.LogError(e));
        SceneMonitor.Transition("Game", new TransitionData
        {
            delay = 3f,
            transitionMode = SceneMonitor.TransitionMode.Relative,
            cancelString = "cancelString",
        }).Then(Reject: e => Debug.LogError(e));
        SceneMonitor.Transition("Menu1", new TransitionData
        {
            delay = 4f,
            transitionMode = SceneMonitor.TransitionMode.Relative,
        }).Then(Reject: e => Debug.LogError(e));
        SceneMonitor.Transition("Level1", new TransitionData
        {
            delay = 5f,
            transitionMode = SceneMonitor.TransitionMode.Relative,
            cancelString = "cancelString",
        }).Then(Reject: e => Debug.LogError(e));

        SceneMonitor.CancelTransition("cancelString", SceneMonitor.TransitionMode.Relative);

        yield return new WaitForSecondsRealtime(3.1f);

        Assert.AreEqual("Intro", SceneManager.GetActiveScene().name);

        yield return new WaitForSecondsRealtime(1);

        Assert.AreEqual("Menu1", SceneManager.GetActiveScene().name);

        yield return new WaitForSecondsRealtime(1);

        Assert.AreEqual("Menu1", SceneManager.GetActiveScene().name);

    }


    [UnityTest]
    public IEnumerator Transition_ManyAbsoluteTransitionsCanBeCanceledWithCancelString()
    {
        yield return new WaitForSecondsRealtime(2);

        SceneMonitor.Transition("Menu", new TransitionData
        {
            delay = 2f,
            transitionMode = SceneMonitor.TransitionMode.Absolute,
            cancelString = "cancelString",
        }).Then(Reject: e => Debug.LogError(e));
        SceneMonitor.Transition("Game", new TransitionData
        {
            delay = 3f,
            transitionMode = SceneMonitor.TransitionMode.Absolute,
            cancelString = "cancelString",
        }).Then(Reject: e => Debug.LogError(e));
        SceneMonitor.Transition("Menu1", new TransitionData
        {
            delay = 4f,
            transitionMode = SceneMonitor.TransitionMode.Absolute,
        }).Then(Reject: e => Debug.LogError(e));
        SceneMonitor.Transition("Level1", new TransitionData
        {
            delay = 5f,
            transitionMode = SceneMonitor.TransitionMode.Absolute,
            cancelString = "cancelString",
        }).Then(Reject: e => Debug.LogError(e));


        SceneMonitor.CancelTransition("cancelString", SceneMonitor.TransitionMode.Absolute);

        yield return new WaitForSecondsRealtime(3.1f);

        Assert.AreEqual("Intro", SceneManager.GetActiveScene().name);

        yield return new WaitForSecondsRealtime(1);

        Assert.AreEqual("Menu1", SceneManager.GetActiveScene().name);

        yield return new WaitForSecondsRealtime(1);

        Assert.AreEqual("Menu1", SceneManager.GetActiveScene().name);

    }


    [UnityTest]
    public IEnumerator Transition_ReloadSceneInmediately()
    {
        var go = new GameObject("Tester");

        yield return new WaitForSecondsRealtime(2);

        SceneMonitor.Transition().Then(Reject: e => Debug.LogError(e));

        yield return new WaitForSecondsRealtime(2);

        Assert.IsTrue(go == null);

    }


    [UnityTest]
    public IEnumerator Transition_ReloadSceneWithDelay()
    {
        var go = new GameObject("Tester");

        yield return new WaitForSecondsRealtime(2);

        SceneMonitor.Transition(new TransitionData { delay = 3, }).Then(Reject: e => Debug.LogError(e));

        yield return new WaitForSecondsRealtime(2);

        Assert.IsFalse(go == null);
        yield return new WaitForSecondsRealtime(2);
        Assert.IsTrue(go == null);

    }


    [UnityTest]
    public IEnumerator Transition_ReloadSceneWithDelay_ButCalcelIt()
    {
        var go = new GameObject("Tester");

        yield return new WaitForSecondsRealtime(2);

        SceneMonitor.Transition(new TransitionData
        {
            delay = 3f,
            transitionMode = SceneMonitor.TransitionMode.Absolute,
            cancelString = "cargaMenu",
        }).Then(Reject: e => Debug.LogError(e));

        yield return new WaitForSecondsRealtime(2);

        Assert.IsFalse(go == null);

        SceneMonitor.CancelTransition("cargaMenu", SceneMonitor.TransitionMode.Relative);

        yield return new WaitForSecondsRealtime(2);
        Assert.IsTrue(go == null);

    }


    [UnityTest]
    public IEnumerator Transition_OnlyOnetransitionAtTimeWillBeDone()
    {
        yield return new WaitForSecondsRealtime(2);

        var operationOne = SceneMonitor.Transition("Menu", new TransitionData
        {
            delay = 3f,
            startDelay = 3,
            endDelay = 3,
            transitionMode = SceneMonitor.TransitionMode.Absolute,
        });
        var operationTwo = SceneMonitor.Transition("Menu1", new TransitionData
        {
            delay = 4f,
            startDelay = 3,
            endDelay = 3,
            transitionMode = SceneMonitor.TransitionMode.Absolute,
        });
        var operationThree = SceneMonitor.Transition("Level1", new TransitionData
        {
            delay = 5f,
            startDelay = 3,
            endDelay = 3,
            transitionMode = SceneMonitor.TransitionMode.Absolute,
        });

        yield return new WaitForSecondsRealtime(3.1f);

        Assert.AreEqual("Intro", SceneManager.GetActiveScene().name);

        yield return new WaitForSecondsRealtime(1);

        Assert.AreEqual("Intro", SceneManager.GetActiveScene().name);

        yield return new WaitForSecondsRealtime(2);

        Assert.AreEqual("Menu", SceneManager.GetActiveScene().name);

        yield return new WaitForSecondsRealtime(4);

        Assert.AreEqual("Menu", SceneManager.GetActiveScene().name);

        Debug.Log("Op1: " + operationOne.Result);
        Debug.Log("Op2: " + operationTwo.Result);
        Debug.Log("Op3: " + operationThree.Result);

    }

}
