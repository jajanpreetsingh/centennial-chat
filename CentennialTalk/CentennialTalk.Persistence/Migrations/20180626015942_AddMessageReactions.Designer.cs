﻿// <auto-generated />
using System;
using CentennialTalk.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace CentennialTalk.Persistence.Migrations
{
    [DbContext(typeof(ChatDBContext))]
    [Migration("20180626015942_AddMessageReactions")]
    partial class AddMessageReactions
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.0-rtm-30799")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("CentennialTalk.Models.Discussion", b =>
                {
                    b.Property<Guid>("DiscussionId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("DiscussionCode")
                        .IsRequired()
                        .HasMaxLength(8);

                    b.Property<bool>("IsLinkOpen");

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

                    b.Property<Guid>("MessageId");

                    b.Property<int>("ReactType");

                    b.Property<string>("Sender");

                    b.HasKey("MessageReactionId");

                    b.ToTable("Reactions");
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
#pragma warning restore 612, 618
        }
    }
}