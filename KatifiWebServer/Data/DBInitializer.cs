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
                    },
                    new Address()
                    {
                        CountryCode = "HU",
                        County = "Baranya",
                        PostCode = 7621,
                        City = "Pécs",
                        Street = "Dóm tér",
                        HouseNumber = 3
                    },
                    new Address() //id=343
                    {
                        CountryCode = "HU",
                        County = "Baranya",
                        City = "Máriagyűd"
                    },
                    new Address() //id=344
                    {
                        CountryCode = "HU",
                        County = "Baranya",
                        City = "Pécs"
                    },
                    new Address() //id=345
                    {
                        CountryCode = "HU",
                        County = "Baranya",
                        City = "Orfű"
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
                            ImageUrl = "https://miserend.hu/kepek/templomok/2017/406371657.jpg",
                            AddressId = 339,
                        },
                        new Church()
                        {
                            Name = "Gyárvárosi templom",
                            ImageUrl = "https://miserend.hu/kepek/templomok/2049/7391438446.jpg",
                            AddressId = 341,
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
                        Start = new DateTime(2023,6,29),
                        End = new DateTime(2023,7,2),
                        RegistrationDeadline = new DateTime(2023,06,23),
                        Organizer = "KatIfi kis csapata",
                        ImageUrl = "https://pecsiegyhazmegye.hu/images/40MIT_17.jpg",
                        Fee = 5500,
                        AddressId = 343
                    },
                    new Event()
                    {
                        Name = "Kirándulás",
                        Start = new DateTime(2023,3,15),
                        End = new DateTime(2023,3,15),
                        RegistrationDeadline = new DateTime(2023,03,14),
                        Organizer = "Kopeczky Ábris",
                        ImageUrl = "https://img.freepik.com/free-photo/small-stream-forest_23-2147632812.jpg",
                        Fee = 0,
                        AddressId = 344
                    },
                    new Event()
                    {
                        Name = "Osztálytalálkozó",
                        Start = new DateTime(2023,4,20),
                        End = new DateTime(2023,4,21),
                        RegistrationDeadline = new DateTime(2023,04,20),
                        Organizer = "Standovár Anna",
                        ImageUrl = "https://aktivmagyarorszag.hu/wp-content/uploads/2021/02/gyopar-kulcsoshaz-3-scaled.jpg",
                        Fee = 0,
                        AddressId = 345
                    },
                    new Event()
                    {
                        Name = "Évzáró üdülés Orfűn",
                        Start = new DateTime(2023,6,25),
                        End = new DateTime(2023,6,27),
                        RegistrationDeadline = new DateTime(2023,06,18),
                        Organizer = "Barcza Gellért",
                        ImageUrl = "https://www.termalfurdo.hu/upload/images/Galeria/furdo/orfu_aquapark/termalfurdo_orfu_aquapark_1.jpg",
                        Fee = 500,
                        AddressId = 345
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
                        var Pandur_Manni = new RegisterModel()
                        {
                            UserName = "pandur_manni",
                            Password = "Q-ittacicaholacica5",
                            Lastname = "Pandur",
                            FirstName = "Manni",
                            Gender = 'F',
                            BornDate = new DateOnly(2000, 3, 31),
                            Email = "misztik2000@gmail.com",
                            AgreeTerm = true,
                            AddressID = 340

                        };
                        var Berecz_Tibi = new RegisterModel()
                        {
                            UserName = "bt_bt",
                            Password = "Q-egyhazmegye1009",
                            Lastname = "Berecz",
                            FirstName = "Tibor",
                            Gender = 'M',
                            BornDate = new DateOnly(1988, 10, 8),
                            Email = "berecz.tibor@crnl.hu",
                            AgreeTerm = true,
                            AddressID = 342

                        };

                        var status1 = await authservice.RegistAdminAsync(Palfalvi_Kristof);
                        var status2 = await authservice.RegistUserAsync(Pandur_Manni);
                        var status3 = await authservice.RegistUserAsync(Berecz_Tibi);
                        var status21 = await authservice.AddRoleToUserAsync(AppRoleEnum.EventOrganizer.ToString(), Pandur_Manni.UserName);
                        var status22 = await authservice.AddRoleToUserAsync(AppRoleEnum.CommunityLeader.ToString(), Pandur_Manni.UserName);
                        var status31 = await authservice.AddRoleToUserAsync(AppRoleEnum.Vicar.ToString(), Berecz_Tibi.UserName);

                        if (status1 != 0)
                            throw new Exception(string.Format("User {0} can not be created.", Palfalvi_Kristof.UserName));
                        if (status2 != 0)
                            throw new Exception(string.Format("User {0} can not be created.", Pandur_Manni.UserName));
                        if (status3 != 0)
                            throw new Exception(string.Format("User {0} can not be created.", Berecz_Tibi.UserName));
                        if (status21 != 0)
                            throw new Exception(string.Format("Role {0} can not be assign to User {1}.", AppRoleEnum.EventOrganizer.ToString(), Pandur_Manni.UserName));
                        if (status22 != 0)
                            throw new Exception(string.Format("Role {0} can not be assign to User {1}.", AppRoleEnum.CommunityLeader.ToString(), Pandur_Manni.UserName));
                        if (status31 != 0)
                            throw new Exception(string.Format("Role {0} can not be assign to User {1}.", AppRoleEnum.Vicar.ToString(), Berecz_Tibi.UserName));

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
                                Status = MemberStatus.Member.ToString(),
                                JoinDate = DateTime.Now.Date,
                                CommunityId = 630,
                                UserId = 5170
                            },
                            new Member()
                            {
                                Status = MemberStatus.Member.ToString(),
                                JoinDate = DateTime.Now.Date,
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

