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
    using RealWear.DeviceManagement.Service.DeviceMessage;
    using RealWear.DeviceManagement.Service.Entities;
    using RealWear.DeviceManagement.Service.Events;
    using RealWear.DeviceManagement.Service.Models;
    using RealWear.DeviceManagement.Service.Utilities;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// Defines the <see cref="DevicesController" />.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = AccountConstants.AllUsers)]
    public class DevicesController : ControllerBase
    {
        /// <summary>
        /// Gets the Workspace.
        /// </summary>
        private string Workspace => GetCurrentUserWorkspace();

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
        private readonly ILogger<DevicesController> _logger;

        /// <summary>
        /// Defines the _serviceBusSender.
        /// </summary>
        private readonly IServiceBusSender _serviceBusSender;

        /// <summary>
        /// Initializes a new instance of the <see cref="DevicesController"/> class.
        /// </summary>
        /// <param name="context">The context<see cref="IDbContext"/>.</param>
        /// <param name="mapper">The mapper<see cref="IMapper"/>.</param>
        /// <param name="logger">The logger<see cref="ILogger{DevicesController}"/>.</param>
        /// <param name="serviceBusSender">The serviceBusSender<see cref="IServiceBusSender"/>.</param>
        public DevicesController(IDbContext context, IMapper mapper, ILogger<DevicesController> logger, IServiceBusSender serviceBusSender)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
            _serviceBusSender = serviceBusSender;
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
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<DeviceReadWrapperModel>> Get([FromQuery] string search, [FromQuery] PagingStrategy paging, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(Workspace))
                return BadRequest("Workspace is required");

            if (!string.IsNullOrWhiteSpace(search) && search.Length < ApiConstant.SearchStringMinLength)
                return BadRequest("Search should be minimum of 5 characters");

            var devices = await GetDevicesAsync(search, Workspace, paging, cancellationToken);
            return Ok(devices);
        }

        /// <summary>
        /// The Post.
        /// </summary>
        /// <param name="models">The models<see cref="DeviceCreateWrapperModel"/>.</param>
        /// <param name="cancellationToken">The cancellationToken<see cref="CancellationToken"/>.</param>
        /// <returns>The <see cref="Task{IActionResult}"/>.</returns>
        [HttpPost]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Post([FromBody] DeviceCreateWrapperModel models, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(Workspace))
                return BadRequest("Workspace is required");

            if (!ModelState.IsValid)
                return BadRequest();

            if (!models.Devices.Any())
                return BadRequest("Request should contain at least one device registration");

            if (models.Devices.Count() > ApiConstant.MaxDeviceRequestToAdd)
                return BadRequest("Request should not contain more than 100 devices");

            var duplicateSerialNumbers = models.Devices.GroupBy(x => new { x.SerialNumber }).Where(g => g.Count() > 1).ToList();
            if (duplicateSerialNumbers.Count > 0)
            {
                return BadRequest("Serial Number cannot be duplicated");
            }

            var conflictSerialNumbers = GetConflictDevicesAsync(models.Devices, cancellationToken);
            if (conflictSerialNumbers.Result != null && conflictSerialNumbers.Result.Any())
            {
                return Conflict(conflictSerialNumbers.Result);
            }

            await CreateDeviceAsync(models.Devices, Workspace, cancellationToken);
            return NoContent();
        }

        /// <summary>
        /// The Put.
        /// </summary>
        /// <param name="id">The id<see cref="string"/>.</param>
        /// <param name="model">The model<see cref="DeviceUpdateModel"/>.</param>
        /// <param name="cancellationToken">The cancellationToken<see cref="CancellationToken"/>.</param>
        /// <returns>The <see cref="Task{IActionResult}"/>.</returns>
        [HttpPut]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Put([FromQuery] string id, [FromBody] DeviceUpdateModel model, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(Workspace))
                return BadRequest("Workspace is required");

            if (!ModelState.IsValid)
                return BadRequest();

            if (!await ValidateWorkspaceAsync(id, Workspace, cancellationToken))
                return NotFound("Device not found");

            await UpdateDeviceAsync(id, model, Workspace, cancellationToken);
            return NoContent();
        }

        /// <summary>
        /// The Delete.
        /// </summary>
        /// <param name="id">The id<see cref="string"/>.</param>
        /// <param name="cancellationToken">The cancellationToken<see cref="CancellationToken"/>.</param>
        /// <returns>The <see cref="Task{IActionResult}"/>.</returns>
        [HttpDelete()]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Delete(string id, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(Workspace))
                return BadRequest("Workspace is required");

            if (!await ValidateWorkspaceAsync(id, Workspace, cancellationToken))
                return NotFound("Device not found");

            await DeleteDeviceAsync(id, Workspace, cancellationToken);
            return NoContent();
        }

        /// <summary>
        /// The GetCurrentUserWorkspace.
        /// </summary>
        /// <returns>The <see cref="string"/>.</returns>
        internal string GetCurrentUserWorkspace()
        {
            if (User.Identity.GetRole() != AccountConstants.SuperAdmin)
                return User.Identity.GetWorkspaceName();

            if (Request.Headers.TryGetValue(AccountConstants.WorkspaceHeaderKey, out var headerValue))
                return headerValue;

            return User.Identity.GetWorkspaceName();
        }

        /// <summary>
        /// The CreateDeviceAsync.
        /// </summary>
        /// <param name="models">The models<see cref="IEnumerable{DeviceCreateModel}"/>.</param>
        /// <param name="workspace">The workspace<see cref="string"/>.</param>
        /// <param name="cancellationToken">The cancellationToken<see cref="CancellationToken"/>.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        private async Task CreateDeviceAsync(IEnumerable<DeviceCreateModel> models, string workspace, CancellationToken cancellationToken)
        {
            try
            {
                var devices = _mapper.Map<IEnumerable<Device>>(models);
                devices.ToList().ForEach(x => x.Workspace = workspace);

                if (_context.SupportsTransaction)
                {
                    using IClientSessionHandle session = await _context.StartSessionAsync();
                    session.StartTransaction();
                    try
                    {
                        await _context.Device.InsertManyAsync(session, devices, null, cancellationToken);
                        session.CommitTransaction();
                    }
                    catch
                    {
                        session.AbortTransaction();
                        throw;
                    }
                    finally
                    {
                        session.Dispose();
                    }
                }
                else
                {
                    await _context.Device.InsertManyAsync(devices, null, cancellationToken);
                }
                DeviceRegistered deviceRegistered = new DeviceRegistered
                {
                    Devices = models
                };
                await _serviceBusSender.SendRegisteredMessageAsync(deviceRegistered, cancellationToken);
            }
            catch (Exception exception)
            {
                _logger.LogError(nameof(CreateDeviceAsync), exception);
                throw;
            }
        }

        /// <summary>
        /// The GetConflictDevicesAsync.
        /// </summary>
        /// <param name="models">The models<see cref="IEnumerable{DeviceCreateModel}"/>.</param>
        /// <param name="cancellation">The cancellation<see cref="CancellationToken"/>.</param>
        /// <returns>The <see cref="Task{string[]}"/>.</returns>
        private async Task<string[]> GetConflictDevicesAsync(IEnumerable<DeviceCreateModel> models, CancellationToken cancellation)
        {
            try
            {
                string[] conflictedSerialNumber = null;
                var devices = _mapper.Map<IEnumerable<Device>>(models);
                FilterDefinition<Device> serialNumberFilter = Builders<Device>.Filter.In(x => x.SerialNumber, devices.Select(x => x.SerialNumber).ToArray());
                var conflictDevice = await _context.Device.Find(serialNumberFilter).SortBy(s => s.SerialNumber).ToListAsync(cancellation);
                if (conflictDevice.Any())
                {
                    var returndevice = devices.Where(x => conflictDevice.Select(y => y.SerialNumber).Contains(x.SerialNumber));
                    conflictedSerialNumber = returndevice.Select(x => x.SerialNumber).ToArray();
                }
                return conflictedSerialNumber;
            }
            catch (Exception ex)
            {
                _logger.LogError(nameof(GetConflictDevicesAsync), ex);
                throw;
            }
        }

        /// <summary>
        /// The UpdateDeviceAsync.
        /// </summary>
        /// <param name="deviceId">The deviceId<see cref="string"/>.</param>
        /// <param name="model">The model<see cref="DeviceUpdateModel"/>.</param>
        /// <param name="workspace">The workspace<see cref="string"/>.</param>
        /// <param name="cancellationToken">The cancellationToken<see cref="CancellationToken"/>.</param>
        /// <returns>The <see cref="Task{bool}"/>.</returns>
        private async Task<bool> UpdateDeviceAsync(string deviceId, DeviceUpdateModel model, string workspace, CancellationToken cancellationToken)
        {
            try
            {
                var device = _mapper.Map<Device>(model);
                device.Id = deviceId;
                device.Workspace = workspace;
                var filter = Builders<Device>.Filter.Where(x => x.Id == device.Id && x.Workspace == device.Workspace);
                var update = Builders<Device>.Update
                    .Set(x => x.Name, device.Name)
                    .Set(x => x.Description, device.Description)
                    .Set(x => x.Email, device.Email);
                var result = await _context.Device.UpdateOneAsync(filter, update, null, cancellationToken);
                if (result.IsAcknowledged && result.MatchedCount > 0)
                {
                    DeviceUpdated deviceUpdated = new DeviceUpdated
                    {
                        Device = model
                    };
                    await _serviceBusSender.SendUpdatedMessageAsync(deviceUpdated, cancellationToken);
                }
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(nameof(UpdateDeviceAsync), ex);
                throw;
            }
        }

        /// <summary>
        /// The ValidateWorkspaceAsync.
        /// </summary>
        /// <param name="deviceId">The deviceId<see cref="string"/>.</param>
        /// <param name="workspace">The workspace<see cref="string"/>.</param>
        /// <param name="cancellationToken">The cancellationToken<see cref="CancellationToken"/>.</param>
        /// <returns>The <see cref="Task{bool}"/>.</returns>
        private async Task<bool> ValidateWorkspaceAsync(string deviceId, string workspace, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _context.Device.Find(p => p.Id == deviceId && p.Workspace == workspace).FirstOrDefaultAsync(cancellationToken);
                return result != null;
            }
            catch (Exception ex)
            {
                _logger.LogError(nameof(ValidateWorkspaceAsync), ex);
                throw;
            }
        }

        /// <summary>
        /// The DeleteDeviceAsync.
        /// </summary>
        /// <param name="deviceId">The deviceId<see cref="string"/>.</param>
        /// <param name="workspace">The workspace<see cref="string"/>.</param>
        /// <param name="cancellationToken">The cancellationToken<see cref="CancellationToken"/>.</param>
        /// <returns>The <see cref="Task{bool}"/>.</returns>
        private async Task<bool> DeleteDeviceAsync(string deviceId, string workspace, CancellationToken cancellationToken)
        {
            try
            {
                var deviceDetail = await _context.Device.Find(p => p.Id == deviceId).FirstOrDefaultAsync(cancellationToken);
                FilterDefinition<Device> filter = Builders<Device>.Filter.Where(x => x.Id == deviceId && x.Workspace == workspace);
                DeleteResult deleteResult = await _context
                .Device
                .DeleteOneAsync(filter, null, cancellationToken);
                if (deleteResult.IsAcknowledged && deleteResult.DeletedCount > 0)
                {
                    DeviceDeleted deviceDeleted = new DeviceDeleted
                    {
                        Name = deviceDetail.Name,
                        SerialNumber = deviceDetail.SerialNumber
                    };
                    await _serviceBusSender.SendDeletedMessageAsync(deviceDeleted, cancellationToken);
                }
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(nameof(DeleteDeviceAsync), ex);
                throw;
            }
        }

        /// <summary>
        /// The GetDevicesAsync.
        /// </summary>
        /// <param name="search">The search<see cref="string"/>.</param>
        /// <param name="workspace">The workspace<see cref="string"/>.</param>
        /// <param name="paging">The paging<see cref="PagingStrategy"/>.</param>
        /// <param name="cancellationToken">The cancellationToken<see cref="CancellationToken"/>.</param>
        /// <returns>The <see cref="Task{DeviceReadWrapperModel}"/>.</returns>
        private async Task<DeviceReadWrapperModel> GetDevicesAsync(string search, string workspace, PagingStrategy paging, CancellationToken cancellationToken)
        {
            try
            {
                FilterCriteria<Device> filterCriteria;
                DeviceReadWrapperModel result = new DeviceReadWrapperModel();

                if (string.IsNullOrEmpty(search))
                {
                    filterCriteria = new FilterCriteria<Device>
                    {
                        Predicate = x => x.Workspace == workspace
                    };
                }
                else
                {
                    filterCriteria = new FilterCriteria<Device>
                    {
                        Predicate = x => x.Workspace == workspace && x.SerialNumber.ToLowerInvariant().Contains(search.ToLowerInvariant())
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
                _logger.LogError(nameof(GetDevicesAsync), ex);
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
