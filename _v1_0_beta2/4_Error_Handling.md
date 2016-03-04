---
id: errorhandling
title: Error Handling
layout: default
---

<h3 id="errorHandlerHeader">Handling error</h3>

There are differet forms of exceptions that can occur. Some are more specific than others. All exceptions related to client behavior inherits from `SignatureException`. 

{% highlight csharp %}

try
{
    //Some signature action
}
catch (BrokerNotAuthorizedException notAuthorizedException)
{
    //Not authorized to perform action. The correct access rights for organization are not set.
}
catch (UnexpectedResponseException unexpectedResponseException)
{
    //UnexpectedResponseException will normally contain an `Error` object giving a more detailed error description. If this error does not exist, 
    // you can still get the status code and message.
    var statusCode = unexpectedResponseException.StatusCode;
    var responseMessage = unexpectedResponseException.Message;

    if (unexpectedResponseException.Error != null)
    {
        var errorMessage = unexpectedResponseException.Error.Message;
        var errorType = unexpectedResponseException.Error.Type;
    }
}
catch (SignatureException exception)
{
   
}
{% endhighlight %}