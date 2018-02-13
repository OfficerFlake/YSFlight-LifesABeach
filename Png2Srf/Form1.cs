using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Threading;

namespace LifesABeach
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            this.AllowDrop = true;
            this.DragEnter += new DragEventHandler(Form1_DragEnter);
            this.DragDrop += new DragEventHandler(Form1_DragDrop);
        }

        void Form1_DragEnter(object sender, DragEventArgs e) {
          if (e.Data.GetDataPresent(DataFormats.FileDrop)) e.Effect = DragDropEffects.Copy;
        }

        void Form1_DragDrop(object sender, DragEventArgs e)
        {
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
            foreach (string file in files)
            {
                if (!file.ToUpperInvariant().EndsWith(".FLD"))
                {
                    StatusLabel.Text = "Dropped File is NOT an FLD File!\nCannot add beaches!";
                    StatusLabel.ForeColor = Color.FromArgb(0, 255, 0, 0);
                    continue;
                }

                int TotalBeaches = 0;

                try
                {
                    StatusLabel.Text = "Working...";
                    StatusLabel.ForeColor = Color.FromArgb(0, 255, 220, 0);
                    this.Refresh();

                    double Distance = 0;
                    if (!Double.TryParse(TextBox_BeachSize.Text, out Distance))
                    {
                        StatusLabel.Text = "Distance specified is NOT a number!";
                        StatusLabel.ForeColor = Color.FromArgb(0, 255, 0, 0);
                        continue;
                    }

                    Code.CurrentOperation Result = Code.CurrentOperation.Closed;
                    Result = Code.LoadFLD(file);

                    if (Result == Code.CurrentOperation.Load_OK)
                    {
                        StatusLabel.Text = "Add Shores...";
                        StatusLabel.ForeColor = Color.FromArgb(0, 255, 220, 0);
                        this.Refresh();
                    }

                    List<Code.Area> Processing_Areas = Code.Loaded_Areas.Where(y => y.Tag.ToUpperInvariant() == "BEACH").ToList();
                    List<Code.Area> Processed_Areas = new List<Code.Area>();
                    foreach (Code.Area ThisArea in Processing_Areas)
                    {
                        Code.Area Dupe = new Code.Area();
                        Dupe.AreaType = ThisArea.AreaType;
                        Dupe.ID = ThisArea.ID;
                        Dupe.Loop = ThisArea.Loop;
                        //Dupe.Points = ThisArea.Points;
                        Dupe.Position = ThisArea.Position;
                        Dupe.Tag = ThisArea.Tag;
                        Processed_Areas.Add(Dupe);
                        //Re-position points!
                        foreach (Code.Point2D ThisPoint in ThisArea.Points)
                        {
                            Processed_Areas.Last().Points.Add(new Code.Point2D(ThisPoint.X + ThisArea.Position.X, ThisPoint.Y + ThisArea.Position.Y));
                        }
                        TotalBeaches++;
                    }

                    List<Code.Area> Beaches = new List<Code.Area>();
                    foreach (Code.Area ThisArea in Processed_Areas)
                    {
                        Code.Area Dupe = new Code.Area();
                        Dupe.AreaType = ThisArea.AreaType;
                        Dupe.ID = ThisArea.ID;
                        Dupe.Loop = ThisArea.Loop;
                        //Dupe.Points = ThisArea.Points;
                        Dupe.Position = ThisArea.Position;
                        Dupe.Tag = ThisArea.Tag;
                        Beaches.Add(Dupe);
                        //Re-position points!
                        for (int i = 0; i < ThisArea.Points.Count; i++)
                        {
                            Code.Point2D P_Prev    = new Code.Point2D(0,0);
                            Code.Point2D P_Current = new Code.Point2D(0,0);
                            Code.Point2D P_Next    = new Code.Point2D(0,0);
                            if (i == 0)
                            {
                                //First Point!
                                P_Prev = ThisArea.Points.Last();
                                if (!ThisArea.Loop) P_Prev = ThisArea.Points[i];
                            }
                            if (i > 0)
                            {
                                P_Prev = ThisArea.Points[i-1];
                            }
                            P_Current = ThisArea.Points[i];
                            if (i < ThisArea.Points.Count - 1)
                            {
                                P_Next = ThisArea.Points[i + 1];
                            }
                            if (i == ThisArea.Points.Count-1)
                            {
                                //Last Point!
                                P_Next = ThisArea.Points.First();
                                if (!ThisArea.Loop) P_Next = ThisArea.Points[i];
                            }

                            Code.Point2D NewPoint = new Code.Point2D(0,0);
                            Code.Point2D __Prev = new Code.Point2D(P_Prev.X - P_Current.X, P_Prev.Y - P_Current.Y);
                            Code.Point2D __Next = new Code.Point2D(P_Next.X - P_Current.X, P_Next.Y - P_Current.Y);
                            double Difference_Angle = -(180 / Math.PI) * Math.Atan2(__Prev.X * __Next.Y - __Prev.Y * __Next.X, __Prev.X * __Next.X + __Prev.Y * __Next.Y);
                            while (Difference_Angle < 0) Difference_Angle += 360;
                            double Current_Angle = -(180 / Math.PI) * Math.Atan2(-__Next.Y, __Next.X);
                            while (Current_Angle < 0) Current_Angle += 360;
                            //MessageBox.Show(
                            //    "i   : " + i + "\n" +
                            //    "CP-1: " + P_Prev + "\n" +
                            //    "CP:   " + P_Current + "\n" +
                            //    "CP+1: " + P_Next + "\n" +
                            //    "\n" +
                            //    "_1    " + new Code.Vector2D(__Prev, new Code.Point2D(0, 0)) + "\n" +
                            //    "_2    " + new Code.Vector2D(__Next, new Code.Point2D(0, 0)) + "\n" +
                            //    "\n" +
                            //    "CA:   " + Current_Angle + "\n" +
                            //    "DA:   " + Difference_Angle + "\n" +
                            //    "NA:   " + (Current_Angle + Difference_Angle / 2)
                            //    );
                            if (!ThisArea.Loop)
                            {
                                if (i == 0)
                                {
                                    //First Point
                                    Difference_Angle = 180; //Hypothetical Straight Line...
                                }
                                if (i == ThisArea.Points.Count - 1)
                                {
                                    //Last Point
                                    Difference_Angle = 180; //Hypothetical Straight Line...
                                    Current_Angle = -(180 / Math.PI) * Math.Atan2(__Prev.Y, -__Prev.X);
                                }
                            }
                            double NewAngle = (Current_Angle + Difference_Angle / 2) * (Math.PI/180);
                            NewPoint.X = P_Current.X + (Math.Cos(NewAngle) * Distance);
                            NewPoint.Y = P_Current.Y + (Math.Sin(NewAngle) * Distance);
                            //MessageBox.Show(
                            //    "i   : " + i + "\n" +
                            //    "CP-1: " + P_Prev + "\n" +
                            //    "CP:   " + P_Current + "\n" +
                            //    "CP+1: " + P_Next + "\n" +
                            //    "\n" +
                            //    "_1    " + new Code.Vector2D(__Prev, new Code.Point2D(0, 0)) + "\n" +
                            //    "_2    " + new Code.Vector2D(__Next, new Code.Point2D(0, 0)) + "\n" +
                            //    "\n" +
                            //    "CA:   " + Current_Angle + "\n" +
                            //    "DA:   " + Difference_Angle + "\n" +
                            //    "NA:   " + (Current_Angle + Difference_Angle / 2) +
                            //    "\n" +
                            //    "CP:   " + P_Current + "\n" +
                            //    "NP:   " + NewPoint + "\n"
                            //    );
                            Beaches.Last().Points.Add(NewPoint);
                        }
                    }
                    string[] FLD_Contents = File.ReadAllLines(file);
                    List<string> Output = FLD_Contents.ToList();
                    //Output.RemoveAt(Output.Count - 1);
                    for (int j = 0; j < Beaches.Count;j++)
                    {
                        Code.Area ThisBeach = Beaches[j];
                        ////Spit out area
                        //Output.Add("PST");
                        //Output.Add("ISLOOP TRUE");
                        //Output.Add("AREA " + ThisBeach.AreaType);

                        //foreach (Code.Point2D ThisPoint in ThisBeach.Points)
                        //{
                        //    Output.Add("PNT " + ThisPoint.X + " 0.00 " + ThisPoint.Y);
                        //}

                        //Output.Add("FIL \"\"");
                        //Output.Add("POS 0.00 0.00 0.00 0.00 0.00 0.00");
                        //Output.Add("ID " + ThisBeach.ID);
                        //Output.Add("TAG BEACHED");
                        //Output.Add("END");

                        //spit out beach
                        string _BeachFileName = DateTime.Now.ToOYSLongDateTime().ReplaceAll("(", "").ReplaceAll(")", "") + "_" + j.ToString() + ".pc2";
                        int ExtraLines = 6;
                        if (ThisBeach.Loop) ExtraLines += 2;
                        Output.Add("PCK \"" + _BeachFileName + "\" " + (ExtraLines + ThisBeach.Points.Count * 2));
                        Output.Add("Pict2");
                        Output.Add("GQS");
                        Output.Add("COL " + Color1_Form.Color.R + " " + Color1_Form.Color.G + " " + Color1_Form.Color.B + " ");
                        Output.Add("CL2 " + Color2_Form.Color.R + " " + Color2_Form.Color.G + " " + Color2_Form.Color.B + " ");

                        for (int i = 0; i < ThisBeach.Points.Count; i++)
                        {
                            Output.Add("VER " + Beaches[j].Points[i].X + " " + Beaches[j].Points[i].Y);
                            Output.Add("VER " + Processed_Areas[j].Points[i].X + " " + Processed_Areas[j].Points[i].Y);

                            if (i == ThisBeach.Points.Count - 1 & ThisBeach.Loop)
                            {
                                Output.Add("VER " + Beaches[j].Points[0].X + " " + Beaches[j].Points[0].Y);
                                Output.Add("VER " + Processed_Areas[j].Points[0].X + " " + Processed_Areas[j].Points[0].Y);
                            }
                        }

                        Output.Add("ENDO");
                        Output.Add("ENDPICT");
                        Output.Add("");
                        Output.Add("PC2");
                        Output.Add("FIL \"" + _BeachFileName + "\"");
                        Output.Add("POS 0.00 0.00 0.00 0.00 0.00 0.00");
                        Output.Add("ID 0");
                        Output.Add("END");
                    }
                    File.WriteAllLines(file.Substring(0, file.Length - 4) + "_Beach.FLD", Output.ToArray());
                }
                catch (Exception error)
                {
                    MessageBox.Show("Exception!\n\n" + error.ToString());
                    StatusLabel.Text = "!!! EXCEPTION !!!";
                    StatusLabel.ForeColor = Color.FromArgb(0, 255, 0, 0);
                    return;
                }

                StatusLabel.Text = "Done (" + TotalBeaches + " Beaches)!\nDrop Another FLD to add more beaches!";
                StatusLabel.ForeColor = Color.FromArgb(0, 0, 255, 0);
            }
        }

        private void Color1_Button_Click(object sender, EventArgs e)
        {
            Color1_Form.ShowDialog();
            Color1_Display.BackColor = Color1_Form.Color;
        }

        private void Color2_Button_Click(object sender, EventArgs e)
        {
            Color2_Form.ShowDialog();
            Color2_Display.BackColor = Color2_Form.Color;
        }
    }
}
