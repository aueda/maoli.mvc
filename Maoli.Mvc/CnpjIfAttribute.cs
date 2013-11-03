namespace Maoli.Mvc
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Web.Mvc;
    using Maoli;

    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public sealed class CnpjIfAttribute : ValidationAttribute, IClientValidatable
    {
        private CnpjPunctuation punctuation = CnpjPunctuation.Loose;

        private string testProperty = string.Empty;

        private string testPropertyValue = string.Empty;

        public CnpjIfAttribute()
            : this("O CNPJ é inválido")
        {
        }

        public CnpjIfAttribute(string errorMessage)
            : base(errorMessage)
        {
        }

        public CnpjIfAttribute(string testProperty, string testPropertyValue)
            : base()
        {
            this.testProperty = testProperty;
            this.testPropertyValue = testPropertyValue;
        }

        public CnpjIfAttribute(string errorMessage, string testProperty, string testPropertyValue)
            : base(errorMessage)
        {
            this.testProperty = testProperty;
            this.testPropertyValue = testPropertyValue;
        }

        public override bool IsValid(object value)
        {
            var valueAsString = value as string;

            if (string.IsNullOrWhiteSpace(valueAsString))
            {
                return true;
            }

            var isValid = Cnpj.Validate(valueAsString, this.punctuation);

            return isValid;
        }

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(
            ModelMetadata metadata,
            ControllerContext context)
        {
            var punctuationName =
                Enum.GetName(typeof(CnpjPunctuation), this.punctuation).ToLower();

            var rule = new ModelClientValidationRule();

            rule.ValidationType = "cnpjif";
            rule.ErrorMessage = this.FormatErrorMessage(metadata.GetDisplayName());
            rule.ValidationParameters.Add("punctuation", punctuationName);
            rule.ValidationParameters.Add("testproperty", testProperty);
            rule.ValidationParameters.Add("testpropertyvalue", testPropertyValue);

            yield return rule;
        }
    }
}