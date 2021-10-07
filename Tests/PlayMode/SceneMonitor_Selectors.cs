using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using NUnit.Framework;
using U.Universal;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using U.Universal.Scenes;

public class SceneMonitor_Selectors
{

    

    [OneTimeSetUp]
    public void OneTimeSetUp() { }


    [SetUp]
    public void SetUp() 
    {
        SceneMonitor.RemoveAllSelectors();
        SceneMonitor.RemoveAllTransitions();
        SceneMonitor.Disable();
    }



    [UnityTest]
    public IEnumerator Selectors_111onInitializeDelegatePassed()
    {
        
        SceneSelector selector = new SceneSelector
        {
            pattern = "*",
            onEnable = (s) => Debug.LogAssertion("First Scene Loaded"),
        };

        SceneSelector selector2 = new SceneSelector
        {
            pattern = "*",
            onEnable = (s) => Debug.LogAssertion("First Scene Loaded Two"),
        };

        // All test must start in intro scene
        var asyncoperation = SceneManager.LoadSceneAsync("Intro", LoadSceneMode.Single);
        while (!asyncoperation.isDone) yield return null;

        LogAssert.Expect(LogType.Assert, "First Scene Loaded");
        LogAssert.Expect(LogType.Assert, "First Scene Loaded Two");

        SceneMonitor.Apply(selector, selector2);

        SceneMonitor.Enable();


        yield return new WaitForSecondsRealtime(1);

        // Change scene
        var asyncoperation2 = SceneManager.LoadSceneAsync("Menu", LoadSceneMode.Single);
        while (!asyncoperation2.isDone) yield return null;

        yield return new WaitForSecondsRealtime(1);

    }

    [UnityTest]
    public IEnumerator Selectors_112OnlyFirstSceneDelegatePassed_ButDisabled()
    {
        SceneSelector config = new SceneSelector
        {
            pattern = "*",
            onEnable = (s) => Debug.LogAssertion("First Scene Loaded"),
        };

        // All test must start in intro scene
        var asyncoperation = SceneManager.LoadSceneAsync("Intro", LoadSceneMode.Single);
        while (!asyncoperation.isDone) yield return null;

        SceneMonitor.Apply(config);
        //SceneMonitor.Enable();

        yield return new WaitForSecondsRealtime(1);

        // Change scene
        var asyncoperation2 = SceneManager.LoadSceneAsync("Menu", LoadSceneMode.Single);
        while (!asyncoperation2.isDone) yield return null;

        yield return new WaitForSecondsRealtime(1);

    }

    [UnityTest]
    public IEnumerator Selectors_113OnlyFirstSceneDelegatePassed_ButError()
    {
        SceneSelector config = new SceneSelector
        {
            pattern = "*",
            onEnable = (s) => throw new Exception("First Scene Loaded"),
        };

        // All test must start in intro scene
        var asyncoperation = SceneManager.LoadSceneAsync("Intro", LoadSceneMode.Single);
        while (!asyncoperation.isDone) yield return null;

        LogAssert.Expect(LogType.Error, new Regex("Error executing delegate onEnable"));
        SceneMonitor.Apply(config);
        SceneMonitor.Enable();


        yield return new WaitForSecondsRealtime(1);

        // Change scene
        var asyncoperation2 = SceneManager.LoadSceneAsync("Menu", LoadSceneMode.Single);
        while (!asyncoperation2.isDone) yield return null;

        yield return new WaitForSecondsRealtime(1);

    }




    [UnityTest]
    public IEnumerator Selectors_211SelectorWithNoPatternWontExecute_ExceptOnEnable()
    {
        SceneSelector[] config = new SceneSelector[]
        {
            new SceneSelector
            {
                //pattern = "*",
                onEnable = (s) => Debug.LogAssertion("First Scene Loaded"),
            },
            new SceneSelector
            {
                // pattern = "Intro",
                onLoad  = (s) => Debug.LogAssertion("OnLoad"),
                onUnload  = (s) => Debug.LogAssertion("OnUnload"),
                onSetActive  = (s) => Debug.LogAssertion("OnSetActive"),
                onSetInactive  = (s) => Debug.LogAssertion("OnSetInactive"),
            },
            new SceneSelector
            {
                // pattern = "Intro",
                onLoad  = (s) => Debug.LogAssertion("OnLoad"),
            }
        };

        // All test must start in intro scene
        var asyncoperation = SceneManager.LoadSceneAsync("Intro", LoadSceneMode.Single);
        while (!asyncoperation.isDone) yield return null;

        
        SceneMonitor.Apply(config);
        SceneMonitor.Enable();

        yield return new WaitForSecondsRealtime(1);

        // Change scene
        var asyncoperation2 = SceneManager.LoadSceneAsync("Menu", LoadSceneMode.Single);
        while (!asyncoperation2.isDone) yield return null;

        yield return new WaitForSecondsRealtime(1);

    }

