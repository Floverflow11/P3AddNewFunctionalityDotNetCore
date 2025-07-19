using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using P3AddNewFunctionalityDotNetCore.Models.ViewModels;

namespace P3AddNewFunctionalityDotNetCore.UnitTests;

internal static class ProductViewModelUtils
{
    public static List<ValidationResult> ValidateModel(ProductViewModel viewModel)
    {
        var result = new List<ValidationResult>();
        var context = new ValidationContext(viewModel);

        Validator.TryValidateObject(viewModel, context, result, true);
        return result;
    }
}