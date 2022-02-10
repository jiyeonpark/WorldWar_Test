using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CSVData
{
    string[] _header = null;
    string[][] _data = null;

    public int Count
    {
        get
        {
            return _data.Length;
        }
    }

    public IEnumerable<string> Indexes
    {
        get
        {
            return _header;
        }
    }

    public CSVData(string text, char split)
    {
        string[] lines = text.Split('\n');

        
        int dataCount = 0;
        foreach (string check in lines)
        {
            if (check.Length > 0)
                dataCount++;
        }

        _data = new string[dataCount-1][];

        int lineNb = 0;

        
        foreach (string line in lines)
        {
            if (line.Length == 0 )
                continue;

            // carrige return delete
            string newLines = line;

            if (newLines.Contains("\r"))
            {
                newLines = newLines.Remove(newLines.IndexOf("\r"));
            }

            // "{ -> { 
            newLines = newLines.Replace("\"{", "{");
            // }" -> }
            newLines = newLines.Replace("}\"", "}");
            // "" -> "
            newLines = newLines.Replace("\"\"", "\"");
            // "[ -> [
            newLines = newLines.Replace("\"[", "[");
            // ]" -> ]
            newLines = newLines.Replace("]\"", "]");

            string[] elems = newLines.Split(split);

            // handle the header
            if (_header == null)
            {
                _header = elems;
                continue;
            }

            // break on invalid lines
            if (elems.Length < _header.Length)
            {
                break;
            }

            _data[lineNb] = elems;
            lineNb++;
        }
    }

    public string GetString(int line, string field)
    {
        int index = 0;
        while (_header[index] != field)
        {
            index++;
        }

        return _data[line][index];
    }

    public int GetInt(int line, string field)
    {
        return int.Parse(GetString(line, field));
    }

    public float GetFloat(int line, string field)
    {
        return float.Parse(GetString(line, field));
    }

    public bool GetBool(int line, string field)
    {
        return bool.Parse(GetString(line, field));
    }
}
