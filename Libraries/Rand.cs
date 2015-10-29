//       __    __     ______     ______   ______     __  __     ______
//      /\ "-./  \   /\  __ \   /\__  _\ /\  ___\   /\ \_\ \   /\  __ \
//      \ \ \-./\ \  \ \  __ \  \/_/\ \/ \ \ \____  \ \  __ \  \ \  __ \
//       \ \_\ \ \_\  \ \_\ \_\    \ \_\  \ \_____\  \ \_\ \_\  \ \_\ \_\
//        \/_/  \/_/   \/_/\/_/     \/_/   \/_____/   \/_/\/_/   \/_/\/_/
//         I  N  D  U  S  T  R  I  E  S             www.matcha.industries

using UnityEngine;
using System;

namespace Matcha.Unity
{

public class Rand
{
    public static int Range(int min, int max)
    {
        return UnityEngine.Random.Range(min, max + 1);
    }

    public static float Range(float min, float max)
    {
        return UnityEngine.Random.Range(min, max);
    }

    public static float Gaussian()
    {
        float v1, v2, s;
        do {
            v1 = 2.0f * UnityEngine.Random.Range(0f,1f) - 1.0f;
            v2 = 2.0f * UnityEngine.Random.Range(0f,1f) - 1.0f;
            s = v1 * v1 + v2 * v2;
        } while (s >= 1.0f || s == 0f);

        s = Mathf.Sqrt((-2.0f * Mathf.Log(s)) / s);

        return v1 * s;
    }

    public static float Gaussian(float mean, float standard_deviation)
    {
        return mean + Gaussian() * standard_deviation;
    }

    public static float Gaussian(float mean, float standard_deviation, float min, float max)
    {
        float x;
        do {
            x = Gaussian(mean, standard_deviation);
        } while (x < min || x > max);
        return x;
    }

    public static int RoundToDivFour(int num)
    {
        while (num % 4 != 0)
        {
            num++;
        }

        return num;
    }

    public static void Seed()
    {
        UnityEngine.Random.seed = (int)System.DateTime.Now.Ticks;
    }

    public static void Seed(int value)
    {
        UnityEngine.Random.seed = value;
    }
}
}