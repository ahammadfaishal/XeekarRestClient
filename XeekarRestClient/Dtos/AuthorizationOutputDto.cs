using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XeekarRestClient.Dtos
{
    public class AuthorizationOutputDto
    {
        public string AccessToken { get; set; }
        public string TokenType { get; set; }
    }
}
