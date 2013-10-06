namespace Maoli.Mvc.Tests
{
    using System;
    using System.Web.Mvc;
    using Maoli;
    using Maoli.Mvc;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    [TestClass]
    public class PunctuatedCpfAttributeTest
    {
        [TestMethod]
        public void IsValidReturnsFalseIfCpfIsValidAndLoose()
        {
            var attr = new PunctuatedCpfAttribute();

            var actual = attr.IsValid("71402565860");

            Assert.IsFalse(actual);
        }

        [TestMethod]
        public void IsValidReturnsFalseIfCpfIsInvalidAndLoose()
        {
            var attr = new PunctuatedCpfAttribute();

            var actual = attr.IsValid("82513676932");

            Assert.IsFalse(actual);
        }

        [TestMethod]
        public void IsValidReturnsTrueIfCpfIsValidAndStrict()
        {
            var attr = new PunctuatedCpfAttribute("The CPF is invalid");

            var actual = attr.IsValid("714.025.658-60");

            Assert.IsTrue(actual);
        }

        [TestMethod]
        public void IsValidReturnsFalseIfCpfIsInvalidAndStrict()
        {
            var attr = new PunctuatedCpfAttribute("The CPF is invalid");

            var actual = attr.IsValid("825.136.769-32");

            Assert.IsFalse(actual);
        }

        [TestMethod]
        public void ErrorMessageReturnsDefaultErrorMessage()
        {
            var expected = "O CPF é inválido";

            var attr = new PunctuatedCpfAttribute();

            var actual = attr.FormatErrorMessage(string.Empty);

            Assert.AreEqual<string>(expected, actual);
        }

        [TestMethod]
        public void ErrorMessageReturnsTheUserDefinedErrorMessage()
        {
            var attr = new PunctuatedCpfAttribute("Error Message");

            var expected = "Error Message";

            var actual = attr.FormatErrorMessage(string.Empty);

            Assert.AreEqual<string>(expected, actual);
        }

        [TestMethod]
        public void GetClientValidationRulesReturnsValidationTypeConsistingOnlyOfLowercaseLetters()
        {
            Mock<ModelMetadataProvider> provider = new Mock<ModelMetadataProvider>();
            Mock<ModelMetadata> metadata = new Mock<ModelMetadata>(provider.Object, null, null, typeof(string), null);

            var attr = new PunctuatedCpfAttribute();
            var expected = "cpf";

            foreach (var validationRule in attr.GetClientValidationRules(metadata.Object, null))
            {
                Assert.AreEqual<string>(expected, validationRule.ValidationType);
            }
        }
    }
}
