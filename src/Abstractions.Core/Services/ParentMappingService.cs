﻿using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

namespace DevOps.Abstractions.Core.Services
{
    public abstract class ParentMappingService<TInput, TOutput> : IParentMappingService<TInput, TOutput>
        where TInput : class
        where TOutput : class
    {
        private readonly ILogger<ParentMappingService<TInput, TOutput>> _logger;

        public ParentMappingService(ILogger<ParentMappingService<TInput, TOutput>> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        protected virtual Func<List<TInput>, TOutput> Result { get; set; } = input => default(TOutput);

        public TOutput Map(List<TInput> input)
        {
            _logger.LogInformation("Returning mapped object");
            return Result(input);
        }
    }
}
