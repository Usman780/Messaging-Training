using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using TrainingTracker.Helper_Classes;
using TrainingTracker.HelpingClasses;
using TrainingTracker.HelpingClasses.Logging;
using TrainingTracker.Models;

namespace TrainingTracker.HelpingClasses
{
    public class BlobManager
    {
        CheckAuthenticationDTO logedinuser = General_Purpose.CheckAuthentication();
        private CloudBlobContainer blobContainer;

        public BlobManager(string ContainerName=null,int CompanyID=-1)
        {
            // Check if Container Name is null or empty  
            Log.Info("BlobManager function started");
            try
            {
                // Get azure table storage connection string.  
                string ConnectionString = ProjectVaraiables.AZURE_CLOUD_STORAGE;
                CloudStorageAccount storageAccount = CloudStorageAccount.Parse(ConnectionString);
                string val = "";
                if(CompanyID!=-1)
                val=ProjectVaraiables.COMPANY_ALPHABET + Convert.ToInt32(CompanyID);
                else
                val = ProjectVaraiables.COMPANY_ALPHABET + Convert.ToInt32(logedinuser.Company);


                CloudBlobClient cloudBlobClient = storageAccount.CreateCloudBlobClient();
                blobContainer = cloudBlobClient.GetContainerReference(val);

                // Create the container and set the permission  
                if (blobContainer.CreateIfNotExists())
                {
                    blobContainer.SetPermissions(
                        new BlobContainerPermissions
                        {
                            PublicAccess = BlobContainerPublicAccessType.Off
                        }
                    );
                }
            }
            catch (Exception ExceptionObj)
            {
                Log.Info("Exception in BlogManager function" + ExceptionObj);
                Log.Info("Exception Message in BlogManager function" + ExceptionObj.Message);
                Log.Info("Inner Exception in BlogManager function" + ExceptionObj.InnerException);
                throw ExceptionObj;
            }
        }
        public List<string> BlobList()
        {
            List<string> _blobList = new List<string>();
            foreach (IListBlobItem item in blobContainer.ListBlobs())
            {
                if (item.GetType() == typeof(CloudBlockBlob))
                {
                    CloudBlockBlob _blobpage = (CloudBlockBlob)item;
                    _blobList.Add(_blobpage.Uri.AbsoluteUri.ToString());
                }
            }
            return _blobList;
        }
        public bool DeleteBlob(string AbsoluteUri)
        {
            try
            {
                Uri uriObj = new Uri(AbsoluteUri);
                string BlobName = Path.GetFileName(uriObj.LocalPath);

                // get block blob refarence  
                CloudBlockBlob blockBlob = blobContainer.GetBlockBlobReference(BlobName);

                // delete blob from container      
                blockBlob.Delete();
                return true;
            }
            catch (Exception ExceptionObj)
            {
                throw ExceptionObj;
            }
        }

        public string UploadFile(HttpPostedFileBase FileToUpload, string fileName)
        {
            string AbsoluteUri;
            // Check HttpPostedFileBase is null or not  
            if (FileToUpload == null || FileToUpload.ContentLength == 0)
                return null;
            try
            {
               
                CloudBlockBlob blockBlob;
                // Create a block blob  
                blockBlob = blobContainer.GetBlockBlobReference(fileName);
                // Set the object's content type  
                blockBlob.Properties.ContentType = FileToUpload.ContentType;
                // upload to blob  
                blockBlob.UploadFromStream(FileToUpload.InputStream);

                // get file uri  
                AbsoluteUri = blockBlob.Uri.AbsoluteUri;
            }
            catch (Exception ExceptionObj)
            {
                throw ExceptionObj;
            }
            return AbsoluteUri;
        }
        public string UploadFileFromStream(FileStream FileToUpload, string fileName, string type)
        {
            string AbsoluteUri;
            // Check HttpPostedFileBase is null or not
            //if (FileToUpload == null || FileToUpload.ContentLength == 0)
            //    return null;
            try
            {
                CloudBlockBlob blockBlob;
                // Create a block blob
                blockBlob = blobContainer.GetBlockBlobReference(fileName);
                // Set the object's content type
                blockBlob.Properties.ContentType = type;
                // upload to blob
                blockBlob.UploadFromStream(FileToUpload);
                // get file uri
                AbsoluteUri = blockBlob.Uri.AbsoluteUri;
            }
            catch (Exception ExceptionObj)
            {
                throw ExceptionObj;
            }
            return AbsoluteUri;
        }





