
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web;
//using TrainingTracker.DAL;
//using TrainingTracker.Models;
//namespace TrainingTracker.BL
//{     
// public class TraineeBL
//    {
       

// #region Trainees
//        public List<Trainee> getTraineesList()
//        {
//            return new TraineeDL().getTraineesList();
//        }

//        public List<Trainee> getActiveandInvitedTrainee()
//        {
//            return new TraineeDL().getActiveandInvitedTrainees();
//        }

//        public List<Trainee> getTraineesListNoAdmin()
//        {
//            return new TraineeDL().getTraineesListNoAdmin();
//        }

//        public List<Trainee> getAllTraineesList()
//        {
//            return new TraineeDL().getAllTraineesList();
//        }

//        public Trainee getTraineesById(int _id, DatabaseEntities de=null)
//        {
//            return new TraineeDL().getTraineeById(_id,de);
//        }

//        public bool AddTrainees(Trainee _Trainees)
//        {

//            bool temp = new TraineeDL().AddTrainee(_Trainees);
//            traineeCount();
//            return temp;
//        }

//        public bool UpdateTrainees(Trainee _Trainees, DatabaseEntities de = null)
//        {
            

//       return new TraineeDL().UpdateTrainee(_Trainees,de);
            
//        }

//        public void DeleteTrainees(int _id)
//        {
//            new TraineeDL().DeleteTrainee(_id);
//        }

//        public void traineeCount()
//        {
//            int adminId = (int)HttpContext.Current.Session["Admin"];
//            Admin admin = new AdminBL().getAdminsById(adminId);
//            if (admin != null)
//            {
//                int mc = getTraineesList().Count;
//                if (mc >= admin.EmployeeNumber.Value)
//                    HttpContext.Current.Session["TraineeCount"] = false;
//                else
//                HttpContext.Current.Session["TraineeCount"] = true;
//                return;
//            }
//            else
//            HttpContext.Current.Session["TraineeCount"] = false;
//        }
//        #endregion

//    }
//}