using ECO.CORE.DTO.AccountingDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECO.CORE.Interface
{
    public interface IAccountingRepository
    {
        Task<AccoutResultDTO> LogIn(LogInDTO logInDTO);
        Task<AccoutResultDTO> RegisteUser(RegisteUserDTO sellerDTO);
        Task<AccoutResultDTO> RegisteSeller(RegisteSellerDTO sellerDTO);
        Task<AccoutResultDTO> CheckRefreshTokenAndRevoke(string token);
        Task<bool> RevokeRefreshToken(string token);
    }
}
