using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using StationApplication.Entity.Entities;
using System.Collections.Generic;

namespace StationApplication.Data
{
    public class SeedData
    {
        IRepository<Station> _StationRepository;
        UserManager<User> _UserManager;
        IPasswordHasher<User> _PasswordHasher;
        public SeedData(IRepository<Station> StationRepository,
            UserManager<User> UserManager, 
            IPasswordHasher<User> PasswordHasher)
        {
            _StationRepository = StationRepository;
            _UserManager = UserManager;
            _PasswordHasher = PasswordHasher;
        }
        public async static void Seed(IApplicationBuilder app)
        {
            ApplicationDbContext context = app.ApplicationServices.GetRequiredService<ApplicationDbContext>();
            context.Database.Migrate();
            IRepository<Station> StationRepository = app.ApplicationServices.GetRequiredService<IRepository<Station>>();
            IRepository<SmartTicketType> SmartTicketTypeRepository = app.ApplicationServices.GetRequiredService<IRepository<SmartTicketType>>();
            UserManager<User> userManager = app.ApplicationServices.GetRequiredService<UserManager<User>>();
            IPasswordHasher<User> passwordHasher = app.ApplicationServices.GetRequiredService<IPasswordHasher<User>>();
            var stationResult = await StationRepository.GetAll().AnyAsync();
            var smartTicketTypeResult = await SmartTicketTypeRepository.GetAll().AnyAsync();
            User user = new User();
            if (!stationResult)
            {
                IList<Station> newStation = new List<Station>() {
                    new Station() { Name = "Avcılar Merkez Üniversite Kampüsü", StartDistance = 0},
                    new Station() { Name = "Şükrübey", StartDistance = 1 },
                    new Station() { Name = "Büyükşehir Belediyesi  Sosyal Tesisleri", StartDistance = 1.5 },
                    new Station() { Name = "Küçükçekmece", StartDistance = 2.3},
                    new Station() { Name = "Cennet Mahallesi", StartDistance = 3.7 },
                    new Station() { Name = "Florya", StartDistance = 5},
                    new Station() { Name = "Beşyol", StartDistance = 6.3 },
                    new Station() { Name = "Sefaköy", StartDistance = 7.2 },
                    new Station() { Name = "Yenibosna", StartDistance = 8.4 },
                    new Station() { Name = "Şirinevler (Ataköy)", StartDistance = 10.2 },
                    new Station() { Name = "Bahçelievler", StartDistance = 12.25},
                    new Station() { Name = "İncirli (Ömür)", StartDistance = 13.75 },
                    new Station() { Name = "Zeytinburnu", StartDistance = 15.15 },
                    new Station() { Name = "Merter", StartDistance = 16.25 },
                    new Station() { Name = "CevizliBağ", StartDistance = 17.45 },
                    new Station() { Name = "Topkapı", StartDistance = 18.6 },
                    new Station() { Name = "Bayrampaşa – Maltepe", StartDistance = 20.5 },
                    new Station() { Name = "Vatan Caddesi", StartDistance = 22.5 },
                    new Station() { Name = "Edirnekapı", StartDistance = 23.5 },
                    new Station() { Name = "Ayvansaray – Eyüp Sultan", StartDistance = 25.6 },
                    new Station() { Name = "Halıcıoğlu", StartDistance = 27 },
                    new Station() { Name = "Okmeydanı", StartDistance = 28.65 },
                    new Station() { Name = "Darülaceze – Perpa", StartDistance = 29.85 },
                    new Station() { Name = "Okmeydanı Hastane", StartDistance = 30.75 },
                    new Station() { Name = "Çağlayan", StartDistance = 31.4 },
                    new Station() { Name = "Zincirlikuyu", StartDistance = 31.5 },
                    new Station() { Name = "15 Temmuz Şehitler Köprüsü", StartDistance = 32.5 },
                    new Station() { Name = "Burhaniye", StartDistance = 34.1 },
                    new Station() { Name = "Altunizade", StartDistance = 36.2 },
                    new Station() { Name = "Acıbadem", StartDistance = 38.5 },
                    new Station() { Name = "Uzunçayır", StartDistance = 40 },
                    new Station() { Name = "Fikirtepe", StartDistance = 43 },
                    new Station() { Name = "Söğütlüçeşme", StartDistance = 45 }
            };
                StationRepository.AddRange(newStation);
            }
            if (!smartTicketTypeResult)
            {
                IList<SmartTicketType> newsmartTicketType = new List<SmartTicketType>()
                {
                    new SmartTicketType() { Name="Standart"},
                    new SmartTicketType() { Name="Ogrenci"}
                };
                SmartTicketTypeRepository.AddRange(newsmartTicketType);
            }
            if (userManager!=null)
            {
                var admin = await userManager.FindByNameAsync("Admin");
                if (admin==null)
                {
                    user.PasswordHash = passwordHasher.HashPassword(user, "Pass1995*");
                    user.PhoneNumber = "0536XXXXXXX";
                    user.UserName = "Admin";
                    user.Email = "gmail@gmail.com";
                    var result = await userManager.CreateAsync(user);
                }
            }
        }
    }
}
