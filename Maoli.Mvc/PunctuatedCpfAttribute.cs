namespace Maoli.Mvc
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Web.Mvc;
    using Maoli;

    /// <summary>
    /// Validates a punctuated CPF.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class PunctuatedCpfAttribute : ValidationAttribute, IClientValidatable
    {
        private CpfPunctuation punctuation = CpfPunctuation.Strict;

        /// <summary>
        /// Initializes a new instance of the PunctuatedCpfAttribute class.
        /// </summary>
        public PunctuatedCpfAttribute()
            : this("O CPF é inválido")
        {
        }

        /// <summary>
        /// Initializes a new instance of the PunctuatedCpfAttribute class.
        /// </summary>
        /// <param name="errorMessage">The error message to associate with a validation control if validation fails.</param>
        public PunctuatedCpfAttribute(string errorMessage)
            : base(errorMessage)
        {
        }

        /// <summary>
        /// Determines whether the specified value matches the pattern of a valid punctuated CPF. 
        /// </summary>
        /// <param name="value">The value to validate.</param>
        /// <returns>true if the specified value is valid or null; otherwise, false.</returns>
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
                Enum.GetName(typeof(CpfPunctuation), this.punctuation).ToLower();

            var rule = new ModelClientValidationRule();

            rule.ValidationType = "cpf";
            rule.ErrorMessage = this.FormatErrorMessage(metadata.GetDisplayName());
            rule.ValidationParameters.Add("punctuation", punctuationName);

            yield return rule;
        }
    }
}
