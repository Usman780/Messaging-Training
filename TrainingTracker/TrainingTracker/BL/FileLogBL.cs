using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TrainingTracker.DAL;
using TrainingTracker.Models;

namespace TrainingTracker.BL
{
    public class FileLogBL
    {
        #region FileLog
    
        public Models.FileLog AddFileLog(Models.FileLog file)
        {
            return new FileLogDAL().AddFileLog(file);
        }

        #endregion
    }
}