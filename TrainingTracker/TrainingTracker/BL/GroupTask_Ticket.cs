
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TrainingTracker.DAL;
using TrainingTracker.Models;
namespace TrainingTracker.BL
{     
 public class GroupTask_TicketBL
    {
       

 #region GroupTask_Tickets
        public List<GroupTask_Ticket> getGroupTask_TicketsList()
        {
            return new GroupTask_TicketDL().getGroupTask_TicketsList();
        }
        public List<GroupTask_Ticket> getAllGroupTask_TicketsList()
        {
            return new GroupTask_TicketDL().getAllGroupTask_TicketsList();
        }
        public GroupTask_Ticket getGroupTask_TicketsById(int _id,DatabaseEntities de=null)
        {
            return new GroupTask_TicketDL().getGroupTask_TicketById(_id,de);
        }

        public bool AddGroupTask_Tickets(GroupTask_Ticket _GroupTask_Tickets)
        {
            //if (_GroupTask_Tickets.Name == null || _GroupTask_Tickets.Email == null || _GroupTask_Tickets.Password == null || _GroupTask_Tickets.Website_Address == null || _GroupTask_Tickets.Phone == null)
            //    return false;
            return new GroupTask_TicketDL().AddGroupTask_Ticket(_GroupTask_Tickets);
        }

        public bool UpdateGroupTask_Tickets(GroupTask_Ticket _GroupTask_Tickets,DatabaseEntities de=null)
        {
            //if (_GroupTask_Tickets.Name == null || _GroupTask_Tickets.Email == null || _GroupTask_Tickets.Password == null || _GroupTask_Tickets.Website_Address == null || _GroupTask_Tickets.Phone == null)
            //    return false;

            return new GroupTask_TicketDL().UpdateGroupTask_Ticket(_GroupTask_Tickets,de);
        }

        public void DeleteGroupTask_Tickets(int _id, DatabaseEntities de= null)
        {
            new GroupTask_TicketDL().DeleteGroupTask_Ticket(_id,de);
        }

        public List<GroupTask_Ticket> groupTask_TicketswithoutWreapper(int gtTaskId, DatabaseEntities de=null)
        {
            return new GroupTask_TicketDL().groupTask_TicketswithoutWreapper(gtTaskId, de);

        }

        #endregion

    }
}