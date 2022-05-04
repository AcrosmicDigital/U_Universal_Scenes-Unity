using UnityEditor;
using static U.Universal.Scenes.Editor.UE;

namespace U.Universal.Scenes.Editor
{
    public class CreateStartupFilesMenuButton : EditorWindow
    {

        #region Startup File
        private static string startupFolderName => "/Scripts/Control/Startup/";
        private static string startupFileName => "SceneMonitor.startup.cs";
        private static readonly string[] startupFile =
        {
            "using UnityEngine;",
            "using U.Universal.Scenes;",
            "using static Uscenes;",
            "",
            "/// <summary>",
            "/// Dont edit this file, to change stage or enable, edit Assets/Scripts/Env/Vars/DevStage.cs",
            "/// </summary>",
            "public static partial class Startup  // Dir: Assets/Scripts/Uscenes/Startup.cs",
            "{",
            "    public static bool isSceneMonitorStartupReady { get; private set; } = false;",
            "",
            "    public static void SceneMonitorStartup()",
            "    {",
            "        if (isSceneMonitorStartupReady) return;",
            "",
            "        // Code here ...",
            "",
            "        // Apply selectors",
            "        SceneMonitor.Apply(SelectorsList);",
            "",
            "        // Apply transitions",
            "        SceneMonitor.Apply(TransitionsList);",
            "",
            "        // Enable",
            "        SceneMonitor.Enable();",
            "",
            "        // End code",
            "        isSceneMonitorStartupReady = true;",
            "    }",
            "",
            "}",
        };
        #endregion Startup File

        #region SelectorsList File
        private static string selectorsListFolderName => "/Scripts/Control/ScenesManagment/";
        private static string selectorsListFileName => "SelectorsList.cs";
        private static readonly string[] selectorsListFile =
        {
            "using U.Universal.Scenes;",
            "using UnityEngine;",
            "",
            "public static partial class Uscenes",
            "{",
            "    public static ISceneSelector[] SelectorsList => new ISceneSelector[]",
            "    {",
            "",
            "        // List of selectors",
            "        //new SceneSelector",
            "        //{",
            "        //    pattern = "+quote+"*"+quote+",",
            "        //    onLoad = (s) => Debug.Log("+quote+"Uscenes: SampleSelector onLoad "+quote+" + s.name),",
            "        //    onSetActive = (s) => Debug.Log("+quote+"Uscenes: SampleSelector onSetActive "+quote+" + s.name),",
            "        //    onSetInactive = (s) => Debug.Log("+quote+"Uscenes: SampleSelector onSetInactive "+quote+" + s.name),",
            "        //    onUnload = (s) => Debug.Log("+quote+"Uscenes: SampleSelector onUnload "+quote+" + s.name),",
            "        //},",
            "        // ... Add here selectors",
            "",
            "",
            "    };",
            "}",
        };
        #endregion SelectorsList File

        #region TransitionsList File
        private static string prodTransitionsListFolderName => "/Scripts/Control/ScenesManagment/Transitions/";
        private static string prodTransitionsListFileName => "TransitionsList.cs";
        private static readonly string[] prodTransitionsListFile =
        {
            "using U.Universal.Scenes;",
            "",
            "public static partial class Uscenes",
            "{",
            "    public static ISceneTransition[] TransitionsList => new ISceneTransition[]",
            "    {",
            "",
            "        // List of prod transitions",
            "        new Transitions.SampleTransition(),",
            "        // ... Add here transitions to use when prod stage is selected",
            "        //     To create  a new transition template go to Menu Bar -> U -> Universal Scenes -> Create Transition",
            "",
            "",
            "    };",
            "}",
        };
        #endregion TransitionsList File

