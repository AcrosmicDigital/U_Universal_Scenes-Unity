using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Reflection;
using System;
using System.Linq;
using U.Universal.Scenes;
using System.IO;


#if UNITY_EDITOR

public class CreateJumpsFileMenuButton : EditorWindow
{

    private static string EnvFolderName => "/Scripts/ScenesManagment/Uscenes/";
    private static string ScenesFileName => "Jumps.cs";
    private static string FormatLog(string text) => "UniversalScenes: " + text;


    [MenuItem("U/Universal Scenes/Create Jumps File")]
    public static void ShowWindow()
    {

        // Check if Env folder exist or create it
        if (!Directory.Exists(Application.dataPath + EnvFolderName))
        {
            Debug.Log(FormatLog("Creating Assets/" + EnvFolderName + " directory"));
            Directory.CreateDirectory(Application.dataPath + EnvFolderName);
        }
        else
        {
            Debug.Log(FormatLog("Assets/" + EnvFolderName + " already exist"));
        }

        // check if scenes file exist or create it
        if (!File.Exists(Application.dataPath + EnvFolderName + ScenesFileName))
        {
            Debug.Log(FormatLog("Creating Assets/" + EnvFolderName + ScenesFileName + " file"));

            // Write the file
            File.WriteAllLines(Application.dataPath + EnvFolderName + ScenesFileName, file); // This should be async when is available

            // Compile
            AssetDatabase.Refresh();
        }
        else
        {
            Debug.LogError(FormatLog("Assets/" + EnvFolderName + ScenesFileName + " already exist"));
        }

    }

    const string quote = "\"";
    static string[] file =
    {
        "using System.Threading.Tasks;",
        "using U.Universal.Scenes;",
        "",
        "public static partial class Uscenes",
        "{",
        "    public static partial class Jumps",
        "    {",
        "        /*",
        "        public static Task<SceneOperation> ToIntro() =>",
        "            SceneMonitor.Jump(Env.Scenes.Intro, new TransitionData",
        "            {",
        "                //delay = 2,",
        "                startDelay = 1,",
        "                endDelay = 1,",
        "                //cancelString = "+quote+"Tr"+quote+",",
        "                transitionMode = SceneMonitor.JumpMode.Absolute,",
        "            });",
        "",
        "        public static Task<SceneOperation> LoadAnimation() =>",
        "            SceneMonitor.Load(Env.Scenes.Animation);",
        "",
        "        public static Task<SceneOperation> UnloadAnimation() =>",
        "            SceneMonitor.Unload(Env.Scenes.Animation);",
        "        */",
        "",
        "    }",
        "}",
    };

}



#endif
