﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobApplicationLibrary.Services
{
    public class IdentityValidator : IIdentityValidator
    {
        public string Country => throw new NotImplementedException();

        public ICountryDataProvider CountryDataProvider => throw new NotImplementedException();

        public bool IsValid(string identityNum)
        {
            return true;
        }
    }
}
