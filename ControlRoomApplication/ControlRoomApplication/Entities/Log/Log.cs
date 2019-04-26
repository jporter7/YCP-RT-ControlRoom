using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ControlRoomApplication.Entities
{
    [Table("telescope_log")]
    public class Log
    {

        public Log()
        {

        }

        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int Id { get; set; }

        [Required]
        [Column("log_date")]
        public DateTime Date { get; set; }

        [Required]
        [Column("log_level")]
        public string LogLevel { get; set; }

        [Required]
        [Column("thread")]
        public string Thread { get; set; }

        [Required]
        [Column("logger")]
        public string Logger { get; set; }

        [Required]
        [Column("message")]
        public string Message { get; set; }
    }
}
