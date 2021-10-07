
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace U.Universal.Scenes
{
    public sealed partial class SceneMonitor
    {

        // This function return if a pattern matches with a specific sceneData
        private static bool SceneMatchPattern(SceneData scene, string pattern)
        {

            var name = scene.name;
            var buildIndex = scene.buildIndex;
            var path = scene.path;


            try
            {
                // Search by OR ||
                if (pattern.Where(c => c == '|').Count() > 0 && pattern.Where(c => c == '&').Count() == 0)
                {
                    foreach (var pat in pattern.Split('|'))
                    {
                        if (String.IsNullOrEmpty(pat) || string.IsNullOrWhiteSpace(pat))
                            continue;

                        if (Search(name, buildIndex, path, pat))
                            return true;
                    }

                    return false;
                }
                // Search by AND &&
                else if (pattern.Where(c => c == '&').Count() > 0 && pattern.Where(c => c == '|').Count() == 0)
                {
                    foreach (var pat in pattern.Split('&'))
                    {
                        if (String.IsNullOrEmpty(pattern) || string.IsNullOrWhiteSpace(pat))
                            continue;

                        if (!Search(name, buildIndex, path, pat))
                            return false;
                    }

                    return true;
                }
                // Search Only one pattern
                else
                {
                    return Search(name, buildIndex, path, pattern);
                }
            }
            catch (Exception)
            {
                return false;
            }


            bool Search(string name, int buildIndex, string path, string pattern)
            {
                if (string.IsNullOrEmpty(pattern) || string.IsNullOrWhiteSpace(pattern))
                    return false;

                // By index
                if (pattern.StartsWith("#"))
                {
                    var p1 = pattern.TrimStart('#');

                    if (buildIndex < 0)
                        return false;

                    if (p1.StartsWith(">="))
                    {

                        if (buildIndex >= Int32.Parse(p1.TrimStart('>', '=')))
                            return true;
                        else
                            return false;

                    }
                    else if (p1.StartsWith("<="))
                    {

                        if (buildIndex <= Int32.Parse(p1.TrimStart('<', '=')))
                            return true;
                        else
                            return false;

                    }
                    else if (p1.StartsWith(">"))
                    {

                        if (buildIndex > Int32.Parse(p1.TrimStart('>')))
                            return true;
                        else
                            return false;

                    }
                    else if (p1.StartsWith("<"))
                    {

                        if (buildIndex < Int32.Parse(p1.TrimStart('<')))
                            return true;
                        else
                            return false;

                    }
                    else
                    {

                        if (buildIndex == Int32.Parse(p1))
                            return true;
                        else
                            return false;
                    }
                }

                // By path
                else if (pattern.StartsWith("."))
                {
                    // Debug.Log("By Path Sel: " + pattern + " Path: " + path);
                    var p1 = pattern.TrimStart('.');

                    if (p1.TrimEnd('*').Length == 0)
                    {
                        //Debug.Log("False .**");
                        return false;
                    }
                    else if (p1.StartsWith("*") && p1.EndsWith("*") && p1.Length > 2)
                    {
                        // Debug.Log("ByName *path*path*");
                        var pieces = p1.TrimStart('*').TrimEnd('*').Split('*');

                        foreach (var p in pieces)
                        {
                            if (!path.Contains(p) || p.Length < 1)
                                return false;
                        }

                        return true;

                    }
                    else if (p1.EndsWith("*") && p1.TrimEnd('*').Length > 0)
                    {
                        // Debug.Log("By Path   path*");
                        if (path.StartsWith(p1.TrimEnd('*')))
                            return true;
                        else
                            return false;

                    }
                    else if (p1.StartsWith("*") && p1.TrimStart('*').Length > 0)
                    {
                        // Debug.Log("By Path   *path");
                        var pathHelpper = path;
                        if (!pathHelpper.EndsWith(".unity"))
                            pathHelpper += ".unity";
                        if (pathHelpper.EndsWith(p1.TrimStart('*') + ".unity"))
                        {
                            // Debug.Log("IsTrue");
                            return true;
                        }
                        else
                        {
                            // Debug.Log("IsFalse");
                            return false;
                        }

                    }
                    else
                    {
                        // Debug.Log("By Path   path");
                        if (path == p1 + ".unity")
                            return true;
                        else
                            return false;

                    }
                }

                // By name
                else
                {
                    //Debug.Log("ByName");
                    var p1 = pattern;

                    if (p1.TrimEnd('*').Length == 0)
                    {
                        //Debug.Log("ByName *");
                        //Debug.Log("True");
                        return true;
                    }
                    else if (p1.StartsWith("*") && p1.EndsWith("*") && p1.Length > 2)
                    {
                        //Debug.Log("ByName *name*name*");
                        var pieces = p1.TrimStart('*').TrimEnd('*').Split('*');

                        foreach (var p in pieces)
                        {
                            if (!name.Contains(p) || p.Length < 1)
                                return false;
                        }

                        return true;

                    }
                    else if (p1.StartsWith("*"))
                    {
                        //Debug.Log("ByName *name");
                        if (name.EndsWith(p1.TrimStart('*')) && p1.TrimStart('*').Length > 0)
                        {
                            //Debug.Log("True");
                            return true;
                        }
                        else
                        {
                            //Debug.Log("False");
                            return false;
                        }

                    }
                    else if (p1.EndsWith("*"))
                    {
                        //Debug.Log("ByName name*");
                        if (name.StartsWith(p1.TrimEnd('*')) && p1.TrimEnd('*').Length > 0)
                        {
                            //Debug.Log("True");
                            return true;
                        }
                        else
                        {
                            //Debug.Log("False");
                            return false;
                        }

                    }
                    else
                    {
                        //Debug.Log("ByName name");
                        if (name == p1)
                        {
                            //Debug.Log("True");
                            return true;
                        }
                        else
                        {
                            //Debug.Log("False");
                            return false;
                        }

                    }

                }
            }

        }

        // Return all the transitions is transitions list that match with the current and next scene 
        private static ISceneTransition[] SearchTransitions(SceneData currentScene, SceneData nextScene, IEnumerable<ISceneTransition> sceneTransitions)
        {

            //Debug.Log(currentScene.ToString("CurrentScene"));
            //Debug.Log(nextScene.ToString("NextScene"));

            return sceneTransitions.Where(transition =>
            {
                try
                {
                    //Debug.Log("Current scene: " + currentScene.name + " Pattern: " + transition.CurrentScenePattern + " Match: " + SceneMatchPattern(currentScene, transition.CurrentScenePattern));
                    //Debug.Log("Next scene: " + nextScene.name + " Pattern: " + transition.NextScenePattern + " Match: " + SceneMatchPattern(nextScene, transition.NextScenePattern));
                    //Debug.Log("Pattern Match: " + (SceneMatchPattern(currentScene, transition.CurrentScenePattern) && SceneMatchPattern(nextScene, transition.NextScenePattern)));
                    return SceneMatchPattern(currentScene, transition.CurrentScenePattern()) && SceneMatchPattern(nextScene, transition.NextScenePattern());
                }
                catch (Exception e)
                {
                    Debug.LogError("SceneMonitor: Transition pattern throw Exception, " + e);
                    return false;
                }
                
            }).ToArray();
        }

        // Return all selectors in the selectors list that match the sceneData
        public static ISceneSelector[] SearchSelectors(SceneData scene, IEnumerable<ISceneSelector> sceneSelectors)
        {
            return sceneSelectors.Where(selector =>
            {
                try
                {
                    return SceneMatchPattern(scene, selector.Pattern());
                }
                catch (Exception e)
                {
                    Debug.LogError("SceneMonitor: Selector pattern throw Exception, " + e);
                    return false;
                }
                
            }).ToArray();


        }

    }
}
