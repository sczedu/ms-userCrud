using FluentValidation.Results;
using System;
using System.Linq;

namespace ms_userCrud._05Helper
{
    public class Helper : IHelper
    {
        public void ValidatorHandler(ValidationResult results)
        {
            if (!results.IsValid)
            {
                var failure = results.Errors.FirstOrDefault();
                throw new Exception("Property " + failure.PropertyName + " failed validation. Error was: " + failure.ErrorMessage);
            }
        }
    }
}
