using KatifiWebServer.Data.Enums;
using KatifiWebServer.Models.DatabaseModels;

namespace KatifiWebServer.Data;

public static class DBInitializer
{
    public static void Seed(IApplicationBuilder applicationBuilder)
    {
        using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
        {
            var context = serviceScope.ServiceProvider.GetService<MicrosoftEFContext>();
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

                if (!context.Roles.Any())
                {
                    context.Roles.AddRange(new List<Role>()
                    {
                        new Role()
                        {
                            Name = IdentityRoles.Guest
                        },
                        new Role()
                        {
                            Name = IdentityRoles.User
                        },
                        new Role()
                        {
                            Name = IdentityRoles.Admin
                        }
                    });
                    context.SaveChanges();
                }

                if (!context.Users.Any())
                {
                    context.Users.AddRange(new List<User>()
                {
                    new User()
                    {
                        Username = "pk1",
                        Password = "passwd01",
                        Lastname = "Pálfalvi",
                        FirstName = "Kristóf",
                        Gender = 'M',
                        AgreeTerm = true,
                        BornDate = new DateOnly(1999,7,17),
                        Email = "palfalvi.kristof@gmail.com",
                        RegistrationDate = DateTime.Now,
                        RoleId = (int)IdentityRoles.Admin
                    },
                    new User()
                    {
                        Username = "pm2",
                        Password = "passwd02",
                        Lastname = "Pandur",
                        FirstName = "Manni",
                        Gender = 'F',
                        AgreeTerm = true,
                        BornDate = new DateOnly(2000,3,31),
                        RegistrationDate = DateTime.Now,
                        Email = "misztik2000@gmail.com",
                        RoleId = (int)IdentityRoles.User,
                        AddressID = 340
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

                if (!context.Messes.Any())
                {
                    context.Messes.AddRange(new List<Mess>()
                    {
                        new Mess() {
                            ChurchId = 50,
                            Day = "Vasárnap",
                            StartTime = new TimeOnly(18,00),
                        },
                        new Mess() {
                            ChurchId = 51,
                            Day = "Vasárnap",
                            StartTime = new TimeOnly(18,00),
                            Priest = context.Churches.Single(c => c.Id == 51).Vicar}
                    });
                    context.SaveChanges();
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
