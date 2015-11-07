using System.ComponentModel.DataAnnotations;

namespace PollPrototypeAPI.Models
{
    using System;
    using System.Data.Entity;
    using System.Linq;

    public class PollSampleContext : DbContext
    {
        // Your context has been configured to use a 'PollDBConnection' connection string from your application's 
        // configuration file (App.config or Web.config). 
        // 
        // If you wish to target a different database and/or database provider, modify the 'PollSampleModel' 
        // connection string in the application configuration file.
        public PollSampleContext()
            : base("name=PollDBConnection")
        {
        }

        // Add a DbSet for each entity type that you want to include in your model. For more information 
        // on configuring and using a Code First model, see http://go.microsoft.com/fwlink/?LinkId=390109.

        public virtual DbSet<PollSampleEntity> PollSamples { get; set; }
    }

    public class PollSampleEntity
    {
        public int PollSampleEntityId { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public string CountryId { get; set; }
        public bool TrueFalseAnswer1 { get; set; }
        public bool TrueFalseAnswer2 { get; set; }
        public int NumericAnswer1 { get; set; }
        public int NumericAnswer2 { get; set; }
    }
}