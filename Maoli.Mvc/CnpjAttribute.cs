namespace Maoli.Mvc
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Web.Mvc;
    using Maoli;

    /// <summary>
    /// Validates a CNPJ.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public sealed class CnpjAttribute : ValidationAttribute, IClientValidatable
    {
        private CnpjPunctuation punctuation = CnpjPunctuation.Loose;

        /// <summary>
        /// Initializes a new instance of the CnpjAttribute class.
        /// </summary>
        public CnpjAttribute()
            : this("O CNPJ é inválido")
        {
        }

        /// <summary>
        /// Initializes a new instance of the CnpjAttribute class.
        /// </summary>
        /// <param name="errorMessage">The error message to associate with a validation control if validation fails.</param>
        public CnpjAttribute(string errorMessage)
            : this(errorMessage, CnpjPunctuation.Loose)
        {
        }

        /// <summary>
        /// Initializes a new instance of the CnpjAttribute class.
        /// </summary>
        /// <param name="errorMessage">The error message to associate with a validation control if validation fails.</param>
        /// <param name="punctuation">Indicates how punctuation will be handled.</param>
        public CnpjAttribute(string errorMessage, CnpjPunctuation punctuation)
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

        /// <summary>
        /// Determines whether the specified value matches the pattern of a valid CNPJ. 
        /// </summary>
        /// <param name="value">The value to validate.</param>
        /// <param name="validationContext">The validation context.</param>
        /// <returns>true if the specified value is valid or null; otherwise, false.</returns>
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
                    return this.ValidateCnpj(valueAsString);   
                }
            }
            else
            {
                return this.ValidateCnpj(valueAsString);
            }

            return null;
        }

        private ValidationResult ValidateCnpj(string cnpj)
        {
            var isValid = Cnpj.Validate(cnpj, this.punctuation);

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

        /// <summary>
        /// Gets a list of client validation rules for the property.
        /// </summary>
        /// <param name="metadata">The model metadata.</param>
        /// <param name="context">The controller context.</param>
        /// <returns>A list of remote client validation rules for the property.</returns>
        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(
            ModelMetadata metadata,
            ControllerContext context)
        {
            var punctuationName =
                Enum.GetName(typeof(CnpjPunctuation), this.punctuation).ToLower();

            var rule = new ModelClientValidationRule();

            rule.ValidationType = "cnpj";
            rule.ErrorMessage = this.FormatErrorMessage(metadata.GetDisplayName());
            rule.ValidationParameters.Add("punctuation", punctuationName);

            if (!string.IsNullOrWhiteSpace(this.TestProperty))
            {
                rule.ValidationParameters.Add("testproperty", this.TestProperty);
                rule.ValidationParameters.Add("testpropertyvalue", this.TestValue);
            }

            yield return rule;
        }
    }
}