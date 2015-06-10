using System;
using System.Collections.Generic;

public class PrintFile
{
    public PrintData LastLine;
    public PrintData Line;
    public int LayerNum;

    private List<string> m_SkipLine = new List<string>();

    public PrintFile()
	{
        LastLine = new PrintData();
        Line = new PrintData();
	}

    public PrintFile(PrintFile src)
    {
        clone(src);
    }

    public void clone(PrintFile src)
    {
        LastLine.clone(src.LastLine);
        Line.clone(src.Line);
        LayerNum = src.LayerNum;
    }

    public bool Parse(string line)
    {
        if (line.Length <= 0) return false;

        if (line[0] == ';')
        {
            string[] keys = line.Split(':');
            if (keys.Length >= 2)
            {
                if (keys[0] == ";LAYER")
                {
                    LayerNum = Int32.Parse(keys[1]);
                }
            }
            return false;
        }
        else
        {
            Line.Parse(line);
            return true;
        }
    }

    public bool NeedConvert(double threshold)
    {
        if ((Line.Cmd == "G0" && LastLine.Cmd == "G1") &&
            ((Math.Abs(Line.X - LastLine.X) > threshold) || (Math.Abs(Line.Y - LastLine.Y) > threshold)) &&
            (Line.Z == LastLine.Z))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void NextLine()
    {
        LastLine.clone(Line);
        m_SkipLine.Clear();
    }

    public void SkipLine(string Line)
    {
        m_SkipLine.Add(Line);
    }

    public List<string> GetSkipLine()
    {
        return m_SkipLine;
    }
}
