using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Media.Imaging;
using System.IO;

namespace ConsoleApp2
{
    class Program
    {
        static void Main(string[] args)
        {
            Program program = new Program();
            program.convertImageData();
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
                    byte[] reverseFileContents = new byte[bitmap.Height * bitmap.Width];
                    byte[] lineContents = new byte[bitmap.Width];

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
                    byte[] combinedFileContents = new byte[bitmap.Height * (bitmap.Width* 2)];

                    for (int j = 0; j < bitmap.Height; j++)
                    {
                        int m = 0;

                        for (int k = 0; k <= bitmap.Width - 1 * 2; k++)
                        {
                            if (k < bitmap.Width)
                            {
                                combinedFileContents[(j * bitmap.Width) + k] = fileContents[(j * bitmap.Width) + k];
                            }
                            else
                            {
                                combinedFileContents[(j * bitmap.Width) + k] = reverseFileContents[(j * bitmap.Width) + m];
                                m++;
                            }
                        }
                    }

                    // delete the current image and save the mirrored image to a different filepath
                    FileInfo fi = new FileInfo(f);

                    if(!Directory.Exists(saveDirectory))
                    {
                        Console.Write("Save directory does not exist!");
                        return;
                    }

                    if (!File.Exists(saveDirectory + "/" + fi.Name))
                    {
                        Console.Write("Creating filepath because could not find it!");
                        File.Create(saveDirectory + "/" + fi.Name);
                    }

                    Bitmap b = new Bitmap(bitmap.Width * 2, bitmap.Width);

                    

                    b.Save(saveDirectory + "/" + fi.Name, System.Drawing.Imaging.ImageFormat.Png);

                    bitmap.Dispose();
                    b.Dispose();

                    //File.WriteAllBytes(saveDirectory + fi.Name, combinedFileContents);
                }

                return;
            }
        }
    }
}