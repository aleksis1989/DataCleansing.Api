namespace DataCleansing.Api.ViewModels
{
    public class ColumnViewModel
    {
        public string ColumnName { get; set; }

        public decimal StringPercentage { get; set; }

        public decimal IntegerPercentage { get; set; }

        public decimal DecimalPercentage { get; set; }

        public decimal DatePercentage  { get; set; }
    }
}
