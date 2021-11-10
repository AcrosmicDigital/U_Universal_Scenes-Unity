using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.SceneManagement;

namespace U.Universal.Scenes
{
    public interface ISceneSelector
    {
        string Pattern();
        void OnUnload(Scene scene);
        void OnLoad(Scene scene);
        void OnSetActive(Scene scene);
        void OnSetInactive(Scene scene);

    }
}
