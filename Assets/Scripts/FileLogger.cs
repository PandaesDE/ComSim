using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

public class FileLogger
{
    private string _output = "";
    private string _path = "";
    
    public FileLogger(string path)
    {
        this._path = path;
    }

    public string GetLog() { return _output; }
    public void ClearLog() { _output = ""; }

    public void AddLogEntry_Count(string title, List<int> counts)
    {
        StringBuilder stringBuilder = new();
        stringBuilder.AppendLine(getTitle(title));

        foreach (int num in counts)
        {
            stringBuilder.Append($"{num}\n");
        }

        _output += stringBuilder.ToString();
    }

    public void AddLogEntry_DeathReaons(string title, Dictionary<Creature.DeathReason,int> dict)
    {
        StringBuilder stringBuilder = new();
        stringBuilder.AppendLine(getTitle(title));

        foreach(KeyValuePair<Creature.DeathReason, int> keyValue in dict)
        {
            stringBuilder.Append($"{keyValue.Key}, {keyValue.Value}\n");
        }

        _output += stringBuilder.ToString();
    }

    public void Log()
    {
        string fileName = System.DateTime.Now.ToString("yyyyMMdd_HHmmss");
        string filePath = Path.Combine( _path, $"{fileName}.csv");
        System.IO.File.WriteAllText(filePath, _output);
    }

    private string getTitle(string title)
    {
        char fillerSymbol = '#';
        int leftRightAmount = 5;
        string fillerShort = new(fillerSymbol, leftRightAmount);
        string fillerLong = new(fillerSymbol, leftRightAmount*2 + title.Length + 2);
        return  $"{fillerLong}\n" +
                $"{fillerShort} {title} {fillerShort}\n" +
                $"{fillerLong}\n";
    }
}
