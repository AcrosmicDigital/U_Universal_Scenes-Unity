using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace U.Universal.Scenes
{
    internal static partial class StaticFunctions
    {

        internal async static Task WaitAsTask(this IEnumerator iEnumerator, GameObject gameObject)
        {

            // The Task that will wait for the coroutine
            TaskCompletionSource<bool> tcs = new TaskCompletionSource<bool>();

            // Exception catched from the IEnumerator
            Exception error = null;

            // Current object returned for the corroutine
            object current;


            // The IEnumerator that will help to wait for the corroutine
            IEnumerator Helper()
            {

                while (true)
                {
                    try
                    {
                        if (!iEnumerator.MoveNext())
                            break;
                        current = iEnumerator.Current;
                    }
                    catch (Exception e)
                    {
                        error = e;
                        break;
                    }

                    yield return current;
                }

                // If exceptions
                if (error != null)
                    tcs.SetException(error);
                else
                {
                    tcs.SetResult(true);
                }

                yield break;
            }


            // Is searched a monobehavior to run the coroutines
            UroutineRunner uroutineRunner = gameObject.GetComponent<UroutineRunner>();
            // Is added the MonoBehaviour to the GameObject if there is no one
            if (uroutineRunner == null)
                uroutineRunner = gameObject.AddComponent<UroutineRunner>();

            // Subscription to the listener
            uroutineRunner.OnDestroyEvent.AddListener(() =>
            {

                try
                {
                    if (!tcs.Task.IsCompleted)
                    {
                        error = new InvalidOperationException("GameObject Destroyed Before Uroutine End");
                        tcs.SetException(error);
                    }
                }
                catch (Exception e)
                {
                    Debug.LogException(e);
                }

            });

            // The coroutine is started and the Task is returned
            uroutineRunner.StartCoroutine(Helper());
            await tcs.Task;

        }


        internal class UroutineRunner : MonoBehaviour
        {

            internal UnityEvent OnDestroyEvent = new UnityEvent();

            private void OnDestroy()
            {
                OnDestroyEvent?.Invoke();
            }

        }


        internal static UInt32 NewIdShort()
        {

            byte[] buffer = Guid.NewGuid().ToByteArray();

            UInt32 num = BitConverter.ToUInt32(buffer, 0);

            if (num < 1000000000)
                num += 1000000000;

            return num;

        }


        internal static async Task WaitForSecondsRealtime(GameObject gameObject, float time)
        {

            IEnumerator Helper()
            {
                yield return new WaitForSecondsRealtime(time);
            }

            await Helper().WaitAsTask(gameObject);

        }




        internal static async Task LoadSceneAsync(GameObject gameObject, int sceneBuildIndex, LoadSceneMode mode, Action<float> whileLoad = null)
        {

            IEnumerator Helper()
            {
                // Carga de forma aditiva la nueva escena y la guarda en una variable
                AsyncOperation loadOperation = SceneManager.LoadSceneAsync(sceneBuildIndex, mode);

                // Mientras la escena se carga espera y ejecuta el delagate
                while (!loadOperation.isDone)
                {
                    try
                    {
                        whileLoad?.Invoke(Mathf.Clamp01(loadOperation.progress / .9f));
                    }
                    catch (Exception e)
                    {
                        Debug.LogError("Uroutine.LoadSceneAsync: Error in whileLoad delegate, scene will be loaded anyway, " + e);
                    }
                    yield return null;
                }
            }

            await Helper().WaitAsTask(gameObject);

        }

        internal static async Task LoadSceneAsync(GameObject gameObject, string sceneName, LoadSceneMode mode, Action<float> whileLoad = null)
        {

            IEnumerator Helper()
            {
                // Carga de forma aditiva la nueva escena y la guarda en una variable
                AsyncOperation loadOperation = SceneManager.LoadSceneAsync(sceneName, mode);

                // Mientras la escena se carga espera y ejecuta el delagate
                while (!loadOperation.isDone)
                {
                    try
                    {
                        whileLoad?.Invoke(Mathf.Clamp01(loadOperation.progress / .9f));
                    }
                    catch (Exception e)
                    {
                        Debug.LogError("Uroutine.LoadSceneAsync: Error in whileLoad delegate, scene will be loaded anyway, " + e);
                    }
                    yield return null;
                }
            }

            await Helper().WaitAsTask(gameObject);

        }

        internal static async Task UnloadSceneAsync(GameObject gameObject, int sceneBuildIndex, Action whileUnload = null)
        {

            IEnumerator Helper()
            {
                // Carga de forma aditiva la nueva escena y la guarda en una variable
                AsyncOperation loadOperation = SceneManager.UnloadSceneAsync(sceneBuildIndex);

                // Mientras la escena se carga espera y ejecuta el delagate
                while (!loadOperation.isDone)
                {
                    try
                    {
                        whileUnload?.Invoke();
                    }
                    catch (Exception e)
                    {
                        Debug.LogError("Uroutine.LoadSceneAsync: Error in whileLoad delegate, scene will be loaded anyway, " + e);
                    }
                    yield return null;
                }
            }

            await Helper().WaitAsTask(gameObject);

        }

        internal static async Task UnloadSceneAsync(GameObject gameObject, string sceneName, Action whileUnload = null)
        {

            IEnumerator Helper()
            {
                // Carga de forma aditiva la nueva escena y la guarda en una variable
                AsyncOperation loadOperation = SceneManager.UnloadSceneAsync(sceneName);

                // Mientras la escena se carga espera y ejecuta el delagate
                while (!loadOperation.isDone)
                {
                    try
                    {
                        whileUnload?.Invoke();
                    }
                    catch (Exception e)
                    {
                        Debug.LogError("Uroutine.LoadSceneAsync: Error in whileLoad delegate, scene will be loaded anyway, " + e);
                    }
                    yield return null;
                }
            }

            await Helper().WaitAsTask(gameObject);

        }

        internal static async Task UnloadSceneAsync(GameObject gameObject, Scene scene, Action whileUnload = null)
        {

            IEnumerator Helper()
            {
                // Carga de forma aditiva la nueva escena y la guarda en una variable
                AsyncOperation loadOperation = SceneManager.UnloadSceneAsync(scene);

                // Mientras la escena se carga espera y ejecuta el delagate
                while (!loadOperation.isDone)
                {
                    try
                    {
                        whileUnload?.Invoke();
                    }
                    catch (Exception e)
                    {
                        Debug.LogError("Uroutine.LoadSceneAsync: Error in whileLoad delegate, scene will be loaded anyway, " + e);
                    }
                    yield return null;
                }
            }

            await Helper().WaitAsTask(gameObject);

        }

    }
}
