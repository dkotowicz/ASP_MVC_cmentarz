using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace Cmentarz.Validators
{
    public class CorrectProlongata : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext val)
        {
            if (value != null)
            {
                string prolongata;
                prolongata = value.ToString();
                int dlugosc = prolongata.Length;

                if (dlugosc < 4)
                {
                    return new ValidationResult("Podana data jest za krótka");
                }
                bool isSpec = false;

                if (isSpec = Regex.IsMatch(prolongata, @"^(3[01]|[12][0-9]|0[1-9])-(1[0-2]|0[1-9])-[0-9]{4}$") == true)
                {
                    return ValidationResult.Success;
                }
                else if (isSpec = Regex.IsMatch(prolongata, @"^(1[0-2]|0[1-9])-[0-9]{4}$") == true)
                {
                    return ValidationResult.Success;
                }
                else if (isSpec = Regex.IsMatch(prolongata, @"^[0-9]{4}$") == true)
                {
                    return ValidationResult.Success;
                }
                else
                {
                    return new ValidationResult("Podałeś zły format daty");
                }
            }
            return ValidationResult.Success;
        }
    }
}