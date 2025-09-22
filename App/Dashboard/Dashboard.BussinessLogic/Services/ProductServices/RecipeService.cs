using AutoMapper;
using Dashboard.BussinessLogic.Dtos;
using Dashboard.BussinessLogic.Dtos.ProductDtos;
using Dashboard.BussinessLogic.Shared;
using Dashboard.DataAccess.Data;
using Dashboard.DataAccess.Models.Entities.Products;
using Dashboard.DataAccess.Repositories;
using Dashboard.DataAccess.Specification;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dashboard.BussinessLogic.Services.ProductServices
{
    public interface IRecipeService
    {
        Task<PagedList<RecipeDto>> GetRecipesAsync(GetRecipesInput input);
        Task<List<RecipeDto>> GetAllRecipesAsync();
        Task<RecipeDto?> GetRecipeByIdAsync(long id);
        Task<RecipeDto> CreateRecipeAsync(CreateRecipeInput input);
        Task<RecipeDto> UpdateRecipeAsync(UpdateRecipeInput input);
        Task<bool> DeleteRecipeAsync(long id);
        Task<List<RecipeDto>> GetRecipesByProductIdAsync(long productId);
        Task<bool> AssignRecipeToProductAsync(long productId, long recipeId);
        Task<bool> UnassignRecipeFromProductAsync(long productId, long recipeId);
    }

    public class RecipeService : BaseTransactionalService, IRecipeService
    {
        private readonly IRepository<Recipe> _recipeRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<RecipeService> _logger;

        public RecipeService(
            IUnitOfWork unitOfWork,
            IRepository<Recipe> recipeRepository,
            IMapper mapper,
            ILogger<RecipeService> logger) : base(unitOfWork)
        {
            _recipeRepository = recipeRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<PagedList<RecipeDto>> GetRecipesAsync(GetRecipesInput input)
        {
            var specification = new Specification<Recipe>(r =>
                (string.IsNullOrEmpty(input.Name) || r.Name.Contains(input.Name)) &&
                (!input.ProductId.HasValue || r.ProductId == input.ProductId.Value) &&
                (!input.IsActive.HasValue || r.IsActive == input.IsActive.Value)
            );

            specification.IncludeStrings.Add("Product");
            specification.IncludeStrings.Add("RecipeIngredients");

            var recipes = await _recipeRepository.GetAllWithSpecAsync(specification, true);
            var totalCount = recipes.Count();

            var pagedRecipes = recipes
                .Skip((input.PageNumber - 1) * input.PageSize)
                .Take(input.PageSize)
                .ToList();

            var recipeDtos = _mapper.Map<List<RecipeDto>>(pagedRecipes);

            return new PagedList<RecipeDto>
            {
                PageNumber = input.PageNumber,
                PageSize = input.PageSize,
                TotalRecords = totalCount,
                Items = recipeDtos
            };
        }

        public async Task<List<RecipeDto>> GetAllRecipesAsync()
        {
            var specification = new Specification<Recipe>(r => r.IsActive);
            specification.IncludeStrings.Add("Product");

            var recipes = await _recipeRepository.GetAllWithSpecAsync(specification, true);
            return _mapper.Map<List<RecipeDto>>(recipes);
        }

        public async Task<RecipeDto?> GetRecipeByIdAsync(long id)
        {
            var specification = new Specification<Recipe>(r => r.Id == id);
            specification.IncludeStrings.Add("Product");
            specification.IncludeStrings.Add("RecipeIngredients");
            specification.IncludeStrings.Add("RecipeIngredients.Ingredient");

            var recipe = await _recipeRepository.GetWithSpecAsync(specification, true);
            return recipe != null ? _mapper.Map<RecipeDto>(recipe) : null;
        }

        public async Task<RecipeDto> CreateRecipeAsync(CreateRecipeInput input)
        {
            return await ExecuteInTransactionAsync(async () =>
            {
                var recipe = _mapper.Map<Recipe>(input);
                recipe.CreatedAt = DateTime.UtcNow;

                await _recipeRepository.AddAsync(recipe);
                await _unitOfWork.SaveChangesAsync();

                return _mapper.Map<RecipeDto>(recipe);
            });
        }

        public async Task<RecipeDto> UpdateRecipeAsync(UpdateRecipeInput input)
        {
            return await ExecuteInTransactionAsync(async () =>
            {
                var recipe = await _recipeRepository.GetAsync(input.Id)
                    ?? throw new ArgumentException($"Recipe with id {input.Id} not found");

                _mapper.Map(input, recipe);
                recipe.LastModified = DateTime.UtcNow;

                _recipeRepository.Update(recipe);
                await _unitOfWork.SaveChangesAsync();

                return _mapper.Map<RecipeDto>(recipe);
            });
        }

        public async Task<bool> DeleteRecipeAsync(long id)
        {
            return await ExecuteInTransactionAsync(async () =>
            {
                var recipe = await _recipeRepository.GetAsync(id);
                if (recipe == null)
                {
                    return false;
                }

                _recipeRepository.Remove(recipe);
                await _unitOfWork.SaveChangesAsync();
                return true;
            });
        }

        public async Task<List<RecipeDto>> GetRecipesByProductIdAsync(long productId)
        {
            try
            {
                var recipes = await _recipeRepository.GetQueryable()
                    .Where(r => r.ProductId == productId && r.IsActive)
                    .Include(r => r.Product)
                    .Include(r => r.RecipeIngredients)
                    .AsNoTracking() 
                    .ToListAsync();

                return _mapper.Map<List<RecipeDto>>(recipes);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading recipes for product {ProductId}", productId);
                throw;
            }
        }

        public async Task<bool> AssignRecipeToProductAsync(long productId, long recipeId)
        {
            return await ExecuteInTransactionAsync(async () =>
            {
                var recipe = await _recipeRepository.GetAsync(recipeId);
                if (recipe == null)
                    return false;

                recipe.ProductId = productId;
                recipe.LastModified = DateTime.UtcNow;

                _recipeRepository.Update(recipe);
                await _unitOfWork.SaveChangesAsync();

                return true;
            });
        }

        public async Task<bool> UnassignRecipeFromProductAsync(long productId, long recipeId)
        {
            return await ExecuteInTransactionAsync(async () =>
            {
                var recipe = await _recipeRepository.GetAsync(recipeId);
                if (recipe == null || recipe.ProductId != productId)
                    return false;

                recipe.ProductId = 0;
                recipe.LastModified = DateTime.UtcNow;

                _recipeRepository.Update(recipe);
                await _unitOfWork.SaveChangesAsync();

                return true;
            });
        }
    }
}