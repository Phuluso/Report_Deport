﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ReportDeport.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class ReportDepotEntities10 : DbContext
    {
        public ReportDepotEntities10()
            : base("name=ReportDepotEntities10")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<C__MigrationHistory> C__MigrationHistory { get; set; }
        public virtual DbSet<AspNetRole> AspNetRoles { get; set; }
        public virtual DbSet<AspNetUserClaim> AspNetUserClaims { get; set; }
        public virtual DbSet<AspNetUserLogin> AspNetUserLogins { get; set; }
        public virtual DbSet<AspNetUser> AspNetUsers { get; set; }
        public virtual DbSet<columnTranslation> columnTranslations { get; set; }
        public virtual DbSet<contactForm> contactForms { get; set; }
        public virtual DbSet<course> courses { get; set; }
        public virtual DbSet<course_categories> course_categories { get; set; }
        public virtual DbSet<course_completions> course_completions { get; set; }
        public virtual DbSet<enrol> enrols { get; set; }
        public virtual DbSet<question> questions { get; set; }
        public virtual DbSet<question_answers> question_answers { get; set; }
        public virtual DbSet<question_attempt_steps> question_attempt_steps { get; set; }
        public virtual DbSet<question_attempts> question_attempts { get; set; }
        public virtual DbSet<question_categories> question_categories { get; set; }
        public virtual DbSet<quiz> quizs { get; set; }
        public virtual DbSet<quiz_attempts> quiz_attempts { get; set; }
        public virtual DbSet<quiz_grades> quiz_grades { get; set; }
        public virtual DbSet<quiz_slots> quiz_slots { get; set; }
        public virtual DbSet<role> roles { get; set; }
        public virtual DbSet<role_assignments> role_assignments { get; set; }
        public virtual DbSet<sysdiagram> sysdiagrams { get; set; }
        public virtual DbSet<template> templates { get; set; }
        public virtual DbSet<templateColumn> templateColumns { get; set; }
        public virtual DbSet<TestPerson> TestPersons { get; set; }
        public virtual DbSet<TestSalePerson> TestSalePersons { get; set; }
        public virtual DbSet<user> users { get; set; }
        public virtual DbSet<user_enrolments> user_enrolments { get; set; }
        public virtual DbSet<user_info_data> user_info_data { get; set; }
        public virtual DbSet<user_info_field> user_info_field { get; set; }
        public virtual DbSet<UserId_Int> UserId_Int { get; set; }
    }
}
