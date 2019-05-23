using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace Cmentarz.Validators
{
    public class CorrectData_smierci : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext val)
        {
            if (value != null)
            {
                string data_smierci;
                data_smierci = value.ToString();
                int dlugosc = data_smierci.Length;
                if (dlugosc < 4)
                {
                    return new ValidationResult("Podana data jest za krótka");
                }
                bool isSpec = false;

                if (isSpec = Regex.IsMatch(data_smierci, @"^(3[01]|[12][0-9]|0[1-9])-(1[0-2]|0[1-9])-[0-9]{4}$") == true)
                {
                    return ValidationResult.Success;
                }
                else if (isSpec = Regex.IsMatch(data_smierci, @"^(1[0-2]|0[1-9])-[0-9]{4}$") == true)
                {
                    return ValidationResult.Success;
                }
                else if (isSpec = Regex.IsMatch(data_smierci, @"^[0-9]{4}$") == true)
                {
                    return ValidationResult.Success;
                }
                else
                {
                    return new ValidationResult("Podałeś złą datę");
                }
            }
            return ValidationResult.Success;
        }
    }
}