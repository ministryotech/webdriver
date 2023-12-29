# WebDriver.Extensions #
This project is set up to provide a basic styled framework to build a page structured model for writing UI Automation tests against in dotnet core or dotnet classic. The attached Google libraries act as samples to create a simple model to run tests against using the framework code in the main libraries. You can use these as a starting point.

# Developers - Getting Started #
In order to make use of this solution you must have the following installed...
- [Visual Studio 2017](https://www.visualstudio.com/downloads/)
- [git](https://git-scm.com/downloads)
- [SDKs and Visual Studio Tools for dotnet Core](https://www.microsoft.com/net/download/core) (While the main library will compile for dotnet classic 4.5 and 4.62 as well, the sample code and tests require dotnet core 1.1)
- IE, Edge (on Windows 10 Creators or above), Firefox and Chrome for all of the tests to pass

## I have VS 2015 will that do? ##
If you have Visual Studio 2015 Installed you will have to upgrade to support the *.csproj format.

## Additional recommendations ##
I recommend [Sourcetree](https://www.sourcetreeapp.com/) to supplement command line and Visual Studio for its visualisation.

# Build and Test #
Once you have cloned this repo and checked out the branch that has been created for you run the following commands in the solution root to download dependencies...

As yourself...
- dotnet restore

You can run tests in Visual Studio 2017 or by using 'dotnet test' on the command line.

Ensure that 'Enable Protected Mode' is turned off in ALL zones in IE.

You will need to make sure that your UI Tests library includes references to any of the Driver packages that you intend to use from nuget to ensure they are added to the output directories...
- PhantomJS: **NO LONGER SUPPORTED**
- Chrome: Selenium.WebDriver.ChromeDriver v120.0.6099.10900
- IE: Selenium.WebDriver.IEDriver v4.14.0 (On modern setups this can be tricky - Scaling will set the Browser zoom level above 100% which will cause failures. The IE driver is very flaky.)
- Firefox: Selenium.WebDriver.GeckoDriver v0.33.0
- Edge: Selenium.WebDriver.MicrosoftWebDriver v10.0.17134 (This is an annoying one as it needs to match the build of Windows 10 used to test with or it will just fail)

# Useful Links #
The following links relate to all of the key elements within the project technology stack...
- [dotnet Core](https://www.microsoft.com/net/core)
- [dotnet Core Downloads](https://www.microsoft.com/net/download/core)
- [Visual Studio Community](https://www.visualstudio.com/downloads/)
- [XUnit](http://xunit.github.io/docs/getting-started-dotnet-core)

## Upgrading v3.x to v4.x
v4 no longer supports older .net Framework versions, except via .netStandard 2.0.
v4 no longer supports Phantom JS, due to lack of support in the underlying Selenium code which has been updated.

## Tests & Samples
A lot of the tests in the code repo don't run anymore (The nature of a changing web!) If anyone wants to get these running again that would be appreciated.
This is a big project and could do with some collaborators.

## The Ministry of Technology Open Source Products
Welcome to The Ministry of Technology open source products. All open source Ministry of Technology products are distributed under the MIT License for maximum re-usability.
Our other open source repositories can be found here...

* [https://github.com/ministryotech](https://github.com/ministryotech)
* [https://github.com/tiefling](https://github.com/tiefling)

### Where can I get it? ###
You can download the package for this project from any of the following package managers...

- **NUGET** - [https://www.nuget.org/packages/WebDriver.Extensions](https://www.nuget.org/packages/WebDriver.Extensions)

### Contribution guidelines ###
If you would like to contribute to the project, please contact me.

### Who do I talk to? ###
* Keith Jackson - temporal-net@live.co.uk
