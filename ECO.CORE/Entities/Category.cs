using ECO.CORE.DTO.CategoryDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECO.CORE.Entities
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<Product> products { get; set; }
        public Category()
        {
            
        }
        public Category(ShowCategoryDTO category)
        {
            this.Description = category.Description;
            this.Name = category.Name;
        }
    }
}