    [UnityTest]
    public IEnumerator Selectors_212OnePatternByName()
    {
        SceneSelector[] config = new SceneSelector[]
        {
            new SceneSelector
            {
                pattern = "*",
                onEnable = (s) => Debug.LogAssertion("First Scene Loaded"),
            },
            new SceneSelector
            {
                pattern = "Intro",
                onLoad  = (s) => Debug.LogAssertion("OnLoad Intro"),
                onUnload  = (s) => Debug.LogAssertion("OnUnload Intro"),
                onSetActive  = (s) => Debug.LogAssertion("OnSetActive Intro"),
                onSetInactive  = (s) => Debug.LogAssertion("OnSetInactive Intro"),
            },
            new SceneSelector
            {
                pattern = "Menu",
                onLoad  = (s) => Debug.LogAssertion("OnLoad Menu"),
                onUnload  = (s) => Debug.LogAssertion("OnUnload Menu"),
                onSetActive  = (s) => Debug.LogAssertion("OnSetActive Menu"),
                onSetInactive  = (s) => Debug.LogAssertion("OnSetInactive Menu"),
            },
        };

        // All test must start in intro scene
        var asyncoperation = SceneManager.LoadSceneAsync("Intro", LoadSceneMode.Single);
        while (!asyncoperation.isDone) yield return null;

        LogAssert.Expect(LogType.Assert, "First Scene Loaded");

        LogAssert.Expect(LogType.Assert, "OnLoad Intro");
        LogAssert.Expect(LogType.Assert, "OnSetActive Intro");
        SceneMonitor.Apply(config);
        SceneMonitor.Enable();

        yield return new WaitForSecondsRealtime(5);

        // Change scene
        LogAssert.Expect(LogType.Assert, "OnUnload Intro");
        LogAssert.Expect(LogType.Assert, "OnSetActive Menu");
        LogAssert.Expect(LogType.Assert, "OnLoad Menu");
        var asyncoperation2 = SceneManager.LoadSceneAsync("Menu", LoadSceneMode.Single);
        while (!asyncoperation2.isDone) yield return null;

        yield return new WaitForSecondsRealtime(1);

       
    }

    [UnityTest]
    public IEnumerator Selectors_213OnePatternByName2()
    {
        SceneSelector[] config = new SceneSelector[]
        {
            new SceneSelector
            {
                pattern = "*",
                onEnable = (s) => Debug.LogAssertion("First Scene Loaded"),
            },
            new SceneSelector
            {
                pattern = "Level*",
                onLoad  = (s) => Debug.LogAssertion("OnLoad Level"),
                onUnload  = (s) => Debug.LogAssertion("OnUnload Level"),
                onSetActive  = (s) => Debug.LogAssertion("OnSetActive Level"),
                onSetInactive  = (s) => Debug.LogAssertion("OnSetInactive Level"),
            },
            new SceneSelector
            {
                pattern = "Level22",
                onLoad  = (s) => Debug.LogAssertion("OnLoad Level22"),
                onUnload  = (s) => Debug.LogAssertion("OnUnload Level22"),
                onSetActive  = (s) => Debug.LogAssertion("OnSetActive Level22"),
                onSetInactive  = (s) => Debug.LogAssertion("OnSetInactive Level22"),
            },
        };

        // All test must start in intro scene
        var asyncoperation = SceneManager.LoadSceneAsync("Intro", LoadSceneMode.Single);
        while (!asyncoperation.isDone) yield return null;

        LogAssert.Expect(LogType.Assert, "First Scene Loaded");
        SceneMonitor.Apply(config);
        SceneMonitor.Enable();

        yield return new WaitForSecondsRealtime(2);

        // Change scene
        LogAssert.Expect(LogType.Assert, "OnSetActive Level");
        LogAssert.Expect(LogType.Assert, "OnLoad Level");
        var asyncoperation2 = SceneManager.LoadSceneAsync("Level1", LoadSceneMode.Single);
        while (!asyncoperation2.isDone) yield return null;

        yield return new WaitForSecondsRealtime(1);

        // Change scene
        LogAssert.Expect(LogType.Assert, "OnUnload Level");
        LogAssert.Expect(LogType.Assert, "OnSetActive Level");
        LogAssert.Expect(LogType.Assert, "OnLoad Level");
        var asyncoperation3 = SceneManager.LoadSceneAsync("Level3", LoadSceneMode.Single);
        while (!asyncoperation3.isDone) yield return null;

        yield return new WaitForSecondsRealtime(1);

        // Change scene
        LogAssert.Expect(LogType.Assert, "OnUnload Level");
        LogAssert.Expect(LogType.Assert, "OnSetActive Level");
        LogAssert.Expect(LogType.Assert, "OnSetActive Level22");
        LogAssert.Expect(LogType.Assert, "OnLoad Level");
        LogAssert.Expect(LogType.Assert, "OnLoad Level22");
        var asyncoperation4 = SceneManager.LoadSceneAsync("Level22", LoadSceneMode.Single);
        while (!asyncoperation4.isDone) yield return null;

        yield return new WaitForSecondsRealtime(1);
    }

