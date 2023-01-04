using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class C
{
    public int I;

    public C(int i)
    {
        I = i;
    }
}

public struct S
{
    public int I;

    public S(int i)
    {
        I = i;
    }
}

public class ArrayIteration : MonoBehaviour
{
    public TextMeshProUGUI ResultText;

    Dictionary<int, int> dict1 = new Dictionary<int, int>(1024);
    Dictionary<int, int> dict2 = new Dictionary<int, int>(1024);
    int[] array1 = new int[1024];
    int[] array2 = new int[1024];
    int[] randomIndices = new int[1024];

    Dictionary<int, C> classDict = new Dictionary<int, C>(1024);
    Dictionary<int, S> structDict = new Dictionary<int, S>(1024);

    private void Awake()
    {
        init();
    }

    // Start is called before the first frame update
    void Start()
    {
        runDictAccessTest();
    }

    private void init()
    {
        for (int i = 0; i < 1024; i++)
        {
            array1[i] = i;
            array2[i] = i;
            dict1[i] = i;
            dict2[i] = i;
            randomIndices[i] = i;

            classDict[i] = new C(i);
            structDict[i] = new S(i);
        }

        // shuffle
        for (int i = 0; i < 1024; i++)
        {
            int index = (Mathf.FloorToInt(Random.value * 1024.0f));
            if (index == 1024)
                index = 1023;

            int a = randomIndices[i];
            randomIndices[i] = randomIndices[index];
            randomIndices[index] = a;
        }

    }

    private void runDictAccessTest()
    {
        ResultText.text = "Dictionary Access Test\n\n";

        int numIterations = 100000;

        double timer1 = 0.0d;
        double timer2 = 0.0d;
        double timer3 = 0.0d;
        double timer4 = 0.0d;
        double timer5 = 0.0d;
        double timer6 = 0.0d;
        double time = 0.0d;

        int SIZE = 64;

        double[] dictTimer = new double[SIZE];
        double[] arrayTimer = new double[SIZE];

        double[] classTimer = new double[SIZE];
        double[] structTimer = new double[SIZE];

        int value = 0;

        for (int i = 0; i < numIterations; i++)
        {
            time = Time.realtimeSinceStartupAsDouble;
            for (int j = 0; j < 1024; j++)
            {
                value = dict2[randomIndices[j]];
            }
            timer1 += (Time.realtimeSinceStartupAsDouble - time);

            time = Time.realtimeSinceStartupAsDouble;
            for (int j = 0; j < 1024; j++)
            {
                value = array2[randomIndices[j]];
            }
            timer2 += (Time.realtimeSinceStartupAsDouble - time);

            time = Time.realtimeSinceStartupAsDouble;
            for (int j = 0; j < 1024; j++)
            {
                dict1[j] = dict2[randomIndices[j]];
            }
            timer3 += (Time.realtimeSinceStartupAsDouble - time);

            time = Time.realtimeSinceStartupAsDouble;
            for (int j = 0; j < 1024; j++)
            {
                array1[j] = array2[randomIndices[j]];
            }
            timer4 += (Time.realtimeSinceStartupAsDouble - time);

            time = Time.realtimeSinceStartupAsDouble;
            foreach (var item in dict1)
            {
                value += item.Value;
            }
            timer5 += (Time.realtimeSinceStartupAsDouble - time);

            time = Time.realtimeSinceStartupAsDouble;
            for (int j = 0; j < 1024; j++)
                value += array1[j];
            timer6 += (Time.realtimeSinceStartupAsDouble - time);

            for (int k = 0; k < SIZE; k++)
            {
                arrayIteration(ref arrayTimer[k], k, ref value);
                dictLookup(ref dictTimer[k], k, ref value);
                dictSructLookup(ref structTimer[k], k, ref value);
                dictClassLookup(ref classTimer[k], k, ref value);
            }
        }

        ResultText.text += "Dictionary access " + timer1.ToString("N4") + "\n";
        ResultText.text += "Array access " + timer2.ToString("N4") + "\n";
        ResultText.text += "\n";
        ResultText.text += "Dictionary to Dictionary " + timer3.ToString("N4") + "\n";
        ResultText.text += "Array to Array " + timer4.ToString("N4") + "\n";
        ResultText.text += "\n";
        ResultText.text += "Dictionary iteration " + timer5.ToString("N4") + "\n";
        ResultText.text += "Array iteration " + timer6.ToString("N4") + "\n";
        ResultText.text += "\n";

        for (int k = SIZE-1; k >= 0; k--)
        {
            if (arrayTimer[k] <= dictTimer[k])
            {
                ResultText.text += "Array iteration to key " + k + " " + arrayTimer[k].ToString("N4") + "\n";
                ResultText.text += "Dictionary access to key " + k + " " + dictTimer[k].ToString("N4") + "\n";
                break;
            }
        }

        for (int k = SIZE-1; k >= 0; k--)
        {
            if (arrayTimer[k] <= structTimer[k])
            {
                ResultText.text += "Array iteration to key " + k + " " + arrayTimer[k].ToString("N4") + "\n";
                ResultText.text += "Dictionary struct access to key " + k + " " + structTimer[k].ToString("N4") + "\n";
                break;
            }
        }

        for (int k = SIZE-1; k >= 0; k--)
        {
            if (arrayTimer[k] <= classTimer[k])
            {
                ResultText.text += "Array iteration to key " + k + " " + arrayTimer[k].ToString("N4") + "\n";
                ResultText.text += "Dictionary class access to key " + k + " " + classTimer[k].ToString("N4") + "\n";
                break;
            }
        }
    }

    private void arrayIteration(ref double timer, int element, ref int value)
    {
        double time = Time.realtimeSinceStartupAsDouble;
        for (int j = 0; j < 1024; j++)
            if (array1[j] == element)
            {
                value += array2[j];
                break;
            }
        timer += (Time.realtimeSinceStartupAsDouble - time);
    }

    private void dictLookup(ref double timer, int element, ref int value)
    {
        double time = Time.realtimeSinceStartupAsDouble;
        value += dict2[element];
        timer += (Time.realtimeSinceStartupAsDouble - time);

    }

    private void dictSructLookup(ref double timer, int element, ref int value)
    {
        double time = Time.realtimeSinceStartupAsDouble;
        value += structDict[element].I;
        timer += (Time.realtimeSinceStartupAsDouble - time);

    }

    private void dictClassLookup(ref double timer, int element, ref int value)
    {
        double time = Time.realtimeSinceStartupAsDouble;
        value += classDict[element].I;
        timer += (Time.realtimeSinceStartupAsDouble - time);
    }
}
