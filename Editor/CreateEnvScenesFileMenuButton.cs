using UnityEditor;
using static U.Universal.Scenes.Editor.UE;

namespace U.Universal.Scenes.Editor
{
    public class CreateEnvScenesFileMenuButton : EditorWindow
    {

        #region EnvScenes File
        private static string FolderName => "/Scripts/Env/";
        private static string FileName => "Scenes.cs";
        private readonly static string[] file =
        {
            "",
            "public static partial class Env",
            "{",
            "    public static partial class Scenes",
            "    {",
            "        // Examples, you can delete them",
            "        //public static string _Main => " + quote + "_Main" + quote + ";",
            "        //public static string Level(int num) => "+quote+"Level"+quote+" + num;",
            "",
            "        // ... Add here your scenes",
            "",
            "",
            "    }",
            "}",
        };
        #endregion EnvScenes File



        private static string FormatLog(string text) => "UniversalScenes: " + text;


        [MenuItem("Universal/Scenes/Create/Scenes File")]
        public static void ShowWindow()
        {

            // Create files
            CreateFile(FolderName, FileName, file, FormatLog);

            // Compile
            AssetDatabase.Refresh();

        }

    }
}