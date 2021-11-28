
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TrainingTracker.Models;
using TrainingTracker.Helper_Classes;
using TrainingTracker.HelpingClasses;



namespace TrainingTracker.DAL
{

  public class ExtensionRequestDL
    {
        CheckAuthenticationDTO logedinuser = General_Purpose.CheckAuthentication();
        int CompanyId = Convert.ToInt32(General_Purpose.CheckAuthentication().Company);
        #region ExtensionRequest
        public List<ExtensionRequest> getExtensionRequestsList()
        {
            DatabaseEntities db = new DatabaseEntities();
            //int comp = Convert.ToInt32(loggedinuser.Company);
            //List<ExtensionRequest> ExtensionRequests = db.ExtensionRequests.Where(x => x.isActive == 1 &&( x.User_Task.User1.CompanyID==comp || x.GroupTasks_Details.User.CompanyID== comp)).ToList();
            List<ExtensionRequest> ExtensionRequests = db.ExtensionRequests.Where(x => x.isActive == 1 && x.CompanyId == CompanyId).ToList();

            return ExtensionRequests;
        }

       


        public List<ExtensionRequest> getAllExtensionRequestsList()
        {
            DatabaseEntities db = new DatabaseEntities();
       
            List<ExtensionRequest> ExtensionRequests = db.ExtensionRequests.Where(x => x.CompanyId == CompanyId).ToList();

            return ExtensionRequests;
        }

        public ExtensionRequest getExtensionRequestById(int _Id, DatabaseEntities de = null)
        {

            if (de != null)
            {
                ExtensionRequest er = de.ExtensionRequests.FirstOrDefault(x => x.Id == _Id && x.isActive == 1);


                return er;

            }
            else
            {
                DatabaseEntities db = new DatabaseEntities();


                ExtensionRequest _ExtensionRequest = db.ExtensionRequests.FirstOrDefault(x => x.Id == _Id && x.isActive == 1);


                return _ExtensionRequest;
            }
        }

        public int AddExtensionRequest(ExtensionRequest _ExtensionRequest)
        {
            _ExtensionRequest.CompanyId = CompanyId;
            using (DatabaseEntities db = new DatabaseEntities())
            {
                db.ExtensionRequests.Add(_ExtensionRequest);
                db.SaveChanges();
            }
            return  _ExtensionRequest.Id;
        }

        public bool UpdateExtensionRequest(ExtensionRequest _ExtensionRequest, DatabaseEntities de = null)
        {
            if(_ExtensionRequest.CompanyId == 0 || _ExtensionRequest.CompanyId == null)
                _ExtensionRequest.CompanyId = CompanyId;

            if (de == null)
            {
                using (DatabaseEntities db = new DatabaseEntities())
                {
                    db.Entry(_ExtensionRequest).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                }
            }
            else
            {
                de.Entry(_ExtensionRequest).State = System.Data.Entity.EntityState.Modified;
                de.SaveChanges();

            }
            return true;
        }





        public void DeleteExtensionRequest(int _id, DatabaseEntities db = null)
        {
            bool varialble = true;
            if (db == null)
            {
                db = new DatabaseEntities();
                varialble = false;
            }



            ExtensionRequest ExtensionRequest = db.ExtensionRequests.FirstOrDefault(x => x.Id == _id && x.isActive == 1);
            ExtensionRequest.isActive = 0;


           

            if (!varialble)
            {
                db.SaveChanges();
            }

        }


        #endregion

    }
}