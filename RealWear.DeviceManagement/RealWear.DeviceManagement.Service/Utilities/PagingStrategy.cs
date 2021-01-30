namespace RealWear.DeviceManagement.Service.Utilities
{
    using RealWear.DeviceManagement.Service.Constant;

    /// <summary>
    /// Defines the <see cref="PagingStrategy" />.
    /// </summary>
    public class PagingStrategy
    {
        /// <summary>
        /// Defines the _limit.
        /// </summary>
        private int? _limit;

        /// <summary>
        /// Defines the _from.
        /// </summary>
        private int? _from;

        /// <summary>
        /// Gets or sets the Limit.
        /// </summary>
        public int? Limit
        {
            get { return FetchLimit(_limit); }
            set { _limit = value; }
        }

        /// <summary>
        /// Gets or sets the From.
        /// </summary>
        public int? From
        {
            get { return _from ?? 0; }
            set { _from = value; }
        }

        /// <summary>
        /// The FetchLimit.
        /// </summary>
        /// <param name="limit">The limit<see cref="int?"/>.</param>
        /// <returns>The <see cref="int?"/>.</returns>
        public int? FetchLimit(int? limit)
        {
            int? reslimit = limit;
            if (limit is null || limit == 0)
            {
                reslimit = ApiConstant.DefaultPageSize;
            }
            else if (limit > ApiConstant.DefaultRecordMaxLimit)
            {
                reslimit = ApiConstant.DefaultRecordMaxLimit;
            }
            return reslimit;
        }
    }
}
