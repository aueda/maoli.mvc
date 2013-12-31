namespace Maoli.Mvc
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Web.Mvc;
    using Maoli;

    /// <summary>
    /// Validates a CEP code.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public sealed class CepAttribute : ValidationAttribute, IClientValidatable
    {
        private CepPunctuation punctuation = CepPunctuation.Loose;

        /// <summary>
        /// Initializes a new instance of the CepAttribute class.
        /// </summary>
        public CepAttribute()
            : this("O CEP é inválido", CepPunctuation.Loose)
        {
        }

        /// <summary>
        /// Initializes a new instance of the CepAttribute class.
        /// </summary>
        /// <param name="errorMessage">The error message to associate with a validation control if validation fails.</param>
        public CepAttribute(string errorMessage)
            : this(errorMessage, CepPunctuation.Loose)
        {
        }

        /// <summary>
        /// Initializes a new instance of the CepAttribute class.
        /// </summary>
        /// <param name="errorMessage">The error message to associate with a validation control if validation fails.</param>
        /// <param name="punctuation">Indicates how punctuation will be handled.</param>
        public CepAttribute(string errorMessage, CepPunctuation punctuation)
            : base(errorMessage)
        {
            this.punctuation = punctuation;
        }

        /// <summary>
        /// Determines whether the specified value matches the pattern of a valid CEP. 
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

            var isValid = Cep.Validate(valueAsString, this.punctuation);

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
                Enum.GetName(typeof(CepPunctuation), this.punctuation).ToLower();

            var rule = new ModelClientValidationRule();

            rule.ValidationType = "cep";
            rule.ErrorMessage = this.FormatErrorMessage(metadata.GetDisplayName());
            rule.ValidationParameters.Add("punctuation", punctuationName);

            yield return rule;
        }
    }
}
