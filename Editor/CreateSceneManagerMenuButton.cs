using UnityEditor;
using static U.Universal.Scenes.Editor.UE;

namespace U.Universal.Scenes.Editor
{
    public class CreateSceneManagerMenuButton : EditorWindow
    {

        #region File
        private static string FolderName => "/Scripts/Control/ScenesManagment/Managers/";
        private static string DefaultFileName => "NewSceneManager";
        static string[] file(string fileName) => new string[]
        {
            "using UnityEngine;",
            "",
            "/// <summary>",
            "/// Add this script to any GameObject in the scene",
            "/// </summary>",
            "public class "+fileName+" : MonoBehaviour",
            "{",
            "",
            "    #region Manager DONT MODIFY THIS REGION",
            "",
            "    private static "+fileName+" _;",
            "    public static "+fileName+" S",
            "    {",
            "        get",
            "        {",
            "            if (_ == null) _ = FindManagerInScene();",
            "",
            "            // If still null throw exception",
            "            if (_ == null) throw new System.NullReferenceException("+quote+""+fileName+": Cant find a valid GameObject with manager script"+quote+");",
            "",
            "            return _;",
            "        }",
            "    }",
            "",
            "    public static bool Exist",
            "    {",
            "        get",
            "        {",
            "            try",
            "            {",
            "                if (S != null) return true;",
            "                else return false;",
            "            }",
            "            catch (System.Exception)",
            "            {",
            "                return false;",
            "            }",
            "        }",
            "    }",
            "",
            "    #endregion Manager DONT MODIFY THIS REGION",
            "",
            "",
            "    // Function to find the scene manager, you can modify",
            "    private static "+fileName+" FindManagerInScene()",
            "    {",
            "        return GameObject.FindGameObjectWithTag("+quote+""+fileName+"SM"+quote+").GetComponent<"+fileName+">();",
            "    }",
            "",
            "",
            "    // Props",
            "    // ...",
            "",
            "",
            "    // Code",
            "    // ...",
            "",
            "}",
        };
        #endregion File



        private static string FormatLog(string text) => "UniversalScenes: " + text;


        [MenuItem("Universal/Scenes/Create/Scene Manager")]
        public static void ShowWindow()
        {

            // Create files
            CreateFileWithSaveFilePanelForceLocation(FolderName, DefaultFileName, file, FormatLog);

            // Compile
            AssetDatabase.Refresh();

        }

    }
}