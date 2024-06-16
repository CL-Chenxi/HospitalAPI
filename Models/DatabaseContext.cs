using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.Extensions.Options;

namespace HospitalAPI.Models
{
    public class DatabaseContext : DbContext  //DbContext is a library from EntityFrameworkCore, its a class to allows u to connect to Databs, insert/rettrivee
    {
        /*
        public DatabaseContext() : base()
        {
        }
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options) { }*/

        protected override void ConfigureConventions(ModelConfigurationBuilder builder)   //because it is inhearitence from DbContext,
                                                                                          //u need to define few things, 1st is configuration, 
                                                                                          //ModelConfigurationBuilder is the type of builder, builder is the var name
        {
            builder.Properties<DateOnly>()              //DateOnly field in models are not compatible with Databse field, so need conversion
                .HaveConversion<DateOnlyConverter>()
                .HaveColumnType("date"); //EntityFrameworkCore.Relational
            base.ConfigureConventions(builder);
        }
        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

            var configstr = @"Data Source=(LocalDB)\MSSQLLocalDB;Initial Catalog=Hospital_DB-9;User id=sa;Password=pwd;
                            Trusted_Connection=True;Integrated Security=SSPI;"; //Secure support provider interface for MS DB
                                                                                    //why do we need varible? we need to define the data source among other thing
                                                                                    //this is the way we define which data source we connect to
                                                                                    //(LocalDB)\MSSQLLocalDB- database type
                                                                                    //Initial Cata -- name of my DB

            optionsBuilder.UseSqlServer(configstr);             //to tell DBContext to use SQL server

        }
        

        public DbSet<Patient> PatientSet { get; set; }            //structure who represent database tables, DbSet<Patient> represent patient table
        public DbSet<Staff> StaffSet { get; set; }          //this is intermediary for database tables
        public DbSet<TreatmentPlan> TreatmentPlanSet { get; set; }
        public DbSet<Drug> DrugSet { get; set; }
        public DbSet<TreatmentPlanEntry> TreatmentPlanEntrySet { get;set; } //replace Medication and TestResult


    }

    //actual converter, all we did above to is to tell Database how to /wher eto connect, if it sees Date feild, its
    //shoudl use the below convert 
    public class DateOnlyConverter : ValueConverter<DateOnly, DateTime>
    {
        public DateOnlyConverter() : base(dateOnly =>           // dateonly is lower key because its variable name
                    dateOnly.ToDateTime(TimeOnly.MinValue),
                dateTime => DateOnly.FromDateTime(dateTime))
        { }

    }

    //when first run add-migration Hospital_DB after created models and controllers, it throws error below:line 54-60(error msg)
    //""add-migration : The term 'add-migration' is not recognized as the name of a cmdlet, function, script file, or operable program. Check the spelling of the 
    //name, or if a path was included, verify that the path is correct and try again.
    //At line:1 char:1
    //+ add-migration CreateDB
    //+ ~~~~~~~~~~~~~
    //  + CategoryInfo          : ObjectNotFound: (add-migration:String) [], CommandNotFoundException
    // + FullyQualifiedErrorId : CommandNotFoundException
    // to Solve:    https://stackoverflow.com/questions/38173404/the-term-add-migration-is-not-recognized
    // to solve: Just install Microsoft.EntityFrameworkCore.Tools package from nuget:



    //add-migration Hospital_DB //create migration folder (use only once)
    //update-database Hospital_DB -verbose //run everytime you change your db

    //if need to delete database and restart from fresh after deleting the .mdf file, and getting rid of the "database already exists" error:
    //sqllocaldb stop
    //sqllocaldb delete
    //update-database Hospital_DB -verbose
}
