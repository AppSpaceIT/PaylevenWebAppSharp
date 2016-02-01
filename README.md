# PaylevenWebAppSharp
PaylevenWebAppSharp is an implementation of the Payleven web API for the .NET framework.<br />
Easily integrate your mobile-friendly website with the Payleven mobile pos.

[![Build status](https://ci.appveyor.com/api/projects/status/a362pwpp7k3onyah?svg=true)](https://ci.appveyor.com/project/petrhaus/paylevenwebappsharp)

### How do I get started?

Install the nuget package

    PM> Install-Package AutoMapper

#### Requests
A request returns a url which you redirect to, so Payleven app opens and you can do one of the following operations:

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

var payleven = new PaylevenWebApp("my_token");
            
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
[TODO]
```

#####Transaction details
Show details about a transaction

```csharp
[TODO]
```

#####Payments history
Opens the payments history

```csharp
[TODO]
```

#### Response
Verify a response sent by Payleven as the result of an operation

```csharp
[TODO]
```
