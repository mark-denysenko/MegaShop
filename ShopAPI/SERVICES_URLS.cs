﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShopAPI
{
    public class SERVICES_URLS
    {
        public class UserService
        {
            // container port 44349
            public const string url = "https://userservice:443/api/user/";
            public const string Users = url;
            public const string Authentication = url + "authentication";
            public const string Register = url + "register";
            public const string DeleteUser = url;
            public const string CheckRefreshToken = url + "refresh";
            public const string RefreshTokenIdentifier = url + "refreshpart/";

            public const string AccessTokenHEADER = "Access-Token";
        }

        public class ProductService
        {
            public const string Url = "https://productservice:443/api/products/";
            public const string Products = Url;
        }
    }
}
