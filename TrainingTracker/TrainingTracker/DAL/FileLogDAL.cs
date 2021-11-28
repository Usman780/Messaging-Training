using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TrainingTracker.HelpingClasses;
using TrainingTracker.Models;

namespace TrainingTracker.DAL
{
    public class FileLogDAL
    {
        CheckAuthenticationDTO logedinuser = General_Purpose.CheckAuthentication();

        #region FileLog

        public Models.FileLog AddFileLog(Models.FileLog log)
        {
            DatabaseEntities db = new DatabaseEntities();

            try
            {
                Models.FileLog _file = db.sp_FileLogAddUpdate("Insert", log.Id, log.IsActive, log.CreatedAt, log.UserId, log.FileId,
               log.Type, log.LogTime, log.NoOfVersions, log.Role).FirstOrDefault();
               
                db.SaveChangesAsync();

                log.Id = _file.Id;

                return log;

            }
            catch (Exception)
            {
                return null;
            }
        }


        #endregion
    }
}