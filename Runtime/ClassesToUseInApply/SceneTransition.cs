using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace U.Universal.Scenes
{
    // This class is just a generic class that implements the itransition interface
    public class SceneTransition : ISceneTransition
    {

        public string currentScenePattern = "";
        public string nextScenePattern = "*";

        public Action<float> loadProgres;
        public Action setUp;
        public Action<float> setUpProgress;
        public Action setUpReady;
        public Action tearDown;
        public Action<float> tearDownProgress;
        public Action tearDownReady;


        public string CurrentScenePattern() => currentScenePattern;
        public string NextScenePattern() => nextScenePattern;

        public void LoadProgres(float p) => loadProgres?.Invoke(p);
        public void SetUp() => setUp?.Invoke();
        public void SetUpProgress(float p) => setUpProgress?.Invoke(p);
        public void SetUpReady() => setUpReady?.Invoke();
        public void TearDown() => tearDown?.Invoke();
        public void TearDownProgress(float p) => tearDownProgress?.Invoke(p);
        public void TearDownReady() => tearDownReady?.Invoke();
    }
}
