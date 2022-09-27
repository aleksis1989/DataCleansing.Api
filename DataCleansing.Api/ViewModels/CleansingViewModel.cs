using System.Collections.Generic;

namespace DataCleansing.Api.ViewModels
{
    public class CleansingViewModel
    {
        public string FileName { get; set; }
        
        public List<OperationTypeViewModel> OperationTypes{ get; set; }
    }
}
