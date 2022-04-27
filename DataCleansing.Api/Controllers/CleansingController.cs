using System.IO;
using Aspose.Cells;
using DataCleansing.Api.Helpers;
using DataCleansing.Api.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace DataCleansing.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CleansingController : ControllerBase
    {
        [HttpPost, DisableRequestSizeLimit]
        [Route("Upload")]
        public void UploadFile()
        {
            var fileName = Request.Form.Files[0].FileName;
            var directoryPath = Path.Combine("/", @"C:\Users\aleksandargl\Desktop\FilesToBeCleaned");

            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            var filePath = Path.Combine(directoryPath, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                Request.Form.Files[0].CopyTo(stream);
                stream.Flush();
                stream.Close();
            }
        }

        [HttpPost, DisableRequestSizeLimit]
        [Route("download")]
        public IActionResult Download(string fileName)
        {
            var directoryPath = Path.Combine("/", @"C:\Users\aleksandargl\Desktop\FilesToBeCleaned");
            var filePath = Path.Combine(directoryPath, "DataSample.xlsx");

            var wb = new Workbook(filePath);
            var ms = new MemoryStream();

            var saveFormat = (SaveFormat)wb.FileFormat;
            wb.Save(ms, saveFormat);

            var content = ms.ToArray();
            return File(
                content,
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                "users.xlsx");
        }

        [HttpPost]
        [Route("GetDocumentColumnStatistic")]
        public IActionResult GetDocumentColumnStatistic(FileViewModel model)
        {
            var result = DataCleansingHelper.GetDocumentColumnStatistic(model.FileName);
            return Ok(result);
        }
    }
}
