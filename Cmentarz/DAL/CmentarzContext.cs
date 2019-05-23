using Cmentarz.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Cmentarz.DAL
{
    public class CmentarzContext : IdentityDbContext<ApplicationUser>
    {
        public CmentarzContext() : base("CmentarzContext")
        {
    
        }

        public static CmentarzContext Create()
        {
            return new CmentarzContext();
        }

        public DbSet<Sektor> sektor { get; set; }
        public DbSet<Rzad> rzad { get; set; }
        public DbSet<Kwatera> kwatera { get; set; }
        public DbSet<Osoba> osoba { get; set; }
        
        
        
    }
}