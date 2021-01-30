namespace RealWear.DeviceManagement.Service.Controller
{
    using AutoMapper;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using MongoDB.Driver;
    using RealWear.DeviceManagement.Service.Constant;
    using RealWear.DeviceManagement.Service.Data;
    using RealWear.DeviceManagement.Service.Entities;
    using RealWear.DeviceManagement.Service.Models;
    using RealWear.DeviceManagement.Service.Utilities;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// Defines the <see cref="AllDevicesController" />.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = AccountConstants.SuperAdmin)]
    public class AllDevicesController : ControllerBase
    {
        /// <summary>
        /// Defines the _context.
        /// </summary>
        private readonly IDbContext _context;

        /// <summary>
        /// Defines the _mapper.
        /// </summary>
        private readonly IMapper _mapper;

        /// <summary>
        /// Defines the _logger.
        /// </summary>
        private readonly ILogger<AllDevicesController> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="AllDevicesController"/> class.
        /// </summary>
        /// <param name="context">The context<see cref="IDbContext"/>.</param>
        /// <param name="mapper">The mapper<see cref="IMapper"/>.</param>
        /// <param name="logger">The logger<see cref="ILogger{AllDevicesController}"/>.</param>
        public AllDevicesController(IDbContext context, IMapper mapper, ILogger<AllDevicesController> logger)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }

        /// <summary>
        /// The Get.
        /// </summary>
        /// <param name="search">The search<see cref="string"/>.</param>
        /// <param name="paging">The paging<see cref="PagingStrategy"/>.</param>
        /// <param name="cancellationToken">The cancellationToken<see cref="CancellationToken"/>.</param>
        /// <returns>The <see cref="Task{ActionResult{DeviceReadWrapperModel}}"/>.</returns>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<DeviceReadModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<DeviceReadWrapperModel>> Get([FromQuery] string search, [FromQuery] PagingStrategy paging, CancellationToken cancellationToken)
        {
            if (!string.IsNullOrWhiteSpace(search) && search.Length < ApiConstant.SearchStringMinLength)
                return BadRequest("Search should be minimum of 5 characters");

            var devices = await GetAllDevicesAsync(search, paging, cancellationToken);
            return Ok(devices);
        }

        /// <summary>
        /// The GetAllDevicesAsync.
        /// </summary>
        /// <param name="search">The search<see cref="string"/>.</param>
        /// <param name="paging">The paging<see cref="PagingStrategy"/>.</param>
        /// <param name="cancellationToken">The cancellationToken<see cref="CancellationToken"/>.</param>
        /// <returns>The <see cref="Task{DeviceReadWrapperModel}"/>.</returns>
        private async Task<DeviceReadWrapperModel> GetAllDevicesAsync(string search, PagingStrategy paging, CancellationToken cancellationToken)
        {
            try
            {
                FilterCriteria<Device> filterCriteria = new FilterCriteria<Device>();
                DeviceReadWrapperModel result = new DeviceReadWrapperModel();

                if (string.IsNullOrEmpty(search))
                {
                    filterCriteria.Predicate = null;
                }
                else
                {
                    filterCriteria = new FilterCriteria<Device>
                    {
                        Predicate = x => x.SerialNumber.ToLowerInvariant().Contains(search.ToLowerInvariant())
                    };
                }

                var response = await FetchByAsync(filterCriteria.Predicate, paging, cancellationToken);
                var devices = _mapper.Map<IEnumerable<DeviceReadModel>>(response);

                paging.From += 1;
                var devicesNextCursorData = await FetchByAsync(filterCriteria.Predicate, paging, cancellationToken);
                if (devicesNextCursorData.Any())
                {
                    result.NextCursor = paging.From == 0 ? null : paging.From;
                }
                result.Devices = new List<DeviceReadModel>(devices);

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(nameof(GetAllDevicesAsync), ex);
                throw;
            }
        }

        /// <summary>
        /// The FetchByAsync.
        /// </summary>
        /// <param name="predicate">The predicate<see cref="Expression{Func{Device, bool}}"/>.</param>
        /// <param name="paging">The paging<see cref="PagingStrategy"/>.</param>
        /// <param name="cancellationToken">The cancellationToken<see cref="CancellationToken"/>.</param>
        /// <returns>The <see cref="Task{IEnumerable{Device}}"/>.</returns>
        private async Task<IEnumerable<Device>> FetchByAsync(Expression<Func<Device, bool>> predicate, PagingStrategy paging, CancellationToken cancellationToken)
        {
            if (predicate is null)
            {
                return await _context.Device.Find(x => true).Skip(paging.From * paging.Limit).Limit(paging.Limit).SortBy(x => x.SerialNumber).ToListAsync(cancellationToken);
            }
            return await _context.Device.Find(predicate).Skip(paging.From * paging.Limit).Limit(paging.Limit).SortBy(x => x.SerialNumber).ToListAsync(cancellationToken);
        }
    }
}
