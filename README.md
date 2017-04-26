# DiffApi

ASP.NET Web Application used to determine the differences between the supplied JSON containing base64 encoded data.

### Usage examples (assuming it is hosted locally)

Send JSON containing base64 encoded binary data to the specified ID:
- PUT localhost:port/v1/diff/{id}/left, request body: {data}
- PUT localhost:port/v1/diff/{id}/right, request body: {data}

Fetch JSON containing final result with differences for the specified ID:
- GET /v1/diff/1/{id}

### Notes, assumptions, choices

- using .NET 4.5.2 framework, compiled with Roslyn, VS15
- temporary data storage provided by using Memory Cache with 1 day expiration date






