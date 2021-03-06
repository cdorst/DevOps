﻿using DevOps.Abstractions.Core.Services;
using DevOps.Abstractions.UniqueStrings;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace DevOps.Abstractions.SourceCode.TypeDeclarations.EntityFramework.Services
{
    public class AttributeListCollectionUpsertService<TDbContext> : UpsertService<TDbContext, AttributeListCollection>
        where TDbContext : SourceCodeTypeDeclarationsDbContext
    {
        private readonly IUpsertService<TDbContext, AsciiStringReference> _strings;

        public AttributeListCollectionUpsertService(ICacheService<AttributeListCollection> cache, TDbContext database, ILogger<UpsertService<TDbContext, AttributeListCollection>> logger, IUpsertService<TDbContext, AsciiStringReference> strings)
            : base(cache, database, logger, database.AttributeListCollections)
        {
            CacheKey = record => $"{nameof(SourceCode)}.{nameof(Expression)}={record.ListIdentifierId}";
            _strings = strings ?? throw new ArgumentNullException(nameof(strings));
        }

        protected override async Task<AttributeListCollection> AssignUpsertedReferences(AttributeListCollection record)
        {
            record.ListIdentifier = await _strings.UpsertAsync(record.ListIdentifier);
            record.ListIdentifierId = record.ListIdentifier?.AsciiStringReferenceId ?? record.ListIdentifierId;
            return record;
        }

        protected override IEnumerable<object> EnumerateReferences(AttributeListCollection record)
        {
            yield return record.AttributeListCollectionAssociations;
            yield return record.ListIdentifier;
        }

        protected override Expression<Func<AttributeListCollection, bool>> FindExisting(AttributeListCollection record)
            => existing => existing.ListIdentifierId == record.ListIdentifierId;
    }
}
