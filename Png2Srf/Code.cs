using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;

namespace LifesABeach
{
    public static partial class Code
    {
        public class Point2D
        {
            public double X = 0;
            public double Y = 0;

            public Point2D(double _X, double _Y)
            {
                X = _X;
                Y = _Y;
            }

            public override string ToString()
            {
                return "(" + X + ", " + Y + ")";
            }
        }

        public class Vector2D
        {
            public Point2D P1 = new Point2D(0, 0);
            public Point2D P2 = new Point2D(0, 0);

            public Vector2D(Point2D _P1, Point2D _P2)
            {
                P1 = _P1;
                P2 = _P2;
            }

            public override string ToString()
            {
                return "(" + P1.ToString() + ") -> (" + P2.ToString() + ")";
            }
        }

        public class Area
        {
            public List<Point2D> Points = new List<Point2D>();
            public bool Loop = false;
            public string AreaType = "NOAREA";
            public Point2D Position = new Point2D(0, 0);
            public int ID = 0;
            public string Tag = "";
        }

        [Flags]
        public enum CurrentOperation
        {
            Closed          = 0 << 0,
            Open            = 1 << 0,
            Fail_Generic    = 1 << 1,
            Fail_ChildSce   = 1 << 2,
            FIELD           = 1 << 3,
            PST             = 1 << 4,
            Load_OK         = 1 << 5,
        }

        public static List<Area> Loaded_Areas = new List<Area>();

        public static CurrentOperation LoadFLD(string Filename)
        {
            if (!File.Exists(Filename)) return CurrentOperation.Fail_Generic;
            CurrentOperation CurrentOp = CurrentOperation.Closed;
            string[] FLD_Contents = File.ReadAllLines(Filename);
            CurrentOp = CurrentOperation.Open;
            List<Area> Areas = new List<Area>();
            #region Iterate over FLD_Contents
            foreach (string ThisLine in FLD_Contents)
            {
                string Process = ThisLine;
                Process = Process.ToUpperInvariant();

                string[] Split = Process.CompressWhiteSpace().SplitPreservingQuotes(' ');

                if (CurrentOp == CurrentOperation.Open)
                {
                    if (Process.StartsWith("FIELD"))
                    {
                        CurrentOp = CurrentOperation.FIELD;
                        continue;
                    }
                }

                if (CurrentOp == CurrentOperation.FIELD)
                {
                    #region PST //Get Path/Area
                    if (Process.StartsWith("PST"))
                    {
                        CurrentOp = CurrentOperation.PST;
                        Areas.Add(new Area());
                        continue;
                    }
                    #endregion
                    #region PCK *.FLD* //Crash if Child Scenery
                    if (Process.StartsWith("PCK") & Process.Contains(".FLD"))
                    {
                        //Doesn't support child sceneries, just too messy to try and deal with the cunts!
                        CurrentOp = CurrentOperation.Fail_ChildSce;
                        continue;
                    }
                    #endregion
                }

                if (CurrentOp == CurrentOperation.PST)
                {
                    //Loading Area/Path!
                    #region ISLOOP
                    if (Process.StartsWith("ISLOOP"))
                    {
                        if (Split.Length < 2)
                        {
                            CurrentOp = CurrentOperation.Fail_Generic;
                            return CurrentOp;
                        }

                        if (!Boolean.TryParse(Split[1], out Areas.Last().Loop))
                        {
                            CurrentOp = CurrentOperation.Fail_Generic;
                            return CurrentOp;
                        }
                        continue;
                    }
                    #endregion

                    #region AREA
                    if (Process.StartsWith("AREA"))
                    {
                        if (Split.Length < 2)
                        {
                            CurrentOp = CurrentOperation.Fail_Generic;
                            return CurrentOp;
                        }

                        Areas.Last().AreaType = Split[1];
                        continue;
                    }
                    #endregion

                    #region PNT
                    if (Process.StartsWith("PNT"))
                    {
                        if (Split.Length < 4)
                        {
                            CurrentOp = CurrentOperation.Fail_Generic;
                            return CurrentOp;
                        }

                        Point2D ThisPoint = new Point2D(0, 0);
                        bool Failed;
                        Failed = !Double.TryParse(Split[1], out ThisPoint.X);
                        Failed = !Double.TryParse(Split[3], out ThisPoint.Y);
                        if (Failed)
                        {
                            CurrentOp = CurrentOperation.Fail_Generic;
                            return CurrentOp;
                        }
                        Areas.Last().Points.Add(ThisPoint);
                        continue;
                    }
                    #endregion

                    #region POS
                    if (Process.StartsWith("POS"))
                    {
                        if (Split.Length < 4)
                        {
                            CurrentOp = CurrentOperation.Fail_Generic;
                            return CurrentOp;
                        }

                        Point2D ThisPoint = new Point2D(0, 0);
                        bool Failed;
                        Failed = !Double.TryParse(Split[1], out ThisPoint.X);
                        Failed = !Double.TryParse(Split[3], out ThisPoint.Y);
                        if (Failed)
                        {
                            CurrentOp = CurrentOperation.Fail_Generic;
                            return CurrentOp;
                        }
                        Areas.Last().Position = ThisPoint;
                        continue;
                    }
                    #endregion

                    #region ID
                    if (Process.StartsWith("ID"))
                    {
                        if (Split.Length < 2)
                        {
                            CurrentOp = CurrentOperation.Fail_Generic;
                            return CurrentOp;
                        }

                        bool Failed;
                        Failed = !Int32.TryParse(Split[1], out Areas.Last().ID);
                        if (Failed)
                        {
                            CurrentOp = CurrentOperation.Fail_Generic;
                            return CurrentOp;
                        }
                        continue;
                    }
                    #endregion

                    #region TAG
                    if (Process.StartsWith("TAG"))
                    {
                        if (Split.Length < 2)
                        {
                            CurrentOp = CurrentOperation.Fail_Generic;
                            return CurrentOp;
                        }

                        Areas.Last().Tag = Split[1];
                        continue;
                    }
                    #endregion

                    #region END
                    if (Process.StartsWith("END"))
                    {
                        CurrentOp = CurrentOperation.FIELD;
                    }
                    #endregion
                }
            }
            #endregion
            if (CurrentOp == CurrentOperation.FIELD) CurrentOp = CurrentOperation.Load_OK;
            Loaded_Areas = Areas;
            return CurrentOp;
        }
    }

