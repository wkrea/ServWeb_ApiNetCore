using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using api.Modelos;

namespace api.Utils
{
    // https://exceptionnotfound.net/ef-core-inmemory-asp-net-core-store-database/
    public class DataSeed
    {
        public static void InicializarDB(IServiceProvider services)
        {
            // https://andrewlock.net/the-difference-between-getservice-and-getrquiredservice-in-asp-net-core/
            // https://medium.com/volosoft/asp-net-core-dependency-injection-best-practices-tips-tricks-c6e9c67f9d96
            // https://stackoverflow.com/questions/29990618/iserviceprovider-in-asp-net-core
            using (var context = 
                        new ApiContext(
                            services.GetRequiredService<DbContextOptions<ApiContext>>() ))
            {
                if (context.Workshops.Any()) {return;} // no hay datos

                // agregar semillas

                context.Workshops.Add(
                    new Workshop{ 
                        Nombre = "Ejemplo de Api Rest NetCore", 
                        Speaker = "profe William Trigos"
                    });

                context.SaveChanges();
            }
        }
        
    }
    
}