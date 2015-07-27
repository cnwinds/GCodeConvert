using System;
using System.IO;
using System.Collections.Generic;
// test
public class ConvertGCodeMode1
{
    public double XYThreshold = 10;
    public double ZAdjust = 0.5;
    public double EAdjust = 6;
    public double EInfoInterval = 1;
    public bool EInfo = false;

    private double m_ELastValue = 0;
    //private string m_LastG0 = "";
    private List<string> m_LastG0List = new List<string>();

    public ConvertGCodeMode1()
	{
	}

    enum ProcessStatus
    {
        PS_NORMAL,
        PS_DELETE,
        PS_CONVERT,
    }

    public void ProcessStream(StreamReader InFile, StreamWriter OutFile, StreamWriter LogFile)
    {
        ProcessStatus status = ProcessStatus.PS_NORMAL;
        PrintFile data = new PrintFile();
        string line;
        int lineCount = 0;
        int newCount = 1;
        int adjustCount = 0;

        while ((line = InFile.ReadLine()) != null)
        {
            lineCount++;
            if (!data.Parse(line))
            {
                OutFile.WriteLine(line);
                newCount++;
                continue;
            }
            switch (status)
            {
                case ProcessStatus.PS_NORMAL:
                    if (data.NeedConvert(XYThreshold))
                    {
                        m_LastG0List.Add(line);
                        status = ProcessStatus.PS_DELETE;
                    }
                    break;
                case ProcessStatus.PS_DELETE:
                    if (data.Line.Cmd == "G1")
                    {
                        status = ProcessStatus.PS_CONVERT;
                    }
                    else
                    {
                        m_LastG0List.Add(line);
                    }
                    break;
                case ProcessStatus.PS_CONVERT:
                    break;
            }

            if (EInfo)
            {
                if (status == ProcessStatus.PS_CONVERT)
                {
                    OutFile.WriteLine("M117 ADJ E" + data.Line.E.ToString());
                    m_ELastValue = data.Line.E;
                    newCount++;
                }
                else if (data.Line.E - m_ELastValue > EInfoInterval)
                {
                    OutFile.WriteLine("M117 E" + data.Line.E.ToString());
                    m_ELastValue = data.Line.E;
                    newCount++;
                }
            }

            // post process
            switch (status)
            {
                case ProcessStatus.PS_NORMAL:
                    OutFile.WriteLine(line);
                    newCount++;
                    data.NextLine();
                    break;
                case ProcessStatus.PS_DELETE:
                    break;
                case ProcessStatus.PS_CONVERT:
                    adjustCount++;
                    LogFile.WriteLine("old line(" + (lineCount-1).ToString() + ") to convert new line(" + newCount.ToString() + ")...");
                    if (data.LastLine.E > EAdjust)
                    {
                        OutFile.WriteLine("G1 F2400 E" + (data.LastLine.E - EAdjust).ToString());
                        newCount++;
                    }
                    OutFile.WriteLine("G0 Z" + (data.LastLine.Z + ZAdjust).ToString());
                    // add g0 lines;
                    foreach (string l in m_LastG0List)
                    {
                        OutFile.WriteLine(l);
                        newCount++;
                    }
                    m_LastG0List.Clear();
                    OutFile.WriteLine("G0 Z" + (data.LastLine.Z).ToString());
                    newCount += 2;
                    if (data.LastLine.E > EAdjust)
                    {
                        OutFile.WriteLine("G1 F2400 E" + (data.LastLine.E).ToString());
                        newCount++;
                    }
                    OutFile.WriteLine(line);
                    newCount++;
                    status = ProcessStatus.PS_NORMAL;
                    data.NextLine();
                    break;
            }
        }
    }


}
