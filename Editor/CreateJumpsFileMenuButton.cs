using UnityEditor;
using static U.Universal.Scenes.Editor.UE;

namespace U.Universal.Scenes.Editor
{
    public class CreateJumpsFileMenuButton : EditorWindow
    {

        #region Jumps File
        private static string FolderName => "/Scripts/ScenesManagment/Uscenes/";
        private static string FileName => "Jumps.cs";
        private readonly static string[] file =
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
        #endregion Jumps File



        private static string FormatLog(string text) => "UniversalScenes: " + text;


        [MenuItem("Universal/Scenes/Create/Jumps File")]
        public static void ShowWindow()
        {

            // Create files
            CreateFile(FolderName, FileName, file, FormatLog);

            // Compile
            AssetDatabase.Refresh();

        }

    }
}