    public static partial class Strings
    {
        /// <summary>
        /// Simplification method: Compresses all tabs and whitespaces into just one space each.
        /// </summary>
        /// <param name="Input"></param>
        /// <returns></returns>
        public static string CompressWhiteSpace(this string Input)
        {
            while (Input.Contains("  ")) Input = Input.ReplaceAll("  ", " ");
            while (Input.Contains("\t\t")) Input = Input.ReplaceAll("\t\t", "\t");
            Input = Input.ReplaceAll("\t", " ");
            return Input;
        }

        /// <summary>
        /// Uses a RegEx to split a string by white spaces - preserving quoted blocks.
        /// </summary>
        /// <param name="Input"></param>
        /// <returns></returns>
        public static string[] SplitPreservingQuotes(this string Input, char splittingchar)
        {
            if (Input == null) Input = "";
            Input = Input.ReplaceAll("\t\t", "\t");
            Input = Input.ReplaceAll("\t", " ");
            List<string> Strings = Regex.Matches(Input, @"[\""].+?[\""]|[^" + splittingchar + "]+")
                .Cast<Match>()
                .Select(m => m.Value)
                .ToList();

            List<string> Output = new List<string>();
            for (int i = 0; i < Strings.Count; i++)
            {
                Output.Add(Strings[i].Split('\t')[0].ReplaceAll("\"", ""));
            }

            return Output.ToArray();
        }

        /// <summary>
        /// Replaces all occurences of oldstr in string Input to newstr. WARNING: DO NOT REPLACE oldstr WHERE newstr CONTAINS oldstr!
        /// </summary>
        /// <param name="Input"></param>
        /// <returns></returns>
        public static string ReplaceAll(this string Input, string oldstr, string newstr)
        {
            while (Input.Contains(oldstr))
            {
                if (newstr.Contains(oldstr)) break;
                if (oldstr == "") break;
                if (oldstr == newstr) break;
                Input = Input.Replace(oldstr, newstr);
            }
            return Input;
        }

        /// <summary>
        /// Makes a string N chars long by trimming excess or adding trailing nulls(\0).
        /// </summary>
        /// <param name="Input"></param>
        /// <returns></returns>
        public static string Resize(this string Input, int limit)
        {
            string output = "";
            if (Input == null) Input = "";
            if (limit < 0) limit = 0;
            foreach (char ThisChar in Input)
            {
                //compresses the input to a max of N.
                if (output.Length < limit)
                {
                    output += ThisChar;
                }
            }
            //extends the input to n if it is under the limit.
            while (output.Length < limit)
            {
                output += '\0';
            }
            //output = output.Substring(0, limit-1) + '\0'; //don't need to terminate with a null anymore!
            return output;
        }

