#Maoli.Mvc

Maoli.Mvc is ASP.NET MVC helper library for common brazilian business rules (CPF, CNPJ and CEP),
compatible with .NET Framework 4.0 and above.

Currently implements:

* CPF validation (both client and server-side)
* CNPJ validation (both client and server-side)

For client-side validation of CPF and CNPJ, please see [Maoli.js](https://github.com/aueda/maoli.js/).

For server-side validation of CPF and CNPJ, please see [Maoli](https://github.com/aueda/maoli/).

## Documentation

### Cep

``Cep`` - this attribute checks if a string value is a valid CEP representation.

```c#
	public class CepModel
    {
        [Cep("CEP is invalid")]
        public string Cep { get; set; }
    }
```

### Cpf

``Cpf`` - this attribute checks if a string value is a valid CPF representation.

```c#
	public class CpfModel
    {
        [Cpf("CPF is invalid")]
        public string Cpf { get; set; }
    }
```

``PunctuatedCpf`` - this attribute checks if a string value is a valid CPF representation. Using this validation is necessary to include the punctuation.

```c#
	public class PunctuatedCpfModel
    {
        [PunctuatedCpf("CPF is invalid or missing punctuation")]
        public string Cpf { get; set; }
    }
```

### Cnpj

``Cnpj`` - this attribute  checks if a string value is a valid CNPJ representation.

```c#
	public class CnpjModel
    {
        [Cnpj("CNPJ is invalid")]
        public string Cnpj { get; set; }
    }
```

``PunctuatedCnpj`` - this attribute checks if a string value is a valid CNPJ representation. Using this validation is necessary to include the punctuation.

```c#
	public class PunctuatedCnpjModel
    {
        [PunctuatedCnpj("CNPJ is invalid or missing punctuation")]
        public string Cnpj { get; set; }
    }
```

## Client-side Validation

First, include [jQuery](http://www.jquery.com/) and [jQuery Validation](http://jqueryvalidation.org/) scripts:

```html
    <script src="jquery-2.0.3.js"></script>
    <script src="jquery.validate.js"></script>
    <script src="jquery.validate.unobtrusive.js"></script>

```

Next, include Maoli scripts:

```html
    <script src="jquery-2.0.3.js"></script>
    <script src="jquery.validate.js"></script>
    <script src="jquery.validate.unobtrusive.js"></script>

    <script src="maoli.js"></script>
    <script src="jquery.validate.maoli.js"></script>
```

## NuGet Package

To install Maoli using NuGet, run the following command in the Package Manager Console:

```
PM> Install-Package Maoli.Mvc
```

### NuGet Dependencies

The NuGet package depends on these packages:

   * [jQuery.Validation](https://www.nuget.org/packages/jQuery.Validation/)
   * [Maoli](https://www.nuget.org/aueda/maoli/)
   * [Maoli.js](https://www.nuget.org/aueda/maoli.js/)

NuGet automatically resolves these dependencies.

## Source Code

Source code is available at [GitHub](https://github.com/aueda/maoli.mvc/).

## License

This project is licensed under the [MIT License](http://opensource.org/licenses/MIT).

## Author

Adriano Ueda [@adriueda](https://twitter.com/adriueda)