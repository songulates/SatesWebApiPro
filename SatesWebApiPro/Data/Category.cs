using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SatesWebApiPro.Data
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Product> Products { get; set; }
        //bir kategorinin birden fazla product ı olabilir
        //bir kategoriye karşılık birden fazla product söz konusu

    }
}