        /// <summary>
        /// Returns a new string where Input is repeated amount times.
        /// </summary>
        /// <param name="Input"></param>
        /// <param name="Ammount"></param>
        /// <returns></returns>
        public static string Repeat(this string Input, int Ammount)
        {
            return String.Concat(Enumerable.Repeat(Input, Ammount));
        }

        /// <summary>
        /// Converts an array of bytes to a standard string.
        /// </summary>
        /// <param name="Input"></param>
        /// <returns></returns>
        public static string ToDataString(this byte[] Input)
        {
            string output = "";
            foreach (byte ThisByte in Input)
            {
                output += Convert.ToChar(ThisByte);
            }
            return output;
        }

        /// <summary>
        /// Converts an array of chars to a standard string.
        /// </summary>
        /// <param name="Input"></param>
        /// <returns></returns>
        public static string ToDataString(this char[] Input)
        {
            string output = "";
            foreach (char ThisChar in Input)
            {
                output += ThisChar;
            }
            return output;
        }

        /// <summary>
        /// Converts a standard string into an array of bytes.
        /// </summary>
        /// <param name="Input"></param>
        /// <returns></returns>
        public static byte[] ToByteArray(this string Input)
        {
            List<byte> Outbytes = new List<byte>();
            foreach (char ThisChar in Input)
            {
                Outbytes.Add(Convert.ToByte(ThisChar));
            }
            return Outbytes.ToArray();
        }

        /// <summary>
        /// Removes the final comma and space from a building string list, and adds the finalising period.
        /// </summary>
        /// <param name="InputString"></param>
        /// <returns></returns>
        public static string FinaliseStringList(this string InputString)
        {
            if (InputString.Length < 2) return InputString;
            if (!InputString.EndsWith(", ")) return InputString + "."; //it wasn't a list!
            return InputString.Remove(InputString.Length - 2) + ".";
        }

        /// <summary>
        /// Converts a List of Strings to a comma seperated, period finalised list.
        /// </summary>
        /// <param name="ThisStringList"></param>
        /// <returns></returns>
        public static string ToStringList(this List<String> ThisStringList)
        {
            string Output = "";
            foreach (string ThisString in ThisStringList)
            {
                Output += ThisString + ", ";
            }
            if (Output == "") return "None.";
            return Output.FinaliseStringList();
        }

        /// <summary>
        /// Converts a List of Strings to a comma seperated, period finalised list.
        /// </summary>
        /// <param name="ThisStringList"></param>
        /// <returns></returns>
        public static string ToStringList(this string[] ThisStringArray)
        {
            return ToStringList(ThisStringArray.ToList());
        }


        public static string ToTitleCaseInvariant(this string Input)
        {
            string output = "";
            if (Input == null) return "";
            foreach (string ThisString in Input.Split(' '))
            {
                if (output.Length > 0) output += " ";
                if (ThisString.Length > 0) output += ThisString.Substring(0, 1).ToUpperInvariant();
                if (ThisString.Length > 1) output += ThisString.Substring(1).ToLowerInvariant();
            }
            return output;
        }

        public static string[] BreakDownFormattedString(this string input)
        {

            List<string> output = new List<string>();
            string nextoutput = "";
            char nextcolor = 'f';
            if (input == null) return output.ToArray();
            if (input.IndexOf('&') == -1)
            {
                output.Add("&f" + input);
                return output.ToArray();
            }
            else
            {
                for (int i = 0; i < input.Length; i++)
                {
                    if (i > 0)
                    {
                        if (input[i] == '\\')
                        {
                            i++;
                            nextoutput += input[i];
                            continue;
                        }
                    }
                    if (input[i] == '&')
                    {
                        if (input.Length > i + 1)
                        {
                            foreach (char colorid in "0123456789abcdef")
                            {
                                if (colorid == input.ToLowerInvariant()[i + 1])
                                {
                                    output.Add("&" + nextcolor + nextoutput);
                                    nextoutput = "";
                                    nextcolor = colorid;
                                    continue;
                                }
                            }
                            foreach (char colorid in "syprhwmi")
                            {
                                if (colorid == input.ToLowerInvariant()[i + 1])
                                {
                                    output.Add("&" + nextcolor + nextoutput);
                                    nextoutput = "";
                                    nextcolor = colorid;
                                    continue;
                                }
                            }
                            continue;
                        }
                    }
                    else if (i > 0)
                    {
                        if (input[i - 1] == '&')
                        {
                            if (i > 1)
                            {
                                if (input[i - 2] == '\\')
                                {
                                    nextoutput += input[i];
                                    continue;
                                }
                            }
                            foreach (char colorid in "0123456789abcdef")
                            {
                                if (colorid == input[i])
                                {
                                    continue;
                                }
                            }
                            foreach (char colorid in "syprhwmi")
                            {
                                if (colorid == input.ToLowerInvariant()[i])
                                {
                                    output.Add("&" + nextcolor + nextoutput);
                                    nextoutput = "";
                                    nextcolor = colorid;
                                    continue;
                                }
                            }
                            continue;
                        }
                    }
                    if (input[i] == '&') nextoutput += "\\";
                    nextoutput += input[i];
                }
                output.Add("&" + nextcolor + nextoutput);
                return output.ToArray();
            }
        }

