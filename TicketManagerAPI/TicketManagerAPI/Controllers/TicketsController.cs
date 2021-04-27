using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using TicketManagerAPI.Database.Models;
using TicketManagerAPI.DataLogic.Tickets;
using TicketManagerAPI.DTOs.Tickets;
using TicketManagerAPI.Repository.Repository;

namespace TicketManagerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TicketsController : ControllerBase
    {
        private readonly IGenericRepository<Ticket> _ticketRepository;

        public TicketsController(IGenericRepository<Ticket> ticketRepository)
        {
            this._ticketRepository = ticketRepository;
        }

        private TicketLogic TicketLogic => new(_ticketRepository);

        [HttpGet]
        public async Task<List<TicketDTO>> GetTickets() => await Task
            .FromResult(await TicketLogic.GetTickets());

        [HttpGet("{id}")]
        public async Task<TicketDTO> GetTicket(int id) => await Task
            .FromResult(await TicketLogic.GetTicket(id));

        [HttpPost]
        public async Task<IActionResult> PostTicket(TicketDTO ticket)
        {
            bool result = await TicketLogic.PostTicket(ticket);
            if (result)
            {
                return await Task.FromResult(Ok());
            }
            return await Task.FromResult(BadRequest());
        }

        [HttpPut]
        public async Task<IActionResult> PutTicket(TicketDTO ticket)
        {
            bool result = await TicketLogic.PutTicket(ticket);
            if (result)
            {
                return await Task.FromResult(Ok());
            }
            return await Task.FromResult(BadRequest());
        }
    }
}