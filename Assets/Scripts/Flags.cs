using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Flags
{
    GameStarted
}

public static class SetFlags
{
    public static HashSet<Flags> activeFlags = new HashSet<Flags>();

    public static void addFlag(Flags f)
    {
        activeFlags.Add(f);
    }

    public static bool containsAllFlags(Flags[] f)
    {
        foreach (Flags flag in f)
        {
            if (!activeFlags.Contains(flag))
                return false;
        }

        return true;
    }
}
