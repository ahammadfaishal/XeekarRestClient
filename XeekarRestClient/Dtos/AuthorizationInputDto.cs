using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XeekarRestClient.Dtos
{
    public class AuthorizationInputDto
    {
        public AuthorizationInputDto()
        {
            TenancyName = "powerhouse";
        }

        public string UserNameOrEmailAddress { get; set; }
        public string Password { get; set; }
        public string TenancyName { get; set; }
    }
}
