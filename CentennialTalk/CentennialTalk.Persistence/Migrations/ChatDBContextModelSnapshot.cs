﻿// <auto-generated />
using System;
using CentennialTalk.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace CentennialTalk.Persistence.Migrations
{
    [DbContext(typeof(ChatDBContext))]
    partial class ChatDBContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.4-rtm-31024")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("CentennialTalk.Models.Discussion", b =>
                {
                    b.Property<Guid>("DiscussionId")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("ActivationDate");

                    b.Property<DateTime>("CreatedDate");

                    b.Property<string>("DiscussionCode")
                        .IsRequired()
                        .HasMaxLength(8);

                    b.Property<DateTime>("ExpirationDate");

                    b.Property<DateTime>("LastUpdated");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(255);

                    b.HasKey("DiscussionId");

                    b.ToTable("Discussions");
                });

            modelBuilder.Entity("CentennialTalk.Models.GroupMember", b =>
                {
                    b.Property<Guid>("GroupMemberId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ChatCode")
                        .IsRequired()
                        .HasMaxLength(255);

                    b.Property<string>("ConnectionId");

                    b.Property<Guid?>("DiscussionId");

                    b.Property<string>("IconPath");

                    b.Property<bool>("IsConnected");

                    b.Property<bool>("IsModerator");

                    b.Property<DateTime>("JoiningTime");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasMaxLength(255);

                    b.HasKey("GroupMemberId");

                    b.HasIndex("DiscussionId");

                    b.ToTable("GroupMembers");
                });

            modelBuilder.Entity("CentennialTalk.Models.Message", b =>
                {
                    b.Property<Guid>("MessageId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ChatCode")
                        .IsRequired()
                        .HasMaxLength(8);

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasMaxLength(255);

                    b.Property<Guid?>("GroupMemberId");

                    b.Property<Guid>("RepliedMessageId");

                    b.Property<string>("Sender")
                        .IsRequired()
                        .HasMaxLength(255);

                    b.Property<DateTime>("SentDate");

                    b.HasKey("MessageId");

                    b.HasIndex("GroupMemberId");

                    b.ToTable("Messages");
                });

            modelBuilder.Entity("CentennialTalk.Models.MessageReaction", b =>
                {
                    b.Property<Guid>("MessageReactionId")
                        .ValueGeneratedOnAdd();

                    b.Property<Guid?>("MessageId");

                    b.Property<int>("ReactType");

                    b.Property<string>("Sender");

                    b.HasKey("MessageReactionId");

                    b.HasIndex("MessageId");

                    b.ToTable("Reactions");
                });

            modelBuilder.Entity("CentennialTalk.Models.QuestionModels.PollingQuestion", b =>
                {
                    b.Property<Guid>("QuestionId")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("ArchiveDate");

                    b.Property<string>("ChatCode");

                    b.Property<string>("Content");

                    b.Property<Guid?>("DiscussionId");

                    b.Property<bool>("IsArchived");

                    b.Property<bool>("IsPublished");

                    b.Property<DateTime>("PublishDate");

                    b.Property<bool>("SelectMultiple");

                    b.Property<int>("Type");

                    b.HasKey("QuestionId");

                    b.HasIndex("DiscussionId");

                    b.ToTable("Polls");
                });

            modelBuilder.Entity("CentennialTalk.Models.QuestionModels.QuestionOption", b =>
                {
                    b.Property<int>("OptionId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<Guid?>("PollingQuestionQuestionId");

                    b.Property<string>("Text");

                    b.HasKey("OptionId");

                    b.HasIndex("PollingQuestionQuestionId");

                    b.ToTable("QuestionOption");
                });

            modelBuilder.Entity("CentennialTalk.Models.QuestionModels.SubjectiveQuestion", b =>
                {
                    b.Property<Guid>("QuestionId")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("ArchiveDate");

                    b.Property<string>("ChatCode");

                    b.Property<string>("Content");

                    b.Property<Guid?>("DiscussionId");

                    b.Property<bool>("IsArchived");

                    b.Property<bool>("IsPublished");

                    b.Property<DateTime>("PublishDate");

                    b.Property<int>("Type");

                    b.HasKey("QuestionId");

                    b.HasIndex("DiscussionId");

                    b.ToTable("Questions");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Name")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("RoleId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUser", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AccessFailedCount");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Email")
                        .HasMaxLength(256);

                    b.Property<bool>("EmailConfirmed");

                    b.Property<bool>("LockoutEnabled");

                    b.Property<DateTimeOffset?>("LockoutEnd");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256);

                    b.Property<string>("PasswordHash");

                    b.Property<string>("PhoneNumber");

                    b.Property<bool>("PhoneNumberConfirmed");

                    b.Property<string>("SecurityStamp");

                    b.Property<bool>("TwoFactorEnabled");

                    b.Property<string>("UserName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("AspNetUsers");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider");

                    b.Property<string>("ProviderKey");

                    b.Property<string>("ProviderDisplayName");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("RoleId");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("LoginProvider");

                    b.Property<string>("Name");

                    b.Property<string>("Value");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("CentennialTalk.Models.GroupMember", b =>
                {
                    b.HasOne("CentennialTalk.Models.Discussion")
                        .WithMany("Members")
                        .HasForeignKey("DiscussionId");
                });

            modelBuilder.Entity("CentennialTalk.Models.Message", b =>
                {
                    b.HasOne("CentennialTalk.Models.GroupMember")
                        .WithMany("Messages")
                        .HasForeignKey("GroupMemberId");
                });

            modelBuilder.Entity("CentennialTalk.Models.MessageReaction", b =>
                {
                    b.HasOne("CentennialTalk.Models.Message")
                        .WithMany("Reactions")
                        .HasForeignKey("MessageId");
                });

            modelBuilder.Entity("CentennialTalk.Models.QuestionModels.PollingQuestion", b =>
                {
                    b.HasOne("CentennialTalk.Models.Discussion")
                        .WithMany("Polls")
                        .HasForeignKey("DiscussionId");
                });

            modelBuilder.Entity("CentennialTalk.Models.QuestionModels.QuestionOption", b =>
                {
                    b.HasOne("CentennialTalk.Models.QuestionModels.PollingQuestion")
                        .WithMany("Options")
                        .HasForeignKey("PollingQuestionQuestionId");
                });

            modelBuilder.Entity("CentennialTalk.Models.QuestionModels.SubjectiveQuestion", b =>
                {
                    b.HasOne("CentennialTalk.Models.Discussion")
                        .WithMany("Questions")
                        .HasForeignKey("DiscussionId");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
