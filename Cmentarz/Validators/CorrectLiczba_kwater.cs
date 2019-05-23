using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Cmentarz.Validators
{
    public class CorrectLiczba_kwater : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext val)
        {
            if (value == null)
            {
                return new ValidationResult("Podaj liczbę kwater");
            }
            string liczba_kwater;
            liczba_kwater = value.ToString();
            int n;
            bool isNumeric = int.TryParse(liczba_kwater, out n);
            if (isNumeric == false)
            {
                return new ValidationResult("Podałeś złą liczbę");
            }
           if(isNumeric==true)
            {
                int x = 0;
                Int32.TryParse(liczba_kwater, out x);
                if(x<1)
                {
                    return new ValidationResult("Podałeś złą liczbę");
                }
            }
            return ValidationResult.Success;
        }
    }
}