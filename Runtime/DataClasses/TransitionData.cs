using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static U.Universal.Scenes.SceneMonitor;

namespace U.Universal.Scenes
{
    public class TransitionData
    {
        public float delay = 0f;
        public float startDelay = 0f;
        public float endDelay = 0f;
        public JumpMode transitionMode = JumpMode.Relative;
        public string cancelString = "";
    }
}
