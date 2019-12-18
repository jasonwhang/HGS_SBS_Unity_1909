using System.Collections;
using System.Collections.Generic;

// 2019.12.18 수요일 - 클래스 추가

public static class UnitBuilder
{
    private static readonly string[] UNIT_ARR = { "", "K", "M", "B", "T", "aa", "ab", "ac" };

    public static string GetUnitStr(double value)
    {
        string valStr = value.ToString("N0");
        string[] splitedStr = valStr.Split(',');

        string result = "";

        if(splitedStr.Length > 1)
        {
            char[] underPoint = splitedStr[1].ToCharArray();

            result = string.Format("{0}.{1}{2} {3}",
                                   splitedStr[0], underPoint[0], underPoint[1],
                                   UNIT_ARR[splitedStr.Length - 1]);
        }
        else
        {
            result = splitedStr[0];
        }

        return result;
    }
}
