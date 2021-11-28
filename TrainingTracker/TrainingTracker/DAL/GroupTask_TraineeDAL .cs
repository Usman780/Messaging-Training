
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web;
//using TrainingTracker.Models;



//namespace TrainingTracker.DAL
//{

//  public class GroupTask_TraineeDL
//    {
// #region GroupTask_Trainee
//        public List<GroupTask_Trainee> getGroupTask_TraineesList()
//        {
//            DatabaseEntities db = new DatabaseEntities();
//            List<GroupTask_Trainee> GroupTask_Trainees = db.GroupTask_Trainee.Where(x=>x.IsActive==1).ToList();

//            return GroupTask_Trainees;
//        }
//        public List<GroupTask_Trainee> getAllGroupTask_TraineesList()
//        {
//            DatabaseEntities db = new DatabaseEntities();
//            List<GroupTask_Trainee> GroupTask_Trainees = db.GroupTask_Trainee.ToList();

//            return GroupTask_Trainees;
//        }
        
//        public GroupTask_Trainee getGroupTask_TraineeById(int _Id)
//        {
//            GroupTask_Trainee _GroupTask_Trainee;
//            DatabaseEntities db = new DatabaseEntities();
            
//                _GroupTask_Trainee = db.GroupTask_Trainee.FirstOrDefault(x => x.Id == _Id && x.IsActive == 1);
           

//            return _GroupTask_Trainee;
//        }

//        public bool AddGroupTask_Trainee(GroupTask_Trainee _GroupTask_Trainee)
//        {
//            using (DatabaseEntities db = new DatabaseEntities())
//            {
//                db.GroupTask_Trainee.Add(_GroupTask_Trainee);
//                db.SaveChanges();
//            }
//            return true;
//        }

//        public bool UpdateGroupTask_Trainee(GroupTask_Trainee _GroupTask_Trainee)
//        {
//            using (DatabaseEntities db = new DatabaseEntities())
//            {
//                db.Entry(_GroupTask_Trainee).State = System.Data.Entity.EntityState.Modified;
//                db.SaveChanges();
//            }
//            return true;
//        }

//        public void DeleteGroupTask_Trainee(int _id, DatabaseEntities db = null)
//        {
//            bool varialble = true;
//            if (db == null)
//            {
//                db = new DatabaseEntities();
//                varialble = false;
//            }




//            GroupTask_Trainee _GroupTask_Trainee = db.GroupTask_Trainee.FirstOrDefault(x => x.Id == _id && x.IsActive == 1);
//            if (_GroupTask_Trainee != null)
//            {
//                foreach (var item in _GroupTask_Trainee.GroupTaskComments)
//                {
//                    new GroupTaskCommentDAL().DeleteGroupTaskComment(item.Id, db);
//                }


//                _GroupTask_Trainee.IsActive = 0;
//                db.Entry(_GroupTask_Trainee).State = System.Data.Entity.EntityState.Modified;
//            }

//            if (!varialble)
//            {
//                db.SaveChanges();
//            }
//        }


//        public bool UpdateTrainee_Task(GroupTask_Trainee _Trainee_Task, DatabaseEntities de = null)
//        {
//            if (de == null)
//            {
//                using (DatabaseEntities db1 = new DatabaseEntities())
//                {

//                    db1.Entry(_Trainee_Task).State = System.Data.Entity.EntityState.Modified;
//                    db1.SaveChanges();
//                }
//                return true;
//            }
//            de.Entry(_Trainee_Task).State = System.Data.Entity.EntityState.Modified;
//            return true;
//        }

//        public GroupTask_Trainee getTrainee_TaskByIdWrapper(int _Id, DatabaseEntities de = null)
//        {
//            GroupTask_Trainee _Trainee_Task;
//            using (DatabaseEntities db1 = new DatabaseEntities())
//            {
//                _Trainee_Task = de.GroupTask_Trainee.FirstOrDefault(x => x.Id == _Id && x.IsActive == 1);

//            }
//            return _Trainee_Task;
//        }
//        #endregion

//    }
//}