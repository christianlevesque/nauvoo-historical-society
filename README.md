# Full-stack .NET 6 web project

This project represents a ready-to-use, full-stack .NET 6 project. It comes complete with an ASP.NET REST API featuring JWT authentication, user login, and email notifications through SendGrid, as well as a Blazor frontend based on the MudBlazor component library. The project is licensed under the CC0 license, so you can feel free to use it in pretty much any way you want without attribution or other licensing restrictions. 

## Architecture

The app makes several opinionated decisions in regards to architecture. Sometimes these are based on my understanding of a best practice; other times, these are based on a tradeoff between a best practice and a practical consideration.

### Namespace structure

In general, namespaces are used to organize *features*, not *similar classes*. This makes it easier for me to find things, but it also results in a namespace structure that is different from most other projects. For example, the `Services.Email` namespace includes both service classes, such as `EmailSenderService`, and options classes, such as `EmailOptions`. Many other apps would have a `Services.Options` namespace, or perhaps a `Services.Email.Options` namespace, but I personally feel like organizing by feature instead of by function leads to an easy-to-navigate codebase.

### Interface placed in same file as implementation in many cases

Again, navigating the codebase more easily is a consideration for some of my design decisions. To promote unit testing, I prefer to use interfaces over virtual methods. This leads to several interfaces with only one implementation. Instead of creating a separate file for an interface that is only implemented once, I decided to create the interface at the bottom of the implementation class file. This is primarily used for classes that superficially extend a base interface. For example, `IStateRepository` extends `IRepository` but adds no additional methods. The only reason `IStateRepository` exists is to simplify asking for the repository for the `State` entity, because the repository interface definition has two generic types, `IRepository<TEntity, TKey>`, which don't change. Since `IStateRepository` is superficial, its definition is located in the same file as `StateRepository`.

Non-superficial interfaces are contained in their own file. For example, the `IRepository` interface is relatively large and is meant to have multiple inheritors, so its definition is in its own file at `~/Repositories/IRepository.cs`

### Autowired dependency injection

Any app that grows beyond basic functionality is going to include dozens of injected dependencies. This can clutter your `Program.cs/Startup.cs` file. To combat this and keep things cleaner, I use the [Autoinjector](https://www.nuget.org/packages/Autoinjector/) Nuget package.

## Application design



## Deviations from best practices

Sometimes you need to balance a practical concern against best practices. This template is no different, so there are times when I choose to do something the wrong way for the right reasons.

### EntityFramework query results are buffered, not streamed

The [EF Core docs](https://docs.microsoft.com/en-us/ef/core/performance/efficient-querying#buffering-and-streaming) recommend streaming results for performance reasons. However, streaming results leads to an open connection to the database being maintained. This means that if you try to run another query, you may encounter a runtime exception.

The simplest way to avoid this problem is to buffer results in memory. This is generally discouraged, but `IRepository` includes two methods (`IRepository.GetPagified()` and `IRepository.Find()`) that support paging results, so even though your query will be buffered, you should ideally limit your results so the overall impact of buffering is minimal.