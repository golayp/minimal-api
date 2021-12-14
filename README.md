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

Installer les packages **Swashbuckle.AspNetCore**.

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

Sources: 

- https://docs.microsoft.com/en-us/aspnet/core/tutorials/min-web-api?view=aspnetcore-6.0&tabs=visual-studio
- https://medium.com/@gerhardmaree/quickly-create-a-net-6-minimal-api-with-swagger-documentation-720d88db79fb