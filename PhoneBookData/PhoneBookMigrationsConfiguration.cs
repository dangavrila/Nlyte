using System;
using System.Data.Entity;
using System.Data.Entity.Migrations;

namespace PhoneBookData
{
    public class PhoneBookMigrationsConfiguration: DbMigrationsConfiguration<PhoneBookContext>
    {
        public PhoneBookMigrationsConfiguration()
        {
            this.AutomaticMigrationDataLossAllowed = true;
            this.AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(PhoneBookContext context)
        {
            base.Seed(context);
        }
    }
}
