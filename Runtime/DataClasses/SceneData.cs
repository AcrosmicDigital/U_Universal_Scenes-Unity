using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace U.Universal.Scenes
{
    public struct SceneData
    {
        public string name;
        public int buildIndex;
        public string path;

        public SceneData(string name, int buildIndex, string path)
        {
            this.name = name;
            this.buildIndex = buildIndex;
            this.path = path;
        }

        public SceneData(string name)
        {
            this.name = name;
            this.buildIndex = -100;
            this.path = "";
        }

        public SceneData(int buildIndex)
        {
            this.name = "";
            this.buildIndex = buildIndex;
            this.path = "";
        }

        public SceneData(string name, int buildIndex)
        {
            this.name = name;
            this.buildIndex = buildIndex;
            this.path = "";
        }

        public SceneData(string name, string path)
        {
            this.name = name;
            this.buildIndex = -100;
            this.path = path;
        }

        public SceneData(Scene scene)
        {
            if (scene.name == null || scene.buildIndex == -1 || scene.path == null)
            {
                this.name = "#";
                this.buildIndex = -1;
                this.path = "#";
                return;
            }

            //Debug.Log("------------<");

            //Debug.Log("Name: " + scene.name);
            //Debug.Log("Name: " + scene.buildIndex);
            //Debug.Log("Name: " + scene.path);

            var path = scene.path;
            if (path.EndsWith(".unity"))
                path = path.Remove(path.Length - 6, 6);
            if (path.StartsWith("Assets/"))
                path = path.Remove(0, 7);

            //Debug.Log("Path: " + path);

            this.name = scene.name;
            this.buildIndex = scene.buildIndex;
            this.path = path;

        }

        public static implicit operator SceneData(Scene scene)
        {
            return new SceneData(scene);
        }

        public static implicit operator SceneData(Scene? scene)
        {
            //Debug.Log("------------<");
            if (scene == null) return new SceneData("#",-1,"#");

            return new SceneData((Scene)scene);
        } 


        public string ToString(string text)
        {
            return (text + " Name: " + name + " BuildIndex: " + buildIndex + " Path: " + path);
        }
        public override string  ToString()
        {
            return ToString("");
        }
    }
}
