namespace Maoli.Mvc
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Web.Mvc;
    using Maoli;

    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public sealed class CepAttribute : ValidationAttribute, IClientValidatable
    {
        private CepPunctuation punctuation = CepPunctuation.Loose;

        public CepAttribute()
            : this("O CEP é inválido", CepPunctuation.Loose)
        {
        }

        public CepAttribute(string errorMessage)
            : this(errorMessage, CepPunctuation.Loose)
        {
        }

        public CepAttribute(string errorMessage, CepPunctuation punctuation)
            : base(errorMessage)
        {
            this.punctuation = punctuation;
        }

        public override bool IsValid(object value)
        {
            var valueAsString = value as string;

            if (string.IsNullOrWhiteSpace(valueAsString))
            {
                return true;
            }

            var isValid = Cep.Validate(valueAsString, this.punctuation);

            return isValid;
        }

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(
            ModelMetadata metadata,
            ControllerContext context)
        {
            var punctuationName =
                Enum.GetName(typeof(CepPunctuation), this.punctuation).ToLower();

            var rule = new ModelClientValidationRule();

            rule.ValidationType = "cep";
            rule.ErrorMessage = this.FormatErrorMessage(metadata.GetDisplayName());
            rule.ValidationParameters.Add("punctuation", punctuationName);

            yield return rule;
        }
    }
}
