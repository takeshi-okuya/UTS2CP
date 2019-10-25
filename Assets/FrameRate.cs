using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System.Linq;

public class FrameRate : MonoBehaviour
{
    public WCGL.CustomPerspectiveCamera customPerspectiveCamera;
    public float start;
    public float end;

    List<float> times = new List<float>();

    void Update()
    {
        float time = Time.time;

        if (start < time)
        {
            times.Add(Time.deltaTime);
        }

        if (end < time)
        {
            save();

#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#elif UNITY_STANDALONE
            Application.Quit();
#endif
        }
    }

    void save()
    {
        var dir = Directory.GetParent(Application.dataPath);
        string fileName = DateTime.Now.ToString("yyyyMMddHmmss") + ".csv";
        string csvPath = Path.Combine(dir.ToString(), fileName);

        using (var sw = new StreamWriter(csvPath))
        {
            sw.WriteLine("CustomPerspective," + customPerspectiveCamera.enabled + ",");
            sw.WriteLine("Start," + start.ToString() + ",");
            sw.WriteLine("End," + end.ToString() + ",");

            double sum = times.Sum(x => (double)x);
            double count = times.Count;

            double average = sum / count;
            sw.WriteLine("Averarge," + average.ToString() + ",");

            double frameRate = count / sum;
            sw.WriteLine("FrameRate," + frameRate);

            sw.WriteLine(",,");

            foreach (float time in times)
            {
                sw.WriteLine(time.ToString() + ",,");
            }
        }
    }
}
