using PhoneBookData.Models;
using System;
using System.Data.Entity;

namespace PhoneBookData
{
    public class PhoneBookContext : DbContext
    {
        public PhoneBookContext()
            : base("PhoneBookContext")
        {
            this.Configuration.LazyLoadingEnabled = false; // to avoid serialization issues related to lazy loading of POCO entities
            this.Configuration.ProxyCreationEnabled = false; // to support serialization in eager loading of entities

            // Code-first migration
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<PhoneBookContext, PhoneBookMigrationsConfiguration>());

        }

        public virtual DbSet<ContactType> ContactTypes { get; set; }
        public virtual DbSet<Contact> Contacts { get; set; }
    }
}
