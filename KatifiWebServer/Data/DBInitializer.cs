using KatifiWebServer.Data.Enums;
using KatifiWebServer.Models.DatabaseModels;
using KatifiWebServer.Models.SecurityModels;
using KatifiWebServer.Services;

namespace KatifiWebServer.Data;

public class DBInitializer
{

    public async void Seed(IApplicationBuilder applicationBuilder)
    {
        using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
        {
            var context = serviceScope.ServiceProvider.GetService<MSEFContext>();
            if (context != null)
            {
                context.Database.EnsureCreated(); // Adatbázis létrehozása ha még nem létezik

                if (!context.Addresses.Any())
                {
                    context.Addresses.AddRange(new List<Address>()
                {
                    new Address()
                    {
                        CountryCode = "HU",
                        County = "Baranya",
                        PostCode = 7621,
                        City = "Pécs",
                        Street = "Hunyadi János utca",
                        HouseNumber = 3
                    },
                    new Address()
                    {
                        CountryCode = "HU",
                        County = "Baranya",
                        PostCode = 7626,
                        City = "Pécs",
                        Street = "Ágoston tér",
                        HouseNumber = 1
                    },
                    new Address()
                    {
                        CountryCode = "HU",
                        County = "Baranya",
                        PostCode = 7900,
                        City = "Szigetvár",
                        Street = "Tinódi Sebestyén utca",
                        HouseNumber = 71
                    },
                    new Address()
                    {
                        CountryCode = "HU",
                        County = "Baranya",
                        PostCode = 7630,
                        City = "Pécs",
                        Street = "Zsolnay Vilmos utca",
                        HouseNumber = 92
                    }
                });
                    context.SaveChanges();
                }

                if (!context.Churches.Any())
                {
                    context.Churches.AddRange(new List<Church>()
                    {
                        new Church()
                        {
                            Name = "Szent Ágoston templom",
                            AdressId = 339,
                        },
                        new Church()
                        {
                            Name = "Gyárvárosi templom",
                            AdressId = 341,
                            Vicar = "Nagy Norbert"
                        }
                    });
                    context.SaveChanges();
                }

                if (!context.Messes.Any())
                {
                    context.Messes.AddRange(new List<Mess>()
                {
                    new Mess() {
                        ChurchId = 50,
                        Day = "Vasárnap",
                        StartTime = new TimeOnly(18,00)
                    },
                    new Mess() {
                        ChurchId = 51,
                        Day = "Vasárnap",
                        StartTime = new TimeOnly(18,00),
                        Priest = context.Churches.Single(c => c.Id == 51).Vicar
                    }
                });
                    context.SaveChanges();
                }

                if (!context.Communities.Any())
                {
                    context.Communities.AddRange(new List<Community>()
                {
                    new Community()
                    {
                        Name = "Boldog Brenner János Antiochia",
                        IsOpen = false,
                        AddressId = 338
                    },
                    new Community()
                    {
                        Name = "Szent Ágoston közzöség",
                        IsOpen = true,
                        AddressId = 338
                    }
                });
                    context.SaveChanges();
                }

                if (!context.Events.Any())
                {
                    context.Events.AddRange(new List<Event>()
                {
                    new Event()
                    {
                        Name = "41. MIT",
                        Date = new DateTime(2023,6,29),
                        RegistrationDeadline = new DateTime(2023,06,22),
                        Organizer = "KatIfi kis csapata",
                        Fee = 5500
                    },
                    new Event()
                    {
                        Name = "Kirándulás",
                        Date = new DateTime(2023,3,15),
                        RegistrationDeadline = new DateTime(2023,03,14),
                        Organizer = "Kopeczky Ábris",
                        Fee = 0
                    }
                });
                    context.SaveChanges();
                }

                if (!context.Users.Any())
                {
                    var authservice = serviceScope.ServiceProvider.GetService<IAuthenticationService>();
                    if (authservice != null)
                    {
                        var Palfalvi_Kristof = new RegisterModel()
                        {
                            UserName = "palfalvi_kristof",
                            Password = "Q-securepassw78",
                            Lastname = "Pálfalvi",
                            FirstName = "Kristóf",
                            Gender = 'M',
                            BornDate = new DateOnly(1999, 7, 17),
                            AgreeTerm = true
                        };
                        var Mandur_Manni = new RegisterModel()
                        {
                            UserName = "mandur_manni",
                            Password = "Q-ittacicaholacica5",
                            Lastname = "Pandur",
                            FirstName = "Manni",
                            Gender = 'F',
                            BornDate = new DateOnly(2000, 3, 31),
                            Email = "misztik2000@gmail.com",
                            AgreeTerm = true,
                            AddressID = 340

                        };
                        var status1 = await authservice.RegisterAdmin(Palfalvi_Kristof);
                        var status2 = await authservice.RegisterUser(Mandur_Manni);

                        Console.WriteLine("INFO - Users are created!");
                    }
                }

                if (!context.Members.Any())
                {
                    context.Members.AddRange(
                        new List<Member>()
                        {
                            new Member()
                            {
                                Status = MemberStatus.Member,
                                JoinDate = DateTime.Now,
                                CommunityId = 630,
                                UserId = 5170
                            },
                            new Member()
                            {
                                Status = MemberStatus.Member,
                                JoinDate = DateTime.Now,
                                CommunityId = 630,
                                UserId = 5171
                            }
                        });
                    context.SaveChanges();
                }

                if (!context.Participants.Any())
                {
                    context.Participants.AddRange(new List<Participant>()
                    {
                        new Participant()
                        {
                            EventId = 1140,
                            UserId = 5171,
                            ApplicationDate = DateTime.Now
                        },
                        new Participant()
                        {
                            EventId = 1140,
                            UserId = 5170,
                            ApplicationDate = DateTime.Now
                        },
                        new Participant()
                        {
                            EventId = 1141,
                            UserId =5170,
                            ApplicationDate = DateTime.Now
                        }
                    });
                    context.SaveChanges();
                }

            }
        }
    }
}
