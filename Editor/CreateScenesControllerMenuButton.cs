using UnityEditor;
using static U.Universal.Scenes.Editor.UE;

namespace U.Universal.Scenes.Editor
{
    public class CreateScenesControllerMenuButton : EditorWindow
    {

        #region File
        private static string FolderName => "/Scripts/Controllers/";
        private static string FileName => "Scenes.controller.cs";
        private readonly static string[] file =
        {
            "using System.Threading.Tasks;",
            "using U.Universal.Scenes;",
            "",
            "public static partial class Control",
            "{",
            "    public static partial class Scenes",
            "    {",
            "        // Examples, you can delete them",
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
            "        // ... Add here your Jumps",
            "        //     Jumps can be transitions between scenes or loads/unloads",
            "",
            "",
            "    }",
            "}",
        };
        #endregion File



        private static string FormatLog(string text) => "UniversalScenes: " + text;


        [MenuItem("Universal/Scenes/Create/Scenes Controller")]
        public static void ShowWindow()
        {

            // Create files
            CreateFile(FolderName, FileName, file, FormatLog);

            // Compile
            AssetDatabase.Refresh();

        }

    }
}