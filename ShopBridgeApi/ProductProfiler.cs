using AutoMapper;
using ShopBridgeApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShopBridgeApi.Repositories
{
    public class ProductProfiler : Profile
    {
        public ProductProfiler()
        {
            CreateMap<ProductDto, Product>();
        }
    }
}