        public static string StripFormatting(this string input)
        {
            if (input == null) return "";
            if (input.IndexOf('&') == -1)
            {
                return input;
            }
            else
            {
                StringBuilder output = new StringBuilder(input.Length);
                for (int i = 0; i < input.Length; i++)
                {
                    if (input[i] == '&')
                    {
                        if (i == input.Length - 1)
                        {
                            break;
                        }
                        i++;
                        if (input[i] == 'n' || input[i] == 'N')
                        {
                            output.Append('\n');
                        }
                    }
                    else
                    {
                        output.Append(input[i]);
                    }
                }
                return output.ToString();
            }
        }
    }

    public static partial class OYS_DateTime
    {
        /// <summary>
        /// Returns a string array of the specified datetime in OYS standard type values.
        /// </summary>
        /// <param name="ThisDateTime"></param>
        /// <returns>YYYY, MM, DD, HH, mm, ss</returns>
        public static string[] ToOYSFormat(this DateTime ThisDateTime)
        {
            #region Year
            string Year = ThisDateTime.Year.ToString();
            if (ThisDateTime.Year.ToString().Length > 4) Year = Year.Substring(0, 4);
            while (Year.Length < 4) Year = "0" + Year;
            #endregion
            #region Month
            string Month = ThisDateTime.Month.ToString();
            if (ThisDateTime.Month.ToString().Length > 2) Month = Month.Substring(0, 2);
            while (Month.Length < 2) Month = "0" + Month;
            #endregion
            #region Day
            string Day = ThisDateTime.Day.ToString();
            if (ThisDateTime.Day.ToString().Length > 2) Day = Day.Substring(0, 2);
            while (Day.Length < 2) Day = "0" + Day;
            #endregion
            #region Hour
            string Hour = ThisDateTime.Hour.ToString();
            if (ThisDateTime.Hour.ToString().Length > 2) Hour = Hour.Substring(0, 2);
            while (Hour.Length < 2) Hour = "0" + Hour;
            #endregion
            #region Minute
            string Minute = ThisDateTime.Minute.ToString();
            if (ThisDateTime.Minute.ToString().Length > 2) Minute = Minute.Substring(0, 2);
            while (Minute.Length < 2) Minute = "0" + Minute;
            #endregion
            #region Second
            string Second = ThisDateTime.Second.ToString();
            if (ThisDateTime.Second.ToString().Length > 2) Second = Second.Substring(0, 2);
            while (Second.Length < 2) Second = "0" + Second;
            #endregion
            string[] Output = { Year, Month, Day, Hour, Minute, Second };
            return Output;
        }
        public static string ToOYSLongDateTime(this DateTime CurrentTime)
        {
            string[] FormattedTime = ToOYSFormat(CurrentTime);
            return String.Format("{0}{1}{2}({3}{4}{5})", FormattedTime[0], FormattedTime[1], FormattedTime[2], FormattedTime[3], FormattedTime[4], FormattedTime[5]);
        }
        public static string ToOYSShortDateTime(this DateTime CurrentTime)
        {
            string[] FormattedTime = ToOYSFormat(CurrentTime);
            return String.Format("{0}{1}{2}({3}{4})", FormattedTime[0], FormattedTime[1], FormattedTime[2], FormattedTime[3], FormattedTime[4]);
        }
        public static string ToOYSDate(this DateTime CurrentTime)
        {
            string[] FormattedTime = ToOYSFormat(CurrentTime);
            return String.Format("{0}{1}{2}", FormattedTime[0], FormattedTime[1], FormattedTime[2]);
        }
        public static string ToOYSLongTime(this DateTime CurrentTime)
        {
            string[] FormattedTime = ToOYSFormat(CurrentTime);
            return String.Format("{0}{1}{2}", FormattedTime[3], FormattedTime[4], FormattedTime[5]);
        }
        public static string ToOYSShortTime(this DateTime CurrentTime)
        {
            string[] FormattedTime = ToOYSFormat(CurrentTime);
            return String.Format("{0}{1}", FormattedTime[3], FormattedTime[4]);
        }
    }
}
