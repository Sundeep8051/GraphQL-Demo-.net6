using CommanderGQL.Data;
using CommanderGQL.GraphQL;
using Microsoft.EntityFrameworkCore;
using GraphQL.Server.Ui.Voyager;
using CommanderGQL.GraphQL.Platforms;
using CommanderGQL.GraphQL.Commands;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddPooledDbContextFactory<AppDbContext>(opt => opt.UseSqlServer(
            builder.Configuration.GetConnectionString("CommandConStr")));

        builder.Services.AddGraphQLServer()
            .AddQueryType<Query>()
            .AddMutationType<Mutation>()
            .AddSubscriptionType<Subscription>()
            .AddType<PlatformType>().AddType<CommandType>()
            .AddFiltering().AddSorting()
            .AddInMemorySubscriptions();

        var app = builder.Build();

        app.UseWebSockets();
        app.UseRouting();
        //app.MapGet("/", () => "Hello World!");
        app.UseEndpoints(endpoints => {
            endpoints.MapGraphQL();
        });

        app.UseGraphQLVoyager(new GraphQLVoyagerOptions()
            {
                GraphQLEndPoint = "/graphql",
                Path = "/graphql-voyager"
            });
        app.Run();
    }
}