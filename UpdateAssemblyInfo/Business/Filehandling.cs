using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace UpdateAssemblyInfo.Business
{
    public class Filehandling
    {
        public List<FileInfo> FileList { get; protected set; }

        public DirectoryInfo SolutionRoot { get; set; }

        protected String FileFilter
        {
            get { return @"AssemblyInfo.cs"; }
        }


        private List<FileInfo> LevelDown(DirectoryInfo node)
        {
            var result = new List<FileInfo>();

            // files.
            var current = node.GetFiles(FileFilter);
            if (current.Length > 0)
            {
                result.AddRange(current);
            }

            // Subdirectories
            var sudirs = node.GetDirectories();
            foreach (var dir in sudirs)
            {
                result.AddRange(LevelDown(dir)); // Recursive
            }

            return result;
        }

        /// <summary>
        /// Fills FileList with files matching filter below SolutionRoot.
        /// </summary>
        /// <returns></returns>
        public int LocateFiles()
        {
            FileList = LevelDown(SolutionRoot);
            return FileList.Count;
        }


        public static String PayloadRead(FileInfo file)
        {
            var fs = file.OpenRead();
            using (var t2 = new System.IO.StreamReader(fs, Encoding.UTF8))
            {
                var data = t2.ReadToEnd();
                return data;
            }
        }

        public static void PayloadWrite(FileInfo file, String payload)
        {
            file.Delete();
            var fs = file.OpenWrite();
            using (var t2 = new StreamWriter(fs, Encoding.UTF8))
            {
                t2.Write(payload);
            }
        }

        public static int PayloadRead(out List<String> rows, FileInfo file)
        {
            rows =new List<string>();

            var fs = file.OpenRead();
            using (var t2 = new StreamReader(fs, Encoding.UTF8))
            {
                var data = t2.ReadLine();
                while (data !=null)
                {
                    rows.Add(data);
                    data = t2.ReadLine();
                }
            }

            return rows.Count;
        }

        public static void PayloadWrite(List<String> rows, FileInfo file)
        {
            file.Delete();
            var fs = file.OpenWrite();
            using (var t2 = new StreamWriter(fs, Encoding.UTF8))
            {
                foreach (var row in rows)
                {
                    t2.WriteLine(row);
                }
            }
            
        }


    }
}
