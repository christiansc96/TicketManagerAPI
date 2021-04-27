using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TicketManagerAPI.Database.Models;
using TicketManagerAPI.DTOs.Tickets;
using TicketManagerAPI.Repository.Repository;
using TicketManagerAPI.Repository.Utils;

namespace TicketManagerAPI.DataLogic.Tickets
{
    public class TicketLogic
    {
        private readonly IGenericRepository<Ticket> _ticketRepository;

        public TicketLogic(IGenericRepository<Ticket> ticketRepository)
        {
            this._ticketRepository = ticketRepository;
        }

        public async Task<List<TicketDTO>> GetTickets()
        {
            IEnumerable<Ticket> ticketsFromDatabase = await _ticketRepository
                .GetAsync(where: ticket => ticket.Status == true);

            return await Task.FromResult(ticketsFromDatabase
                .Select(ticket => BuildTicketDTO(ticket)).ToList());
        }

        public async Task<TicketDTO> GetTicket(int id)
        {
            Ticket ticketFromDatabase = await _ticketRepository
                .GetByIdAsync(where: ticket => ticket.Status == true && ticket.TicketId == id);
            return await Task.FromResult(BuildTicketDTO(ticketFromDatabase));
        }

        public async Task<bool> PostTicket(TicketDTO ticket)
        {
            bool result = await _ticketRepository.CreateAsync(new Ticket()
            {
                UserName = ticket.UserName,
                CreatedAt = DateTime.Now,
                ModifiedAt = new DateTime(2019, 01, 01),
                Status = true
            });
            return await Task.FromResult(result);
        }

        public async Task<bool> PutTicket(TicketDTO ticket)
        {
            bool result = false;
            Ticket ticketFromDatabase = await _ticketRepository
                .GetByIdAsync(where: ticket => ticket.Status == true && ticket.TicketId == ticket.TicketId);

            if (!LogicValidations.ValidateIfDataIsNull(ticketFromDatabase))
            {
                ticketFromDatabase.UserName = ticket.UserName;
                ticketFromDatabase.ModifiedAt = DateTime.Now;
                ticketFromDatabase.Status = ticket.Status;
                result = await _ticketRepository.UpdateAsync(ticketFromDatabase);
            }
            return await Task.FromResult(result);
        }

        public static TicketDTO BuildTicketDTO(Ticket ticket) => new()
        {
            TicketId = ticket.TicketId,
            UserName = ticket.UserName,
            CreatedAt = ticket.CreatedAt,
            ModifiedAt = ticket.ModifiedAt,
            Status = ticket.Status
        };
    }
}