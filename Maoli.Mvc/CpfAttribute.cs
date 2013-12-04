namespace Maoli.Mvc
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Web.Mvc;
    using Maoli;

    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public sealed class CpfAttribute : ValidationAttribute, IClientValidatable
    {
        private CpfPunctuation punctuation = CpfPunctuation.Loose;

        public CpfAttribute()
            : this("O CPF é inválido", CpfPunctuation.Loose)
        {
        }

        public CpfAttribute(string errorMessage)
            : this(errorMessage, CpfPunctuation.Loose)
        {
        }

        public CpfAttribute(string errorMessage, CpfPunctuation punctuation)
            : base(errorMessage)
        {
            this.punctuation = punctuation;
            
            this.TestProperty = string.Empty;
            this.TestValue = string.Empty;
        }

        /// <summary>
        /// Gets or sets the property name
        /// </summary>
        public string TestProperty { get; set; }

        /// <summary>
        /// Gets or sets the propery value
        /// </summary>
        public string TestValue { get; set; }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var valueAsString = value as string;

            if (!this.ValidationIsRequired(value))
            {
                return null;
            }

            if (this.HasOtherPropertyDefined(validationContext))
            {
                var otherValue = GetTestValue(validationContext);

                // TODO: to test with enum and other types (string, int, long etc)
                if (otherValue == this.TestValue)
                {
                    return this.ValidateCpf(valueAsString);
                }
            }
            else
            {
                return this.ValidateCpf(valueAsString);
            }

            return null;
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

            if (!string.IsNullOrWhiteSpace(this.TestProperty))
            {
                rule.ValidationParameters.Add("testproperty", this.TestProperty);
                rule.ValidationParameters.Add("testpropertyvalue", this.TestValue);
            }

            yield return rule;
        }

        private ValidationResult ValidateCpf(string cpf)
        {
            var isValid = Cpf.Validate(cpf, this.punctuation);

            if (isValid)
            {
                return null;
            }

            return new ValidationResult(this.ErrorMessage);
        }

        private bool HasOtherPropertyDefined(ValidationContext validationContext)
        {
            if (string.IsNullOrWhiteSpace(this.TestProperty))
            {
                return false;
            }

            var propertyInfo = validationContext.ObjectType.GetProperty(this.TestProperty);

            if (propertyInfo == null)
            {
                return false;
            }

            return true;
        }

        private string GetTestValue(ValidationContext validationContext)
        {
            string resultValue = null;

            if (string.IsNullOrWhiteSpace(this.TestProperty))
            {
                return null;
            }

            var propertyInfo = validationContext.ObjectType.GetProperty(this.TestProperty);

            if (propertyInfo == null)
            {
                return null;
            }

            var otherPropertyValue = propertyInfo.GetValue(validationContext.ObjectInstance, null);

            if (otherPropertyValue.GetType().IsEnum)
            {
                var enumAsInt = (int)otherPropertyValue;

                resultValue = enumAsInt.ToString();
            }
            else
            {
                resultValue = otherPropertyValue.ToString();
            }

            return resultValue;
        }

        private bool ValidationIsRequired(object value)
        {
            var valueAsString = value as string;

            if (string.IsNullOrWhiteSpace(valueAsString))
            {
                return false;
            }

            return true;
        }
    }
}
