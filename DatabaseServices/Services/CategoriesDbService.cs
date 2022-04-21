using NotesLearningApplication.Shared.DTO;
using AutoMapper;
using DatabaseServices.Entities;
using Microsoft.EntityFrameworkCore;

namespace DatabaseServices.Services {
    public class CategoriesDbService : ServiceBase,  ICategoriesDbService {
        public CategoriesDbService(NotesDbContext dbContext, IMapper mapper) : base(dbContext, mapper) { }

        public async Task<int> AddCategoryAsync(CategoryDTO CategoryDTO) {
            var newCategory = _mapper.Map<Category>(CategoryDTO);
            if (_dbContext.Categories.Any(category => category.Name.ToLower() == newCategory.Name.ToLower())) {
                throw new ArgumentException("Category [" + newCategory.Name + "] already exists in database.");
            } else {
                await _dbContext.Categories.AddAsync(newCategory);
                await _dbContext.SaveChangesAsync();
            }
            return newCategory.Id;
        }
        public async Task RemoveCategoryAsync(int categoryId) {
            _dbContext.Notes.Where(n => n.CategoryId ==categoryId).Load();
            var categoryToRemove = _dbContext.Categories.FirstOrDefault(category => category.Id == categoryId);
            if (categoryToRemove == null) {
                throw new KeyNotFoundException("Id [" + categoryId + "] not found in database.");
            } else {
                _dbContext.Categories.Remove(categoryToRemove);
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task ModifyCategoryAsync(CategoryDTO CategoryDTO) {
            var categoryToModify = _dbContext.Categories.FirstOrDefault(category => category.Id == CategoryDTO.Id);
            if (categoryToModify == null) {
                throw new KeyNotFoundException("Id [" + CategoryDTO.Id + "] not found in database.");
            } else {
                categoryToModify.Name = CategoryDTO.Name;
                categoryToModify.Description = CategoryDTO.Description;

                await _dbContext.SaveChangesAsync();
            }
        }

        public CategoryDTO GetCategory(int categoryId) {
            var categoryToGet = _dbContext.Categories.FirstOrDefault(category => category.Id == categoryId, null);
            if (categoryToGet == null) {
                throw new KeyNotFoundException("Id [" + categoryId + "] not found in database.");
            } else {
                return _mapper.Map<CategoryDTO>(categoryToGet);
            }
        }

        public IEnumerable<CategoryDTO> GetAllCategories() {
            return _mapper.Map<List<CategoryDTO>>(_dbContext.Categories.ToList());
        }
    }
}

