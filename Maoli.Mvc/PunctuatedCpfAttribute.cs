namespace Maoli.Mvc
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Web.Mvc;
    using Maoli;

    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class PunctuatedCpfAttribute : ValidationAttribute, IClientValidatable
    {
        private CpfPunctuation punctuation = CpfPunctuation.Strict;

        public PunctuatedCpfAttribute()
            : this("O CPF é inválido")
        {
        }

        public PunctuatedCpfAttribute(string errorMessage)
            : base(errorMessage)
        {
        }

        public override bool IsValid(object value)
        {
            var valueAsString = value as string;

            if (string.IsNullOrWhiteSpace(valueAsString))
            {
                return true;
            }

            var isValid = Cpf.Validate(valueAsString, this.punctuation);

            return isValid;
        }

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(
            ModelMetadata metadata,
            ControllerContext context)
        {
            var punctuationName =
                Enum.GetName(typeof(CpfPunctuation), this.punctuation).ToLower();

            var rule = new ModelClientValidationRule();

            rule.ValidationType = "cpf";
            rule.ErrorMessage = this.FormatErrorMessage(metadata.GetDisplayName());
            rule.ValidationParameters.Add("punctuation", punctuationName);

            yield return rule;
        }
    }
}
