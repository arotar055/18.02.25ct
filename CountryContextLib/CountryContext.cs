using System.Collections.Generic;
using System.Diagnostics.Metrics;
using Microsoft.EntityFrameworkCore;
using CountryLib;
using Microsoft.Extensions.Configuration;

namespace CountryContextLib
{
    public class CountryContext : DbContext
    {
        static DbContextOptions<CountryContext> opt;

        static CountryContext()
        {
            var builder = new ConfigurationBuilder();
            builder.SetBasePath(Directory.GetCurrentDirectory());
            builder.AddJsonFile("connection.json");
            var config = builder.Build();
            string connectionString = config.GetConnectionString("DefaultConnection");
            var optionsBuilder = new DbContextOptionsBuilder<CountryContext>();
            opt = optionsBuilder.UseSqlServer(connectionString).Options;
        }

        public CountryContext()
            : base(opt)
        {
            if (Database.EnsureCreated())
            {
                Continent europe = new Continent { Name = "Europe" };
                Continent asia = new Continent { Name = "Asia" };
                Continent southAmerica = new Continent { Name = "South America" };
                Continent africa = new Continent { Name = "Africa" };
                Continent oceania = new Continent { Name = "Oceania" };

                Continents.Add(europe);
                Continents.Add(asia);
                Continents.Add(southAmerica);
                Continents.Add(africa);
                Continents.Add(oceania);

                Countries.Add(new Country { Name = "Spain", Capital = "Madrid", Population = 47000000, Area = 505990, Continent = europe });
                Countries.Add(new Country { Name = "Ukraine", Capital = "Kyiv", Population = 44134693, Area = 603628, Continent = europe });

                Countries.Add(new Country { Name = "India", Capital = "New Delhi", Population = 1366000000, Area = 3287263, Continent = asia });
                Countries.Add(new Country { Name = "Japan", Capital = "Tokyo", Population = 126300000, Area = 377975, Continent = asia });

                Countries.Add(new Country { Name = "Argentina", Capital = "Buenos Aires", Population = 45195774, Area = 2780400, Continent = southAmerica });
                Countries.Add(new Country { Name = "Colombia", Capital = "Bogotá", Population = 50372424, Area = 1141748, Continent = southAmerica });

                Countries.Add(new Country { Name = "Nigeria", Capital = "Abuja", Population = 206139589, Area = 923768, Continent = africa });
                Countries.Add(new Country { Name = "South Africa", Capital = "Pretoria", Population = 59308690, Area = 1221037, Continent = africa });

                Countries.Add(new Country { Name = "New Zealand", Capital = "Wellington", Population = 5084300, Area = 268838, Continent = oceania });
                Countries.Add(new Country { Name = "Fiji", Capital = "Suva", Population = 896444, Area = 18274, Continent = oceania });

                SaveChanges();
            }
        }

        public DbSet<Country> Countries { get; set; }
        public DbSet<Continent> Continents { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
            optionsBuilder.UseLazyLoadingProxies();
        }
    }
}