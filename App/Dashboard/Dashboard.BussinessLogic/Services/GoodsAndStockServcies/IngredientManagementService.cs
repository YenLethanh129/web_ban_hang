using AutoMapper;
using Dashboard.BussinessLogic.Dtos;
using Dashboard.BussinessLogic.Dtos.IngredientDtos;
using Dashboard.BussinessLogic.Shared;
using Dashboard.DataAccess.Models.Entities.GoodsIngredientsAndStock;
using Dashboard.DataAccess.Repositories;

namespace Dashboard.BussinessLogic.Services.GoodsAndStockServcies;

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

public class IngredientManagementService : IIngredientManagementService
{
    private readonly IIngredientRepository _ingredientRepository;
    private readonly IMapper _mapper;

    public IngredientManagementService(IIngredientRepository ingredientRepository, IMapper mapper)
    {
        _ingredientRepository = ingredientRepository;
        _mapper = mapper;
    }

    public async Task<int> GetCountAsync()
    {
        return await _ingredientRepository.GetCountAsync();
    }

    public async Task<IngredientDto?> GetIngredientByIdAsync(long id)
    {
        var ingredient = await _ingredientRepository.GetAsync(id);
        return ingredient != null ? _mapper.Map<IngredientDto>(ingredient) : null;
    }

    public async Task<PagedList<IngredientDto>> GetIngredientsAsync(GetIngredientsInput input)
    {
        var allIngredients = await _ingredientRepository.GetIngredientsWithCategoryAsync();

        if (!string.IsNullOrWhiteSpace(input.SearchTerm))
        {
            allIngredients = allIngredients
                .Where(i => i.Name.Contains(input.SearchTerm, StringComparison.OrdinalIgnoreCase));
        }

        var totalRecords = allIngredients.Count();
        var pagedIngredients = allIngredients
            .Skip((input.PageNumber - 1) * input.PageSize)
            .Take(input.PageSize)
            .ToList();

        var ingredientDtos = _mapper.Map<List<IngredientDto>>(pagedIngredients);

        return new PagedList<IngredientDto>
        {
            Items = ingredientDtos,
            TotalRecords = totalRecords,
            PageNumber = input.PageNumber,
            PageSize = input.PageSize
        };
    }

    public async Task<IngredientDto> CreateIngredientAsync(CreateIngredientInput input)
    {
        if (string.IsNullOrWhiteSpace(input.Name))
            throw new ArgumentException("Ingredient name is required");

        if (await _ingredientRepository.IngredientNameExistsAsync(input.Name))
            throw new InvalidOperationException($"Ingredient with name '{input.Name}' already exists");

        var ingredient = _mapper.Map<Ingredient>(input);
        var created = await _ingredientRepository.CreateIngredientAsync(ingredient);

        return _mapper.Map<IngredientDto>(created);
    }

    public async Task<IngredientDto> UpdateIngredientAsync(UpdateIngredientInput input)
    {
        if (string.IsNullOrWhiteSpace(input.Name))
            throw new ArgumentException("Ingredient name is required");

        var existingIngredient = await _ingredientRepository.GetAsync(input.Id)
            ?? throw new InvalidOperationException($"Ingredient with ID {input.Id} not found");

        if (await _ingredientRepository.IngredientNameExistsAsync(input.Name, input.Id))
            throw new InvalidOperationException($"Another ingredient with name '{input.Name}' already exists");

        existingIngredient.Unit = input.Unit;
        existingIngredient.Name = input.Name;
        existingIngredient.Description = input.Description;
        existingIngredient.CategoryId = input.CategoryId;
        existingIngredient.IsActive = input.IsActive;


        var updated = await _ingredientRepository.UpdateIngredientAsync(existingIngredient);

        return _mapper.Map<IngredientDto>(updated);
    }

    public async Task<bool> DeleteIngredientAsync(long id)
    {
        return await _ingredientRepository.DeleteIngredientAsync(id);
    }

    public async Task<bool> ValidateIngredientAsync(long ingredientId)
    {
        if (!await _ingredientRepository.IngredientExistsAsync(ingredientId))
            throw new InvalidOperationException($"Ingredient with ID {ingredientId} not found");

        return true;
    }
}
