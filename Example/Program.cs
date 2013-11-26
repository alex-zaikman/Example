using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;


namespace asz
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            //get local config
            string localConfigPath = Util.getCurrentPath() + "config.asz";
            Dictionary<string, string> localConfig = Util.readLocalConfig(localConfigPath);
            //get remote config
            Dictionary<string, string> remoteConfig = Util.readRemoteConfig(localConfig["updateserver"]+"/config.asz");
            if(Convert.ToInt32(localConfig["versionnum"])< Convert.ToInt32(remoteConfig["versionnum"])){
                //ask user to update
                DialogResult res = Util.raiseYesNo("new vertion founs","new vertion of CGS found\nplease update");
                switch (res)
                {
                    case DialogResult.Yes:
                            //start progress indicator
                            Application.EnableVisualStyles();
                            Application.SetCompatibleTextRenderingDefault(false);
                            Application.Run(new CGS());       
                            //create tmp dir
                            DirectoryInfo di = null;
                            string tmpdirpath = Util.getCurrentPath() + "~update";
                            
                            if (Directory.Exists(tmpdirpath)) 
                            {
                                Directory.Delete(tmpdirpath,true);   
                            }
                            di = Directory.CreateDirectory(tmpdirpath);
            
                            //download zip
                            string zipFilePath = tmpdirpath + "\\" + remoteConfig["filetodownload"].TrimEnd('\r', '\n');
                            Util.downloadFile(localConfig["updateserver"].TrimEnd( '\r', '\n' ,'/') +"/"+remoteConfig["filetodownload"].TrimEnd( '\r', '\n' ), 
                             zipFilePath );
                            //extract
                            Util.unzip(zipFilePath, tmpdirpath);
                            File.Delete(zipFilePath);

                            Util.CopyFolder(tmpdirpath,Util.getCurrentPath());
                            //write new local config
                            localConfig["versionnum"]= remoteConfig["versionnum"];
                            localConfig["versionname"]= remoteConfig["versionname"];
                            Util.writeLocalConfig(localConfigPath,localConfig);
                            //del dir
                            di.Delete(true);
                           


                            break;
                    case DialogResult.No:
                    default:
                        break;

                }

                 //run cef proccess
                Process.Start("cgsclient.exe");
            }

        

        }
    }
}
