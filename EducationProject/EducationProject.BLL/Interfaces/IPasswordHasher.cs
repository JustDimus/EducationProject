using System;
using System.Collections.Generic;
using System.Text;

namespace EducationProject.BLL.Interfaces
{
    public interface IPasswordHasher
    {
        string Hash(string password);
    }
}
