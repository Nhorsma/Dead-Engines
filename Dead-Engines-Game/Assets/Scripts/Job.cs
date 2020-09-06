using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Job : MonoBehaviour
{
    private Vector3 posOfJob;
    private string task;

    public Job()
    {
        posOfJob = new Vector3(0,0,0);
        task = "";
    }

    public Vector3 GetJobPos()
    {
        return posOfJob;
    }

    public void SetJobPos(Vector3 pos)
    {
        posOfJob = pos;
    }

    public string GetTask()
    {
        return task;
    }

    public void SetTask(string newtask)
    {
        task = newtask;
    }

}
