using Maitre.Core.Entities;
using Maitre.Core.Repostories;
using Maitre.DAL.Contexts;
using System;
using System.Collections.Generic;
using System.Text;

namespace Maitre.DAL.GenericRepositories;

public class ProductRepository : GenericRepository<Product> , IProductRepository
{
    public ProductRepository(MaitreDbContext context) : base(context)
    {
    }
}
