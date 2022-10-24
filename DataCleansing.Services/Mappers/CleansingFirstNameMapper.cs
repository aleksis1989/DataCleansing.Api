using System.Collections.Generic;
using System.Linq;
using DataCleansing.Core.Domain;
using DataCleansing.Services.Enums;
using DataCleansing.Services.ViewModels;

namespace DataCleansing.Services.Mappers
{
    public static class CleansingFirstNameMapper
    {
        public static CleansingFirstNameGridModel ToGridModel(this CleansingFirstName domain)
        {
            var processedStatuses = new List<int>
            {
                (int) CleansingFirstNameStatusEnum.AcceptSimilarity,
                (int) CleansingFirstNameStatusEnum.AcceptPermutation,
                (int) CleansingFirstNameStatusEnum.AcceptSuggestion,
                (int) CleansingFirstNameStatusEnum.ManualCorrection
            };

            var gridModel = new CleansingFirstNameGridModel
            {
                Id = domain.Id,
                FirstName = domain.FirstName,
                SimilarityFirstName = domain.SimilarityFirstName,
                SimilarityTypeId = domain.SimilarityType?.Id,
                SimilarityTypeName = domain.SimilarityType?.SimilarityTypeName,
                CleansingFirstNameStatusName = domain.CleansingFirstNameStatus != null
                    ? domain.CleansingFirstNameStatus.CleansingFirstNameStatusName
                    : "Непроцесиран",
                CanProcess = domain.Levenshtein.HasValue &&
                             (domain.CleansingFirstNameStatus == null || domain.CleansingFirstNameStatus?.Id == (int)CleansingFirstNameStatusEnum.Rejected),
                CanReject = domain.Levenshtein.HasValue && domain.CleansingFirstNameStatus != null && processedStatuses.Contains(domain.CleansingFirstNameStatus.Id)
            };

            return gridModel;
        }

        public static List<CleansingFirstNameGridModel> ToGridModelList(this List<CleansingFirstName> domainList)
        {
            return domainList.Select(x => x.ToGridModel()).ToList();
        }

        public static CleansingFirstNameViewModel ToViewModel(this CleansingFirstName domain)
        {
            var viewModel = new CleansingFirstNameViewModel
            {
                Id = domain.Id,
                PersonId = domain.PersonId,
                FirstName = domain.FirstName,
                Levenshtein = (int?)(domain.Levenshtein.HasValue ? domain.Levenshtein * 100 : 0),
                LevenshteinFirstName = domain.LevenshteinFirstName,
                Jaccard = (int?)(domain.Jaccard.HasValue ? domain.Jaccard * 100 : 0),
                JaccardFirstName = domain.JaccardFirstName,
                JaroWinkler = (int?)(domain.JaroWinkler.HasValue ? domain.JaroWinkler * 100 : 0),
                JaroWinklerFirstName = domain.JaroWinklerFirstName,
                LongestCommonSubsequence = (int?)(domain.LongestCommonSubsequence.HasValue ? domain.LongestCommonSubsequence * 100 : 0),
                LongestCommonSubsequenceFirstName = domain.LongestCommonSubsequenceFirstName,
                SimilarityTypeId = domain.SimilarityType?.Id,
                SimilarityTypeName = domain.SimilarityType?.SimilarityTypeName,
                SimilarityFirstNameId = domain.SimilarityFirstNameId,
                SimilarityFirstName = domain.SimilarityFirstName,
                CleansingFirstNameStatusId = domain.CleansingFirstNameStatus?.Id,
                ManualFirstName = domain.ManualFirstName,
                ManualFirstNameId = domain.ManualFirstNameId,
            };

            if (!string.IsNullOrWhiteSpace(domain.SimilarityFirstName))
            {
                viewModel.SimilarityFirstNameCleansingResult = domain.SimilarityFirstName;
            }

            return viewModel;
        }
    }
}
