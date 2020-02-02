# Moogie.Http

> Syntactic sugar in a single file for the `System.Net.Http.HttpClient` class.

## Introduction

Moogie.Http is a single file fluent interface for the `System.Net.Http.HttpClient` class. This means that you can simply
drop the `MoogieHttpRequest.cs` file into your project and begin making http requests easily and beautifully.

I built this for one reason: to stop repeating myself whenever I'm working on a library or project where one of the
requirements is to keep external dependencies to a minimum, or where a fully featured request library such as
`Flurl.Http` feels a little overkill.

If you have no qualms about using external dependencies in your project, we provide an official NuGet package too.

Moogie.Http is under active development, so I wouldn't use it just yet!
