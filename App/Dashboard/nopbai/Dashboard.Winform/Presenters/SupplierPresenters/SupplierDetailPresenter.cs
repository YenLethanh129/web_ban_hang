using AutoMapper;
using Dashboard.BussinessLogic.Dtos.SupplierDtos;
using Dashboard.BussinessLogic.Services.SupplierServices;
using Dashboard.DataAccess.Data;
using Dashboard.Winform.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Dashboard.Winform.Presenters.SupplierPresenters
{
    public interface ISupplierDetailPresenter
    {
        Task<SupplierDetailViewModel?> LoadSupplierAsync(long id);
        Task<SupplierDetailViewModel?> CreateSupplierAsync(SupplierDetailViewModel model);
        Task<SupplierDetailViewModel?> UpdateSupplierAsync(SupplierDetailViewModel model);

        event EventHandler<SupplierDetailViewModel?>? OnSupplierSaved;
    }

    public class SupplierDetailPresenter : ISupplierDetailPresenter
    {
        private readonly ILogger<SupplierDetailPresenter> _logger;
        private readonly ISupplierManagementService _supplierService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IServiceProvider _serviceProvider;

        public event EventHandler<SupplierDetailViewModel?>? OnSupplierSaved;

        public SupplierDetailPresenter(
            IServiceProvider serviceProvider,
            ILogger<SupplierDetailPresenter> logger,
            IUnitOfWork unitOfWork,
            IMapper mapper,
            ISupplierManagementService supplierService)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _supplierService = supplierService;
        }

        public async Task<SupplierDetailViewModel?> LoadSupplierAsync(long id)
        {
            try
            {
                var dto = await _supplierService.GetSupplierByIdAsync(id);
                if (dto == null)
                {
                    _logger.LogWarning("Supplier with ID {SupplierId} not found", id);
                    return null;
                }

                var vm = _mapper.Map<SupplierDetailViewModel>(dto);
                return vm;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading supplier {SupplierId}", id);
                return null;
            }
        }

        public async Task<SupplierDetailViewModel?> CreateSupplierAsync(SupplierDetailViewModel model)
        {
            try
            {
                if (model == null)
                {
                    throw new ArgumentNullException(nameof(model));
                }

                if (string.IsNullOrWhiteSpace(model.Name))
                {
                    throw new ArgumentException("Supplier name is required", nameof(model));
                }

                var input = new CreateSupplierInput
                {
                    Name = model.Name.Trim(),
                    Phone = string.IsNullOrWhiteSpace(model.Phone) ? null : model.Phone.Trim(),
                    Email = string.IsNullOrWhiteSpace(model.Email) ? null : model.Email.Trim(),
                    Address = string.IsNullOrWhiteSpace(model.Address) ? null : model.Address.Trim(),
                    Note = string.IsNullOrWhiteSpace(model.Note) ? null : model.Note.Trim()
                };

                var created = await _supplierService.CreateSupplierAsync(input);
                if (created == null)
                {
                    _logger.LogWarning("Failed to create supplier: service returned null");
                    return null;
                }

                var vm = _mapper.Map<SupplierDetailViewModel>(created);

                OnSupplierSaved?.Invoke(this, vm);

                _logger.LogInformation("Successfully created supplier {SupplierId} - {SupplierName}",
                    created.Id, created.Name);

                return vm;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating supplier {SupplierName}", model?.Name);
                throw;
            }
        }

        public async Task<SupplierDetailViewModel?> UpdateSupplierAsync(SupplierDetailViewModel model)
        {
            try
            {
                if (model == null)
                {
                    throw new ArgumentNullException(nameof(model));
                }

                if (model.Id <= 0)
                {
                    throw new ArgumentException("Valid supplier ID is required for update", nameof(model));
                }

                if (string.IsNullOrWhiteSpace(model.Name))
                {
                    throw new ArgumentException("Supplier name is required", nameof(model));
                }

                var input = new UpdateSupplierInput
                {
                    Id = model.Id,
                    Name = model.Name.Trim(),
                    Phone = string.IsNullOrWhiteSpace(model.Phone) ? null : model.Phone.Trim(),
                    Email = string.IsNullOrWhiteSpace(model.Email) ? null : model.Email.Trim(),
                    Address = string.IsNullOrWhiteSpace(model.Address) ? null : model.Address.Trim(),
                    Note = string.IsNullOrWhiteSpace(model.Note) ? null : model.Note.Trim()
                };

                var updated = await _supplierService.UpdateSupplierAsync(input);
                if (updated == null)
                {
                    _logger.LogWarning("Failed to update supplier {SupplierId}: service returned null", model.Id);
                    return null;
                }

                var vm = _mapper.Map<SupplierDetailViewModel>(updated);

                OnSupplierSaved?.Invoke(this, vm);

                _logger.LogInformation("Successfully updated supplier {SupplierId} - {SupplierName}",
                    updated.Id, updated.Name);

                return vm;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating supplier {SupplierId} - {SupplierName}",
                    model?.Id, model?.Name);
                throw;
            }
        }
    }
}