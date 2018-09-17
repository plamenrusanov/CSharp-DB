using System.ComponentModel.DataAnnotations;

namespace P01_BillsPaymentSystem.Data.Models.Attributes
{
     public class NonUnicodeAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            string nullErrMesage = "Value can not be null!";
            if (value == null)
            {
                return new ValidationResult(nullErrMesage);
            }

            string text = (string)value;
            string errMessage = "Value can not contain unicode characters!";
            for (int i = 0; i < text.Length; i++)
            {
                if (text[i] > 255)
                {
                    return new ValidationResult(errMessage);
                }
            }
            return ValidationResult.Success; 
        }
    }
}
