﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PagedList;
using PagedList.Mvc;

namespace ShoppingWeb.Models.ViewModel
{
    public class ProductProductCommetViewModel
    {
        public Product Products { get; set; }
        public IPagedList<ProductCommet> ProductCommet { get; set; }
        //總分
        public decimal Starrating { get; set; }
        //算分數
        public List<int> Staritem { get; set; }
        //最高分數
        public int Starmax { get; set; }
    }
}