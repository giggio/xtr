# Xtr -  A tool to crawl and parse web pages

This tool will read html from a page or a string
and show you the links.

To use this you have to have at least .NET Core 2.1,
which has global tools.

To download it, go [here](https://www.microsoft.com/net/download)
and install **the .NET Core SDK**. Don't install the
runtime, as it will not be enough to run global tools.

## Synopsis

```bash
xtr <url>
```

For example:

```bash
xtr https://www.bing.com/search?q=lambda3
```

Sample output:

````
Links:
Lambda3,https://www.lambda3.com.br/
Os Lambdasequipe,https://www.lambda3.com.br/pessoas
(...)
````


## Install


```bash
dotnet tool install -g xtr
```

### Parameters

This tool accepts parameters. You call it like so:

```bash
  xtr [options] -
  xtr [options] <urlorcontent>
```

Run `xtr` with `--help`  to see possible options. Here we document a few:

* `--output=<file>` - File path where to save the output, if absent output is sent to stdout
* `--use-browser` - Use a browser to fetch the contents [default: false]
* `--include-empty-links` - Include links without href or value [default: false]
* `--include-hash-links` - Include links with # on href [default: false]
* `--include-js-links` - Include links with 'javascript:' on  href [default: false]

## Testing install during development

Just cd to `src/xtr` and run `.\pack.ps1` or `dotnet pack -C Release -o ../nupkg`.

Then cd to `src/nupkg` and run `dotnet install tool -g xtr`.

## Maintainers/Core team

* [Giovanni Bassi](http://blog.lambda3.com.br/L3/giovannibassi/), aka Giggio, [Lambda3](http://www.lambda3.com.br), [@giovannibassi](https://twitter.com/giovannibassi)

Contributors can be found at the [contributors](https://github.com/giggio/xtr/graphs/contributors) page on Github.

## Contact

Use Twitter.

## License

This software is open source, licensed under the Apache License, Version 2.0.
See [LICENSE.txt](https://github.com/giggio/xtr/blob/master/LICENSE.txt) for details.
Check out the terms of the license before you contribute, fork, copy or do anything
with the code. If you decide to contribute you agree to grant copyright of all your contribution to this project, and agree to
mention clearly if do not agree to these terms. Your work will be licensed with the project at Apache V2, along the rest of the code.
