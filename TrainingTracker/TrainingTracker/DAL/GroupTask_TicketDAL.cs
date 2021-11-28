
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TrainingTracker.HelpingClasses;
using TrainingTracker.Models;



namespace TrainingTracker.DAL
{

  public class GroupTask_TicketDL
    {
        #region GroupTask_Ticket
        CheckAuthenticationDTO logedinuser = General_Purpose.CheckAuthentication();
        int CompanyId = Convert.ToInt32(General_Purpose.CheckAuthentication().Company);
        public List<GroupTask_Ticket> getGroupTask_TicketsList()
        {
            DatabaseEntities db = new DatabaseEntities();
      
           // List<GroupTask_Ticket> GroupTask_Tickets = db.GroupTask_Ticket.Where(x => x.IsActive == 1 ).ToList();
            List<GroupTask_Ticket> GroupTask_Tickets = db.GroupTask_Ticket.Where(x => x.IsActive == 1 && x.CompanyId == CompanyId).ToList();

            return GroupTask_Tickets;
        }

       


        public List<GroupTask_Ticket> getAllGroupTask_TicketsList()
        {
            DatabaseEntities db = new DatabaseEntities();
      
            //List<GroupTask_Ticket> GroupTask_Tickets = db.GroupTask_Ticket.ToList();
            List<GroupTask_Ticket> GroupTask_Tickets = db.GroupTask_Ticket.Where(x=>x.CompanyId == CompanyId).ToList();

            return GroupTask_Tickets;
        }
        
        public GroupTask_Ticket getGroupTask_TicketById(int _Id,DatabaseEntities de=null)
        {

            DatabaseEntities db = new DatabaseEntities();
            GroupTask_Ticket _GroupTask_Ticket = new GroupTask_Ticket();
            if (de != null)
               _GroupTask_Ticket = de.GroupTask_Ticket.FirstOrDefault(x => x.Id == _Id && x.IsActive == 1);
            else
               _GroupTask_Ticket = db.GroupTask_Ticket.FirstOrDefault(x => x.Id == _Id && x.IsActive == 1);
            

            return _GroupTask_Ticket;
        }

        public bool AddGroupTask_Ticket(GroupTask_Ticket _GroupTask_Ticket)
        {
            _GroupTask_Ticket.CompanyId = CompanyId;
            using (DatabaseEntities db = new DatabaseEntities())
            {
                db.GroupTask_Ticket.Add(_GroupTask_Ticket);
                db.SaveChanges();
            }
            return true;
        }

        public bool UpdateGroupTask_Ticket(GroupTask_Ticket _GroupTask_Ticket,DatabaseEntities de=null)
        {
            _GroupTask_Ticket.CompanyId = CompanyId;
            if (de == null)
            {
                using (DatabaseEntities db = new DatabaseEntities())
                {
                    db.Entry(_GroupTask_Ticket).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                }
            } else
            {
                de.Entry(_GroupTask_Ticket).State = System.Data.Entity.EntityState.Modified;
                de.SaveChanges();
            }
            return true;
        }

        public void DeleteGroupTask_Ticket(int _id, DatabaseEntities db = null)
        {
            bool varialble = true;
            if (db == null)
            {
                db = new DatabaseEntities();
                varialble = false;
            }



            GroupTask_Ticket GroupTask_Ticket = db.GroupTask_Ticket.FirstOrDefault(x => x.Id == _id && x.IsActive == 1);
            if(GroupTask_Ticket!=null)
            GroupTask_Ticket.IsActive = 0;


            

            if (!varialble)
            {
                db.SaveChanges();
            }

        }

        public List<GroupTask_Ticket> groupTask_TicketswithoutWreapper(int gtTaskId, DatabaseEntities de = null)
        {
            if (de == null)
            {
                DatabaseEntities db = new DatabaseEntities();
                db.Configuration.ProxyCreationEnabled = true;
                db.Configuration.LazyLoadingEnabled = false;
                List<GroupTask_Ticket> GroupTask_Tickets = db.GroupTask_Ticket.Where(x => x.IsActive == 1 && x.GroupTaskDetails_Id==gtTaskId).ToList();
                db.Configuration.ProxyCreationEnabled = false;
                db.Configuration.LazyLoadingEnabled = true;
                return GroupTask_Tickets;
            }
            else
            {

                de.Configuration.ProxyCreationEnabled = true;
                de.Configuration.LazyLoadingEnabled = false;
                List<GroupTask_Ticket> GroupTask_Tickets = de.GroupTask_Ticket.Where(x => x.IsActive == 1 && x.GroupTaskDetails_Id == gtTaskId).ToList();
                de.Configuration.ProxyCreationEnabled = false;
                de.Configuration.LazyLoadingEnabled = true;
                return GroupTask_Tickets;
            }
        }
        #endregion

    }
}