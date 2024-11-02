using ECO.CORE.DTO.AccountingDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECO.CORE.Entities
{
    public class User:ApplicationUser
    {
        public string City { get; set; }
        public string Address {  get; set; }
        public List<Order>? Orders { get; set; }
        public User()
        {
            
        }
        public User(RegisteUserDTO user)
        {
            this.Name = user.Name;
            this.UserName = user.UserName;
            this.PhoneNumber = user.PhoneNumber;
            this.Email = user.Email;
            this.City = user.City;
            this.Address = user.Address;
        }
    }
}
