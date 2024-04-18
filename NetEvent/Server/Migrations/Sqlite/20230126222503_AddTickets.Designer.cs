﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using NetEvent.Server.Data;

#nullable disable

namespace NetEvent.Server.Migrations.Sqlite
{
    [DbContext(typeof(SqliteApplicationDbContext))]
    [Migration("20230126222503_AddTickets")]
    partial class AddTickets
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "7.0.2");

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("ClaimType")
                        .HasColumnType("TEXT");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("TEXT");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("RoleClaims", (string)null);

                    b.HasData(
                        new
                        {
                            Id = 1,
                            ClaimType = "Admin.Users.Read",
                            ClaimValue = "",
                            RoleId = "admin"
                        },
                        new
                        {
                            Id = 2,
                            ClaimType = "Admin.Users.Write",
                            ClaimValue = "",
                            RoleId = "admin"
                        },
                        new
                        {
                            Id = 3,
                            ClaimType = "Admin.Roles.Read",
                            ClaimValue = "",
                            RoleId = "admin"
                        },
                        new
                        {
                            Id = 4,
                            ClaimType = "Admin.Roles.Write",
                            ClaimValue = "",
                            RoleId = "admin"
                        },
                        new
                        {
                            Id = 5,
                            ClaimType = "Admin.System.Read",
                            ClaimValue = "",
                            RoleId = "admin"
                        },
                        new
                        {
                            Id = 6,
                            ClaimType = "Admin.System.Write",
                            ClaimValue = "",
                            RoleId = "admin"
                        },
                        new
                        {
                            Id = 7,
                            ClaimType = "Admin.Images.Read",
                            ClaimValue = "",
                            RoleId = "admin"
                        },
                        new
                        {
                            Id = 8,
                            ClaimType = "Admin.Images.Write",
                            ClaimValue = "",
                            RoleId = "admin"
                        },
                        new
                        {
                            Id = 9,
                            ClaimType = "Admin.Events.Read",
                            ClaimValue = "",
                            RoleId = "admin"
                        },
                        new
                        {
                            Id = 10,
                            ClaimType = "Admin.Events.Write",
                            ClaimValue = "",
                            RoleId = "admin"
                        },
                        new
                        {
                            Id = 11,
                            ClaimType = "Admin.Venues.Read",
                            ClaimValue = "",
                            RoleId = "admin"
                        },
                        new
                        {
                            Id = 12,
                            ClaimType = "Admin.Venues.Write",
                            ClaimValue = "",
                            RoleId = "admin"
                        });
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("ClaimType")
                        .HasColumnType("TEXT");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("TEXT");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("UserClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasMaxLength(128)
                        .HasColumnType("TEXT");

                    b.Property<string>("ProviderKey")
                        .HasMaxLength(128)
                        .HasColumnType("TEXT");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("TEXT");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("UserLogins", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("TEXT");

                    b.Property<string>("RoleId")
                        .HasColumnType("TEXT");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("UserRoles", (string)null);

                    b.HasData(
                        new
                        {
                            UserId = "BAFC89CF-4F3E-4595-8256-CCA19C260FBD",
                            RoleId = "admin"
                        });
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("TEXT");

                    b.Property<string>("LoginProvider")
                        .HasMaxLength(128)
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .HasMaxLength(128)
                        .HasColumnType("TEXT");

                    b.Property<string>("Value")
                        .HasColumnType("TEXT");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("UserTokens", (string)null);
                });

            modelBuilder.Entity("NetEvent.Server.Models.ApplicationRole", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("TEXT");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("TEXT");

                    b.Property<bool>("IsDefault")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("TEXT");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex");

                    b.ToTable("Role", (string)null);

                    b.HasData(
                        new
                        {
                            Id = "user",
                            IsDefault = true,
                            Name = "User",
                            NormalizedName = "USER"
                        },
                        new
                        {
                            Id = "orga",
                            IsDefault = false,
                            Name = "Orga",
                            NormalizedName = "ORGA"
                        },
                        new
                        {
                            Id = "admin",
                            IsDefault = false,
                            Name = "Admin",
                            NormalizedName = "ADMIN"
                        });
                });

            modelBuilder.Entity("NetEvent.Server.Models.ApplicationUser", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("TEXT");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("INTEGER");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("TEXT");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("TEXT");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("INTEGER");

                    b.Property<string>("FirstName")
                        .HasColumnType("TEXT");

                    b.Property<string>("LastName")
                        .HasColumnType("TEXT");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("INTEGER");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("TEXT");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("TEXT");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("TEXT");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("TEXT");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("TEXT");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("INTEGER");

                    b.Property<byte[]>("ProfilePicture")
                        .HasColumnType("BLOB");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("TEXT");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("INTEGER");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex");

                    b.ToTable("User", (string)null);

                    b.HasData(
                        new
                        {
                            Id = "BAFC89CF-4F3E-4595-8256-CCA19C260FBD",
                            AccessFailedCount = 0,
                            ConcurrencyStamp = "5d10c057-f0c5-4439-9ea5-52da9de25db4",
                            Email = "admin@admin.de",
                            EmailConfirmed = true,
                            FirstName = "Admin",
                            LastName = "istrator",
                            LockoutEnabled = false,
                            NormalizedEmail = "ADMIN@ADMIN.DE",
                            NormalizedUserName = "ADMIN",
                            PasswordHash = "AQAAAAIAAYagAAAAEHl8xBgwOlrJfSfDpoRyQLa1TdrX7zMysaC62jvZEjAyngzzdjD+zyfPkY2zk66ZGQ==",
                            PhoneNumberConfirmed = false,
                            SecurityStamp = "83f864b3-fb09-44e4-9e5c-748beadc4af3",
                            TwoFactorEnabled = false,
                            UserName = "admin"
                        });
                });

            modelBuilder.Entity("NetEvent.Server.Models.EmailTemplate", b =>
                {
                    b.Property<string>("TemplateId")
                        .HasColumnType("TEXT");

                    b.Property<string>("ContentTemplate")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("SubjectTemplate")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("TemplateId");

                    b.ToTable("EmailTemplates", (string)null);

                    b.HasData(
                        new
                        {
                            TemplateId = "UserEmailConfirmEmailTemplate",
                            ContentTemplate = "<h1>@Model.TemplateVariables[\"firstName\"], welcome to NetEvent.</h1>\n<p> Please confirm your E-Mail by clicking on the following link:</p><a href=\"@Model.TemplateVariables[\"confirmUrl\"]\">@Model.TemplateVariables[\"confirmUrl\"]</a>   ",
                            SubjectTemplate = "@Model.TemplateVariables[\"firstName\"], please confirm your E-Mail address."
                        });
                });

            modelBuilder.Entity("NetEvent.Server.Models.Event", b =>
                {
                    b.Property<long?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Description")
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("EndDate")
                        .HasColumnType("TEXT");

                    b.Property<int>("EventFormat")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.Property<string>("ShortDescription")
                        .HasColumnType("TEXT");

                    b.Property<string>("Slug")
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("StartDate")
                        .HasColumnType("TEXT");

                    b.Property<int>("State")
                        .HasColumnType("INTEGER");

                    b.Property<long?>("VenueId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Visibility")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("Slug")
                        .IsUnique();

                    b.HasIndex("VenueId");

                    b.ToTable("Events", (string)null);
                });

            modelBuilder.Entity("NetEvent.Server.Models.EventTicketType", b =>
                {
                    b.Property<long?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<long>("AvailableTickets")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Currency")
                        .HasColumnType("INTEGER");

                    b.Property<long?>("EventId")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("IsGiftable")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.Property<int>("Price")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("SellEndDate")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("SellStartDate")
                        .HasColumnType("TEXT");

                    b.Property<string>("Slug")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("EventId");

                    b.ToTable("EventTicketTypes", (string)null);
                });

            modelBuilder.Entity("NetEvent.Server.Models.Purchase", b =>
                {
                    b.Property<long?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime?>("PurchaseTime")
                        .HasColumnType("TEXT");

                    b.Property<string>("UserId")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Purchases", (string)null);
                });

            modelBuilder.Entity("NetEvent.Server.Models.SystemImage", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("TEXT");

                    b.Property<byte[]>("Data")
                        .HasColumnType("BLOB");

                    b.Property<string>("Extension")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("UploadTime")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("SystemImages", (string)null);
                });

            modelBuilder.Entity("NetEvent.Server.Models.SystemSettingValue", b =>
                {
                    b.Property<string>("Key")
                        .HasColumnType("TEXT");

                    b.Property<string>("SerializedValue")
                        .HasColumnType("TEXT");

                    b.HasKey("Key");

                    b.ToTable("SystemSettings", (string)null);

                    b.HasData(
                        new
                        {
                            Key = "OrganizationName",
                            SerializedValue = "NetEvent"
                        },
                        new
                        {
                            Key = "HideOrganizationNameInNavBar",
                            SerializedValue = "False"
                        },
                        new
                        {
                            Key = "DataCultureInfo",
                            SerializedValue = "en-US"
                        },
                        new
                        {
                            Key = "Favicon",
                            SerializedValue = ""
                        },
                        new
                        {
                            Key = "Logo",
                            SerializedValue = ""
                        },
                        new
                        {
                            Key = "AboutUs",
                            SerializedValue = ""
                        },
                        new
                        {
                            Key = "LegalNotice",
                            SerializedValue = ""
                        },
                        new
                        {
                            Key = "PrivacyPolicy",
                            SerializedValue = ""
                        },
                        new
                        {
                            Key = "PrimaryColor",
                            SerializedValue = ""
                        },
                        new
                        {
                            Key = "PrimaryTextColor",
                            SerializedValue = ""
                        },
                        new
                        {
                            Key = "SecondaryColor",
                            SerializedValue = ""
                        },
                        new
                        {
                            Key = "SecondaryTextColor",
                            SerializedValue = ""
                        },
                        new
                        {
                            Key = "Background",
                            SerializedValue = ""
                        },
                        new
                        {
                            Key = "AppbarBackground",
                            SerializedValue = ""
                        },
                        new
                        {
                            Key = "AppbarText",
                            SerializedValue = ""
                        },
                        new
                        {
                            Key = "CustomCss",
                            SerializedValue = ""
                        },
                        new
                        {
                            Key = "Standard",
                            SerializedValue = "True"
                        },
                        new
                        {
                            Key = "Steam",
                            SerializedValue = "False"
                        },
                        new
                        {
                            Key = "AdyenApiKey",
                            SerializedValue = ""
                        },
                        new
                        {
                            Key = "AdyenClientKey",
                            SerializedValue = ""
                        },
                        new
                        {
                            Key = "AdyenMerchantAccount",
                            SerializedValue = ""
                        });
                });

            modelBuilder.Entity("NetEvent.Server.Models.TicketPurchase", b =>
                {
                    b.Property<long?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("Currency")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Price")
                        .HasColumnType("INTEGER");

                    b.Property<long?>("PurchaseId")
                        .HasColumnType("INTEGER");

                    b.Property<long?>("TicketId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("PurchaseId");

                    b.HasIndex("TicketId");

                    b.ToTable("TicketPurchases", (string)null);
                });

            modelBuilder.Entity("NetEvent.Server.Models.Venue", b =>
                {
                    b.Property<long?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("City")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.Property<string>("Number")
                        .HasColumnType("TEXT");

                    b.Property<string>("Slug")
                        .HasColumnType("TEXT");

                    b.Property<string>("Street")
                        .HasColumnType("TEXT");

                    b.Property<string>("ZipCode")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Venues", (string)null);
                });

            modelBuilder.Entity("NetEvent.Shared.Dto.ThemeDto", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("ThemeData")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Themes", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("NetEvent.Server.Models.ApplicationRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("NetEvent.Server.Models.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("NetEvent.Server.Models.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("NetEvent.Server.Models.ApplicationRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("NetEvent.Server.Models.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("NetEvent.Server.Models.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("NetEvent.Server.Models.Event", b =>
                {
                    b.HasOne("NetEvent.Server.Models.Venue", "Venue")
                        .WithMany()
                        .HasForeignKey("VenueId");

                    b.Navigation("Venue");
                });

            modelBuilder.Entity("NetEvent.Server.Models.EventTicketType", b =>
                {
                    b.HasOne("NetEvent.Server.Models.Event", "Event")
                        .WithMany("TicketTypes")
                        .HasForeignKey("EventId");

                    b.Navigation("Event");
                });

            modelBuilder.Entity("NetEvent.Server.Models.Purchase", b =>
                {
                    b.HasOne("NetEvent.Server.Models.ApplicationUser", "User")
                        .WithMany()
                        .HasForeignKey("UserId");

                    b.Navigation("User");
                });

            modelBuilder.Entity("NetEvent.Server.Models.TicketPurchase", b =>
                {
                    b.HasOne("NetEvent.Server.Models.Purchase", "Purchase")
                        .WithMany("TicketPurchases")
                        .HasForeignKey("PurchaseId");

                    b.HasOne("NetEvent.Server.Models.EventTicketType", "Ticket")
                        .WithMany()
                        .HasForeignKey("TicketId");

                    b.Navigation("Purchase");

                    b.Navigation("Ticket");
                });

            modelBuilder.Entity("NetEvent.Server.Models.Event", b =>
                {
                    b.Navigation("TicketTypes");
                });

            modelBuilder.Entity("NetEvent.Server.Models.Purchase", b =>
                {
                    b.Navigation("TicketPurchases");
                });
#pragma warning restore 612, 618
        }
    }
}
