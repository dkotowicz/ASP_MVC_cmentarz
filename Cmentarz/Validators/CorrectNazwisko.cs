using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace Cmentarz.Validators
{
    public class CorrectNazwisko : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext val)
        {
            if(value!=null)
            {
                string nazwisko;
                nazwisko = value.ToString();
                int dlugosc = nazwisko.Length;

                if (dlugosc < 2)
                {
                    return new ValidationResult("Podane nazwisko jest za krótkie");
                }

                bool isSpec = Regex.IsMatch(nazwisko, @"^[a-zA-ZĄĆĘŁŃÓŚŹŻąćęłńóżź \\s \\-]*$");

                if (isSpec == false)
                {
                    return new ValidationResult("Nazwisko moze zawierac tylko litery i myślnik");
                }
            }            
            return ValidationResult.Success;
        }
    }
}