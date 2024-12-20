using System;
using System.IO;
using Microsoft.AspNetCore.Http;

namespace Project.PL.Helper
{
    public class DocumentSettings
    {
        public static string UploadFile(IFormFile file,string foldername)
        {
            
            string folderPath = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\Files" , foldername);

            string filename =$"{Guid.NewGuid()}{file.FileName}";

            string filepath=Path.Combine(folderPath,filename);

            using var filestream = new FileStream(filepath, FileMode.Create);
            file.CopyTo(filestream);

            return filename;
        }

        public static void DeleteFile(string filename,string foldername)
        {
            string filepath = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\Files" , foldername,filename);
            if(File.Exists(filepath)) 
                File.Delete(filepath);
        }
    }
}
