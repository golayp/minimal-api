# minimal-api

Découverte de la fonction minimal api de .net 6

## Génération de l'api

Lancer dans un répertoire vide pour créer une solution.

```bash
dotnet new sln
```

Lancer ensuite la commande pour créer un projet.

```bash
dotnet new web
```

Dans votre IDE ouvrir la solution et ajouter le projet nouvellement créé.

<u>Optionnel:</u> si vous utiliser git comme gestionnaire de source ajouter un gitignore.

```bash
dotnet new gitignore
```

## Ajout de swagger

Installer le package nuget **Swashbuckle.AspNetCore**.

Ajouter les lignes suivantes dans le fichier **Program.cs**

```c#
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer(); //added line
builder.Services.AddSwaggerGen(); //added line

var app = builder.Build();

app.UseSwagger();//added line

app.MapGet("/", () => "Hello World!");

app.UseSwaggerUI(); //added line

app.Run();
```

Dans le fichier **launchSettings.json** ajouter au profile de démarrage IIS Express:

```json
"launchUrl": "swagger"
```

Lancer le debug de la solution cela devrait ouvrir un navigateur avec la page swagger ou vous pouvez tester la seule méthode exposée qui affiche la string Hello World.

## Ajout d'entity framework

On ajoute  entity framework (ef ci-après).

Installer les packages nuget **Microsoft.EntityFrameworkCore.InMemory** et **Microsoft.AspNetCore.Diagnostics.EntityFrameworkCore** au projet.

Ajouter une classe Todo qui servira à stocker des tâches.

```c#
namespace minimal_api;

public class Todo
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public bool IsComplete { get; set; }
}
```

Ajouter une classe TodoDb qui sert de contexte de base de données.

```c#
using Microsoft.EntityFrameworkCore;

namespace minimal_api;

public class TodoDb: DbContext
{
    public TodoDb(DbContextOptions<TodoDb> options)
        : base(options) { }

    public DbSet<Todo> Todos => Set<Todo>();
    
}
```

Ajouter dans le fichier **Program.cs** les lignes suivantes pour déclarer le contexte de base de données et activer l'affichage des exceptions  liées à la base de données.

```c#
//code above

builder.Services.AddDbContext<TodoDb>(opt => opt.UseInMemoryDatabase("TodoList")); //added line
builder.Services.AddDatabaseDeveloperPageExceptionFilter(); //added line

var app = builder.Build();

//code below
```

## Ajout des méthodes de gestion des todos (tâches)

Dans le fichier **Program.cs** ajouter les chemins pour

- lister les todos *GET /todoitems*
- lister les todos terminés  *GET /todoitems/complete*
- avoir le détail d'un todo à partir de son id *GET /todoitems/{id}*
- ajouter un todo *POST /todoitems*
- mettre à jour un todo à partir de son id *PUT /todoitems/{id}*
- supprimer un todo à partir de son id *DELETE /todoitems/{id}*

```c#
//code above
app.MapGet("/", () => "Hello World!");
//ADDED CONTENT
app.MapGet("/todoitems", async (TodoDb db) =>
    await db.Todos.ToListAsync());

app.MapGet("/todoitems/complete", async (TodoDb db) =>
    await db.Todos.Where(t => t.IsComplete).ToListAsync());

app.MapGet("/todoitems/{id}", async (int id, TodoDb db) =>
    await db.Todos.FindAsync(id)
        is Todo todo
        ? Results.Ok(todo)
        : Results.NotFound());

app.MapPost("/todoitems", async (Todo todo, TodoDb db) =>
{
    db.Todos.Add(todo);
    await db.SaveChangesAsync();

    return Results.Created($"/todoitems/{todo.Id}", todo);
});

app.MapPut("/todoitems/{id}", async (int id, Todo inputTodo, TodoDb db) =>
{
    var todo = await db.Todos.FindAsync(id);

    if (todo is null) return Results.NotFound();

    todo.Name = inputTodo.Name;
    todo.IsComplete = inputTodo.IsComplete;

    await db.SaveChangesAsync();

    return Results.NoContent();
});

app.MapDelete("/todoitems/{id}", async (int id, TodoDb db) =>
{
    if (await db.Todos.FindAsync(id) is Todo todo)
    {
        db.Todos.Remove(todo);
        await db.SaveChangesAsync();
        return Results.Ok(todo);
    }

    return Results.NotFound();
});
//END ADDED CONTENT
app.UseSwaggerUI();

app.Run();
```

Sources: 

- https://docs.microsoft.com/en-us/aspnet/core/tutorials/min-web-api?view=aspnetcore-6.0&tabs=visual-studio
- https://medium.com/@gerhardmaree/quickly-create-a-net-6-minimal-api-with-swagger-documentation-720d88db79fb