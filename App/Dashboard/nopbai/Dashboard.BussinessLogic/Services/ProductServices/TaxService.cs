using AutoMapper;
using Dashboard.BussinessLogic.Dtos;
using Dashboard.BussinessLogic.Dtos.ProductDtos;
using Dashboard.DataAccess.Data;
using Dashboard.DataAccess.Models.Entities.FinacialAndReports;
using Dashboard.DataAccess.Repositories;
using Microsoft.Extensions.Logging;

namespace Dashboard.BussinessLogic.Services.ProductServices
{
    public interface ITaxService
    {
        Task<PagedList<TaxDto>> GetTaxesAsync(DefaultInput input);
        Task<List<TaxDto>> GetAllTaxesAsync();
        Task<TaxDto?> GetTaxByIdAsync(long id);
        Task<TaxDto> CreateTaxAsync(CreateTaxInput input);
        Task<TaxDto> UpdateTaxAsync(UpdateTaxInput input);
        Task<bool> DeleteTaxAsync(long id);
    }

    public class TaxService : ITaxService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ITaxRepository _taxRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<TaxService> _logger;

        public TaxService(
            IUnitOfWork unitOfWork,
            ITaxRepository taxRepository,
            IMapper mapper,
            ILogger<TaxService> logger)
        {
            _unitOfWork = unitOfWork;
            _taxRepository = taxRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<PagedList<TaxDto>> GetTaxesAsync(DefaultInput input)
        {
            var taxes = await _taxRepository.GetAllAsync();
            var totalRecords = taxes.Count();

            var pagedTaxes = taxes
                .Skip((input.PageNumber - 1) * input.PageSize)
                .Take(input.PageSize)
                .ToList();

            var taxDtos = _mapper.Map<List<TaxDto>>(pagedTaxes);

            return new PagedList<TaxDto>
            {
                PageNumber = input.PageNumber,
                PageSize = input.PageSize,
                TotalRecords = totalRecords,
                Items = taxDtos
            };
        }

        public async Task<List<TaxDto>> GetAllTaxesAsync()
        {
            var taxes = await _taxRepository.GetAllAsync();
            return _mapper.Map<List<TaxDto>>(taxes);
        }

        public async Task<TaxDto?> GetTaxByIdAsync(long id)
        {
            var tax = await _taxRepository.GetAsync(id);
            return tax != null ? _mapper.Map<TaxDto>(tax) : null;
        }

        public async Task<TaxDto> CreateTaxAsync(CreateTaxInput input)
        {
            var tax = _mapper.Map<Taxes>(input);
            tax.CreatedAt = DateTime.UtcNow;

            await _taxRepository.AddAsync(tax);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<TaxDto>(tax);
        }

        public async Task<TaxDto> UpdateTaxAsync(UpdateTaxInput input)
        {
            var tax = await _taxRepository.GetAsync(input.Id);
            if (tax == null)
                throw new ArgumentException($"Tax with id {input.Id} not found");

            _mapper.Map(input, tax);
            tax.LastModified = DateTime.UtcNow;

            _taxRepository.Update(tax);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<TaxDto>(tax);
        }

        public async Task<bool> DeleteTaxAsync(long id)
        {
            var tax = await _taxRepository.GetAsync(id);
            if (tax == null)
                return false;

            _taxRepository.Remove(tax);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }
    }
}