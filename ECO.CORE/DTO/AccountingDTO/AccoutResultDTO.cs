using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ECO.CORE.DTO.AccountingDTO
{
    public class AccoutResultDTO
    {
        public bool IsAuthenticated {  get; set; }
        public string? Token { get; set; }
        public string? Massage {  get; set; }
        [JsonIgnore]
        public string? RefreshToken {  get; set; }
        public DateTime? RefreshTokenExpired { get; set; }

    }
}
