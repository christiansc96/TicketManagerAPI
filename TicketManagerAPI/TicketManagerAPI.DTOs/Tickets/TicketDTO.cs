using System;
using System.ComponentModel.DataAnnotations;

namespace TicketManagerAPI.DTOs.Tickets
{
    public class TicketDTO
    {
        public int TicketId { get; set; }

        [Required(ErrorMessage = "This field is required")]
        [StringLength(400, ErrorMessage = "Only 400 characters are accepted")]
        public string UserName { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime ModifiedAt { get; set; }

        public bool Status { get; set; }
    }
}