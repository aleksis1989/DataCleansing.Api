using System.IO;
using Aspose.Cells;
using DataCleansing.Api.Helpers;
using DataCleansing.Api.ViewModels;
using DataCleansing.Services.Interfaces;
using DataCleansing.Services.ViewModels;
using DataCleansing.Services.ViewModels.Search;
using Microsoft.AspNetCore.Mvc;

namespace DataCleansing.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CleansingController : ControllerBase
    {
        private readonly ICleansingFirstNameService _cleansingFirstNameService;

        public CleansingController(ICleansingFirstNameService cleansingFirstNameService)
        {
            _cleansingFirstNameService = cleansingFirstNameService;
        }

        [HttpPost, DisableRequestSizeLimit]
        [Route("Upload")]
        public void UploadFile()
        {
            var fileName = Request.Form.Files[0].FileName;
            var basePath = Startup.StaticConfig["BasePath"];
            var directoryPath = Path.Combine("/", basePath);

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
        public IActionResult Download(CleanedFileNameViewModel viewModel)
        {
            //var basePath = Startup.StaticConfig["BasePath"];
            //var directoryPath = Path.Combine("/", basePath);

            var basePath = Startup.StaticConfig["BasePath"];

            var fileName = viewModel.FileName + ".xlsx";
            var filePath = Path.Combine(basePath, fileName);

            var wb = new Workbook(filePath);
            var ms = new MemoryStream();

            var saveFormat = (SaveFormat)wb.FileFormat;
            wb.Save(ms, saveFormat);

            var content = ms.ToArray();
            return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", viewModel.FileName + ".xls");
        }

        [HttpPost]
        [Route("GetDocumentColumnStatistic")]
        public IActionResult GetDocumentColumnStatistic(FileViewModel model)
        {
            var result = DataCleansingHelper.GetDocumentColumnStatistic(model.FileName);
            return Ok(result);
        }

        [HttpPost]
        [Route("GetCleansingMethods")]
        public IActionResult GetCleansingMethods()
        {
            var result = DataCleansingHelper.GetCleansingMethods();
            return Ok(result);
        }

        [HttpPost]
        [Route("CleanFile")]
        public IActionResult CleanFile(CleansingViewModel cleansingViewModel)
        {
            var result = DataCleansingHelper.CleanFile(cleansingViewModel);
            return Ok(result);
        }

        [HttpPost]
        [Route("FilterCleansingFirstNamesInGrid")]
        public IActionResult FilterCleansingFirstNamesInGrid(CleansingFirstNameSearchModel searchModel)
        {
            var result = _cleansingFirstNameService.FilterCleansingFirstNamesInGrid(searchModel);
            return Ok(result);
        }

        [HttpPost]
        [Route("GetFirstNameForCleansingById")]
        public IActionResult GetFirstNameForCleansingById(IdentificatorViewModel viewModel)
        {
            var result = _cleansingFirstNameService.GetFirstNameForCleansingById(viewModel.Id);
            return Ok(result);
        }

        [HttpPost]
        [Route("GetAllFirstNames")]
        public IActionResult GetAllFirstNames()
        {
            var result = _cleansingFirstNameService.GetAllFirstNames();
            return Ok(result);
        }

        [HttpPost]
        [Route("GetCleansingFirstNameReport")]
        public IActionResult GetCleansingFirstNameReport()
        {
            var result = _cleansingFirstNameService.GetCleansingFirstNameReport();
            return Ok(result);
        }

        [HttpPost]
        [Route("MergeFirstName")]
        public IActionResult MergeFirstName(MergeFirstNameViewModel viewModel)
        {
            _cleansingFirstNameService.MergeFirstName(viewModel);
            return Ok(true);
        }

        [HttpPost]
        [Route("UndoMerge")]
        public IActionResult UndoMerge(IdentificatorViewModel viewModel)
        {
            _cleansingFirstNameService.UndoMerge(viewModel);
            return Ok(true);
        }

        [HttpPost]
        [Route("RejectMergeFirstName")]
        public IActionResult RejectMergeFirstName(IdentificatorViewModel viewModel)
        {
            _cleansingFirstNameService.RejectMergeFirstName(viewModel.Id);
            return Ok(true);
        }
    }
}
