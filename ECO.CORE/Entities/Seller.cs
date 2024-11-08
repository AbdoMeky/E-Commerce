using ECO.CORE.DTO.AccountingDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECO.CORE.Entities
{
    public class Seller :ApplicationUser
    {
        public List<Product>? Products { get; set; }
        public Seller()
        {
            
        }
        public Seller(RegisteSellerDTO saller)
        {
            this.Name = saller.Name;
            this.UserName = saller.UserName;
            this.PhoneNumber = saller.PhoneNumber;
            this.Email = saller.Email;
        }
    }
}