        #region SampleTransition File
        private static string sampleTransitionFolderName => "/Scripts/Control/ScenesManagment/Transitions/";
        private static string sampleTransitionFileName => "Sample.transition.cs";
        private static readonly string[] sampleTransitionFile =
        {
            "using UnityEngine;",
            "using U.Universal.Scenes;",
            "using UnityEngine.UI;",
            "",
            "/// <summary>",
            "/// This is just a sample transition, you cad edit or remove,",
            "/// if you remove this transition remove from transitions list too",
            "/// </summary>",
            "public static partial class Uscenes",
            "{",
            "    public static partial class Transitions",
            "    {",
            "        public class SampleTransition : ISceneTransition",
            "        {",
            "",
            "            // Patterns",
            "            public string CurrentScenePattern() => "+quote+"*"+quote+";",
            "            public string NextScenePattern() => "+quote+"*"+quote+";",
            "",
            "",
            "            // Props",
            "            private GameObject blackScreenGO;",
            "            private CanvasGroup canvasGroupCmp;",
            "",
            "            public void SetUp()",
            "            {",
            "",
            "                // Create the black screen object with alpha = 0 in DontDestroyOnLoad",
            "                blackScreenGO = new GameObject("+quote+"Black Screen"+quote+");",
            "                UnityEngine.Object.DontDestroyOnLoad(blackScreenGO);",
            "                // Canvas cmp",
            "                var canvasCmp = blackScreenGO.AddComponent<Canvas>();",
            "                canvasCmp.renderMode = RenderMode.ScreenSpaceOverlay;",
            "                canvasCmp.sortingOrder = 100;",
            "                // Canvas scaler cmp",
            "                var scalerCmp = blackScreenGO.AddComponent<CanvasScaler>();",
            "                scalerCmp.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;",
            "                // Graphic raycaster cmp",
            "                blackScreenGO.AddComponent<GraphicRaycaster>();",
            "                // Canvas group cmp",
            "                canvasGroupCmp = blackScreenGO.AddComponent<CanvasGroup>();",
            "                canvasGroupCmp.alpha = 0;",
            "                canvasGroupCmp.interactable = false;",
            "                // Create panel GO",
            "                var panelGO = new GameObject("+quote+"Panel"+quote+");",
            "                panelGO.transform.parent = blackScreenGO.transform;",
            "                // Panel Rect transform",
            "                var panelRectT = panelGO.AddComponent<RectTransform>();",
            "                panelRectT.localPosition = Vector3.zero;",
            "                panelRectT.anchorMin = Vector2.zero;",
            "                panelRectT.anchorMax = Vector2.one;",
            "                panelRectT.sizeDelta = Vector3.zero;",
            "                // Panel canvas renderer",
            "                panelGO.AddComponent<CanvasRenderer>();",
            "                // Panel Image",
            "                var imageCmp = panelGO.AddComponent<Image>();",
            "                imageCmp.color = Color.black;",
            "",
            "            }",
            "",
            "            public void SetUpProgress(float p)",
            "            {",
            "                // p will go from 0 to 1",
            "                canvasGroupCmp.alpha = p;",
            "            }",
            "",
            "            public void SetUpReady()",
            "            {",
            "                //Debug.Log("+quote+"SetUpReady SampleTransition"+quote+");",
            "            }",
            "",
            "",
            "",
            "            public void LoadProgres(float p)",
            "            {",
            "                //Debug.Log("+quote+"LoadProgress SampleTransition"+quote+");",
            "            }",
            "",
            "",
            "",
            "            public void TearDown()",
            "            {",
            "                //Debug.Log("+quote+"TearDown SampleTransition"+quote+");",
            "            }",
            "",
            "            public void TearDownProgress(float p)",
            "            {",
            "                // p will go from 0 to 1",
            "                canvasGroupCmp.alpha = (1 - p);",
            "            }",
            "",
            "            public void TearDownReady()",
            "            {",
            "                // Destroy the screen",
            "                UnityEngine.Object.Destroy(blackScreenGO);",
            "            }",
            "",
            "        }",
            "    }",
            "}",
        };
        #endregion SampleTransition File



        private static string FormatLog(string text) => "UniversalScenes: " + text;


        [MenuItem("Universal/Scenes/Create/Startup Files")]
        public static void ShowWindow()
        {

            // Create files
            CreateFile(startupFolderName, startupFileName, startupFile, FormatLog);
            CreateFile(selectorsListFolderName, selectorsListFileName, selectorsListFile, FormatLog);

            CreateFile(prodTransitionsListFolderName, prodTransitionsListFileName, prodTransitionsListFile, FormatLog);
            CreateFile(sampleTransitionFolderName, sampleTransitionFileName, sampleTransitionFile, FormatLog);

            // Compile
            AssetDatabase.Refresh();

        }

    }
}