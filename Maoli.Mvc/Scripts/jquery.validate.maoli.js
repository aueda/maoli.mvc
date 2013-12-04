// ==ClosureCompiler==
// @output_file_name jquery.validate.maoli.min.js
// @compilation_level SIMPLE_OPTIMIZATIONS
// ==/ClosureCompiler==

(function (window) {

    "use strict";

    var maoli = window.Maoli,
        $ = window.jQuery,
        validator = null,
		addCepMethod = function () {
		    validator.addMethod("cep_validator", function (value, element) {

		        var punctuation = $(element).data("val-cep-punctuation") || "loose";

		        return maoli.Cep.validate(value, punctuation);
		    });

		    validator.unobtrusive.adapters.addBool('cep', 'cep_validator');
		},
        addCpfMethod = function () {
            validator.addMethod("cpf_validator", function (value, element) {

                var punctuation = $(element).data("val-cpf-punctuation") || "loose",
                    testProperty = $(element).data("val-cpf-testproperty"),
                    testPropertyValue = $(element).data("val-cpf-testpropertyvalue").toString(),
                    isValid = maoli.Cpf.validate(value, punctuation);

				if (value === "") {
					return true;
				}

                if (!!testProperty) {
                    if ($("#" + testProperty).val() !== testPropertyValue) {
                        isValid = true;
                    }
                }

                return isValid;
            });

            validator.unobtrusive.adapters.addBool('cpf', 'cpf_validator');
        },
        addCnpjMethod = function () {
            validator.addMethod("cnpj_validator", function (value, element) {

                var punctuation = $(element).data("val-cnpj-punctuation") || "loose",
                    testProperty = $(element).data("val-cnpj-testproperty"),
                    testPropertyValue = $(element).data("val-cnpj-testpropertyvalue").toString(),
                    isValid = maoli.Cnpj.validate(value, punctuation);

				if (value === "") {
					return true;
				}

                if (!!testProperty) {
                    if ($("#" + testProperty).val() !== testPropertyValue) {
                        isValid = true;
                    }
                }

                return isValid;
            });

            validator.unobtrusive.adapters.addBool('cnpj', 'cnpj_validator');
        };

    if (maoli === "undefined" || $ === "undefined" || $.validator === "undefined") {
        return;
    }

    validator = $.validator;

    addCepMethod();

    addCpfMethod();

    addCnpjMethod();

}(this));