    [UnityTest]
    public IEnumerator Selectors_214AndPatternByName3()
    {
        SceneSelector[] config = new SceneSelector[]
        {
            new SceneSelector
            {
                pattern = "*",
                onEnable = (s) => Debug.LogAssertion("First Scene Loaded"),
            },
            new SceneSelector
            {
                pattern = "Level*&&*3*",
                onLoad  = (s) => Debug.LogAssertion("OnLoad Level"),
                onUnload  = (s) => Debug.LogAssertion("OnUnload Level"),
                onSetActive  = (s) => Debug.LogAssertion("OnSetActive Level"),
                onSetInactive  = (s) => Debug.LogAssertion("OnSetInactive Level"),
            },
            new SceneSelector
            {
                pattern = "Level*&&*22",
                onLoad  = (s) => Debug.LogAssertion("OnLoad Level22"),
                onUnload  = (s) => Debug.LogAssertion("OnUnload Level22"),
                onSetActive  = (s) => Debug.LogAssertion("OnSetActive Level22"),
                onSetInactive  = (s) => Debug.LogAssertion("OnSetInactive Level22"),
            },
        };

        // All test must start in intro scene
        var asyncoperation = SceneManager.LoadSceneAsync("Intro", LoadSceneMode.Single);
        while (!asyncoperation.isDone) yield return null;

        LogAssert.Expect(LogType.Assert, "First Scene Loaded");
        SceneMonitor.Apply(config);
        SceneMonitor.Enable();

        yield return new WaitForSecondsRealtime(2);

        // Change scene
        var asyncoperation2 = SceneManager.LoadSceneAsync("Level1", LoadSceneMode.Single);
        while (!asyncoperation2.isDone) yield return null;

        yield return new WaitForSecondsRealtime(1);

        // Change scene
        LogAssert.Expect(LogType.Assert, "OnSetActive Level");
        LogAssert.Expect(LogType.Assert, "OnLoad Level");
        var asyncoperation3 = SceneManager.LoadSceneAsync("Level3", LoadSceneMode.Single);
        while (!asyncoperation3.isDone) yield return null;

        yield return new WaitForSecondsRealtime(1);

        // Change scene
        LogAssert.Expect(LogType.Assert, "OnUnload Level");
        LogAssert.Expect(LogType.Assert, "OnSetActive Level22");
        LogAssert.Expect(LogType.Assert, "OnLoad Level22");
        var asyncoperation4 = SceneManager.LoadSceneAsync("Level22", LoadSceneMode.Single);
        while (!asyncoperation4.isDone) yield return null;

        yield return new WaitForSecondsRealtime(1);
    }

