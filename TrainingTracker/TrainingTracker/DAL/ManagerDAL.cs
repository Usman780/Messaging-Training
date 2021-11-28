//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web;
//using TrainingTracker.Models;



//namespace TrainingTracker.DAL
//{

//  public class ManagerDL
//    {
// #region Manager
//        public List<Manager> getManagersList()
//        {
//            DatabaseEntities db = new DatabaseEntities();
//            int adminId = (int)HttpContext.Current.Session["Admin"];
//            List<Manager> Managers = db.Managers.Where(x=>x.IsActive==1 && x.AdminID==adminId).ToList();

//            return Managers;
//        }
        
//        public List<Manager> getActiveandInvitedManager()
//        {
//            DatabaseEntities db = new DatabaseEntities();
//            //int adminId = (int)HttpContext.Current.Session["Admin"];
//            List<Manager> Managers = db.Managers.Where(x => x.IsActive == 1 || x.IsActive==2).ToList();

//            return Managers;
//        }

//        public List<Manager> getManagerListNOAdmin()
//        {
//            DatabaseEntities db = new DatabaseEntities();
        
//            List<Manager> Managers = db.Managers.Where(x => x.IsActive == 1).ToList();
            
//            return Managers;
//        }

//        public List<Manager> getAllManagersList()
//        {
//            DatabaseEntities db = new DatabaseEntities();
//            int adminId = (int)HttpContext.Current.Session["Admin"];
//            List<Manager> Managers = db.Managers.Where(x =>x.AdminID == adminId).ToList();
//            return Managers;
//        }
        
//        public Manager getManagerById(int _Id, DatabaseEntities de=null)
//        {

//            Manager _Manager;

//            if (de == null)
//            {
//                DatabaseEntities db = new DatabaseEntities();
//                _Manager = db.Managers.FirstOrDefault(x => x.Id == _Id && (x.IsActive == 1 || x.IsActive == 2));


//                return _Manager;
//            }
//            else
//            {
//                _Manager = de.Managers.FirstOrDefault(x => x.Id == _Id && (x.IsActive == 1 || x.IsActive == 2));


//                return _Manager;
//            }
//        }

//        public bool AddManager(Manager _Manager)
//        {
//            using (DatabaseEntities db = new DatabaseEntities())
//            {
//                db.Managers.Add(_Manager);
//                db.SaveChanges();
//            }

           
//            HttpContext.Current.Session["ManagerCount"] = true;
//            return true;
//        }

//        public bool UpdateManager(Manager _Manager, DatabaseEntities de=null)
//        {
//            if(de==null)
//            using (DatabaseEntities db = new DatabaseEntities())
//            {
//                db.Entry(_Manager).State = System.Data.Entity.EntityState.Modified;
//                db.SaveChanges();
//            }
//            else
//            {
//                de.Entry(_Manager).State = System.Data.Entity.EntityState.Modified;
//                de.SaveChanges();

//            }
//            return true;
//        }

//        public void DeleteManager(int _id,DatabaseEntities db=null)
//        {
//            bool varialble = true;
//            if (db == null)
//            {
//                db = new DatabaseEntities();
//                varialble = false;
//            }

//            Manager  manager = db.Managers.FirstOrDefault(x => x.Id == _id && (x.IsActive == 1 ||x.IsActive==2) );

//            //foreach (var item in manager.Trainees.Where(x=>x.IsActive==1).ToList())
//            //{
//            //    new DAL.TraineeDL().DeleteTrainee(item.Id, db);

//            //}
//            if (manager != null)
//            {
//                manager.IsActive = 0;
//                db.Entry(manager).State = System.Data.Entity.EntityState.Modified;

//                if (!varialble)
//                {
//                    db.SaveChanges();
//                }
//            }
//        }

//        public Manager getSingleManagerObject(int id)
//        {
//            DatabaseEntities de = new DatabaseEntities();
//            de.Configuration.ProxyCreationEnabled = true;
//            de.Configuration.LazyLoadingEnabled = false;
//            Manager m = de.Managers.SqlQuery("Select * from Manager where id = " + id).FirstOrDefault();
//            de.Configuration.LazyLoadingEnabled = true;
//            de.Configuration.ProxyCreationEnabled = false;

//            return m;
//        }

//#endregion

//}
//}