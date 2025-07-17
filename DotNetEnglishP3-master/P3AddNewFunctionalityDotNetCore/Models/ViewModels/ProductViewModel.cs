using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using ResourcesProductService = P3AddNewFunctionalityDotNetCore.Resources.Models.Services.ProductService;

namespace P3AddNewFunctionalityDotNetCore.Models.ViewModels
{
    public class ProductViewModel
    {
        [BindNever] public int Id { get; set; }

        [Required(
            ErrorMessageResourceName = "MissingName",
            ErrorMessageResourceType = typeof(ResourcesProductService))]
        public string Name { get; set; }

        public string Description { get; set; }

        public string Details { get; set; }

        [Required(
            ErrorMessageResourceName = "MissingStock",
            ErrorMessageResourceType = typeof(ResourcesProductService))]
        [Range(
            1,
            int.MaxValue,
            ErrorMessageResourceName = "StockNotGreaterThanZero",
            ErrorMessageResourceType = typeof(ResourcesProductService))]
        [RegularExpression(
            @"^-?\d+$",
            ErrorMessageResourceName = "StockNotAnInteger",
            ErrorMessageResourceType = typeof(ResourcesProductService))]
        public string Stock { get; set; }

        [Required(
            ErrorMessageResourceName = "MissingPrice", 
            ErrorMessageResourceType = typeof(ResourcesProductService))]
        [Range(
            0.01,
            double.MaxValue,
            ErrorMessageResourceName = "PriceNotGreaterThanZero",
            ErrorMessageResourceType = typeof(ResourcesProductService))]
        [RegularExpression(
            @"^-?\d+(\.\d{1,2})?$",
            ErrorMessageResourceName = "PriceNotANumber",
            ErrorMessageResourceType = typeof(ResourcesProductService))]
        public string Price { get; set; }
    }
}