    [UnityTest]
    public IEnumerator Selectors_215OneByIndex()
    {
        SceneSelector[] config = new SceneSelector[]
        {
            new SceneSelector
            {
                pattern = "*",
                onEnable = (s) => Debug.LogAssertion("First Scene Loaded"),
            },
            new SceneSelector
            {
                pattern = "#0",
                onLoad  = (s) => Debug.LogAssertion("OnLoad 0"),
                onUnload  = (s) => Debug.LogAssertion("OnUnload 0"),
                onSetActive  = (s) => Debug.LogAssertion("OnSetActive 0"),
                onSetInactive  = (s) => Debug.LogAssertion("OnSetInactive 0"),
            },
            new SceneSelector
            {
                pattern = "#>=1",
                onLoad  = (s) => Debug.LogAssertion("OnLoad 1"),
                onUnload  = (s) => Debug.LogAssertion("OnUnload 1"),
                onSetActive  = (s) => Debug.LogAssertion("OnSetActive 1"),
                onSetInactive  = (s) => Debug.LogAssertion("OnSetInactive 1"),
            },
        };

        // All test must start in intro scene
        var asyncoperation = SceneManager.LoadSceneAsync(0, LoadSceneMode.Single);
        while (!asyncoperation.isDone) yield return null;

        LogAssert.Expect(LogType.Assert, "First Scene Loaded");

        LogAssert.Expect(LogType.Assert, "OnLoad 0");
        LogAssert.Expect(LogType.Assert, "OnSetActive 0");
        SceneMonitor.Apply(config);
        SceneMonitor.Enable();

        yield return new WaitForSecondsRealtime(5);

        // Change scene
        LogAssert.Expect(LogType.Assert, "OnUnload 0");
        LogAssert.Expect(LogType.Assert, "OnSetActive 1");
        LogAssert.Expect(LogType.Assert, "OnLoad 1");
        var asyncoperation2 = SceneManager.LoadSceneAsync(1, LoadSceneMode.Single);
        while (!asyncoperation2.isDone) yield return null;

        yield return new WaitForSecondsRealtime(1);


    }

    [UnityTest]
    public IEnumerator Selectors_216OnePatternByPath()
    {
        SceneSelector[] config = new SceneSelector[]
        {
            new SceneSelector
            {
                pattern = "*",
                onEnable = (s) => Debug.LogAssertion("First Scene Loaded"),
            },
            new SceneSelector
            {
                pattern = ".*/Tests/PlayMode/Intro",
                onLoad  = (s) => Debug.LogAssertion("OnLoad Intro"),
                onUnload  = (s) => Debug.LogAssertion("OnUnload Intro"),
                onSetActive  = (s) => Debug.LogAssertion("OnSetActive Intro"),
                onSetInactive  = (s) => Debug.LogAssertion("OnSetInactive Intro"),
            },
            new SceneSelector
            {
                pattern = ".*/Tests/PlayMode/Menu",
                onLoad  = (s) => Debug.LogAssertion("OnLoad Menu"),
                onUnload  = (s) => Debug.LogAssertion("OnUnload Menu"),
                onSetActive  = (s) => Debug.LogAssertion("OnSetActive Menu"),
                onSetInactive  = (s) => Debug.LogAssertion("OnSetInactive Menu"),
            },
        };

        // All test must start in intro scene
        var asyncoperation = SceneManager.LoadSceneAsync("Intro", LoadSceneMode.Single);
        while (!asyncoperation.isDone) yield return null;

        LogAssert.Expect(LogType.Assert, "First Scene Loaded");

        LogAssert.Expect(LogType.Assert, "OnLoad Intro");
        LogAssert.Expect(LogType.Assert, "OnSetActive Intro");
        SceneMonitor.Apply(config);
        SceneMonitor.Enable();

        yield return new WaitForSecondsRealtime(5);

        // Change scene
        LogAssert.Expect(LogType.Assert, "OnUnload Intro");
        LogAssert.Expect(LogType.Assert, "OnSetActive Menu");
        LogAssert.Expect(LogType.Assert, "OnLoad Menu");
        var asyncoperation2 = SceneManager.LoadSceneAsync("Menu", LoadSceneMode.Single);
        while (!asyncoperation2.isDone) yield return null;

        yield return new WaitForSecondsRealtime(1);


    }



    [UnityTest]
    public IEnumerator Selectors_311PassingSingleSelector()
    {

        SceneSelector selector = new SceneSelector
        {
            pattern = "*",
            onEnable = (s) => Debug.LogAssertion("First Scene Loaded"),
        };

        // All test must start in intro scene
        var asyncoperation = SceneManager.LoadSceneAsync("Intro", LoadSceneMode.Single);
        while (!asyncoperation.isDone) yield return null;

        LogAssert.Expect(LogType.Assert, "First Scene Loaded");

        SceneMonitor.Apply(selector);

        SceneMonitor.Enable();


        yield return new WaitForSecondsRealtime(1);

        // Change scene
        var asyncoperation2 = SceneManager.LoadSceneAsync("Menu", LoadSceneMode.Single);
        while (!asyncoperation2.isDone) yield return null;

        yield return new WaitForSecondsRealtime(1);

    }

