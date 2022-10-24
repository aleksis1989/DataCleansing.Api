using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataCleansing.Services.Enums
{
    /// <summary>
    /// Енумератор за статус за прочиствање на име на лице
    /// </summary>
    public enum CleansingFirstNameStatusEnum
    {
        // Не процесирани
        NonProcessed = 0,

        // Прифатенa сугестија од алгоритамот
        AcceptSuggestion = 1,

        // Прифатено Similarity
        AcceptSimilarity = 2,

        // Прифатено Permutation
        AcceptPermutation = 3,

        // Рачна корекција
        ManualCorrection = 4,

        /// <summary>
        /// Одбиено прочистување
        /// </summary>
        Rejected = 5,

        /// <summary>
        /// Прифатени (Изведен статус)
        /// </summary>
        Accepted = 6
    }
}
