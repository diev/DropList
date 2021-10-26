#region License
//------------------------------------------------------------------------------
// Copyright (c) Dmitrii Evdokimov
// Source https://github.com/diev/
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// http://www.apache.org/licenses/LICENSE-2.0
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//------------------------------------------------------------------------------
#endregion

using System;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace DropList
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void FilelistBox_DragDrop(object sender, DragEventArgs e)
        {
            //Извлекаем имя перетаскиваемого файла
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop, true);

            foreach (string file in files)
            {
                if (!FilelistBox.Items.Contains(file))
                {
                    FilelistBox.Items.Add(file);
                }
            }
        }

        private void FilelistBox_DragEnter(object sender, DragEventArgs e)
        {
            //Разрешаем Drop только файлам
            e.Effect = e.Data.GetDataPresent(DataFormats.FileDrop)
                ? DragDropEffects.All
                : DragDropEffects.None;
        }

        private void ClearButton_Click(object sender, EventArgs e)
        {
            if (FilelistBox.Items.Count > 0)
            {
                FilelistBox.Items.Clear();
            }
            else
            {
                MessageBox.Show("Нечего очищать", Text);
            }
        }

        private void DelButton_Click(object sender, EventArgs e)
        {
            int selected = FilelistBox.SelectedIndex;

            if (selected > -1)
            {
                FilelistBox.Items.RemoveAt(selected);
            }
            else
            {
                MessageBox.Show("Выделите строку", Text);
            }
        }

        private void CopyButton_Click(object sender, EventArgs e)
        {
            if (FilelistBox.Items.Count > 0)
            {
                Clipboard.SetText(GetListText());
            }
            else
            {
                MessageBox.Show("Нечего копировать", Text);
            }
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            if (FilelistBox.Items.Count > 0)
            {
                var dialog = new SaveFileDialog
                {
                    Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*",
                    FilterIndex = 1,
                    DefaultExt = "txt"
                };

                string path = Properties.Settings.Default.SaveDirectory;

                if (!string.IsNullOrEmpty(path))
                {
                    dialog.InitialDirectory = path;
                }

                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    string file = dialog.FileName;
                    File.WriteAllText(file, GetListText());

                    path = Path.GetDirectoryName(file);
                    Properties.Settings.Default.SaveDirectory = path;
                }
            }
            else
            {
                MessageBox.Show("Нечего сохранять", Text);
            }
        }

        private string GetListText()
        {
            var sb = new StringBuilder();

            foreach (string s in FilelistBox.Items)
            {
                if (s.Contains(" "))
                {
                    sb.Append("\"").Append(s).AppendLine("\"");
                }
                else
                {
                    sb.AppendLine(s);
                }
            }

            return sb.ToString();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            PersistentWindow.Restore(this);

            string[] args = Environment.GetCommandLineArgs();

            foreach (string s in args)
            {
                FilelistBox.Items.Add(s);
            }

            FilelistBox.Items.RemoveAt(0); // App's exe
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            PersistentWindow.Save(this);
        }
    }
}
