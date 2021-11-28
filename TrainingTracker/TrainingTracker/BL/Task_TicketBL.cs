using System;
using System.Collections.Generic;
using System.Linq;
using TrainingTracker.DAL;
using TrainingTracker.Models;

namespace TrainingTracker.BL
{
    public class Task_TicketBL
    {
        public List<Task_Ticket> getTask_TicketsList()
        {
            return new Task_TicketDAL().getTask_TicketsList();
        }
        public List<Task_Ticket> getAllTask_TicketsList()
        {
            return new Task_TicketDAL().getAllTask_TicketsList();
        }
        public Task_Ticket getTask_TicketsById(int _id,DatabaseEntities de=null)
        {
            return new Task_TicketDAL().getTask_TicketById(_id,de);
        }

        public bool AddTask_Tickets(Task_Ticket Task_Tickets)
        {
            //if (_GroupTask_Tickets.Name == null || _GroupTask_Tickets.Email == null || _GroupTask_Tickets.Password == null || _GroupTask_Tickets.Website_Address == null || _GroupTask_Tickets.Phone == null)
            //    return false;
            return new Task_TicketDAL().AddTask_Ticket(Task_Tickets);
        }

        public bool UpdateTask_Tickets(Task_Ticket Task_Tickets, DatabaseEntities de = null)
        {
            //if (_GroupTask_Tickets.Name == null || _GroupTask_Tickets.Email == null || _GroupTask_Tickets.Password == null || _GroupTask_Tickets.Website_Address == null || _GroupTask_Tickets.Phone == null)
            //    return false;

            return new Task_TicketDAL().UpdateTask_Ticket(Task_Tickets,de);
        }

        public void DeleteTask_Tickets(int _id, DatabaseEntities de = null)
        {
            new Task_TicketDAL().DeleteTask_Ticket(_id, de);
        }

        public List<Task_Ticket> Task_TicketswithoutWreapper(int gtTaskId, DatabaseEntities de = null)
        {
            return new Task_TicketDAL().Task_TicketswithoutWreapper(gtTaskId, de);

        }
    }
}