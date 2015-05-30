namespace Maoli.Mvc.Tests
{
    using System;
    using System.Web.Mvc;
    using Maoli;
    using Maoli.Mvc;
    using Moq;
    using Xunit;

    public class CpfAttributeTest
    {
        [Fact]
        public void IsValidReturnsTrueIfCpfIsValidAndLoose()
        {
            var attr = new CpfAttribute();

            var actual = attr.IsValid("71402565860");

            Assert.True(actual);
        }

        [Fact]
        public void IsValidReturnsTrueIfCpfIsValidAndStrict()
        {
            var attr = new CpfAttribute("The CPF is invalid");

            var actual = attr.IsValid("714.025.658-60");

            Assert.True(actual);
        }

        [Fact]
        public void IsValidReturnsFalseIfCpfIsInvalidAndStrict()
        {
            var attr = new CpfAttribute("The CPF is invalid");

            var actual = attr.IsValid("825.136.769-32");

            Assert.False(actual);
        }

        [Fact]
        public void IsValidReturnsFalseIfCpfIsInvalidAndLoose()
        {
            var attr = new CpfAttribute("The CPF is invalid");

            var actual = attr.IsValid("82513676932");

            Assert.False(actual);
        }

        [Fact]
        public void ErrorMessageReturnsDefaultErrorMessage()
        {
            var expected = "O CPF é inválido";

            var attr = new CpfAttribute();

            var actual = attr.FormatErrorMessage(string.Empty);

            Assert.Equal<string>(expected, actual);
        }

        [Fact]
        public void ErrorMessageReturnsTheUserDefinedErrorMessage()
        {
            var attr = new CpfAttribute("Error Message");

            var expected = "Error Message";

            var actual = attr.FormatErrorMessage(string.Empty);

            Assert.Equal<string>(expected, actual);
        }

        [Fact]
        public void GetClientValidationRulesReturnsValidationTypeConsistingOnlyOfLowercaseLetters()
        {
            Mock<ModelMetadataProvider> provider = new Mock<ModelMetadataProvider>();
            Mock<ModelMetadata> metadata = new Mock<ModelMetadata>(provider.Object, null, null, typeof(string), null);

            var attr = new CpfAttribute();
            var expected = "cpf";

            foreach (var validationRule in attr.GetClientValidationRules(metadata.Object, null))
            {
                Assert.Equal<string>(expected, validationRule.ValidationType);
            }
        }
    }
}
