# hotchocolate-stitching-error

Issue: Error on calling external scheme is not handled correctly and results in the following stacktrace.

```
System.InvalidOperationException: StatusCode cannot be set because the response has already started.
   at Microsoft.AspNetCore.Server.Kestrel.Core.Internal.Http.HttpProtocol.ThrowResponseAlreadyStartedException(String value)
   at Microsoft.AspNetCore.Server.Kestrel.Core.Internal.Http.HttpProtocol.set_StatusCode(Int32 value)
   at Microsoft.AspNetCore.Server.Kestrel.Core.Internal.Http.HttpProtocol.Microsoft.AspNetCore.Http.Features.IHttpResponseFeature.set_StatusCode(Int32 value)
   at Microsoft.AspNetCore.Http.DefaultHttpResponse.set_StatusCode(Int32 value)
   at HotChocolate.AspNetCore.QueryMiddlewareBase.InvokeAsync(HttpContext context)
   at Microsoft.AspNetCore.Server.Kestrel.Core.Internal.Http.HttpProtocol.ProcessRequests[TContext](IHttpApplication`1 application)
```

How to reproduce:
- Open in VS2019
- Hit F5
- Run query: 

```graphql
{
  one
}
```

What happens:
- The stacktrace is shown on the server debug output.
- The http response contains invalid json.

What was expected:
A valid response with an error because 'one' can't be null.