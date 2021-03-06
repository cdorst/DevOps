﻿using DevOps.Abstractions.Core.Services;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace DevOps.Abstractions.SourceCode.TypeDeclarations.EntityFramework.Services
{
    public class ArgumentListAssociationUpsertService<TDbContext> : UpsertService<TDbContext, ArgumentListAssociation>
        where TDbContext : SourceCodeTypeDeclarationsDbContext
    {
        public ArgumentListAssociationUpsertService(ICacheService<ArgumentListAssociation> cache, TDbContext database, ILogger<UpsertService<TDbContext, ArgumentListAssociation>> logger)
            : base(cache, database, logger, database.ArgumentListAssociations)
        {
            CacheKey = record => $"{nameof(SourceCode)}.{nameof(ArgumentListAssociation)}={record.ArgumentId}:{record.ArgumentListId}";
        }

        protected override IEnumerable<object> EnumerateReferences(ArgumentListAssociation record)
        {
            yield return record.Argument;
            yield return record.ArgumentList;
        }

        protected override Expression<Func<ArgumentListAssociation, bool>> FindExisting(ArgumentListAssociation record)
            => existing
                => existing.ArgumentId == record.ArgumentId
                && existing.ArgumentListId == record.ArgumentListId;
    }
}
