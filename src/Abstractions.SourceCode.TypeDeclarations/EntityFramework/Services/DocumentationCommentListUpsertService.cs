﻿using DevOps.Abstractions.Core.Services;
using DevOps.Abstractions.UniqueStrings;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace DevOps.Abstractions.SourceCode.TypeDeclarations.EntityFramework.Services
{
    public class DocumentationCommentListUpsertService<TDbContext> : UpsertService<TDbContext, DocumentationCommentList>
        where TDbContext : SourceCodeTypeDeclarationsDbContext
    {
        private readonly IUpsertService<TDbContext, AsciiStringReference> _strings;

        public DocumentationCommentListUpsertService(ICacheService<DocumentationCommentList> cache, TDbContext database, ILogger<UpsertService<TDbContext, DocumentationCommentList>> logger, IUpsertService<TDbContext, AsciiStringReference> strings)
            : base(cache, database, logger, database.DocumentationCommentLists)
        {
            CacheKey = record => $"{nameof(SourceCode)}.{nameof(Expression)}={record.ListIdentifierId}";
            _strings = strings ?? throw new ArgumentNullException(nameof(strings));
        }

        protected override async Task<DocumentationCommentList> AssignUpsertedReferences(DocumentationCommentList record)
        {
            record.ListIdentifier = await _strings.UpsertAsync(record.ListIdentifier);
            record.ListIdentifierId = record.ListIdentifier?.AsciiStringReferenceId ?? record.ListIdentifierId;
            return record;
        }

        protected override IEnumerable<object> EnumerateReferences(DocumentationCommentList record)
        {
            yield return record.DocumentationComments;
            yield return record.ListIdentifier;
        }

        protected override Expression<Func<DocumentationCommentList, bool>> FindExisting(DocumentationCommentList record)
            => existing => existing.ListIdentifierId == record.ListIdentifierId;
    }
}
