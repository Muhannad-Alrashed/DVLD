using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DVLD_Business;
using System.IO;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;
using System.Windows.Forms;
using System.Drawing;

namespace DVLD.Core
{
    public class clsGlobal
    {
        public static clsUser CurrentUser = new clsUser();

        public static  bool RememberUserNameAndPassword(string Username , string Password)
        {
            try
            {
                string currentDirectory = System.IO.Directory.GetCurrentDirectory();
                string filePath = currentDirectory + "\\data.txt";

                string dataToSave = Username + "#//#" + Password;

                using (StreamWriter writer = new StreamWriter(filePath))
                {
                    writer.WriteLine(dataToSave);
                    return true;
                }
            }
            catch (Exception ex)
            {
                string Error = ex.Message;
                return false;
            }
        }

        public static bool GetLoginCredentials(ref string Username, ref string Password)
        {
            try
            {
                string currentDirectory = System.IO.Directory.GetCurrentDirectory();
                string filePath = currentDirectory + "\\data.txt";

                if (!File.Exists(filePath))
                    return false;

                using (StreamReader reader = new StreamReader(filePath))
                {
                    List<string> LoginInfo = clsUtil.Split(reader.ReadLine() , "#//#");
                    
                    if(LoginInfo.Count > 0)
                    {
                        Username = LoginInfo[0];
                        Password = LoginInfo[1];
                    }
                    return true;
                }
            }
            catch (Exception ex)
            {
                string Error = ex.Message;
                return false;
            }
        }

        public static string SaveImageAndGetFilePath(Image Image)
        {
            try
            {
                string currentDirectory = System.IO.Directory.GetCurrentDirectory();
                string folderPath = currentDirectory + "\\DVLD_Images";

                if (!Directory.Exists(folderPath))
                    Directory.CreateDirectory(folderPath); ;

                string fileName = $"{Guid.NewGuid()}.jpg";
                string filePath = Path.Combine(folderPath, fileName);

                Image.Save(filePath, System.Drawing.Imaging.ImageFormat.Jpeg);
                return filePath;

            }
            catch (Exception ex)
            {
                string Error = ex.Message;
                return string.Empty;
            }
        }

        public static void DeleteFile(string filePath)
        {
            if (filePath == "")
                return;

            if (File.Exists(filePath))
            {
                try
                {
                    File.Delete(filePath);
                }
                catch (Exception ex)
                {
                    string Error = ex.Message;
                }
            }
        }
    }
}