    [UnityTest]
    public IEnumerator Selectors_312PassingManySingleSelector()
    {

        SceneSelector selector = new SceneSelector
        {
            pattern = "*",
            onEnable = (s) => Debug.LogAssertion("First Scene Loaded"),
        };

        SceneSelector selector2 = new SceneSelector
        {
            pattern = "*",
            onEnable = (s) => Debug.LogAssertion("First Scene Loaded Two"),
        };

        SceneSelector selector3 = new SceneSelector
        {
            pattern = "*",
            onEnable = (s) => Debug.LogAssertion("First Scene Loaded Three"),
        };

        SceneSelector selector4 = new SceneSelector
        {
            pattern = "*",
            onEnable = (s) => Debug.LogAssertion("First Scene Loaded Four"),
        };

        // All test must start in intro scene
        var asyncoperation = SceneManager.LoadSceneAsync("Intro", LoadSceneMode.Single);
        while (!asyncoperation.isDone) yield return null;

        LogAssert.Expect(LogType.Assert, "First Scene Loaded");
        LogAssert.Expect(LogType.Assert, "First Scene Loaded Two");
        LogAssert.Expect(LogType.Assert, "First Scene Loaded Three");
        LogAssert.Expect(LogType.Assert, "First Scene Loaded Four");

        SceneMonitor.Apply(selector, selector2, selector3, selector4);

        SceneMonitor.Enable();


        yield return new WaitForSecondsRealtime(1);

        // Change scene
        var asyncoperation2 = SceneManager.LoadSceneAsync("Menu", LoadSceneMode.Single);
        while (!asyncoperation2.isDone) yield return null;

        yield return new WaitForSecondsRealtime(1);

    }

    [UnityTest]
    public IEnumerator Selectors_313PassingListOfSelectors()
    {

        var config = new SceneSelector[]
        {
            new SceneSelector
            {
                pattern = "*",
                onEnable = (s) => Debug.LogAssertion("First Scene Loaded"),
            },
            new SceneSelector
            {
                pattern = "*",
                onEnable = (s) => Debug.LogAssertion("First Scene Loaded Two"),
            },
        };

        // All test must start in intro scene
        var asyncoperation = SceneManager.LoadSceneAsync("Intro", LoadSceneMode.Single);
        while (!asyncoperation.isDone) yield return null;

        LogAssert.Expect(LogType.Assert, "First Scene Loaded");
        LogAssert.Expect(LogType.Assert, "First Scene Loaded Two");

        SceneMonitor.Apply(config);

        SceneMonitor.Enable();


        yield return new WaitForSecondsRealtime(1);

        // Change scene
        var asyncoperation2 = SceneManager.LoadSceneAsync("Menu", LoadSceneMode.Single);
        while (!asyncoperation2.isDone) yield return null;

        yield return new WaitForSecondsRealtime(1);

    }

    [UnityTest]
    public IEnumerator Selectors_314PassingManyListsOfSelectors()
    {

        var config = new SceneSelector[]
        {
            new SceneSelector
            {
                pattern = "*",
                onEnable = (s) => Debug.LogAssertion("First Scene Loaded"),
            },
            new SceneSelector
            {
                pattern = "*",
                onEnable = (s) => Debug.LogAssertion("First Scene Loaded Two"),
            },
        };

        var config2 = new SceneSelector[]
        {
            new SceneSelector
            {
                pattern = "*",
                onEnable = (s) => Debug.LogAssertion("First Scene Loaded Three"),
            },
            new SceneSelector
            {
                pattern = "*",
                onEnable = (s) => Debug.LogAssertion("First Scene Loaded Four"),
            },
        };

        // All test must start in intro scene
        var asyncoperation = SceneManager.LoadSceneAsync("Intro", LoadSceneMode.Single);
        while (!asyncoperation.isDone) yield return null;

        LogAssert.Expect(LogType.Assert, "First Scene Loaded");
        LogAssert.Expect(LogType.Assert, "First Scene Loaded Two");
        LogAssert.Expect(LogType.Assert, "First Scene Loaded Three");
        LogAssert.Expect(LogType.Assert, "First Scene Loaded Four");

        SceneMonitor.Apply(config, config2);

        SceneMonitor.Enable();


        yield return new WaitForSecondsRealtime(1);

        // Change scene
        var asyncoperation2 = SceneManager.LoadSceneAsync("Menu", LoadSceneMode.Single);
        while (!asyncoperation2.isDone) yield return null;

        yield return new WaitForSecondsRealtime(1);

    }

