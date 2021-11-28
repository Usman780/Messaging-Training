
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web;
//using TrainingTracker.DAL;
//using TrainingTracker.Models;
//namespace TrainingTracker.BL
//{     
// public class GroupTask_TraineeBL
//    {
       

// #region GroupTask_Trainees
//        public List<GroupTask_Trainee> getGroupTask_TraineesList()
//        {
//            return new GroupTask_TraineeDL().getGroupTask_TraineesList();
//        }
       
//        public List<GroupTask_Trainee> getAllGroupTask_TraineesList()
//        {
//            return new GroupTask_TraineeDL().getAllGroupTask_TraineesList();
//        }
//        public GroupTask_Trainee getGroupTask_TraineesById(int _id)
//        {
//            return new GroupTask_TraineeDL().getGroupTask_TraineeById(_id);
//        }

//        public GroupTask_Trainee getTrainee_TasksByIdWrapper(int _id, DatabaseEntities de = null)
//        {
//            return new GroupTask_TraineeDL().getTrainee_TaskByIdWrapper(_id, de);
//        }

//        public bool UpdategroupTask_TraineeWrapper(GroupTask_Trainee _Trainee_Tasks, DatabaseEntities de = null)
//        {
//            //if (_Trainee_Tasks.Name == null || _Trainee_Tasks.Email == null || _Trainee_Tasks.Password == null || _Trainee_Tasks.Website_Address == null || _Trainee_Tasks.Phone == null)
//            //    return false;

//            return new GroupTask_TraineeDL().UpdateTrainee_Task(_Trainee_Tasks, de);
//        }
//        public bool AddGroupTask_Trainees(GroupTask_Trainee _GroupTask_Trainees)
//        {
//            //if (_GroupTask_Trainees.Name == null || _GroupTask_Trainees.Email == null || _GroupTask_Trainees.Password == null || _GroupTask_Trainees.Website_Address == null || _GroupTask_Trainees.Phone == null)
//            //    return false;
//            return new GroupTask_TraineeDL().AddGroupTask_Trainee(_GroupTask_Trainees);
//        }

//        public bool UpdateGroupTask_Trainees(GroupTask_Trainee _GroupTask_Trainees)
//        {
//            //if (_GroupTask_Trainees.Name == null || _GroupTask_Trainees.Email == null || _GroupTask_Trainees.Password == null || _GroupTask_Trainees.Website_Address == null || _GroupTask_Trainees.Phone == null)
//            //    return false;

//            return new GroupTask_TraineeDL().UpdateGroupTask_Trainee(_GroupTask_Trainees);
//        }

//        public void DeleteGroupTask_Trainees(int _id)
//        {
//            new GroupTask_TraineeDL().DeleteGroupTask_Trainee(_id);
//        }
//        #endregion

//}
//}