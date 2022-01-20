using UnityEditor;
using static U.Universal.Scenes.Editor.UE;

namespace U.Universal.Scenes.Editor
{
    public class CreateStartupFilesMenuButton : EditorWindow
    {

        #region Startup File
        private static string startupFolderName => "/Scripts/ScenesManagment/Uscenes/";
        private static string startupFileName => "Startup.cs";
        private static readonly string[] startupFile =
        {
            "using UnityEngine;",
            "using U.Universal.Scenes;",
            "",
            "/// <summary>",
            "/// Dont edit this file, to change stage or enable, edit Assets/Scripts/Env/Vars/DevStage.cs",
            "/// </summary>",
            "public static partial class Uscenes  // Dir: Assets/Scripts/Uscenes/Startup.cs",
            "{",
            "",
            "    [RuntimeInitializeOnLoadMethod]",
            "    public static void Startup()",
            "    {",
            "",
            "        // Dev",
            "        if(Env.Vars.devStage == Env.Vars.DevStages.Dev)",
            "        {",
            "            // Apply selectors",
            "            SceneMonitor.Apply(DevSelectorsList);",
            "",
            "            // Apply transitions",
            "            SceneMonitor.Apply(Uscenes.DevTransitionsList);",
            "        }",
            "",
            "        // Prod",
            "        else if(Env.Vars.devStage == Env.Vars.DevStages.Prod)",
            "        {",
            "            // Apply selectors",
            "            SceneMonitor.Apply(ProdSelectorsList);",
            "",
            "            // Apply transitions",
            "            SceneMonitor.Apply(Uscenes.ProdTransitionsList);",
            "        }",
            "",
            "",
            "        // Enable",
            "        if(Env.Vars.enableUscenes) SceneMonitor.Enable();",
            "",
            "    }",
            "",
            "}",
        };
        #endregion Startup File

        #region DevStage File
        private static string devStageFolderName => "/Scripts/Env/Vars/";
        private static string devStageFileName => "DevStage.cs";
        private static readonly string[] devStageFile =
        {
            "",
            "/// <summary>",
            "/// You can edit this file, but dont modify the current enum members",
            "/// </summary>",
            "public static partial class Env",
            "{",
            "    public static partial class Vars",
            "    {",
            "",
            "        // Enable or disable Universal Scenes",
            "        public static bool enableUscenes = true;",
            "",
            "        // Select stage",
            "        public static DevStages devStage => DevStages.Dev;",
            "",
            "        // Add more stages",
            "        public enum DevStages",
            "        {",
            "            Prod,",
            "            Dev,",
            "        }",
            "",
            "    }",
            "}",
        };
        #endregion DevStage File

        #region DevSelectorsList File
        private static string devSelectorsListFolderName => "/Scripts/ScenesManagment/Uscenes/Selectors/";
        private static string devSelectorsListFileName => "DevSelectorsList.cs";
        private static readonly string[] devSelectorsListFile =
        {
            "using U.Universal.Scenes;",
            "",
            "public static partial class Uscenes",
            "{",
            "    public static ISceneSelector[] DevSelectorsList => new ISceneSelector[]",
            "    {",
            "",
            "        // List of dev selectors",
            "        Selectors.SampleSelector,",
            "        // ... Add here selectors to use when dev stage is selected",
            "",
            "",
            "    };",
            "}",
        };
        #endregion DevSelectorsList File

        #region ProdSelectorsList File
        private static string prodSelectorsListFolderName => "/Scripts/ScenesManagment/Uscenes/Selectors/";
        private static string prodSelectorsListFileName => "ProdSelectorsList.cs";
        private static readonly string[] prodSelectorsListFile =
        {
            "using U.Universal.Scenes;",
            "",
            "public static partial class Uscenes",
            "{",
            "    public static ISceneSelector[] ProdSelectorsList => new ISceneSelector[]",
            "    {",
            "",
            "        // List of prod selectors",
            "        Selectors.SampleSelector,",
            "        // ... Add here selectors to use when prod stage is selected",
            "",
            "",
            "    };",
            "}",
        };
        #endregion ProdSelectorsList File

