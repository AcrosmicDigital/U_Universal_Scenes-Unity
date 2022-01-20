using System;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace U.Universal.Scenes.Editor
{
    internal static class UE
    {
        public static void CreateFile(string folderName, string fileName, string[] file, Func<string, string> FormatLog)
        {
            // Check if Env folder exist or create it
            if (!Directory.Exists(Application.dataPath + folderName))
            {
                Debug.Log(FormatLog("Creating Assets/" + folderName + " directory"));
                Directory.CreateDirectory(Application.dataPath + folderName);
            }
            else
            {
                Debug.Log(FormatLog("Assets/" + folderName + " already exist"));
            }

            // check if scenes file exist or create it
            if (!File.Exists(Application.dataPath + folderName + fileName))
            {
                Debug.Log(FormatLog("Creating Assets/" + folderName + fileName + " file"));

                // Write the file
                File.WriteAllLines(Application.dataPath + folderName + fileName, file); // This should be async when is available

            }
            else
            {
                Debug.LogError(FormatLog("Assets/" + folderName + fileName + " already exist"));
            }
        }

        public static void CreateFileWithSaveFilePanel(string folderName, string defaultFileName, Func<string, string[]> file, Func<string, string> FormatLog)
        {

            // Check if Env folder exist or create it
            if (!Directory.Exists(Application.dataPath + folderName))
            {
                Debug.Log(FormatLog("Creating Assets/" + folderName + " directory"));
                Directory.CreateDirectory(Application.dataPath + folderName);
            }
            else
            {
                Debug.Log(FormatLog("Assets/" + folderName + " already exist"));
            }

            // Read the File Name from the save file Panel
            var path = EditorUtility.SaveFilePanel(title: "Create Transition Class", directory: Application.dataPath + folderName, defaultName: defaultFileName, extension: "cs");

            // check if scenes file exist or create it
            if (!File.Exists(path))
            {
                Debug.Log(FormatLog("Creating Assets/" + path + " file"));

                // Write the file
                File.WriteAllLines(path, file(Path.GetFileNameWithoutExtension(path))); // This should be async when is available

                // Compile
                AssetDatabase.Refresh();
            }
            else
            {
                Debug.LogError(FormatLog("Assets/" + path + " already exist"));
            }

        }

        public const string quote = "\"";

    }
}