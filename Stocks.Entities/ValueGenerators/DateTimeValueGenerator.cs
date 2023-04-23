using System;
using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.ValueGeneration;

namespace Stocks.Entities.ValueGenerators
{
    public class DateTimeUtcGenerator : ValueGenerator
    {
        public override bool GeneratesTemporaryValues => throw new NotImplementedException();

        protected override object NextValue([NotNullAttribute] EntityEntry entry)
        {
            return DateTime.UtcNow;
        }
    }
}
