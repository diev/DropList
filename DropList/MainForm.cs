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
            ListMethods.SetListText(FilelistBox, files);
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
                MessageBox.Show("Нечего очищать", Program.Title);
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
                MessageBox.Show("Выделите строку", Program.Title);
            }
        }

        private void CopyButton_Click(object sender, EventArgs e)
        {
            if (FilelistBox.Items.Count > 0)
            {
                string text = ListMethods.GetListText(FilelistBox);
                Clipboard.SetText(text);
            }
            else
            {
                MessageBox.Show("Нечего копировать", Program.Title);
            }
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            if (FilelistBox.Items.Count > 0)
            {
                string text = ListMethods.GetListText(FilelistBox);
                DialogMethods.SaveText(text);
            }
            else
            {
                MessageBox.Show("Нечего сохранять", Program.Title);
            }
        }

        private void AboutButton_Click(object sender, EventArgs e)
        {
            DialogMethods.About();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            PersistentWindow.Restore(this);

            string[] args = Environment.GetCommandLineArgs();
            ListMethods.SetListText(FilelistBox, args);
            FilelistBox.Items.RemoveAt(0); // App's exe
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            PersistentWindow.Save(this);
        }
    }
}
