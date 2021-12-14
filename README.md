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



Sources: 

- https://docs.microsoft.com/en-us/aspnet/core/tutorials/min-web-api?view=aspnetcore-6.0&tabs=visual-studio
- https://medium.com/@gerhardmaree/quickly-create-a-net-6-minimal-api-with-swagger-documentation-720d88db79fb