# Payment API

## Summary
Sample Payment API implementation to demonstrate basic payment operations using REST APIs and Domain Driven Design principles

## Assumptions
- Authentication is not within the scope of this sample. This could be implemented using token-based authentication patterns.
- An account key will be provided in the request headers to identify the relevant account to use for payments. The solution does not consider the relationships between users and their accounts.

## Pre-requisites
The solution requires .net core 3.1

## Install
Clone the repository and launch using Visual Studio or via the command prompt with:

``` bash
dotnet run --project ./Payment.API/Payment.API.csproj
```

## Usage
View the swagger UI by browsing to the Payment.API site. eg. http://localhost:5000/swagger

The solution uses an in-memory database which is seeded with a default account.
The following account key must be added to the header of all requests.

**ACCOUNT_KEY: 6339d07a-430e-4029-a35c-13e815bcfab4**

Requests can be issued using Swagger UI or via the command line:

For example:
``` bash
curl -X GET "http://localhost:5000/paymentapi/payments" -H "accept: application/json" -H "ACCOUNT_KEY: 6339d07a-430e-4029-a35c-13e815bcfab4"
```

## Unit tests
The unit tests can be executed using Visual Studio's Test Explorer or via the command line: 

``` bash
dotnet test -v n ./Payment.API.Test
```