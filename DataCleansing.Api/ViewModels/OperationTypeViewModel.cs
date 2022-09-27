using System.Collections.Generic;

namespace DataCleansing.Api.ViewModels
{
    public class OperationTypeViewModel
    {
        public int OperationTypeId { get; set; }

        public string OperationTypeName { get; set; }

        public List<string> DateFormats { get; set; }

        public string SelectedDateFormat { get; set; }

        public bool IsForCleansing { get; set; }
    }
}
