namespace Elmah.SqlServer.EFInitializer
{
    #region References

    using System;
    using System.Collections;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Configuration;
    using System.Data.Entity;

    #endregion

    public class ElmahContext : DbContext
    {
        #region Constructors

        public ElmahContext() : this(GetConnectionStringFromConfig()) {}
        public ElmahContext(string nameOrConnectionString) : base(nameOrConnectionString) {}

        #endregion

        #region Fields and Properties

        // ReSharper disable once InconsistentNaming
        public virtual DbSet<ELMAH_Error> ELMAH_Errors { get; set; }

        #endregion

        private static string GetConnectionStringFromConfig()
        {
            var errorLogSection = ConfigurationManager.GetSection("elmah/errorLog") as IDictionary;
            if (errorLogSection == null)
            {
                throw new ConfigurationErrorsException("The elmah/errorLog section is missing from the application's configuration file.");
            }
            if (!errorLogSection.Contains("connectionStringName"))
            {
                throw new ConfigurationErrorsException("The elmah/errorLog section in the application's configuration file is missing the connectionStringName attribute.");
            }
            return errorLogSection["connectionStringName"].ToString();
        }
    }

    // ReSharper disable once InconsistentNaming
    public class ELMAH_Error
    {
        #region Fields and Properties

        [Column(TypeName = "ntext")]
        [Required]
        public string AllXml { get; set; }

        [Required]
        [StringLength(60)]
        public string Application { get; set; }

        [Key]
        public Guid ErrorId { get; set; }

        [Required]
        [StringLength(50)]
        public string Host { get; set; }

        [Required]
        [StringLength(500)]
        public string Message { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Sequence { get; set; }

        [Required]
        [StringLength(60)]
        public string Source { get; set; }

        public int StatusCode { get; set; }
        public DateTime TimeUtc { get; set; }

        [Required]
        [StringLength(100)]
        public string Type { get; set; }

        [Required]
        [StringLength(50)]
        public string User { get; set; }

        #endregion
    }
}