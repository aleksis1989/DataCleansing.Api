namespace DataCleansing.Services.ViewModels
{
    public class CleansingFirstNameReportModel
    {
        public int Total { get; set; }

        public int TotalForCleansing { get; set; }

        public int TotalWaitForCleansing { get; set; }

        public int TotalAccept { get; set; }

        public int TotalReject { get; set; }
    }
}
