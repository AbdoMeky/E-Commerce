using ECO.CORE.DTO;
using ECO.CORE.DTO.SellerDTO;
using ECO.CORE.Interface;
using ECO.EF.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECO.EF.Repository
{
    internal class SellerRepository : ISellerRepository
    {
        private readonly AppDbContext _context;
        public SellerRepository(AppDbContext context)
        {
            _context = context;
        }
        public StringResultDTO Edit(EditSellerDTO seller, string id)
        {
            var sellerDB = _context.Sellers.Find(id);
            if (sellerDB == null)
            {
                return new StringResultDTO() { Massage = "No Seller has this id" };
            }
            sellerDB.Name = seller.Name;
            sellerDB.PhoneNumber = seller.PhoneNumber;
            try
            {
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                return new StringResultDTO() { Massage=ex.Message};
            }
            return new StringResultDTO() { Id = id };
        }
    }
}
