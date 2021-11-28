using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TrainingTracker.Models;
using TrainingTracker.DAL;


namespace TrainingTracker.BL
{
    public class User_WorktypeBL
    {
        #region User_Worktypes
        public List<User_Worktype> getUser_WorktypesList(DatabaseEntities de = null)
        {
            return new User_WorktypeDL().getUser_WorktypesList(de);
        }
        public List<User_Worktype> getUser_WorktypesListWithoutAdmin(DatabaseEntities de = null)
        {
            return new User_WorktypeDL().GetUser_WorktypesListWithoutCompany(de);
        }
        public List<User_Worktype> getAllUser_WorktypesList()
        {
            return new User_WorktypeDL().getAllUser_WorktypesList();
        }
        public User_Worktype getUser_WorktypesById(int _id)
        {
            return new User_WorktypeDL().getUser_WorktypeById(_id);
        }
        public User_Worktype getUser_WorktypesByIdWrapper(int _id, DatabaseEntities de = null)
        {
            return new User_WorktypeDL().getUser_WorktypeByIdWrapper(_id, de);
        }

        public User_Worktype AddUser_Worktypes(User_Worktype _User_Worktypes, DatabaseEntities de = null)
        {
            //if (_User_Worktypes.Name == null || _User_Worktypes.Email == null || _User_Worktypes.Password == null || _User_Worktypes.Website_Address == null || _User_Worktypes.Phone == null)
            //    return false;
            return new User_WorktypeDL().AddUser_Worktype(_User_Worktypes, de);
        }

        public bool UpdateUser_Worktypes(User_Worktype _User_Worktypes, DatabaseEntities de = null)
        {
            //if (_User_Worktypes.Name == null || _User_Worktypes.Email == null || _User_Worktypes.Password == null || _User_Worktypes.Website_Address == null || _User_Worktypes.Phone == null)
            //    return false;

            return new User_WorktypeDL().UpdateUser_Worktype(_User_Worktypes, de);
        }

        public void DeleteUser_Worktypes(int _id)
        {
            new User_WorktypeDL().DeleteUser_Worktype(_id);
        }
        #endregion
    }
}