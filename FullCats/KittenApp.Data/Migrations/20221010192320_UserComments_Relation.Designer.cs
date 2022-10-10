﻿// <auto-generated />
using KittenApp.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using System;

namespace KittenApp.Data.Migrations
{
    [DbContext(typeof(KittenAppContext))]
    [Migration("20221010192320_UserComments_Relation")]
    partial class UserComments_Relation
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.0.0-rtm-26452")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("KittenApp.Models.Breed", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name")
                        .IsRequired();

                    b.HasKey("Id");

                    b.ToTable("Breeds");
                });

            modelBuilder.Entity("KittenApp.Models.Comment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("CommentContent")
                        .IsRequired();

                    b.Property<int>("KittenId");

                    b.Property<int>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("KittenId");

                    b.HasIndex("UserId");

                    b.ToTable("Comments");
                });

            modelBuilder.Entity("KittenApp.Models.Kitten", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("Age");

                    b.Property<int>("BreedId");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("BreedId");

                    b.ToTable("Kittens");
                });

            modelBuilder.Entity("KittenApp.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Email")
                        .IsRequired();

                    b.Property<string>("PasswordHash")
                        .IsRequired();

                    b.Property<string>("Username")
                        .IsRequired();

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("KittenApp.Models.Comment", b =>
                {
                    b.HasOne("KittenApp.Models.Kitten", "Kitten")
                        .WithMany("Comments")
                        .HasForeignKey("KittenId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("KittenApp.Models.User", "User")
                        .WithMany("Comments")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("KittenApp.Models.Kitten", b =>
                {
                    b.HasOne("KittenApp.Models.Breed", "Breed")
                        .WithMany("Kittens")
                        .HasForeignKey("BreedId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
