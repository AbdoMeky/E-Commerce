using ECO.CORE.DTO;
using ECO.CORE.DTO.CategoryDTO;
using ECO.CORE.Entities;
using ECO.CORE.Interface;
using ECO.EF.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECO.EF.Repository
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly AppDbContext _context;
        public CategoryRepository(AppDbContext context)
        {
            _context = context;
        }
        public IntResultDTO Add(ShowCategoryDTO categoryDTO)
        {
            if (_context.Categorys.Any(c => c.Name == categoryDTO.Name)){
                return new IntResultDTO() { Massage="there is category of the name of "+categoryDTO.Name};
            }
            Category category = new Category(categoryDTO);
            _context.Categorys.Add(category);
            try
            {
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                return new IntResultDTO() { Massage = ex.Message };
            }
            return new IntResultDTO() { Id = category.Id };
        }

        public IntResultDTO Delete(int id)
        {
            var category=GetCategory(id);
            if(category is null) {
                return new IntResultDTO() { Massage = "no Category has this Id" };
            }
            _context.Categorys.Remove(category);
            try
            {
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                return new IntResultDTO() { Massage = ex.Message };
            }
            return new IntResultDTO() { Id = 1};
        }

        public List<ShowCategoryDTO> GetAll()
        {
            var result= new List<ShowCategoryDTO>();
            var categories = _context.Categorys.ToList();
            foreach (var category in categories)
            {
                result.Add(new ShowCategoryDTO(category));
            }
            return result;
        }

        public ShowCategoryDTO GetById(int id)
        {
            var category=GetCategory(id);
            if(category is null)
            {
                return null;
            }
            return new ShowCategoryDTO(category);
        }

        public IntResultDTO Update(ShowCategoryDTO categoryDTO,int id)
        {
            var checkCategory = _context.Categorys.SingleOrDefault(c => c.Name == categoryDTO.Name);
            if (checkCategory is not null && checkCategory.Id!=id)
            {
                return new IntResultDTO() { Massage = "there is category of the name of " + categoryDTO.Name };
            }
            var category = GetCategory(id);
            category.Name=categoryDTO.Name;
            category.Description=categoryDTO.Description;
            try
            {
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                return new IntResultDTO() { Massage = ex.Message };
            }
            return new IntResultDTO() { Id = category.Id };
        }
        Category GetCategory(int id) => _context.Categorys.Find(id);
    }
}
