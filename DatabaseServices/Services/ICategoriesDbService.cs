using NotesLearningApplication.Shared.DTO;

namespace DatabaseServices.Services {
    public interface ICategoriesDbService {
        Task<int> AddCategoryAsync(CategoryDTO CategoryDTO);
        IEnumerable<CategoryDTO> GetAllCategories();
        CategoryDTO GetCategory(int categoryId);
        Task ModifyCategoryAsync(CategoryDTO CategoryDTO);
        Task RemoveCategoryAsync(int categoryId);
    }
}