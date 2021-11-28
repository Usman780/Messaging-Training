
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TrainingTracker.DAL;
using TrainingTracker.Models;
namespace TrainingTracker.BL
{     
 public class ExtensionRequestBL
    {
       

 #region ExtensionRequests
        public List<ExtensionRequest> getExtensionRequestsList()
        {
            return new ExtensionRequestDL().getExtensionRequestsList();
        }
        public List<ExtensionRequest> getAllExtensionRequestsList()
        {
            return new ExtensionRequestDL().getAllExtensionRequestsList();
        }
        public ExtensionRequest getExtensionRequestsById(int _id,DatabaseEntities de=null)
        {
            return new ExtensionRequestDL().getExtensionRequestById(_id,de);
        }

        public int AddExtensionRequests(ExtensionRequest _ExtensionRequests)
        {
            //if (_ExtensionRequests.Name == null || _ExtensionRequests.Email == null || _ExtensionRequests.Password == null || _ExtensionRequests.Website_Address == null || _ExtensionRequests.Phone == null)
            //    return false;
            return new ExtensionRequestDL().AddExtensionRequest(_ExtensionRequests);
        }

        public bool UpdateExtensionRequests(ExtensionRequest _ExtensionRequests, DatabaseEntities de=null)
        {
            //if (_ExtensionRequests.Name == null || _ExtensionRequests.Email == null || _ExtensionRequests.Password == null || _ExtensionRequests.Website_Address == null || _ExtensionRequests.Phone == null)
            //    return false;

            return new ExtensionRequestDL().UpdateExtensionRequest(_ExtensionRequests,de);
        }

        public void DeleteExtensionRequests(int _id)
        {
            new ExtensionRequestDL().DeleteExtensionRequest(_id);
        }
        #endregion

}
}