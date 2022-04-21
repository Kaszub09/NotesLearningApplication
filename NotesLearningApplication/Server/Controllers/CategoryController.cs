using DatabaseServices.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NotesLearningApplication.Shared.DTO;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace NotesLearningApplication.Server.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase {
        ICategoriesDbService _categoriesDbService;
        public CategoryController(ICategoriesDbService categoriesDbService) {
            _categoriesDbService = categoriesDbService;
        }

        // GET: api/<Category>
        [HttpGet]
        public ActionResult<IEnumerable<CategoryDTO>> Get() {
            return Ok(_categoriesDbService.GetAllCategories());
        }



        // GET api/<Category>/5
        [HttpGet("{id}")]
        public ActionResult<CategoryDTO> Get(int id) {
            return Ok(_categoriesDbService.GetCategory(id));
        }

        // POST api/<Category>
        [HttpPost("new")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<int>> Post([FromBody] CategoryDTO categoryDTO) {
            return Ok(await _categoriesDbService.AddCategoryAsync(categoryDTO));
        }

        // PUT api/<Category>/5
        [HttpPut("edit")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Put([FromBody] CategoryDTO categoryDTO) {
            await _categoriesDbService.ModifyCategoryAsync(categoryDTO);
            return Ok();
        }

        // DELETE api/<Category>/5
        [HttpDelete("{id}")]
        [Authorize(Roles ="Admin")]
        public async Task<ActionResult> Delete(int id) {
            await _categoriesDbService.RemoveCategoryAsync(id);
            return Ok();
        }
    }
}
