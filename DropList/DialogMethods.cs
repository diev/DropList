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

using DropList.Properties;

using System.IO;
using System.Windows.Forms;

namespace DropList
{
    public static class DialogMethods
    {
        public static void SaveText(string text)
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
                File.WriteAllText(file, text);

                path = Path.GetDirectoryName(file);
                Settings.Default.SaveDirectory = path;
            }
        }
    }
}
