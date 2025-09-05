using AutoMapper;
using Dashboard.BussinessLogic.Dtos;
using Dashboard.BussinessLogic.Dtos.IngredientDtos;
using Dashboard.BussinessLogic.Shared;
using Dashboard.BussinessLogic.Specifications;
using Dashboard.DataAccess.Data;
using Dashboard.DataAccess.Models.Entities;

namespace Dashboard.BussinessLogic.Services;

public interface IIngredientManagementService
{
    Task<int> GetCountAsync();
    Task<IngredientDto?> GetIngredientByIdAsync(long id);
    Task<PagedList<IngredientDto>> GetIngredientsAsync(GetIngredientsInput input);
    Task<IngredientDto> CreateIngredientAsync(CreateIngredientInput input);
    Task<IngredientDto> UpdateIngredientAsync(UpdateIngredientInput input);
    Task<bool> DeleteIngredientAsync(long id);
    Task<bool> ValidateIngredientAsync(long ingredientId);
}

public class IngredientManagementService : BaseTransactionalService, IIngredientManagementService
{
    private readonly IMapper _mapper;

    public IngredientManagementService(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork)
    {
        _mapper = mapper;
    }

    public async Task<int> GetCountAsync()
    {
        return await _unitOfWork.Repository<Ingredient>().GetCountAsync();
    }

    public async Task<IngredientDto?> GetIngredientByIdAsync(long id)
    {
        var specification = IngredientSpecifications.ById(id);
        var ingredient = await _unitOfWork.Repository<Ingredient>().GetWithSpecAsync(specification);
        return ingredient != null ? _mapper.Map<IngredientDto>(ingredient) : null;
    }

    public async Task<PagedList<IngredientDto>> GetIngredientsAsync(GetIngredientsInput input)
    {
        var specification = IngredientSpecifications.BySearchCriteria(input);
        var allIngredients = await _unitOfWork.Repository<Ingredient>().GetAllWithSpecAsync(specification, true);
        
        var pagedIngredients = allIngredients
            .Skip((input.PageNumber - 1) * input.PageSize)
            .Take(input.PageSize)
            .ToList();

        var ingredientDtos = _mapper.Map<List<IngredientDto>>(pagedIngredients);

        return new PagedList<IngredientDto>
        {
            Items = ingredientDtos,
            TotalRecords = allIngredients.Count(),
            PageNumber = input.PageNumber,
            PageSize = input.PageSize
        };
    }

    public async Task<IngredientDto> CreateIngredientAsync(CreateIngredientInput input)
    {
        if (string.IsNullOrWhiteSpace(input.Name))
            throw new ArgumentException("Ingredient name is required");

        if (input.CostPerUnit < 0)
            throw new ArgumentException("Cost per unit must be greater than or equal to 0");

        // Check if name already exists
        var nameSpec = IngredientSpecifications.ByName(input.Name);
        var existingIngredient = await _unitOfWork.Repository<Ingredient>().GetWithSpecAsync(nameSpec);
        
        if (existingIngredient != null)
            throw new InvalidOperationException($"Ingredient with name '{input.Name}' already exists");

        var ingredient = _mapper.Map<Ingredient>(input);
        await _unitOfWork.Repository<Ingredient>().AddAsync(ingredient);
        await _unitOfWork.SaveChangesAsync();

        return await GetIngredientByIdAsync(ingredient.Id) ?? throw new InvalidOperationException("Failed to retrieve created ingredient");
    }

    public async Task<IngredientDto> UpdateIngredientAsync(UpdateIngredientInput input)
    {
        if (string.IsNullOrWhiteSpace(input.Name))
            throw new ArgumentException("Ingredient name is required");

        if (input.CostPerUnit < 0)
            throw new ArgumentException("Cost per unit must be greater than or equal to 0");

        var existingIngredient = await _unitOfWork.Repository<Ingredient>().GetAsync(input.Id);
        if (existingIngredient == null)
            throw new InvalidOperationException($"Ingredient with ID {input.Id} not found");

        // Check if name already exists (excluding current ingredient)
        var nameSpec = IngredientSpecifications.ByName(input.Name, input.Id);
        var duplicateIngredient = await _unitOfWork.Repository<Ingredient>().GetWithSpecAsync(nameSpec);
        
        if (duplicateIngredient != null)
            throw new InvalidOperationException($"Another ingredient with name '{input.Name}' already exists");

        _mapper.Map(input, existingIngredient);
        _unitOfWork.Repository<Ingredient>().Remove(existingIngredient);
        _unitOfWork.Repository<Ingredient>().Add(existingIngredient);
        await _unitOfWork.SaveChangesAsync();

        return await GetIngredientByIdAsync(input.Id) ?? throw new InvalidOperationException("Failed to retrieve updated ingredient");
    }

    public async Task<bool> DeleteIngredientAsync(long id)
    {
        var existingIngredient = await _unitOfWork.Repository<Ingredient>().GetAsync(id);
        if (existingIngredient == null)
            throw new InvalidOperationException($"Ingredient with ID {id} not found");

        // TODO: Add business logic to check if ingredient is being used in recipes, orders, etc.
        
        _unitOfWork.Repository<Ingredient>().Remove(existingIngredient);
        await _unitOfWork.SaveChangesAsync();
        return true;
    }

    public async Task<bool> ValidateIngredientAsync(long ingredientId)
    {
        var ingredient = await _unitOfWork.Repository<Ingredient>().GetAsync(ingredientId);
        if (ingredient == null)
            throw new InvalidOperationException($"Ingredient with ID {ingredientId} not found");

        return true;
    }
}
