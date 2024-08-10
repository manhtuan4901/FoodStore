﻿namespace FoodStore.Models.ViewModel
{
    public class ProductCategoryViewModel
    {
        public IEnumerable<Product> Products { get; set; }
        public IEnumerable<Category> Categories { get; set; }

        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
    }

}
