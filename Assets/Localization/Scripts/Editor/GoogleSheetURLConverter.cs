using System.Text.RegularExpressions;
using UnityEngine;

public static class GoogleSheetURLConverter
{
    public static string ConvertToCSVUrl(string url)
    {
        Match sheetMatch = Regex.Match(
            url,
            @"/spreadsheets/d/([a-zA-Z0-9-_]+)");

        if (!sheetMatch.Success)
        {
            Debug.LogError("Invalid Google Sheet URL");
            return null;
        }

        string sheetId = sheetMatch.Groups[1].Value;

        string gid = "0";

        Match gidMatch = Regex.Match(url, @"gid=([0-9]+)");

        if (gidMatch.Success)
        {
            gid = gidMatch.Groups[1].Value;
        }

        return
            $"https://docs.google.com/spreadsheets/d/{sheetId}/export?format=csv&gid={gid}";
    }
}