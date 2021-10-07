using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace U.Universal.Scenes
{
    public interface ISceneTransition
    {
        string CurrentScenePattern();
        string NextScenePattern();
        void SetUp();  // When transition start
        void SetUpProgress(float p);  // While start delay
        void SetUpReady();  // While start delay ready and will load scene
        void LoadProgres(float p);  // While the scene loads
        void TearDown();  // When transition is loaded
        void TearDownProgress(float p);  // While end delay
        void TearDownReady();  // When transition ends

    }
}
