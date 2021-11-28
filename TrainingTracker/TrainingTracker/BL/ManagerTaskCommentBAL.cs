
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web;
//using TrainingTracker.DAL;
//using TrainingTracker.Models;
//namespace TrainingTracker.BL
//{     
// public class ManagerTaskCommentBL
//    {
       

// #region ManagerTaskComments
//        public List<ManagerTaskComment> getManagerTaskCommentsList()
//        {
//            return new ManagerTaskCommentDAL().getAllManagerTaskCommentsList();
//        }
//        public List<ManagerTaskComment> getAllManagerTaskCommentsList()
//        {
//            return new ManagerTaskCommentDAL().getAllManagerTaskCommentsList();
//        }
//        public ManagerTaskComment getManagerTaskCommentsById(int _id, DatabaseEntities de=null)
//        {
//            return new ManagerTaskCommentDAL().getManagerTaskCommentById(_id,de);
//        }

//        public bool AddManagerTaskComments(ManagerTaskComment _ManagerTaskComments)
//        {
//            //if (_ManagerTaskComments.Name == null || _ManagerTaskComments.Email == null || _ManagerTaskComments.Password == null || _ManagerTaskComments.Website_Address == null || _ManagerTaskComments.Phone == null)
//            //    return false;
//            return new ManagerTaskCommentDAL().AddManagerTaskComment(_ManagerTaskComments);
//        }

//        public bool UpdateManagerTaskComments(ManagerTaskComment _ManagerTaskComments, DatabaseEntities de=null)
//        {
//            //if (_ManagerTaskComments.Name == null || _ManagerTaskComments.Email == null || _ManagerTaskComments.Password == null || _ManagerTaskComments.Website_Address == null || _ManagerTaskComments.Phone == null)
//            //    return false;

//            return new ManagerTaskCommentDAL().UpdateManagerTaskComment(_ManagerTaskComments,de);
//        }

//        public void DeleteManagerTaskComments(int _id)
//        {
//            new ManagerTaskCommentDAL().DeleteManagerTaskComment(_id);
//        }
//        #endregion

//}
//}