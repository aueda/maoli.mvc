﻿// ==ClosureCompiler==
// @output_file_name jquery.validate.maoli.min.js
// @compilation_level SIMPLE_OPTIMIZATIONS
// ==/ClosureCompiler==

(function (window) {

    "use strict";

    var maoli = window.Maoli,
        $ = window.jQuery,
        validator = null;

    if (maoli === "undefined" || $ === "undefined" || $.validator === "undefined") {
        return;
    }

    validator = $.validator;

    validator.addMethod("cnpj_validator", function (value, element) {

        var punctuation = $(element).data("val-cnpj-punctuation") || "loose";

        return maoli.Cnpj.validate(value, punctuation);
    });

    validator.addMethod("cpf_validator", function (value, element) {

        var punctuation = $(element).data("val-cpf-punctuation") || "loose";

        return maoli.Cpf.validate(value, punctuation);
    });

    validator.unobtrusive.adapters.addBool('cnpj', 'cnpj_validator');

    validator.unobtrusive.adapters.addBool('cpf', 'cpf_validator');

}(this));