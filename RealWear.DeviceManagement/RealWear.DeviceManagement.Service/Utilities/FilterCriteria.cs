namespace RealWear.DeviceManagement.Service.Utilities
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;

    /// <summary>
    /// Defines the <see cref="FilterCriteria{T}" />.
    /// </summary>
    /// <typeparam name="T">.</typeparam>
    public class FilterCriteria<T>
        where T : class
    {
        /// <summary>
        /// Gets or sets the Predicate.
        /// </summary>
        public Expression<Func<T, bool>> Predicate { get; set; }

        /// <summary>
        /// Gets the Includes.
        /// </summary>
        public IList<Expression<Func<T, object>>> Includes { get; } = new List<Expression<Func<T, object>>>();
    }
}
