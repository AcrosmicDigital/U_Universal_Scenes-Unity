using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public static class TasksExtension
{
    public static async void Then<TResult>(this Task<TResult> task, Action<Exception> Reject = null, Action<TResult> Resolve = null, Action Finally = null)
    {

        try
        {
            TResult result = await task;
            Resolve?.Invoke(result);
        }
        catch (Exception e)
        {
            Reject?.Invoke(e);
        }
        finally
        {
            Finally?.Invoke();
        }

    }
}