        public CloudBlockBlob getCloudBlockBlob(string path)
        {
            CloudBlockBlob blob = blobContainer.GetBlockBlobReference(path);
            return blob;
        }

       public  static string GetBlobSasUri( string blobName, string company, string policyName = null)
        {
            try
            {
                CloudBlobContainer container;
                string ConnectionString = ProjectVaraiables.AZURE_CLOUD_STORAGE;
                CloudStorageAccount storageAccount = CloudStorageAccount.Parse(ConnectionString);
                string val = ProjectVaraiables.COMPANY_ALPHABET + Convert.ToInt32(company);
                CloudBlobClient cloudBlobClient = storageAccount.CreateCloudBlobClient();
                container = cloudBlobClient.GetContainerReference(val);




                string sasBlobToken;

                // Get a reference to a blob within the container.
                // Note that the blob may not exist yet, but a SAS can still be created for it.
                CloudBlockBlob blob = container.GetBlockBlobReference(blobName);

                if (policyName == null)
                {
                    // Create a new access policy and define its constraints.
                    // Note that the SharedAccessBlobPolicy class is used both to define the parameters of an ad hoc SAS, and
                    // to construct a shared access policy that is saved to the container's shared access policies.
                    SharedAccessBlobPolicy adHocSAS = new SharedAccessBlobPolicy()
                    {
                        // When the start time for the SAS is omitted, the start time is assumed to be the time when the storage service receives the request.
                        // Omitting the start time for a SAS that is effective immediately helps to avoid clock skew.
                        SharedAccessExpiryTime = DateTime.UtcNow.AddDays(30),
                        Permissions = SharedAccessBlobPermissions.Read | SharedAccessBlobPermissions.Write | SharedAccessBlobPermissions.Create
                    };

                    // Generate the shared access signature on the blob, setting the constraints directly on the signature.
                    sasBlobToken = blob.GetSharedAccessSignature(adHocSAS);

                    Console.WriteLine("SAS for blob (ad hoc): {0}", sasBlobToken);
                    Console.WriteLine();
                }
                else
                {
                    // Generate the shared access signature on the blob. In this case, all of the constraints for the
                    // shared access signature are specified on the container's stored access policy.
                    sasBlobToken = blob.GetSharedAccessSignature(null, policyName);

                    Console.WriteLine("SAS for blob (stored access policy): {0}", sasBlobToken);
                    Console.WriteLine();
                }

                // Return the URI string for the container, including the SAS token.
                return blob.Uri + sasBlobToken;
            }catch(Exception e)
            {
                return string.Empty;
            }
        }

        public AzureBlobDownloadInfo getCloudBlockBlob(string FilePath="",string FileName="", string u = "")
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(ProjectVaraiables.AZURE_CLOUD_STORAGE);
            CloudBlobClient cloudBlobClient = storageAccount.CreateCloudBlobClient();
            //system user download
            if (u == "")
            {
                blobContainer = cloudBlobClient.GetContainerReference("c00" + Convert.ToInt32(logedinuser.Company));
            }
            //outside user download
            else
            {
                blobContainer = cloudBlobClient.GetContainerReference("c00" + General_Purpose.DecryptId(u));
            }
            //For downloading direct from azure blob
            CloudBlockBlob blockBlob = blobContainer.GetBlockBlobReference(FilePath);
            SharedAccessBlobPolicy sasConstraints = new SharedAccessBlobPolicy();
            sasConstraints.SharedAccessStartTime = DateTime.UtcNow.AddMinutes(-5);
            sasConstraints.SharedAccessExpiryTime = DateTime.UtcNow.AddMinutes(30);
            sasConstraints.Permissions = SharedAccessBlobPermissions.Read;
            //download with the azure blob storage name
            //string sasBlobToken = blockBlob.GetSharedAccessSignature(sasConstraints);
            //download with the file name of metadata in our database
            string sasBlobToken = blockBlob.GetSharedAccessSignature(sasConstraints, new SharedAccessBlobHeaders()
            {
                ContentDisposition = "attachment; filename=" + FileName
            });
            AzureBlobDownloadInfo model = new AzureBlobDownloadInfo()
            {
                AccessToken = sasBlobToken,
                RedirectUri = blockBlob.Uri + sasBlobToken
            };
            //return Json(new { Status = true, Model = blockBlob.Uri + sasBlobToken, Summary = "done" });
            CloudBlockBlob blob = blobContainer.GetBlockBlobReference(FilePath);
            //return blob;
            return model;
        }







    }
}