﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using System.IO.Compression;
using System.Windows.Forms;
using System.ComponentModel;
using System.Threading;
using System.Resources;
using System.Drawing;
using System.Reflection;


namespace asz
{
    class Util
    {

        public static string getCurrentPath()
        {

            return AppDomain.CurrentDomain.BaseDirectory;
        }


        public static void writeLocalConfig(string fileName, Dictionary<string, string> newLocalConfig)
        {
            Dictionary<string, string>.KeyCollection keys = newLocalConfig.Keys;

            string[] lines = new string[keys.Count];
            int i = 0;
            foreach (string key in keys)
            {
                lines[i] = key + "*" + newLocalConfig[key];
                i++;
            }

            System.IO.File.WriteAllLines(fileName, lines);
        }

        public static Dictionary<string, string> readLocalConfig(string fileName)
        {
            Dictionary<string, string> localConfig = new Dictionary<string, string>();

            string line = "";
            System.IO.StreamReader file = new System.IO.StreamReader(fileName);
            while ((line = file.ReadLine()) != null)
            {
                string[] part = line.Split('*');
                localConfig.Add(part[0], part[1]);
            }
            file.Close();
            return localConfig;
        }

        public static Dictionary<string, string> readRemoteConfig(string confurl)
        {
            Dictionary<string, string> remoteConfig = new Dictionary<string, string>();

            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(confurl);
            HttpWebResponse resp = (HttpWebResponse)req.GetResponse();
            StreamReader sr;
            string conffile = "";
            using (sr = new StreamReader(resp.GetResponseStream()))
            {
                conffile = sr.ReadToEnd();
            }


            string[] lines = conffile.Split('\n');

            foreach (string line in lines)
            {
                string[] part = line.Split('*');
                remoteConfig.Add(part[0], part[1]);
                // Console.Write(part[0]+" * "+ part[1]+"\n");
            }
            return remoteConfig;
        }

        public static void unzip(string zipPath, string extractPath)
        {
            ZipFile.ExtractToDirectory(zipPath, extractPath);
        }

        public static DialogResult raiseYesNo(string title, string msg)
        {
            DialogResult result = MessageBox.Show(msg, title, MessageBoxButtons.YesNo);
            return result;
        }

        public static DialogResult raiseOK(string title, string msg)
        {
            DialogResult result = MessageBox.Show(msg, title, MessageBoxButtons.OK);
            return result;
        }

        public static DialogResult raiseErrorOK(string title, string msg)
        {

            DialogResult result = MessageBox.Show(msg, title, MessageBoxButtons.OK, MessageBoxIcon.Error);
            return result;
        }


    }
}
