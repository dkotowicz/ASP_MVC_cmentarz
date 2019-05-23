using Cmentarz.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace Cmentarz.Validators
{
    public class CorrectImie : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext val)
        {
            if (value != null)
            {
                string imie;
                imie = value.ToString();
                int dlugosc = imie.Length;

                if (dlugosc < 2)
                {
                    return new ValidationResult("Podane imie jest za krótkie");
                }
                bool isSpec = Regex.IsMatch(imie, @"^[a-zA-ZĄĆĘŁŃÓŚŹŻąćęłńóżź \\s \\-]*$");

                if (isSpec == false)
                {
                    return new ValidationResult("Imie moze zawierac tylko litery i myślnik");
                }
            }
            return ValidationResult.Success;
        }
    }
}