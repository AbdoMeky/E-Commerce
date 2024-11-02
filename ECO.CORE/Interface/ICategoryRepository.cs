using ECO.CORE.DTO;
using ECO.CORE.DTO.CategoryDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECO.CORE.Interface
{
    public interface ICategoryRepository
    {
        List<ShowCategoryDTO> GetAll();
        ShowCategoryDTO GetById(int id);
        IntResultDTO Add(ShowCategoryDTO categoryDTO);
        IntResultDTO Update(ShowCategoryDTO categoryDTO,int id);
        IntResultDTO Delete(int id); 
    }
}
