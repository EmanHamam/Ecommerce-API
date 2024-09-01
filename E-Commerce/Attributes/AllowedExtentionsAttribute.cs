﻿using System.ComponentModel.DataAnnotations;

namespace E_Commerce.Attributes
{
    public class AllowedExtentionsAttribute : ValidationAttribute
    {
        private readonly string _allowedExtensions;

        public AllowedExtentionsAttribute(string allowedExtensions)
        {
            _allowedExtensions = allowedExtensions;
        }
        protected override ValidationResult? IsValid
            (object? value, ValidationContext validationContext)
        {
            var file = value as IFormFile;
            if (file != null)
            {
                var extention = Path.GetExtension(file.FileName);

                var Allowed = _allowedExtensions.Split(',').Contains(extention, StringComparer.OrdinalIgnoreCase);
                if (!Allowed)
                {
                    return new ValidationResult($"Only {_allowedExtensions} are allowed! ");

                }
            }
            return ValidationResult.Success;
        }
    }
}
