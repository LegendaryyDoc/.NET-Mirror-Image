using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Drawing;
using System.Windows.Media.Imaging;

namespace ConsoleApp1
{
    class Convert
    {
        private void Main(string[] args)
        {
            convertImageData();

            return;
        }

        public string directoryPath = "C:/Users/s189065/source/repos/ImageFlipTool/TestStart";
        public string saveDirectory = "C:/Users/s189065/source/repos/ImageFlipTool/TestEnd";

        public void convertImageData()
        {
            if (!Directory.Exists(directoryPath))
            {
                Console.Write("Path trying to pull from does not exist!");
                return;
            }

            // Get all the file paths in it
            string[] fileNames;
            fileNames = Directory.GetFiles(directoryPath);

            // Check if the file path is the type we want (.png)
            foreach (string f in fileNames)
            {
                if (!File.Exists(f))
                {
                    Console.Write("File does not exist!");
                    return;
                }

                string strFileExtention = System.IO.Path.GetExtension(f).ToUpper();

                //If path is what we want get all the data for the image
                if (strFileExtention == ".jpg".ToUpper() || strFileExtention == ".jpeg".ToUpper() || strFileExtention == ".png".ToUpper())
                {
                    Bitmap bitmap = new Bitmap(f);

                    byte[] fileContents = File.ReadAllBytes(f);
                    byte[] reverseFileContents = new byte[bitmap.Height - 1 * bitmap.Width - 1];
                    byte[] lineContents = new byte[bitmap.Width - 1];

                    int byteNum = 0;

                    // go through the image and mirror each row
                    for (int i = 0; i < bitmap.Height; i++)
                    {
                        lineContents = new byte[bitmap.Width];

                        for (int j = 0; j < bitmap.Width; j++)
                        {
                            lineContents[j] = fileContents[(i * bitmap.Width) + j];
                        }

                        Array.Reverse(lineContents);

                        for (int j = 0; j < lineContents.Length; j++)
                        {
                            reverseFileContents[byteNum] = lineContents[j];
                            byteNum++;
                        }
                    }

                    // combine the original array and the mirrored array
                    byte[] combinedFileContents = new byte[bitmap.Height * (bitmap.Width - 1 * 2)];

                    for (int j = 0; j < bitmap.Height; j++)
                    {
                        for (int k = 0; k <= bitmap.Width - 1 * 2; k++)
                        {
                            if (k < bitmap.Width)
                            {
                                combinedFileContents[(j * bitmap.Width) + k] = fileContents[(j * bitmap.Width) + k];
                            }
                            else
                            {
                                combinedFileContents[(j * bitmap.Width) + k] = reverseFileContents[(j * bitmap.Width) + k];
                            }
                        }
                    }

                    // delete the current image and save the mirrored image to a different filepath
                    FileInfo fi = new FileInfo(f);

                    if (!File.Exists(saveDirectory + fi.Name))
                    {
                        File.Create(saveDirectory + fi.Name);
                    }

                    File.WriteAllBytes(saveDirectory + fi.Name, combinedFileContents);
                }

                return;
            }
        }
    }
}
