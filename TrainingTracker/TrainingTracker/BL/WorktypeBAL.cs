using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TrainingTracker.DAL;
using TrainingTracker.Models;

namespace TrainingTracker.BL
{
    public class WorktypeBL
    {
        #region Worktypes
        public List<Worktype> getWorktypesList()
        {
            return new WorktypeDL().getWorktypesList();
        }
        public List<Worktype> getAllWorktypesList()
        {
            return new WorktypeDL().getAllWorktypesList();
        }
        public Worktype getWorktypesById(int _id)
        {
            return new WorktypeDL().getWorktypeById(_id);
        }

        public bool AddWorktypes(Worktype _Worktypes)
        {
            //if (_Worktypes.Name == null || _Worktypes.Email == null || _Worktypes.Password == null || _Worktypes.Website_Address == null || _Worktypes.Phone == null)
            //    return false;
            return new WorktypeDL().AddWorktype(_Worktypes);
        }

        public bool UpdateWorktypes(Worktype _Worktypes)
        {
            //if (_Worktypes.Name == null || _Worktypes.Email == null || _Worktypes.Password == null || _Worktypes.Website_Address == null || _Worktypes.Phone == null)
            //    return false;

            return new WorktypeDL().UpdateWorktype(_Worktypes);
        }

        public void DeleteWorktypes(int _id)
        {
            new WorktypeDL().DeleteWorktype(_id);
        }
        #endregion
    }
}