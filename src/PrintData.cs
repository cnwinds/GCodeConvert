using System;

public class PrintData
{
    public string Cmd;
    public double F;
    public double X;
    public double Y;
    public double Z;
    public double E;

    public PrintData()
	{
        Cmd = "";
        F = 0.0;
        X = 0.0;
        Y = 0.0;
        Z = 0.0;
        E = 0.0;
	}

    public PrintData(PrintData src)
    {
        clone(src);
    }

    public void clone(PrintData src)
    {
        Cmd = src.Cmd;
        F = src.F;
        X = src.X;
        Y = src.Y;
        Z = src.Z;
        E = src.E;
    }

    public void Parse(string line)
    {
        string[] keys = line.Split(' ');
        if (keys.Length > 1)
        {
            Cmd = keys[0];

            foreach(string i in keys)
            {
                if (i.Length > 2)
                {
                    switch(i[0])
                    {
                        case 'F':
                            F = Double.Parse(i.Substring(1));
                            break;
                        case 'X':
                            X = Double.Parse(i.Substring(1));
                            break;
                        case 'Y':
                            Y = Double.Parse(i.Substring(1));
                            break;
                        case 'Z':
                            Z = Double.Parse(i.Substring(1));
                            break;
                        case 'E':
                            E = Double.Parse(i.Substring(1));
                            break;
                    }
                }
            }
        }
    }
}
