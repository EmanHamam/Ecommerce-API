using System.ComponentModel.DataAnnotations;

namespace E_Commerce.Attributes
{
    public class MaxFileSizeAttribute : ValidationAttribute
    {
        private readonly int _maxFileSize;

        public MaxFileSizeAttribute(int maxFileSize)
        {
            _maxFileSize = maxFileSize;
        }
        protected override ValidationResult? IsValid
            (object? value, ValidationContext validationContext)
        {
            var file = value as IFormFile;
            if (file != null)
            {
                if(file.Length > _maxFileSize)
                {
                    return new ValidationResult($"Maximum allowed Size is {_maxFileSize} bytes");
                }

               
            }
            return ValidationResult.Success;
        }
    }
}
