﻿namespace Maoli.Mvc.Tests
{
    using System;
    using System.Web.Mvc;
    using Maoli;
    using Maoli.Mvc;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    [TestClass]
    public class PunctuatedCnpjAttributeTest
    {
        private const string looseValidCnpj = "63943315000192";

        private const string looseInvalidCnpj = "32343315/000134";

        private const string strictValidCnpj = "63.943.315/0001-92";

        private const string strictInvalidCnpj = "32.343.315/0001-34";

        [TestMethod]
        public void IsValidReturnsFalseIfCnpjIsValidAndLoose()
        {
            var attr = new PunctuatedCnpjAttribute();

            var actual = attr.IsValid("63943315000192");

            Assert.IsFalse(actual);
        }

        [TestMethod]
        public void IsValidReturnsFalseIfCnpjIsInvalidAndLoose()
        {
            var attr = new PunctuatedCnpjAttribute();

            var actual = attr.IsValid("32343315/000134");

            Assert.IsFalse(actual);
        }

        [TestMethod]
        public void IsValidReturnsTrueIfCnpjIsValidAndStrict()
        {
            var attr = new PunctuatedCnpjAttribute("The CNPJ is invalid");

            var actual = attr.IsValid("63.943.315/0001-92");

            Assert.IsTrue(actual);
        }

        [TestMethod]
        public void IsValidReturnsFalseIfCnpjIsInvalidAndStrict()
        {
            var attr = new PunctuatedCnpjAttribute("The CNPJ is invalid");

            var actual = attr.IsValid(strictInvalidCnpj);

            Assert.IsFalse(actual);
        }

        [TestMethod]
        public void ErrorMessageReturnsDefaultErrorMessage()
        {
            var expected = "O CNPJ é inválido";

            var attr = new PunctuatedCnpjAttribute();

            var actual = attr.FormatErrorMessage(string.Empty);

            Assert.AreEqual<string>(expected, actual);
        }

        [TestMethod]
        public void ErrorMessageReturnsTheUserDefinedErrorMessage()
        {
            var attr = new PunctuatedCnpjAttribute("Error Message");

            var expected = "Error Message";

            var actual = attr.FormatErrorMessage(string.Empty);

            Assert.AreEqual<string>(expected, actual);
        }

        [TestMethod]
        public void GetClientValidationRulesReturnsValidationTypeConsistingOnlyOfLowercaseLetters()
        {
            Mock<ModelMetadataProvider> provider = new Mock<ModelMetadataProvider>();
            Mock<ModelMetadata> metadata = new Mock<ModelMetadata>(provider.Object, null, null, typeof(string), null);

            var attr = new PunctuatedCnpjAttribute();
            var expected = "cnpj";

            foreach (var validationRule in attr.GetClientValidationRules(metadata.Object, null))
            {
                Assert.AreEqual<string>(expected, validationRule.ValidationType);
            }
        }
    }
}
