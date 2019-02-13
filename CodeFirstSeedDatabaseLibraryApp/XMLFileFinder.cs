using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace CodeFirstSeedDatabaseLibraryApp
{
    public static class XMLFileFinder
    {
        /// <summary>
        /// Searches the directory for a file matching the name given, assuming the file is present.
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static string FindPath(string fileName) //This method is static as it will be used to locate the XML files when the program closes
        {
            //First search in the assembly directory for the file, this step just skips the search method below.
            if (File.Exists(fileName))
            {
                return fileName;
            }

            string root;
            try
            {
                root = Path.GetPathRoot(System.Reflection.Assembly.GetEntryAssembly().Location);
            }
            catch //This try/catch is for the unit tests which don't appear to work with System.Reflection
            {
                root = @"..\..\..\";
            }
            string file;

            file = Directory.GetFiles(root, fileName, SearchOption.AllDirectories).FirstOrDefault();

            //This if statement is included incase the XML files are missing but the database does exist, it is called when the program exits to create the files in the assembly location
            if (file == null)
            {
                file = fileName;
            }
            return file;
        }
    }
}