    [UnityTest]
    public IEnumerator Selectors_315PassingManyTimesTheSameSelector_WillRunMultipleTimesTheSame()
    {

        SceneSelector selector = new SceneSelector
        {
            pattern = "*",
            onEnable = (s) => Debug.LogAssertion("First Scene Loaded"),
        };

        // All test must start in intro scene
        var asyncoperation = SceneManager.LoadSceneAsync("Intro", LoadSceneMode.Single);
        while (!asyncoperation.isDone) yield return null;

        LogAssert.Expect(LogType.Assert, "First Scene Loaded");
        LogAssert.Expect(LogType.Assert, "First Scene Loaded");
        LogAssert.Expect(LogType.Assert, "First Scene Loaded");

        SceneMonitor.Apply(selector, selector, selector);

        SceneMonitor.Enable();


        yield return new WaitForSecondsRealtime(1);

        // Change scene
        var asyncoperation2 = SceneManager.LoadSceneAsync("Menu", LoadSceneMode.Single);
        while (!asyncoperation2.isDone) yield return null;

        yield return new WaitForSecondsRealtime(1);

    }



    [UnityTest]
    public IEnumerator Selectors_411RemovingSingleSelector()
    {

        SceneSelector selector = new SceneSelector
        {
            pattern = "*",
            onEnable = (s) => Debug.LogAssertion("First Scene Loaded"),
        };

        SceneMonitor.Apply(selector);
        Assert.AreEqual(1, SceneMonitor.CountSelectors);


        SceneMonitor.Remove(selector);
        Assert.AreEqual(0, SceneMonitor.CountSelectors);


        yield return new WaitForSecondsRealtime(1);

    }

    [UnityTest]
    public IEnumerator Selectors_412RemovingManySingleSelector()
    {

        SceneSelector selector = new SceneSelector
        {
            pattern = "*",
            onEnable = (s) => Debug.LogAssertion("First Scene Loaded"),
        };

        SceneSelector selector2 = new SceneSelector
        {
            pattern = "*",
            onEnable = (s) => Debug.LogAssertion("First Scene Loaded Two"),
        };

        SceneSelector selector3 = new SceneSelector
        {
            pattern = "*",
            onEnable = (s) => Debug.LogAssertion("First Scene Loaded Three"),
        };

        SceneSelector selector4 = new SceneSelector
        {
            pattern = "*",
            onEnable = (s) => Debug.LogAssertion("First Scene Loaded Four"),
        };

        SceneMonitor.Apply(selector, selector2, selector3, selector4);
        Assert.AreEqual(4, SceneMonitor.CountSelectors);


        SceneMonitor.Remove(selector, selector2, selector3, selector4);
        Assert.AreEqual(0, SceneMonitor.CountSelectors);


        yield return new WaitForSecondsRealtime(1);

    }

    [UnityTest]
    public IEnumerator Selectors_413RemovingListOfSelectors()
    {

        var config = new SceneSelector[]
        {
            new SceneSelector
            {
                pattern = "*",
                onEnable = (s) => Debug.LogAssertion("First Scene Loaded"),
            },
            new SceneSelector
            {
                pattern = "*",
                onEnable = (s) => Debug.LogAssertion("First Scene Loaded Two"),
            },
        };

        SceneMonitor.Apply(config);
        Assert.AreEqual(2, SceneMonitor.CountSelectors);


        SceneMonitor.Remove(config);
        Assert.AreEqual(0, SceneMonitor.CountSelectors);


        yield return new WaitForSecondsRealtime(1);

    }

