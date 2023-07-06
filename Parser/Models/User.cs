using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Parser.Models
{

    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Index { get; set; }
        [JsonPropertyName("User Id")]
        public string UserId { get; set; }
        [JsonPropertyName("First Name")]
        public string FirstName { get; set; }
        [JsonPropertyName("Last Name")]
        public string LastName { get; set; }
        public string Sex { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        [JsonPropertyName("Date of birth")]
        public string DateOfBirth { get; set; }
        [JsonPropertyName("Job Title")]
        public string JobTitle { get; set; }

    }
}
