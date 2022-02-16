using UnityEditor;
using static U.Universal.Scenes.Editor.UE;

namespace U.Universal.Scenes.Editor
{
    public class CreateTransitionClassMenuButton : EditorWindow
    {

        #region File
        private static string FolderName => "/Scripts/Control/ScenesManagment/Transitions/TransitionClasses/";
        private static string DefaultFileName => "New";
        private static string CustomExtension => "transition";
        static string[] file(string fileName) => new string[]
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
            "        public class "+fileName+" : ISceneTransition",
            "        {",
            "",
            "            // Patterns",
            "            public string CurrentScenePattern() => "+quote+"*"+quote+";",
            "            public string NextScenePattern() => "+quote+"*"+quote+";",
            "",
            "",
            "            // Props",
            "            // ...",
            "",
            "            public void SetUp()",
            "            {",
            "                //Debug.Log("+quote+"SetUp SampleTransition"+quote+");",
            "            }",
            "",
            "            public void SetUpProgress(float p)",
            "            {",
            "                //Debug.Log("+quote+"SetUpProgress SampleTransition: "+quote+" + p);",
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
            "                //Debug.Log("+quote+"LoadProgress SampleTransition: "+quote+" + p);",
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
            "                //Debug.Log("+quote+"SetUpReady SampleTransition: "+quote+" + p);",
            "            }",
            "",
            "            public void TearDownReady()",
            "            {",
            "                //Debug.Log("+quote+"SetUpReady SampleTransition"+quote+");",
            "            }",
            "",
            "        }",
            "    }",
            "}",
        };
        #endregion File



        private static string FormatLog(string text) => "UniversalScenes: " + text;


        [MenuItem("Universal/Scenes/Create/Transition Class")]
        public static void ShowWindow()
        {

            // Create files
            CreateFileWithSaveFilePanelAndCustomExtension(FolderName, DefaultFileName, file, FormatLog, CustomExtension);

            // Compile
            AssetDatabase.Refresh();

        }

    }
}