
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web;
//using TrainingTracker.DAL;
//using TrainingTracker.Models;
//namespace TrainingTracker.BL
//{     
// public class ManagerBL
//    {
       

// #region Managers
//        public List<Manager> getManagersList()
//        {
//            return new ManagerDL().getManagersList();
//        }

//        public List<Manager> getActiveandInvitedManager()
//        {
//            return new ManagerDL().getActiveandInvitedManager();
//        }

//        public List<Manager> getManagerListNOAdmin()
//        {
//            return new ManagerDL().getManagerListNOAdmin();
//        }

//        public List<Manager> getAllManagersList()
//        {
//            return new ManagerDL().getAllManagersList();
//        }

//        public Manager getManagersById(int _id, DatabaseEntities de=null)
//        {
//            return new ManagerDL().getManagerById(_id,de);
//        }

//        public bool AddManagers(Manager _Managers)
//        {
//            //if (_Managers.Name == null || _Managers.Email == null || _Managers.Password == null || _Managers.Website_Address == null || _Managers.Phone == null)
//            //    return false;
//            return new ManagerDL().AddManager(_Managers);
//        }

//        public bool UpdateManagers(Manager _Managers, DatabaseEntities de=null)
//        {
//            //if (_Managers.Name == null || _Managers.Email == null || _Managers.Password == null || _Managers.Website_Address == null || _Managers.Phone == null)
//            //    return false;

//            return new ManagerDL().UpdateManager(_Managers,de);
//        }

//        public void DeleteManagers(int _id)
//        {
//            new ManagerDL().DeleteManager(_id);
//        }

//        public void managerCount()
//        {
//            int adminId = (int)HttpContext.Current.Session["Admin"];
//            Admin admin = new AdminBL().getAdminsById(adminId);
//            if (admin != null)
//            {
//                int mc = getManagersList().Count;
//                if (mc >= admin.ManagerNumber.Value)
//                    HttpContext.Current.Session["ManagerCount"] = false;
//                HttpContext.Current.Session["ManagerCount"] = true;
//            }
//            else
//            HttpContext.Current.Session["ManagerCount"] = false;
//        }

//        public Manager getSingleManagerObject(int id)
//        {
//            return new ManagerDL().getSingleManagerObject(id);
//        }
//        #endregion

//}
//}