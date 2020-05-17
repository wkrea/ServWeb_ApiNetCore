using System;
using Microsoft.EntityFrameworkCore;

using api.Modelos;

namespace api.Modelos {
    public class ApiContext : DbContext {
        public ApiContext (DbContextOptions<ApiContext> options) : base (options) { }
        
        public DbSet<BlogPost> BlogPosts { get; set; }
        public DbSet<Workshop> Workshops { get; set; }

        public void Seed(){
            this.Workshops.Add(
                new Workshop { 
                    Nombre = "Ejemplo de Api Rest NetCore", 
                    Speaker = "profe William Trigos"
                    }
            );
            this.SaveChanges();
        }
    }
}