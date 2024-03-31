using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Flags
{
    GameStarted,
    tutorialAccepted,
    tutorialDeclined,
    phase2Started,
    DayOneCompleted,
    WeekOneCompleted,
    DaySixCompleted,
    DaySevenCompleted,
    DayEightCompleted,
    DayNineCompleted,
    WeekTwoCompleted,
    ReblockerPurchased
}

public static class SetFlags
{
    public static HashSet<Flags> activeFlags = new HashSet<Flags>();

    public static void addFlag(Flags f)
    {
        activeFlags.Add(f);
    }

    public static void removeFlag(Flags f)
    {
        activeFlags.Remove(f);
    }

    public static void ResetFlags()
    {
        activeFlags.Clear();
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

    public static bool containsNoFlags(Flags[] f)
    {
        foreach (Flags flag in f)
        {
            if (activeFlags.Contains(flag))
                return false;
        }

        return true;
    }
}
