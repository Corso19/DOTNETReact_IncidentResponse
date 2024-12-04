﻿// <auto-generated />
using System;
using IncidentResponseAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace IncidentResponseAPI.Migrations
{
    [DbContext(typeof(IncidentResponseContext))]
    [Migration("20241204093742_AddRetrievalIntervalToSensorsModel")]
    partial class AddRetrievalIntervalToSensorsModel
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("IncidentResponseAPI.Models.EventsModel", b =>
                {
                    b.Property<int>("EventId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("EventId"));

                    b.Property<string>("Details")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("SensorId")
                        .HasColumnType("int");

                    b.Property<DateTime>("Timestamp")
                        .HasColumnType("datetime2");

                    b.Property<string>("TypeName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("isProcessed")
                        .HasColumnType("bit");

                    b.HasKey("EventId");

                    b.HasIndex("SensorId");

                    b.ToTable("Events");
                });

            modelBuilder.Entity("IncidentResponseAPI.Models.IncidentEventModel", b =>
                {
                    b.Property<int>("IncidentId")
                        .HasColumnType("int");

                    b.Property<int>("EventId")
                        .HasColumnType("int");

                    b.HasKey("IncidentId", "EventId");

                    b.HasIndex("EventId");

                    b.ToTable("IncidentEvents");
                });

            modelBuilder.Entity("IncidentResponseAPI.Models.IncidentsModel", b =>
                {
                    b.Property<int>("IncidentId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IncidentId"));

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("DetectedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("IncidentId");

                    b.ToTable("Incidents");
                });

            modelBuilder.Entity("IncidentResponseAPI.Models.RecommendationsModel", b =>
                {
                    b.Property<int>("RecommendationId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("RecommendationId"));

                    b.Property<int>("IncidentId")
                        .HasColumnType("int");

                    b.Property<string>("Recommendation")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("isCompleted")
                        .HasColumnType("bit");

                    b.HasKey("RecommendationId");

                    b.HasIndex("IncidentId");

                    b.ToTable("RecommendationsModel");
                });

            modelBuilder.Entity("IncidentResponseAPI.Models.SensorsModel", b =>
                {
                    b.Property<int>("SensorId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("SensorId"));

                    b.Property<string>("ApplicationId")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClientSecret")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedAd")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("LastRunAt")
                        .HasColumnType("datetime2");

                    b.Property<int>("RetrievalInterval")
                        .HasColumnType("int");

                    b.Property<string>("SensorName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TenantId")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("isEnabled")
                        .HasColumnType("bit");

                    b.HasKey("SensorId");

                    b.ToTable("Sensors");
                });

            modelBuilder.Entity("IncidentResponseAPI.Models.EventsModel", b =>
                {
                    b.HasOne("IncidentResponseAPI.Models.SensorsModel", "Sensor")
                        .WithMany()
                        .HasForeignKey("SensorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Sensor");
                });

            modelBuilder.Entity("IncidentResponseAPI.Models.IncidentEventModel", b =>
                {
                    b.HasOne("IncidentResponseAPI.Models.EventsModel", "Event")
                        .WithMany()
                        .HasForeignKey("EventId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("IncidentResponseAPI.Models.IncidentsModel", "Incident")
                        .WithMany("IncidentEvent")
                        .HasForeignKey("IncidentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Event");

                    b.Navigation("Incident");
                });

            modelBuilder.Entity("IncidentResponseAPI.Models.RecommendationsModel", b =>
                {
                    b.HasOne("IncidentResponseAPI.Models.IncidentsModel", "Incident")
                        .WithMany()
                        .HasForeignKey("IncidentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Incident");
                });

            modelBuilder.Entity("IncidentResponseAPI.Models.IncidentsModel", b =>
                {
                    b.Navigation("IncidentEvent");
                });
#pragma warning restore 612, 618
        }
    }
}
