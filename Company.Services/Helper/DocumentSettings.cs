using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Company.Services.Helper
{
    public class DocumentSettings
    {
        public static string UploadFile(IFormFile file ,string folderName )
        {
            //1.get folder path  
         
            var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Files", folderName);

            // step 2 get file name
            var filename = $"{Guid.NewGuid()}-{file.FileName}";

            // 3 combine folderPath + Filepath
             var filePath = Path.Combine(folderPath, filename);

            // 4. save file
            using var fileStream = new FileStream(filePath, FileMode.Create);
            file.CopyTo(fileStream);
            return filename;
        }
    }
}