    [UnityTest]
    public IEnumerator Selectors_414RemovingManyListsOfSelectors()
    {

        var config = new SceneSelector[]
        {
            new SceneSelector
            {
                pattern = "Intro",
                onEnable = (s) => Debug.LogAssertion("First Scene Loaded"),
            },
            new SceneSelector
            {
                pattern = "Menu",
                onEnable = (s) => Debug.LogAssertion("First Scene Loaded Two"),
            },
        };

        var config2 = new SceneSelector[]
        {
            new SceneSelector
            {
                pattern = "Play",
                onEnable = (s) => Debug.LogAssertion("First Scene Loaded Three"),
            },
            new SceneSelector
            {
                pattern = "Game",
                onEnable = (s) => Debug.LogAssertion("First Scene Loaded Four"),
            },
        };

        SceneMonitor.Apply(config, config2);
        Assert.AreEqual(4, SceneMonitor.CountSelectors);


        SceneMonitor.Remove(config, config2);
        Assert.AreEqual(0, SceneMonitor.CountSelectors);


        yield return new WaitForSecondsRealtime(.1f);

    }

    [UnityTest]
    public IEnumerator Selectors_414RemovingByPattern()
    {

        var config = new SceneSelector[]
        {
            new SceneSelector
            {
                pattern = "Intro",
                onEnable = (s) => Debug.LogAssertion("First Scene Loaded"),
            },
            new SceneSelector
            {
                pattern = "Menu",
                onEnable = (s) => Debug.LogAssertion("First Scene Loaded Two"),
            },
        };

        var config2 = new SceneSelector[]
        {
            new SceneSelector
            {
                pattern = "Play",
                onEnable = (s) => Debug.LogAssertion("First Scene Loaded Three"),
            },
            new SceneSelector
            {
                pattern = "Game",
                onEnable = (s) => Debug.LogAssertion("First Scene Loaded Four"),
            },
        };

        SceneMonitor.Apply(config, config2);
        Assert.AreEqual(4, SceneMonitor.CountSelectors);


        SceneMonitor.Remove("Intro");
        Assert.AreEqual(3, SceneMonitor.CountSelectors);


        SceneMonitor.Remove("Menu", "Play");
        Assert.AreEqual(1, SceneMonitor.CountSelectors);


        yield return new WaitForSecondsRealtime(.1f);

    }

    [UnityTest]
    public IEnumerator Selectors_415RemovingByPatternInLList()
    {

        var config = new SceneSelector[]
        {
            new SceneSelector
            {
                pattern = "Intro",
                onEnable = (s) => Debug.LogAssertion("First Scene Loaded"),
            },
            new SceneSelector
            {
                pattern = "Menu",
                onEnable = (s) => Debug.LogAssertion("First Scene Loaded Two"),
            },
        };

        var config2 = new SceneSelector[]
        {
            new SceneSelector
            {
                pattern = "Play",
                onEnable = (s) => Debug.LogAssertion("First Scene Loaded Three"),
            },
            new SceneSelector
            {
                pattern = "Game",
                onEnable = (s) => Debug.LogAssertion("First Scene Loaded Four"),
            },
        };

        SceneMonitor.Apply(config, config2);
        Assert.AreEqual(4, SceneMonitor.CountSelectors);


        SceneMonitor.Remove(new string[] { "Intro", "Menu", "Play" });
        Assert.AreEqual(1, SceneMonitor.CountSelectors);


        yield return new WaitForSecondsRealtime(.1f);

    }

    [UnityTest]
    public IEnumerator Selectors_416RemovingAll()
    {

        var config = new SceneSelector[]
        {
            new SceneSelector
            {
                pattern = "Intro",
                onEnable = (s) => Debug.LogAssertion("First Scene Loaded"),
            },
            new SceneSelector
            {
                pattern = "Menu",
                onEnable = (s) => Debug.LogAssertion("First Scene Loaded Two"),
            },
        };

        var config2 = new SceneSelector[]
        {
            new SceneSelector
            {
                pattern = "Play",
                onEnable = (s) => Debug.LogAssertion("First Scene Loaded Three"),
            },
            new SceneSelector
            {
                pattern = "Game",
                onEnable = (s) => Debug.LogAssertion("First Scene Loaded Four"),
            },
        };

        SceneMonitor.Apply(config, config2);
        Assert.AreEqual(4, SceneMonitor.CountSelectors);


        SceneMonitor.RemoveAllSelectors();
        Assert.AreEqual(0, SceneMonitor.CountSelectors);


        yield return new WaitForSecondsRealtime(.1f);

    }
}
