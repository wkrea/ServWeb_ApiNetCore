> * https://www.c-sharpcorner.com/article/crud-operations-using-asp-net-core-2-0-and-in-memory-database-with-entity-framew/
> * https://dev.to/dileno/build-a-simple-crud-app-with-angular-8-and-asp-net-core-2-2-part-1-back-end-39e1
> * https://www.c-sharpcorner.com/article/crud-operations-using-asp-net-core-2-0-and-in-memory-database-with-entity-framew/
> * https://www.c-sharpcorner.com/article/how-to-use-postman-with-asp-net-core-web-api-testing/



> ## Extensiones VsCode Utiles 
> * **.NET Core Extension Pack**

# Desarrollo de una App por capas

Partiendo desde la rama master se ha creado la rama actual `BlogPostApp`.

Aqui se presenta el diseño de una app modular dividida en 3 capas, con la tecnología NetCore v2.2.207.

Para el desarrollo se ha decidido emplear MsSqlServer, bajo el enfoque de una base de datos construida y controlada desde el código de la aplicación; esto es posible gracias al uso del Framework EF Core.

## EF core y la Base de datos

Si bien ha conocido Entity Framework para el manejo de persistencia en proyectos con tecnología NetFramework algunos conceptos le pueden ser familiares, sin embargo, hay que dejar en claridad que Entitty Framework 6 y versiones anteriores, difieren mucho de EF Core, pues este ha sido construido para trabajar bajo la filosofía de .Net Core que a futuro se conocerá como .Net 5.0.

Considerando que ya se tiene un proyecto `webapi` creado con VsCode, se procederá a construir las clases o modelos que describen el contexto (entidades o tablas) de la aplicación. 

### Modelos

Creando una carpeta Modelos en la Raiz del proyecto, cree dentro de esta una clase con el nombre `BlogPost.cs`, con el siguiente contenido:
```csharp
public class BlogPost
{
	public int PostId { get; set; }
	public string Author { get; set; }
	public string Titulo { get; set; }
	public string Contenido { get; set; }
	public DateTime Fecha { get; set; }
}
```
por lo pronto, podemos ver que los atributos de la clase definen la información base para guardar información de un Post de Blog Web.

