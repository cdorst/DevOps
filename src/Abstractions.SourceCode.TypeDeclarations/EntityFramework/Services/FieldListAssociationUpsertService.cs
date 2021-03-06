﻿using DevOps.Abstractions.Core.Services;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace DevOps.Abstractions.SourceCode.TypeDeclarations.EntityFramework.Services
{
    public class FieldListAssociationUpsertService<TDbContext> : UpsertService<TDbContext, FieldListAssociation>
        where TDbContext : SourceCodeTypeDeclarationsDbContext
    {
        public FieldListAssociationUpsertService(ICacheService<FieldListAssociation> cache, TDbContext database, ILogger<UpsertService<TDbContext, FieldListAssociation>> logger)
            : base(cache, database, logger, database.FieldListAssociations)
        {
            CacheKey = record => $"{nameof(SourceCode)}.{nameof(FieldListAssociation)}={record.FieldId}:{record.FieldListId}";
        }

        protected override IEnumerable<object> EnumerateReferences(FieldListAssociation record)
        {
            yield return record.Field;
            yield return record.FieldList;
        }

        protected override Expression<Func<FieldListAssociation, bool>> FindExisting(FieldListAssociation record)
            => existing
                => existing.FieldId == record.FieldId
                && existing.FieldListId == record.FieldListId;
    }
}
