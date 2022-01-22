using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Ookii.Dialogs.Wpf;
using System.Windows.Input;

namespace Client
{
    internal class File
    {
        string path = "";
        string text = "";
        bool is_polindrome = false;

        public File(string path)
        {
            this.path = path;
            SetText();
        }

        private void SetText()
        {
            using (StreamReader reader = new StreamReader(path))
            {
                char[] buff = new char[128];
                reader.ReadBlock(buff, 0, 128);
                text = new string(buff) + "...";
            }
        }
        public string Text { get { return text; } set { text = value; } }
        public bool IsPolindrome { get { return is_polindrome; } set { is_polindrome = value; } }

    }
}
