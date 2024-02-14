using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DayBreakdownClass
{
    public static int messagesMissed = 0;
    public static int messagesCorrect = 0;
    public static int messagesIncorrect = 0;
    public static int moneyEarned = 0;

    public static string GetHeaderString()
    {
        string toReturn = "DAY ";
        
        switch (GameManager.Instance.day)
        {
            case 1:
                toReturn += "ONE ";
                break;
            case 2:
                toReturn += "TWO ";
                break;
            case 3:
                toReturn += "THREE ";
                break;
            case 4:
                toReturn += "FOUR ";
                break;
            case 5:
                toReturn += "FIVE ";
                break;
        }

        return toReturn + "COMPLETED";
    }

    public static float GetPercent()
    {
        if (messagesMissed + messagesCorrect + messagesIncorrect > 0)
            return ((float)messagesCorrect / (float)(messagesMissed + messagesCorrect + messagesIncorrect)) * 100f;
        else
            return 100;
    }

    public static string GetBreakdownString()
    {
        string rank;
        float percent = GetPercent();

        if (percent > 95f)
            rank = "S";
        else if (percent > 85f)
            rank = "A";
        else if (percent > 75f)
            rank = "B";
        else if (percent > 65f)
            rank = "C";
        else if (percent > 55f)
            rank = "D";
        else
            rank = "F";

        string toReturn = string.Format(
            "{0} REQUESTS ANSWERED CORRECTLY\n\n" +
            "{1} REQUESTS ANSWERED INCORRECTLY\n\n" +
            "{2} REQUESTS NOT ANSWERED\n\n" +
            "FINAL WORK REPORT...\n\n" +
            "RANK {3}\n\n" +
            "${4} DOLLARS EARNED", messagesCorrect, messagesIncorrect, messagesMissed, rank, moneyEarned);


        return toReturn;
    }

    public static void Reset()
    {
        messagesMissed = 0;
        messagesCorrect = 0;
        messagesIncorrect = 0;
        moneyEarned = 0;
    }
}
