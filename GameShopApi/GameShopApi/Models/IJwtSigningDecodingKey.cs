using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameShopApi.Models
{
    public interface IJwtSigningDecodingKey
    {
        SecurityKey GetKey();
    }
}
