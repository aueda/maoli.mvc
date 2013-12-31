namespace Maoli.Mvc.Tests
{
    using System;
    using System.Text.RegularExpressions;
    using System.Web.Mvc;
    using Maoli;
    using Maoli.Mvc;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    [TestClass]
    public class CepAttributeTest
    {
        [TestMethod]
        public void IsValidReturnsTrueIfCepIsValidAndLoose()
        {
            var attr = new CepAttribute();

            var actual = attr.IsValid("12345678");

            Assert.IsTrue(actual);
        }

        [TestMethod]
        public void IsValidReturnsFalseIfCepIsInvalidAndLoose()
        {
            var attr = new CepAttribute();

            var actual = attr.IsValid("123456-78");

            Assert.IsFalse(actual);
        }

        [TestMethod]
        public void IsValidReturnsTrueIfCepIsValidAndStrict()
        {
            var attr = new CepAttribute();

            var actual = attr.IsValid("12345-678");

            Assert.IsTrue(actual);
        }

        [TestMethod]
        public void IsValidReturnsFalseIfCepIsInvalidAndStrict()
        {
            var attr = new CepAttribute();

            var actual = attr.IsValid("12345-67a");

            Assert.IsFalse(actual);
        }

        [TestMethod]
        public void ErrorMessageReturnsDefaultErrorMessage()
        {
            var expected = "O CEP é inválido";

            var attr = new CepAttribute();

            var actual = attr.FormatErrorMessage(string.Empty);

            Assert.AreEqual<string>(expected, actual);
        }

        [TestMethod]
        public void ErrorMessageReturnsTheUserDefinedErrorMessage()
        {
            var attr = new CepAttribute("Error Message");

            var expected = "Error Message";

            var actual = attr.FormatErrorMessage(string.Empty);

            Assert.AreEqual<string>(expected, actual);
        }

        [TestMethod]
        public void GetClientValidationRulesReturnsValidationTypeConsistingOnlyOfLowercaseLetters()
        {
            Mock<ModelMetadataProvider> provider = new Mock<ModelMetadataProvider>();
            Mock<ModelMetadata> metadata = new Mock<ModelMetadata>(provider.Object, null, null, typeof(string), null);

            var attr = new CepAttribute();
            var expected = "cep";

            foreach (var validationRule in attr.GetClientValidationRules(metadata.Object, null))
            {
                Assert.AreEqual<string>(expected, validationRule.ValidationType);
            }
        }
    }
}
