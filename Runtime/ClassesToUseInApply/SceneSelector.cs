using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.SceneManagement;

namespace U.Universal.Scenes
{
    public class SceneSelector : ISceneSelector
    {

        public string pattern = "";  // The pattern that scenes will match

        public Action<Scene> onUnload; // Will excecute before this scene is unloaded
        public Action<Scene> onLoad; // Will excecute after this scene is loaded
        public Action<Scene> onSetActive; // Will excecute after this scene is set as active
        public Action<Scene> onSetInactive; // Will excecute after this scene is set to inactive


        public string Pattern() => pattern;

        public void OnLoad(Scene scene) => onLoad?.Invoke(scene);
        public void OnSetActive(Scene scene) => onSetActive?.Invoke(scene);
        public void OnSetInactive(Scene scene) => onSetInactive?.Invoke(scene);
        public void OnUnload(Scene scene) => onUnload?.Invoke(scene);

    }
}