Para poder hacer uso de esta clase como base para la descripción de la base de datos, se requiere de la instalación del Nugget [EntityFrameworkCore](https://docs.microsoft.com/en-us/ef/core/get-started/install/).

Para instalar **EntityFrameworkCore** desde la consola de VsCode y ubicado dentro de la carpeta del proyecto, debe ejecutarse:
```bash
dotnet add package Microsoft.EntityFrameworkCore.Sqlite
```

> **NOTA**
> * Recuerde que la webapi que está creada tiene como base NetCore v2.2.207, por ende debe instalarse una versión compatible con ella. El comando anterior está en capacidad de instalar la mejor versión compatible con el proyecto.
> * Si desea instalar una versión especifica de Entity Framework, puede emplear un parámetro adicional para indicar la versión, por ejemplo para instalar la versión 3.1.0 de EF Core, debería usar, **`dotnet add package Microsoft.EntityFrameworkCore.Sqlite -v 3.1.0`**
> * Si desea instalar una versión de EF Core para interactuar con una base MsSqlServer, debería usar:
    * En **Terminal de VsCode**: 
    ```
    dotnet add package Microsoft.EntityFrameworkCore.SqlServer -v 2.2.0
    ```
    * En **package Manager - Visual Studio**: 
    ```
    Install-Package Microsoft.EntityFrameworkCore.SqlServer -v 2.2.0
    ```

#### DataAnnotations

Esta funcionalidad de los frameworks .Net, no es más que una implementación del patrón decorador, que permite extender las funcionalidades de una clase o métodos de la clase.

En este momento pueden ser de utilidad para definir las caracterisiticas de la base a partir de los atributos de la clase `BlogPost.cs` creada anteriormente. 

Por ejemplo: 

* Se puede definir que el campo `PostId` sea la llave primaria de la tabla, empleando el DataAnnotation **[Key]**. 

    > ⚠ ⚙ Si este valor no estuviera definido, pero el nombre del atribuito contiene el prefijo o sufijo **Id**, Entity Framework lo definirá automáticamente como llave de la tabla, en caso de usar esta clase como base para la migración hacia la base de datos MsSqlServer.

* La DataAnnotation **[Required]**, permite indicar que un campo sea requerido para poder ser almacenado en la base. Esto puede ser muy util para efectos de validación desde el FrontEnd de la aplicación o incluso para efectos de validación de lado del BackEnd, cuando los datos son recibidos a través de un endopoint de la WebApi.

    > **Nota** Habrá notado algo importante en lo mencionado anteriormente, que quizás le parezca poco familiar, extraño o exagerado. Si, el considerar que el BackEnd realice validación de los datos a ser procesados o almacenados de lado de la base, pareciera ser un trabajo de más aparte de la lógica de negocio involucrada con la aplicación.

    > Sin embargo esto puede ser muy útil en algunos casos de aplicación, pues: 
    > * Garantiza que la información se encuentre en el formato apropiado o rangos validos antes de ser procesada, por un segmento de código mucho más lento, demorado en procesamiento o incluso **más delicado** para efecto de **la integridad de los datos** que maneja la aplicación.
    > * Permite tener un mecánismo del lado del BackEnd que sirve como guía para la validación de la información al FrontEnd; es decir, las condiciones definidas en los *DataAnnotations* sirven como referencia para los mensajes emitidos en la interfáz de usuario de la aplicación.
    > * Garantiza que la lógica de validación está distribuida y tiene coherencia, pues al estar definida en el BackEnd y servir como referencia para la integración con tecnologías de FrontEnd; los datos que sean admitidos por las validaciones de lado del FrontEnd son coherentes con el BackEnd, o incluso **si alguna validación escapa del FrontEnd, será controlada antes de ir a la base**. Esto da una caracteristica de robustes y mayor nivel de responsabilidad con respecto a **la integridad de los datos por parte de la aplicación**.

Ajustando la clase `BlogPost.cs`:

```csharp
public class BlogPost
    {
        [Key]
        public int PostId { get; set; }
        
        [Required]
        public string Autor { get; set; }
        
        [Required]
        [DataType(DataType.Text)]  
        [Required(ErrorMessage = "Por favor suministre un Titulo"), MaxLength(30)]  
        [Display(Name = "Titulo")]  
        public string Titulo { get; set; }
        
        [Required]
        public string Contenido { get; set; }
        
        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime Fecha { get; set; }
    }
```

Para mas información acerca de DataAnnotations puede consultar:
* [Validando sin parar. Uso de DataAnnotations](https://geeks.ms/jorge/2012/04/26/validando-sin-parar-uso-de-dataannotations/)
* [Model Validation Using Data Annotations In ASP.NET MVC](https://www.c-sharpcorner.com/article/model-validation-using-data-annotations-in-asp-net-mvc/)


### Definir la clase Contexto

Una vez se ha creado un modelo para poder definir la estructura de una tabla en la base mediante el enfoque de [CodeFirst en Entity Framework](https://entityframeworkcore.com/approach-code-first), hará falta asociar dicha clase o "tabla" a un contexto ([DbContext](https://entityframework.net/what-is-dbcontext)).

Se denomina **contexto** a una interfaz de software dentro del framework que permite estrablecer un puente o equivalencia entre los modelos de dominio (**de la aplicación**) y los modelos de persistencia (**en la base de datos**). Es decir, en palabras castisas; una clase intermediaría que permite definir la estructura de la base de datos a partir del código fuente de la aplicación, para poder establecer migraciones y control sobre el estado de la base de datos a medida que la aplicación crece.

Dentro de la carpeta **Modelos** se crea una nueva clase con el nombre `ApiContext.cs` con el siguiente contenido:

```csharp
public class ApiContext : DbContext
{
    public ApiContext (DbContextOptions<ApiContext> options)
        : base(options)
    {
    }

    public DbSet<BlogPost> BlogPosts { get; set; }
}
```
Esta clase tiene algunos aspectos importantes a mencionar y aclarar:


* La clase que define el contexto de la aplicación **en Entity Framework siempre deberá derivar de una clase denominada `DbContext`** que proviene del nucleo de EntifyFramework.
* El constructor de la clase tiene como parámetros **`DbContextOptions<ApiContext> options`**, este parámetro tipado es necesario para poder heredar todas las propiedades de la clase **`DbContext`** de EntityFramework.
* La sentencia **`base(options)`** pasa los parámetros anteriores, al constructor de la clase **`DbContext`** de EntityFramework, para poder generar una instancia con capacidad de gestionar una base de datos desde el código de la aplicación para crear tablas y migraciones.
* Finalmente, los sentencias **`DbSet<T>`** que actuan como propiedades de la clase  `ApiContext`, "emulan" el comportamiento de una tabla de la base a la cual se pueden hacer consultas y modificaciones, como si se tratará de un atributo común y corriente en una clase (por esta razón los **`get; set; `**)


### Configurar migraciones y crear la base de datos

Ahora que tenemos `ApiContext` y comprendemos un poco sobre como utilizar el enfoque de **`CodeFirst de  Entity Framework Core`**, es hora de configurar las migraciones. 

#### Base de datos en memoria
```csharp
public void ConfigureServices(IServiceCollection services)
{
    services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

    // CONFIGURACION DE SQLITE IN-MEMORY
    services.AddDbContext<ApplicationDbContext>(
        context => { 
            context.UseInMemoryDatabase("ConferencePlanner"); 
            }
        );  
}
```

> ⚠ ⚙ **NOTA**: Las base de datos en memoria como, **no cuentan con la capacidad para manejar relaciones entre tablas**, para ello se requiere de un motor de base de datos más estructuradop y completo como SqlServer.

Si desea ver ese tipo de escenario y configuración, puede encontrar un ejemplo en [Configurar migraciones y crear la base de datos](https://dev.to/dileno/build-a-simple-crud-app-with-angular-8-and-asp-net-core-2-2-part-1-back-end-39e1#setup-migrations-and-create-the-database).


Para mas información acerca de los conceptos detrás de **`DbContext`** y **`EntityFramework Migrations`** puede consultar:
* [Llamando al constructor de la clase base](https://riptutorial.com/es/csharp/example/64/llamando-al-constructor-de-la-clase-base)
* [Configurar migraciones y crear la base de datos](https://dev.to/dileno/build-a-simple-crud-app-with-angular-8-and-asp-net-core-2-2-part-1-back-end-39e1#setup-migrations-and-create-the-database)
* [Entity Framework Core y SqLite in-memory en ASP.NET Core](http://www.rafaelacosta.net/Blog/2019/4/3/entity-framework-core-y-sqlite-in-memory-en-aspnet-core)



## Base de datos

## Base de datos


