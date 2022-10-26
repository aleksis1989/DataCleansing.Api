namespace DataCleansing.Services.ViewModels
{
    public class MergeFirstNameViewModel
    {
        /// <summary>
        /// Редот кој се прочистува
        /// </summary>
        public int CleansingFirstNameId { get; set; }

        /// <summary>
        /// Името кое е избрано за прочистување
        /// </summary>
        public KeyValue<int,string> CleansingFirstName { get; set; }

        /// <summary>
        /// Статус на името (Алгоиратам по сличност, Одбиено прочистување, Рачно избрано име) 
        /// </summary>
        public int CleansingFirstNameStatusId { get; set; }
    }
}
