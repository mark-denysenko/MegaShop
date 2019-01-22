using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UserService.Infrastructure
{
    public interface IPasswordHasher
    {
        string GetHash(string value);
        string GetHash(byte[] value);
    }
}
