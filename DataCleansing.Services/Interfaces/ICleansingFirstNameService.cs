using System.Collections.Generic;
using DataCleansing.Services.ViewModels;
using DataCleansing.Services.ViewModels.Search;

namespace DataCleansing.Services.Interfaces
{
    public interface ICleansingFirstNameService
    {
        SearchResult<CleansingFirstNameGridModel> FilterCleansingFirstNamesInGrid(CleansingFirstNameSearchModel searchModel);

        CleansingFirstNameViewModel GetFirstNameForCleansingById(int id);

        List<KeyValue<int, string>> GetAllFirstNames();

        CleansingFirstNameReportModel GetCleansingFirstNameReport();
    }
}
