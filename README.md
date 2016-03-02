# PaylevenWebAppSharp
PaylevenWebAppSharp is an implementation of the Payleven web API for the .NET framework, which allows you to easily integrate your mobile-friendly website with the Payleven mobile pos.<br />
More info about Payleven offer and pos on https://payleven.com/

[![Build status](https://ci.appveyor.com/api/projects/status/a362pwpp7k3onyah?svg=true)](https://ci.appveyor.com/project/petrhaus/paylevenwebappsharp)

### Demo

After you have installed the Payleven app in a iOS or Android device, just point your mobile browser to http://appspace.cloud/paylevenwebapp to test all the functionalities.

### How do I get started?

Install the nuget package

    PM> Install-Package PaylevenWebAppSharp

Get a reference to the PaylevenWebApp class. The constructor takes a string parameter, which is the token you have been provided by Payleven, used to sign requests and verify response signature.

```csharp
using PaylevenWebAppSharp;
using PaylevenWebAppSharp.Enums;
...

var payleven = new PaylevenWebApp("my_token");
```

#### Requests
A request returns a url which you redirect to, so that the Payleven app opens and you can do one of the operations listed below.
Every request must contain a callback uri where the Payleven app will redirect when done, passing in the result of the operation and other parameters.

#####Payment
Starts a transaction

```csharp
var request = new PaymentRequest
{
    CallbackUri = new Uri("http://domain.example/payleven/callback"),
    DisplayName = "MyStore",
    Currency = Currencies.EUR,
    PriceInCents = 1050,
    Description = "Payment for order on domain.example",
    OrderId = "123456"
};

try
{
    var url = payleven.GetPaymentUrl(request);
            
    Response.Redirect(url);
}
catch (Exception exc)
{
    ...
}
```

#####Refund
Starts a refund

```csharp
var request = new RefundRequest
{
    CallbackUri = new Uri("http://domain.example/payleven/callback"),
    DisplayName = "MyStore",
    OrderId = "123456"
};

try
{
    var url = payleven.GetRefundUrl(request);
            
    Response.Redirect(url);
}
catch (Exception exc)
{
    ...
}
```

#####Transaction details
Show details about a transaction

```csharp
var request = new DetailsRequest
{
    CallbackUri = new Uri("http://domain.example/payleven/callback"),
    DisplayName = "MyStore",
    OrderId = "123456"
};

try
{
    var url = payleven.GetDetailsUrl(request);
            
    Response.Redirect(url);
}
catch (Exception exc)
{
    ...
}
```

#####Payments history
Opens the payments history

```csharp
var request = new PaymentsHistoryRequest
{
    CallbackUri = new Uri("http://domain.example/payleven/callback"),
    DisplayName = "MyStore"
};

try
{
    var url = payleven.GetPaymentsHistoryUrl(request);
            
    Response.Redirect(url);
}
catch (Exception exc)
{
    ...
}
```

#### Response
Verify a response sent by Payleven to the callback uri, as the result of an operation.
You just need to pass a reference to the actual HttpRequest: if the request is invalid or tampered an exception will be thrown, otherwise you'll get back a PaylevenResponse containing the result and an error code, plus a bunch of additional parameters (check the PaylevenResponse class).

```csharp
try
{
    var response = payleven.ValidateResponse(Request);
    
    if (response.Result == Results.PaymentSuccessful)
    {
        ...
    }
    
    ...
}
catch (Exception exc)
{
    ...
}
```
