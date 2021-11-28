
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TrainingTracker.DAL;
using TrainingTracker.Models;
namespace TrainingTracker.BL
{     
 public class GroupTasks_DetailsBL
    {
       

 #region GroupTasks_Detailss
        public List<GroupTasks_Details> getGroupTasks_DetailssList(DatabaseEntities de=null)
        {
            return new GroupTasks_DetailsDL().getGroupTasks_DetailssList(de);
        }
        public List<GroupTasks_Details> getAllGroupTasks_DetailssList()
        {
            return new GroupTasks_DetailsDL().getAllGroupTasks_DetailssList();
        }
        public GroupTasks_Details getGroupTasks_DetailssById(int _id)
        {
            return new GroupTasks_DetailsDL().getGroupTasks_DetailsById(_id);
        }
        public GroupTasks_Details getGroupTasks_DetailssByIdWrapepr(int _id,DatabaseEntities de=null)
        {
            return new GroupTasks_DetailsDL().getGroupTasks_DetailsByIdWrapper(_id,de);
        }

        public int AddGroupTasks_Detailss(GroupTasks_Details _GroupTasks_Detailss)
        {
            //if (_GroupTasks_Detailss.Name == null || _GroupTasks_Detailss.Email == null || _GroupTasks_Detailss.Password == null || _GroupTasks_Detailss.Website_Address == null || _GroupTasks_Detailss.Phone == null)
            //    return false;
            return new GroupTasks_DetailsDL().AddGroupTasks_Details(_GroupTasks_Detailss);
        }

        public bool UpdateGroupTasks_Detailss(GroupTasks_Details _GroupTasks_Detailss , DatabaseEntities de = null)
        {
            //if (_GroupTasks_Detailss.Name == null || _GroupTasks_Detailss.Email == null || _GroupTasks_Detailss.Password == null || _GroupTasks_Detailss.Website_Address == null || _GroupTasks_Detailss.Phone == null)
            //    return false;

            return new GroupTasks_DetailsDL().UpdateGroupTasks_Details(_GroupTasks_Detailss,de);
        }

        public void DeleteGroupTasks_Detailss(int _id)
        {
            new GroupTasks_DetailsDL().DeleteGroupTasks_Details(_id);
        }
        public bool UpdateGroupTasks_DetailsWrapper(GroupTasks_Details _GroupTasks_Details, DatabaseEntities de = null)
        {
            return new GroupTasks_DetailsDL().UpdateGroupTasks_DetailsWrapper(_GroupTasks_Details, de);
        }
        #endregion

    }
}