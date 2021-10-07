using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using U.Universal;
using U.Universal.Scenes;
using UnityEngine.SceneManagement;

public class Uscenes_SearchPatterns
{



    private static SceneSelector[] configDev = new SceneSelector[]
    {
        // By name
            new SceneSelector
            {
                pattern = "Start",
            },
            new SceneSelector
            {
                pattern = "Intro",
            },
            new SceneSelector
            {
                pattern = "Game",
            },
            new SceneSelector
            {
                pattern = "*",
            },
            new SceneSelector
            {
                pattern = "*Nitro",
            },
            new SceneSelector
            {
                pattern = "*Vector",
            },
            new SceneSelector
            {
                pattern = "Level*",
            },
            new SceneSelector
            {
                pattern = "Menu*",
            },
            new SceneSelector
            {
                pattern = "Level*s",
            },
            new SceneSelector
            {
                pattern = "Menu*Intro",
            },
            new SceneSelector
            {
                pattern = "*Game*",
            },
            new SceneSelector
            {
                pattern = "*Menu*",
            },
            new SceneSelector
            {
                pattern = "**",
            },
            new SceneSelector
            {
                pattern = "**Game",
            },
            new SceneSelector
            {
                pattern = "Game**",
            },
            new SceneSelector
            {
                pattern = "**re*",
            },
            new SceneSelector
            {
                pattern = "*Vector*Level*",
            },
            new SceneSelector
            {
                pattern = "*re**",
            },
            new SceneSelector
            {
                pattern = "yu**",
            },
            new SceneSelector
            {
                pattern = "Ga**me",
            },
            new SceneSelector
            {
                pattern = "*Game==",
            },

            // By Index
            new SceneSelector
            {
                pattern = "#0",
            },
            new SceneSelector
            {
                pattern = "#3",
            },
            new SceneSelector
            {
                pattern = "#11",
            },
            new SceneSelector
            {
                pattern = "#<3",
            },
            new SceneSelector
            {
                pattern = "#>10",
            },
            new SceneSelector
            {
                pattern = "#<=3",
            },
            new SceneSelector
            {
                pattern = "#>=3",
            },
            new SceneSelector
            {
                pattern = "#3ee3",
            },
            new SceneSelector
            {
                pattern = "#>pooo",
            },
            new SceneSelector
            {
                pattern = "#>=3.22",
            },
            new SceneSelector
            {
                pattern = "#>=3s222s",
            },

            // By path
            new SceneSelector
            {
                pattern = ".Assets/*",
            },
            new SceneSelector
            {
                pattern = ".Assets/Scenes/*",
            },
            new SceneSelector
            {
                pattern = ".Assets/Scenes/Intro",
            },
            new SceneSelector
            {
                pattern = ".*",
            },
            new SceneSelector
            {
                pattern = ".",
            },
            new SceneSelector
            {
                pattern = ".*Scenes/Intro",
            },

            new SceneSelector
            {
                pattern = "*Game|||#2",
            },
            new SceneSelector
            {
                pattern = "*Game&&&#2",
            },

        // Scenes para solo dar el path y que haga referencia a solo una esena y haga lo de
        // solo cargar una a la vez y todo eso ... que sea aparte de los scenes patterns

    };





    // A Test behaves as an ordinary method
    [Test]
    [TestCase("Intro", 2, "Assets/Scenes/Intro.Unity")]
    [TestCase("Start", 2, "Assets/Scenes/Intro.Unity")]
    [TestCase("Game", 2, "Assets/Scenes/Intro.Unity")]
    [TestCase("SuperNitro", 2, "Assets/Scenes/Intro.Unity")]
    [TestCase("VectorLevel", 2, "Assets/Scenes/Intro.Unity")]
    [TestCase("wVectorLevel", 2, "Assets/Scenes/Intro.Unity")]
    [TestCase("VectorwLevel", 2, "Assets/Scenes/Intro.Unity")]
    [TestCase("VectorLevelw", 2, "Assets/Scenes/Intro.Unity")]
    [TestCase("LevelVector", 2, "Assets/Scenes/Intro.Unity")]
    [TestCase("MegaVector", 2, "Assets/Scenes/Intro.Unity")]
    [TestCase("Level11", 2, "Assets/Scenes/Intro.Unity")]
    [TestCase("Level21", 2, "Assets/Scenes/Intro.Unity")]
    [TestCase("Menu3", 2, "Assets/Scenes/Intro.Unity")]
    [TestCase("Level222", 2, "Assets/Scenes/Intro.Unity")]
    [TestCase("23Levels", 2, "Assets/Scenes/Intro.Unity")]

    [TestCase("Dog", 0, "Assets/Scenes/Intro.Unity")]
    [TestCase("Dog", 3, "Assets/Scenes/Intro.Unity")]
    [TestCase("Dog", 11, "Assets/Scenes/Intro.Unity")]
    [TestCase("Dog", 2, "Assets/Scenes/Intro.Unity")]
    [TestCase("Dog", 40, "Assets/Scenes/Intro.Unity")]

    [TestCase("Dog", 40, "Assets/Intro.unity")]
    [TestCase("Dog", 40, "Assets/Scenes/Menu.unity")]
    [TestCase("Dog", 40, "Assets/Scenes/Intro.unity")]
    [TestCase("Dog", 40, "Assets/Perro/Gato/ScenesIntro.unity")]
    [TestCase("Dog", 40, "Perro/Scenes/Intro.unity")]
    public void Uscenes_SearchPatternsSimplePasses(string name, int buildIndex, string path)
    {
        var scene = new SceneData(name, buildIndex, path);

        var search = SceneMonitor.SearchSelectors(scene, configDev);

        Debug.Log("Scene: " + name + " " + buildIndex + " " + path);

        foreach (var item in search)
        {
            Debug.Log("PSelector: " + item.Pattern());
        }

    }


}
