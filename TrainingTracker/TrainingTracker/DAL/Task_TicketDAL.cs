using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TrainingTracker.HelpingClasses;
using TrainingTracker.Models;

namespace TrainingTracker.DAL
{
    public class Task_TicketDAL
    {
        #region Task_Ticket
        CheckAuthenticationDTO logedinuser = General_Purpose.CheckAuthentication();
        public List<Task_Ticket> getTask_TicketsList()
        {
            DatabaseEntities db = new DatabaseEntities();
            CheckAuthenticationDTO logedinuser = General_Purpose.CheckAuthentication();

            int com = Convert.ToInt32(logedinuser.Company);
            //List<Task_Ticket> Task_Tickets = db.Task_Ticket.Where(x => x.IsActive == 1 && x.User_Task.User1.CompanyID == com && x.User_Task.User1!=null).ToList();
            List<Task_Ticket> Task_Tickets = db.Task_Ticket.Where(x => x.IsActive == 1 && x.CompanyId == com && x.User_Task.User1!=null).ToList();

            return Task_Tickets;
        }

        public List<Task_Ticket> getAllTask_TicketsList()
        {
            DatabaseEntities db = new DatabaseEntities();
            int com = Convert.ToInt32(logedinuser.Company);
            //List<Task_Ticket> Task_Tickets = db.Task_Ticket.ToList();
            List<Task_Ticket> Task_Tickets = db.Task_Ticket.Where(x=>x.CompanyId == com).ToList();

            return Task_Tickets;
        }

        public Task_Ticket getTask_TicketById(int _Id, DatabaseEntities de=null)
        {

            DatabaseEntities db = new DatabaseEntities();
            Task_Ticket Task_Ticket = new Task_Ticket();
            if (de != null)
            {
                Task_Ticket = de.Task_Ticket.FirstOrDefault(x => x.Id == _Id && x.IsActive == 1);

            }
            else
            Task_Ticket = db.Task_Ticket.FirstOrDefault(x => x.Id == _Id && x.IsActive == 1);


            return Task_Ticket;
        }

        public bool AddTask_Ticket(Task_Ticket Task_Ticket)
        {
            Task_Ticket.CompanyId = Convert.ToInt32(logedinuser.Company);
            using (DatabaseEntities db = new DatabaseEntities())
            {
                db.Task_Ticket.Add(Task_Ticket);
                db.SaveChanges();
            }
            return true;
        }

        public bool UpdateTask_Ticket(Task_Ticket Task_Ticket,DatabaseEntities de=null)
        {
            Task_Ticket.CompanyId = Convert.ToInt32(logedinuser.Company);
            if (de == null)
                using (DatabaseEntities db = new DatabaseEntities())
                {
                    db.Entry(Task_Ticket).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                }
            else
            {
                de.Entry(Task_Ticket).State = System.Data.Entity.EntityState.Modified;
                de.SaveChanges();
            }
          
            return true;
        }

        public void DeleteTask_Ticket(int _id, DatabaseEntities db = null)
        {
            bool varialble = true;
            if (db == null)
            {
                db = new DatabaseEntities();
                varialble = false;
            }


            Task_Ticket Task_Ticket = db.Task_Ticket.FirstOrDefault(x => x.Id == _id && x.IsActive == 1);
            if (Task_Ticket != null)
                Task_Ticket.IsActive = 0;


            if (!varialble)
            {
                db.SaveChanges();
            }

        }

        public List<Task_Ticket> Task_TicketswithoutWreapper(int gtTaskId, DatabaseEntities de = null)
        {
            if (de == null)
            {
                DatabaseEntities db = new DatabaseEntities();
                db.Configuration.ProxyCreationEnabled = true;
                db.Configuration.LazyLoadingEnabled = false;
                List<Task_Ticket> Task_Tickets = db.Task_Ticket.Where(x => x.IsActive == 1 && x.UserTask_Id == gtTaskId).ToList();
                db.Configuration.ProxyCreationEnabled = false;
                db.Configuration.LazyLoadingEnabled = true;
                return Task_Tickets;
            }
            else
            {

                de.Configuration.ProxyCreationEnabled = true;
                de.Configuration.LazyLoadingEnabled = false;
                List<Task_Ticket> Task_Tickets = de.Task_Ticket.Where(x => x.IsActive == 1 && x.UserTask_Id == gtTaskId).ToList();
                de.Configuration.ProxyCreationEnabled = false;
                de.Configuration.LazyLoadingEnabled = true;
                return Task_Tickets;
            }
        }
        #endregion
    }
}