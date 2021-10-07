
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

    public class TransitionTimer : MonoBehaviour
    {

        private float time = 0;
        private float duration = 0;
        private bool inverse = false;
        private Action<float> animate = null;
        private TaskCompletionSource<bool> tks = new TaskCompletionSource<bool>();


        public TransitionTimer Set(float duration, Action<float> animate, bool inverse)
        {
            this.duration = duration;
            this.animate = animate;
            this.inverse = inverse;

            return this;
        }

        public Task Task()
        {
            return tks.Task;
        }

        private void Update()
        {
            if (time > duration)
            {
                tks.SetResult(true);
                Destroy(this);
                return;
            }

            time += Time.unscaledDeltaTime;

            try
            {
                if (inverse)
                {
                    animate?.Invoke(1f - Mathf.Clamp01(time / duration));
                }
                else
                {
                    animate?.Invoke(Mathf.Clamp01(time / duration));
                }

            }
            catch (Exception e)
            {
                tks.SetResult(false);
                Debug.LogError("Error in transition function " + e);
                Destroy(this);
            }
        }

    }

}
