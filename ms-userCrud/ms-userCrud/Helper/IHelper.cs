using FluentValidation.Results;

namespace ms_userCrud.Helper
{
    public interface IHelper
    {
        void ValidatorHandler(ValidationResult results);
    }
}
