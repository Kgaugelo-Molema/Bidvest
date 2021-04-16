using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Transactions.Helpers
{
    public static class MapperHelper
    {
        public static IMapper GetMapper<T1, T2>(this ControllerBase controller)
        {
            var config = new MapperConfiguration(cfg => cfg.CreateMap<T1, T2>());
            return config.CreateMapper();
        }
    }
}
