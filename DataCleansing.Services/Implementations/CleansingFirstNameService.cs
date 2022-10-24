using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using DataCleansing.Base.Implementations;
using DataCleansing.Base.Interfaces;
using DataCleansing.Core.Domain;
using DataCleansing.Core.Repositories;
using DataCleansing.Services.Enums;
using DataCleansing.Services.Interfaces;
using DataCleansing.Services.Mappers;
using DataCleansing.Services.ViewModels;
using DataCleansing.Services.ViewModels.Search;

namespace DataCleansing.Services.Implementations
{
    public class CleansingFirstNameService : ICleansingFirstNameService
    {
        private readonly ICleansingFirstNameRepository _cleansingFirstNameRepository;
        private readonly IRepository<KnowlegeFirstName> _knowledgeFirstNameRepository;

        public CleansingFirstNameService(
            ICleansingFirstNameRepository cleansingFirstNameRepository, 
            IRepository<KnowlegeFirstName> knowledgeFirstNameRepository)
        {
            _cleansingFirstNameRepository = cleansingFirstNameRepository;
            _knowledgeFirstNameRepository = knowledgeFirstNameRepository;
        }

        public SearchResult<CleansingFirstNameGridModel> FilterCleansingFirstNamesInGrid(CleansingFirstNameSearchModel searchModel)
        {
            if (searchModel == null)
            {
                throw new ArgumentNullException("searchModel");
            }

            if (searchModel.Size > 50)
            {
                searchModel.Size = 50;
            }

            if (searchModel.Size < 0)
            {
                searchModel.Size = 5;
            }

            using (new UnitOfWorkScope())
            {
                var query = _cleansingFirstNameRepository.Query();

                query = FilterCleansingFirstNames(query, searchModel);

                searchModel.TotalElements = query.Count();
                searchModel.TotalPages = searchModel.TotalElements / searchModel.Size;

                query = SortCleansingFirstNames(query, searchModel.SortColumn, searchModel.SortOrder);

                var start = (searchModel.PageNumber - 1) * searchModel.Size;
                var finalQuery = query.Skip(start).Take(searchModel.Size);

                var result = new SearchResult<CleansingFirstNameGridModel>
                {
                    Data = finalQuery.ToList().ToGridModelList(),
                    Page = searchModel
                };

                return result;
            }
        }

        public CleansingFirstNameViewModel GetFirstNameForCleansingById(int id)
        {
            using (new UnitOfWorkScope())
            {
                var cleansingFirstName = _cleansingFirstNameRepository.Get(id);
                if (cleansingFirstName == null)
                {
                    throw new Exception("Записот за прочистување на улица не постои во системот.");
                }

                var result = cleansingFirstName.ToViewModel();
                return result;
            }
        }

        public List<KeyValue<int, string>> GetAllFirstNames()
        {
            using (new UnitOfWorkScope())
            {
                var names = _knowledgeFirstNameRepository.GetAll().Select(x => new KeyValue<int, string>
                {
                    Key = x.Id,
                    Value = x.FirstName
                }).ToList();

                return names;
            }
        }

        public CleansingFirstNameReportModel GetCleansingFirstNameReport()
        {
            using (new UnitOfWorkScope())
            {
                var report = _cleansingFirstNameRepository.GetCleansingFirstNameReport<CleansingFirstNameReportModel>();
                return report;
            }
        }

        private IQueryable<CleansingFirstName> FilterCleansingFirstNames(IQueryable<CleansingFirstName> query, CleansingFirstNameSearchModel searchModel)
        {
            if (!string.IsNullOrEmpty(searchModel.FirstName))
            {
                query = query.Where(x => x.FirstName.Contains(searchModel.FirstName));
            }

            if (searchModel.SimilarityTypeId > 0)
            {
                query = query.Where(x => x.SimilarityType != null && x.SimilarityType.Id == searchModel.SimilarityTypeId);
            }

            var processedRowStatus = new List<int>
            {
                (int) CleansingFirstNameStatusEnum.AcceptSuggestion,
                (int) CleansingFirstNameStatusEnum.AcceptSimilarity,
                (int) CleansingFirstNameStatusEnum.AcceptPermutation,
                (int) CleansingFirstNameStatusEnum.ManualCorrection
            };

            switch (searchModel.CleansingFirstNameStatusId)
            {
                case (int)CleansingFirstNameStatusEnum.AcceptSuggestion:
                case (int)CleansingFirstNameStatusEnum.AcceptSimilarity:
                case (int)CleansingFirstNameStatusEnum.AcceptPermutation:
                case (int)CleansingFirstNameStatusEnum.ManualCorrection:
                case (int)CleansingFirstNameStatusEnum.Rejected:
                    query = query.Where(x => x.CleansingFirstNameStatus != null &&
                                             x.CleansingFirstNameStatus.Id == searchModel.CleansingFirstNameStatusId);
                    break;
                case (int)CleansingFirstNameStatusEnum.NonProcessed:
                    query = query.Where(x => x.CleansingFirstNameStatus == null);
                    break;
                case (int)CleansingFirstNameStatusEnum.Accepted:
                    query = query.Where(x => x.CleansingFirstNameStatus != null &&
                                             processedRowStatus.Contains(x.CleansingFirstNameStatus.Id));
                    break;
            }

            switch (searchModel.CleansingFirstNameRowFilter)
            {
                case (int)CleansingFirstNameRowFilterEnum.Cleansable:
                    query = query.Where(x => x.Levenshtein != null);
                    break;

                case (int)CleansingFirstNameRowFilterEnum.NonCleansable:
                    query = query.Where(x => x.Levenshtein == null);
                    break;

                case (int)CleansingFirstNameRowFilterEnum.WithSuggestedFirstName:
                    query = query.Where(x => x.SimilarityFirstNameId != null);
                    break;
            }

            return query;
        }

        private IOrderedQueryable<CleansingFirstName> SortCleansingFirstNames(IQueryable<CleansingFirstName> query, string sortColumn, string sortOrder)
        {
            Expression<Func<CleansingFirstName, string>> sortExpression1;
            Expression<Func<CleansingFirstName, int?>> sortExpression2;

            if (sortColumn.Equals("StreetName", StringComparison.InvariantCultureIgnoreCase))
            {
                sortExpression1 = t => t.FirstName;
                return sortOrder.Equals("desc", StringComparison.InvariantCultureIgnoreCase)
                    ? query.OrderByDescending(sortExpression1)
                    : query.OrderBy(sortExpression1);
            }

            if (sortColumn.Equals("SimilarityType", StringComparison.InvariantCultureIgnoreCase))
            {
                sortExpression2 = t => t.SimilarityType.Id;
                return sortOrder.Equals("desc", StringComparison.InvariantCultureIgnoreCase)
                    ? query.OrderByDescending(sortExpression2)
                    : query.OrderBy(sortExpression2);
            }

            if (sortColumn.Equals("SimilarityFirstName", StringComparison.InvariantCultureIgnoreCase))
            {
                sortExpression1 = t => t.SimilarityFirstName;
                return sortOrder.Equals("desc", StringComparison.InvariantCultureIgnoreCase)
                    ? query.OrderByDescending(sortExpression1)
                    : query.OrderBy(sortExpression1);
            }

            sortExpression1 = t => t.FirstName;
            return sortOrder.Equals("desc", StringComparison.InvariantCultureIgnoreCase)
                ? query.OrderByDescending(sortExpression1)
                : query.OrderBy(sortExpression1);
        }
    }
}
