namespace DataCleansing.Services.Enums
{
    /// <summary>
    /// Енумератор за статус за прочиствање на име на лице
    /// </summary>
    public enum CleansingFirstNameStatusEnum
    {
        /// <summary>
        /// Не процесирани
        /// </summary>
        NonProcessed = 0,

        /// <summary>
        /// Прифатенa сугестија од алгоритамот
        /// </summary>
        AcceptSimilarity = 1,

        /// <summary>
        /// Рачна корекција
        /// </summary>
        ManualCorrection = 2,

        /// <summary>
        /// Одбиено прочистување
        /// </summary>
        Rejected = 3,
    }
}
