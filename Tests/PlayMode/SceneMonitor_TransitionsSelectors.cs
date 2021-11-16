using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using U.Universal.Scenes;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

public class SceneMonitor_TransitionsSelectors
{

    [SetUp]
    public void SetUp()
    {
        // All test must start in intro scene
        SceneManager.LoadSceneAsync("Intro", LoadSceneMode.Single);
        SceneMonitor.RemoveAllSelectors();
        SceneMonitor.RemoveAllJumps();
    }



    [UnityTest]
    public IEnumerator TransitionSelector_000AnyToAny()
    {
        yield return new WaitForSecondsRealtime(2f);

        var trn = new SceneTransition[]
        {
            new SceneTransition
            {
                currentScenePattern = "*",
                nextScenePattern = "*",
                setUp = () => Debug.LogAssertion("* to *"),
            },
            new SceneTransition
            {
                currentScenePattern = "*",
                nextScenePattern = "*",
                setUp = () => Debug.LogAssertion("* to * Two"),
            }
        };

        // Subscribe the transitions
        SceneMonitor.Apply(trn);


        // Start the transition froom Intro to Menu
        LogAssert.Expect(LogType.Assert, "* to *");  // -> Trn
        LogAssert.Expect(LogType.Assert, "* to * Two");  // -> Trn
        SceneMonitor.Jump("Menu", new TransitionData
        {
            startDelay = 3,
            endDelay = 3,
        }).Then(Reject: e => Debug.LogError(e));

        // Wait for  the transition ends
        yield return new WaitForSecondsRealtime(6.1f);
        Assert.AreEqual("Menu", SceneManager.GetActiveScene().name);


        // Start the transition froom Menu to Level1
        LogAssert.Expect(LogType.Assert, "* to *");  // -> Trn
        LogAssert.Expect(LogType.Assert, "* to * Two");  // -> Trn
        SceneMonitor.Jump("Level1", new TransitionData
        {
            startDelay = 3,
            endDelay = 3,
        }).Then(Reject: e => Debug.LogError(e));

        // Wait for  the transition ends
        yield return new WaitForSecondsRealtime(8.1f);
        Debug.Log("Endtransition");
        Assert.AreEqual("Level1", SceneManager.GetActiveScene().name);
    }

    [UnityTest]
    public IEnumerator TransitionSelector_001ByName()
    {
        yield return new WaitForSecondsRealtime(2f);

        var trn = new SceneTransition[]
        {
            new SceneTransition
            {
                currentScenePattern = "Intro",
                nextScenePattern = "Menu",
                setUp = () => Debug.LogAssertion("Intro to Menu"),
            },
            new SceneTransition
            {
                currentScenePattern = "Menu",
                nextScenePattern = "Level1",
                setUp = () => Debug.LogAssertion("Menu to Level1"),
            }
        };

        // Subscribe the transitions
        SceneMonitor.Apply(trn);


        // Start the transition froom Intro to Menu
        LogAssert.Expect(LogType.Assert, "Intro to Menu");  // -> Trn
        SceneMonitor.Jump("Menu", new TransitionData
        {
            startDelay = 3,
            endDelay = 3,
        }).Then(Reject: e => Debug.LogError(e));

        // Wait for  the transition ends
        yield return new WaitForSecondsRealtime(6.1f);
        Assert.AreEqual("Menu", SceneManager.GetActiveScene().name);


        // Start the transition froom Menu to Level1
        LogAssert.Expect(LogType.Assert, "Menu to Level1");  // -> Trn
        SceneMonitor.Jump("Level1", new TransitionData
        {
            startDelay = 3,
            endDelay = 3,
        }).Then(Reject: e => Debug.LogError(e));

        // Wait for  the transition ends
        yield return new WaitForSecondsRealtime(8.1f);
        Debug.Log("Endtransition");
        Assert.AreEqual("Level1", SceneManager.GetActiveScene().name);
    }


