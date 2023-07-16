using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Windows.Forms;

namespace DayzTreeRandom
{
    public partial class Form1 : Form
    {
        private List<int[]> PositionInfo;
        private double scaleMinInput;
        private double scaleMaxInput;
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {            
            OpenFileDialog file = new OpenFileDialog();
            file.Title = "Select File";
            file.InitialDirectory = @"C:\";
            file.Filter = "Terrain Builder Txt file only|*.txt";
            file.FilterIndex = 1;
            file.ShowDialog();
            if (file.FileName != "")
            {
                textBox3.Text = file.FileName;               
                textBox4.Text = Path.ChangeExtension(file.FileName, null) + "_resized.txt";
            }
            else
            {
                textBox3.Text = "";
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (!CheckInputData())
            {                
                return;
            }
            Random rnd = new Random();
            var fileContent = InputFileContent(textBox3.Text);
            var fileContentExported = new List<String>();
            PositionInfo = new List<int[]>();
            for (int i = 0; i < fileContent.Count; i++)
            {
                GetStringLocationInfo(fileContent[i]);
            }
            if (PositionInfo[0][4] == 0)
            {
                MessageBox.Show("Invalid data found in the txt file, Check file and try again.");
                return;
            }

            string origLine;
            string lineStart;            
            string nextPart;            
            int scaleMin = (int)(scaleMinInput * 1000);
            int scaleMax = (int)(scaleMaxInput * 1000);

            for (int i = 0; i < PositionInfo.Count; i++)
            {                
                origLine = fileContent[i];
                StringBuilder newLine = new StringBuilder(origLine);
                
                if (checkBox1.Checked) // rotation is checked, create string up to existing rotation data and then randomize it
                {
                    lineStart = newLine.ToString(0, PositionInfo[i][0] + 1);
                    newLine = new StringBuilder(lineStart);
                    int rotation = rnd.Next(0, 360);
                    newLine.Append(rotation);
                }
                else //if random rotate isn't ticked, start string after existing rotation data
                {
                    lineStart = newLine.ToString(0, PositionInfo[i][1]);
                    newLine = new StringBuilder(lineStart);
                }     
                
                if (checkBox2.Checked) // scaling is checked
                {
                    //append existing data after rotation up to the start of scale data
                    nextPart = origLine.Substring(PositionInfo[i][1], PositionInfo[i][2] - PositionInfo[i][1] + 1);
                    newLine.Append(nextPart);
                    int scale = rnd.Next(scaleMin, scaleMax);
                    float scaleDiv = (float)scale / 1000;
                    newLine.Append(scaleDiv);
                    nextPart = origLine.Substring(PositionInfo[i][3], fileContent[i].Length - PositionInfo[i][3]);
                    newLine.Append(nextPart);
                }
                else // no scaling done, append all the remaining data
                {
                    nextPart = origLine.Substring(PositionInfo[i][1], fileContent[i].Length - PositionInfo[i][1]);
                    newLine.Append(nextPart);
                }                
                
                fileContentExported.Add(newLine.ToString());
            }

            StreamWriter sw = new StreamWriter(textBox4.Text);
            foreach (var str in fileContentExported)
            {
                sw.WriteLine(str);
            }
            sw.Close();
            MessageBox.Show("Export completed!");


        }

        /// <summary>
        /// gets the location of the semi-colons in input file so we can preserve the data thats not being changed
        /// </summary>
        /// <param name="str"></param>
        private void GetStringLocationInfo(string str)
        {
            int[] p = new int[5];
            
            char  t = ';';
            int count = 0;
            for (int i = 0; i < str.Length; i++)
            {
                if (str[i] == t)
                {
                    count++;
                    if (count == 3)
                    {
                        p[0] = i;
                    }
                    else if(count == 4)
                    {
                        p[1] = i;
                    }
                    else if (count == 6)
                    {
                        p[2] = i;
                    }
                    else if (count == 7)
                    {
                        p[3] = i;
                    }
                    else if (count == 8)
                    {
                        p[4] = i;
                    }
                }                
            }
            PositionInfo.Add(p);            
        }

        /// <summary>
        /// grabs input file data and puts it into a list for later use
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        private static List<string> InputFileContent(string filePath)
        {
            List<string> fileContent = new List<string>();
            StreamReader sr = new StreamReader(filePath);
            while (!sr.EndOfStream)
            {
                fileContent.Add(sr.ReadLine());
            };
            sr.Close();
            return fileContent;
        }

        /// <summary>
        /// some basic data checks to stop some errors
        /// </summary>
        /// <returns></returns>
        private bool CheckInputData()
        {
            // check scale data correct
            if (checkBox2.Checked)
            {
                if (!double.TryParse(textBox1.Text, out double parsedValue1) || !double.TryParse(textBox2.Text, out double parsedValue2))
                {
                    MessageBox.Show("scale size is invalid");
                    return false;
                }
                else if (parsedValue1 > parsedValue2)
                {
                    MessageBox.Show("scale size is invalid - Min is greater than Max!");
                    return false;
                }
                else if (parsedValue1 == parsedValue2)
                {
                    MessageBox.Show("scale size is invalid - Min is equal to Max, uncheck scale if you dont want to scale");
                    return false;
                }
                else if (parsedValue2 > 5)
                {
                    MessageBox.Show("scale size is invalid - Max value is greater than limit of 5");
                    return false;
                }
                else
                {
                    scaleMinInput = Math.Round(parsedValue1, 2);
                    scaleMaxInput = Math.Round(parsedValue2, 2);
                }
            }
            if (string.IsNullOrEmpty(textBox3.Text))
            {
                MessageBox.Show("No input file has been selected!");
                return false;
            }
            if (!checkBox1.Checked && !checkBox2.Checked)
            {
                MessageBox.Show("Neither scale or rotation is checked, need at least one checked to do some work!");
                return false;
            }
            return true;
        }         
    }
}