        #region Selectors File
        private static string selectorsFolderName => "/Scripts/ScenesManagment/Uscenes/Selectors/";
        private static string selectorsFileName => "Selectors.cs";
        private static readonly string[] selectorsFile =
        {
            "using UnityEngine;",
            "using U.Universal.Scenes;",
            "",
            "/// <summary>",
            "/// Edit selectors or add more, this selectors are added in dev and prod selectors list",
            "/// </summary>",
            "public static partial class Uscenes",
            "{",
            "    public static partial class Selectors",
            "    {",
            "",
            "        public static readonly SceneSelector SampleSelector = new SceneSelector",
            "        {",
            "            pattern = "+quote+"*"+quote+",",
            "            onLoad = (s) => Debug.Log("+quote+"Uscenes: SampleSelector onLoad "+quote+" + s.name),",
            "            onSetActive = (s) => Debug.Log("+quote+"Uscenes: SampleSelector onSetActive "+quote+" + s.name),",
            "            onSetInactive = (s) => Debug.Log("+quote+"Uscenes: SampleSelector onSetInactive "+quote+" + s.name),",
            "            onUnload = (s) => Debug.Log("+quote+"Uscenes: SampleSelector onUnload "+quote+" + s.name),",
            "        };",
            "",
            "        // ... Add more selectors here",
            "",
            "    }",
            "}",
        };
        #endregion Selectors File

        #region DevTransitionsList File
        private static string devTransitionsListFolderName => "/Scripts/ScenesManagment/Uscenes/Transitions/";
        private static string devTransitionsListFileName => "DevTransitionsList.cs";
        private static readonly string[] devTransitionsListFile =
        {
            "using U.Universal.Scenes;",
            "",
            "public static partial class Uscenes",
            "{",
            "    public static ISceneTransition[] DevTransitionsList => new ISceneTransition[]",
            "    {",
            "",
            "        // List of dev transitions",
            "        new Transitions.SampleTransition(),",
            "        // ... Add here transitions to use when dev stage is selected",
            "",
            "",
            "    };",
            "}",
        };
        #endregion DevTransitionsList File

        #region ProdTransitionsList File
        private static string prodTransitionsListFolderName => "/Scripts/ScenesManagment/Uscenes/Transitions/";
        private static string prodTransitionsListFileName => "ProdTransitionsList.cs";
        private static readonly string[] prodTransitionsListFile =
        {
            "using U.Universal.Scenes;",
            "",
            "public static partial class Uscenes",
            "{",
            "    public static ISceneTransition[] ProdTransitionsList => new ISceneTransition[]",
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
        #endregion ProdSelectorsList File

        #region SampleTransition File
        private static string sampleTransitionFolderName => "/Scripts/ScenesManagment/Uscenes/Transitions/TransitionClasses/";
        private static string sampleTransitionFileName => "SampleTransition.cs";
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
            CreateFile(devStageFolderName, devStageFileName, devStageFile, FormatLog);
            CreateFile(devSelectorsListFolderName, devSelectorsListFileName, devSelectorsListFile, FormatLog);
            CreateFile(prodSelectorsListFolderName, prodSelectorsListFileName, prodSelectorsListFile, FormatLog);
            CreateFile(selectorsFolderName, selectorsFileName, selectorsFile, FormatLog);
            CreateFile(devTransitionsListFolderName, devTransitionsListFileName, devTransitionsListFile, FormatLog);
            CreateFile(prodTransitionsListFolderName, prodTransitionsListFileName, prodTransitionsListFile, FormatLog);
            CreateFile(sampleTransitionFolderName, sampleTransitionFileName, sampleTransitionFile, FormatLog);

            // Compile
            AssetDatabase.Refresh();

        }

    }
}