    [UnityTest]
    public IEnumerator TransitionSelector_002ByBuilIndex()
    {
        yield return new WaitForSecondsRealtime(2f);

        var trn = new SceneTransition[]
        {
            new SceneTransition
            {
                currentScenePattern = "Intro",
                nextScenePattern = "#>0",
                setUp = () => Debug.LogAssertion("Intro to >0"),
            },
            new SceneTransition
            {
                currentScenePattern = "#>0",
                nextScenePattern = "#3",  // This be executed nly if in the transition function this value is passed
                setUp = () => Debug.LogAssertion(">0 to #3"),
            }
        };

        // Subscribe the transitions
        SceneMonitor.Apply(trn);


        // Start the transition froom Intro to Menu
        LogAssert.Expect(LogType.Assert, "Intro to >0");  // -> Trn
        SceneMonitor.Jump(2, new TransitionData
        {
            startDelay = 3,
            endDelay = 3,
        }).Then(Reject: e => Debug.LogError(e));

        // Wait for  the transition ends
        yield return new WaitForSecondsRealtime(6.1f);
        Assert.AreEqual(2, SceneManager.GetActiveScene().buildIndex);


        // Start the transition froom Menu to Level1
        LogAssert.Expect(LogType.Assert, ">0 to #3");  // -> Trn
        SceneMonitor.Jump(3, new TransitionData
        {
            startDelay = 3,
            endDelay = 3,
        }).Then(Reject: e => Debug.LogError(e));

        // Wait for  the transition ends
        yield return new WaitForSecondsRealtime(8.1f);
        Debug.Log("Endtransition");
        Assert.AreEqual(3, SceneManager.GetActiveScene().buildIndex);
    }

    [UnityTest]
    public IEnumerator TransitionSelector_003ByPath()
    {
        yield return new WaitForSecondsRealtime(2f);

        var trn = new SceneTransition[]
        {
            new SceneTransition
            {
                currentScenePattern = ".*Tests/PlayMode/Intro",
                nextScenePattern = ".*Tests/PlayMode/Menu",
                setUp = () => Debug.LogAssertion(".*Tests/PlayMode/Intro to .*Tests/PlayMode/Menu"),
            },
            new SceneTransition
            {
                currentScenePattern = ".*Tests/PlayMode/Menu",
                nextScenePattern = ".*Tests/PlayMode/Level*",  // This be executed nly if in the transition function this value is passed
                setUp = () => Debug.LogAssertion(".*Tests/PlayMode/Menu to .*Tests/PlayMode/Level*"),
            }
        };

        // Subscribe the transitions
        SceneMonitor.Apply(trn);


        // Start the transition froom Intro to Menu
        LogAssert.Expect(LogType.Assert, ".*Tests/PlayMode/Intro to .*Tests/PlayMode/Menu");  // -> Trn
        SceneMonitor.Jump(new SceneData("Menu", "U.Universal.Scenes-v1.0.1/Tests/PlayMode/Menu"), new TransitionData
        {
            startDelay = 3,
            endDelay = 3,
        }).Then(Reject: e => Debug.LogError(e));

        // Wait for  the transition ends
        yield return new WaitForSecondsRealtime(6.1f);
        Assert.AreEqual("Assets/U.Universal.Scenes-v1.0.1/Tests/PlayMode/Menu.unity", SceneManager.GetActiveScene().path);


        // Start the transition froom Menu to Level1
        LogAssert.Expect(LogType.Assert, ".*Tests/PlayMode/Menu to .*Tests/PlayMode/Level*");  // -> Trn
        SceneMonitor.Jump(new SceneData("Level1", "U.Universal.Scenes-v1.0.1/Tests/PlayMode/Level1"), new TransitionData
        {
            startDelay = 3,
            endDelay = 3,
        }).Then(Reject: e => Debug.LogError(e));

        // Wait for  the transition ends
        yield return new WaitForSecondsRealtime(8.1f);
        Debug.Log("Endtransition");
        Assert.AreEqual("Assets/U.Universal.Scenes-v1.0.1/Tests/PlayMode/Level1.unity", SceneManager.GetActiveScene().path);
    }
}
