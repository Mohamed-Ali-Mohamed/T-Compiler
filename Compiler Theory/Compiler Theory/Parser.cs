﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Compiler_Theory
{
    public partial class Parser : Form
    {
        public Parser()
        {
            InitializeComponent();
        }

        private void AddFileButton_Click(object sender, EventArgs e)
        {
           OpenFileDialog openFileDialog1 = new OpenFileDialog();
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string OpenedFilePath = openFileDialog1.FileName;//Get Path of file
                OpenFile newFile = new OpenFile();
                string [] FileData = newFile.GetFile(OpenedFilePath);//Get file in array of string
                FileDataBox.Text="";//Clear output list
                foreach (string line in FileData)
                {
                    FileDataBox.Text+=(line+"\r\n");
                }
            }
                
        }

        private void RunButton_Click(object sender, EventArgs e)
        {
            string[] FileData = FileDataBox.Text.Split(new string[] { "\r\n" }, StringSplitOptions.None);
            
            CompilerScanner newScanner = new CompilerScanner();
            List<KeyValuePair<string, string>> ScannerData = new List<KeyValuePair<string, string>>();
            newScanner.StartScanner(FileData, ref ScannerData);

            TreeNode ParserTreeRoot = null;
            bool NoError = true;
            ParserTreeView.Nodes.Clear();
            ScannerData=newScanner.GetScannerData(ScannerData, ref NoError);
            if(!NoError)
            {
                MessageBox.Show("Error in Scanner");
                return;
            }
            CompilerParser newParser = new CompilerParser();
            bool IsParserTreeDone = newParser.CreateParseTree(ScannerData, ref ParserTreeRoot);
            
            if (ParserTreeRoot!=null)
                this.ParserTreeView.Nodes.Add(ParserTreeRoot);
            CheckExpandAllCheckBox();
        }

        private void ExpandAllCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            CheckExpandAllCheckBox();
        }

        private void CheckExpandAllCheckBox()
        {
            if (ExpandAllCheckBox.Checked)
            {
                ParserTreeView.ExpandAll();
            }
            else
            {
                ParserTreeView.CollapseAll();
            }
        }
    }
}
