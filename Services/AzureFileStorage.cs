using Azure;
using Azure.Storage.Files.Shares;
using Azure.Storage.Files.Shares.Models;
using Microsoft.WindowsAzure.Storage.File;
using Microsoft.WindowsAzure.Storage;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Configuration;

namespace FileStorage.Services
{
    public interface IAzureFileStorage{
        void Init();
        void Init(string dirName);
        void Init(string shareName, string dirName);
        void UploadFile(string fileName, string filePath);
        CloudFile DownloadFile(string fileName);
        IEnumerable<ShareFileItem> ViewFiles();
        bool DeleteFile(string fileName);
        IEnumerable<ShareFileItem> GetDirectories();
        bool CreateDirectory(string dirName);
    }
    public class AzureFileStorage : IAzureFileStorage
    {
        private string connectionString;
        private string shareName;
        private string dirName;

        public AzureFileStorage() {
            connectionString = ConfigurationManager.AppSettings["connectionString"];
        }
        public void Init()
        {
            this.shareName = "share";
        }
        public void Init(string dirName)
        {
            this.shareName = "share";
            this.dirName = dirName;
        }
        public void Init(string shareName,string dirName)
        {
            this.shareName = shareName;
            this.dirName = dirName;
        }

        public void UploadFile(string fileName, string filePath)
        {
            // Get a reference to a share and then create it
            ShareClient share = new ShareClient(connectionString, shareName);
            share.CreateIfNotExists();

            // Get a reference to a directory and create it
            ShareDirectoryClient directory = share.GetDirectoryClient(dirName);
            directory.CreateIfNotExists();

            // Get a reference to a file and upload it
            ShareFileClient file = directory.GetFileClient(fileName);
            using (FileStream stream = File.OpenRead(filePath))
            {
                file.Create(stream.Length);
                file.UploadRange(
                    new HttpRange(0, stream.Length),
                    stream);
            }

            Console.WriteLine("File Uploaded...");
        }


        public CloudFile DownloadFile(string fileName)
        {

            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(connectionString);
            // Create a CloudFileClient object for credentialed access to Azure Files.
            CloudFileClient fileClient = storageAccount.CreateCloudFileClient();

            // Get a reference to the file share.
            CloudFileShare share = fileClient.GetShareReference(shareName);

            // Get a reference to the root directory for the share.
            CloudFileDirectory rootDir = share.GetRootDirectoryReference();

            // Get a reference to the directory.
            CloudFileDirectory sampleDir = rootDir.GetDirectoryReference(dirName);

            // Get a reference to the file we created previously.
            CloudFile file = sampleDir.GetFileReference(fileName);

            Console.WriteLine("File Downloaded...");
            return file;
        }

        public IEnumerable<ShareFileItem> ViewFiles()
        {
            ShareClient share = new ShareClient(connectionString, shareName);
            ShareDirectoryClient directory = share.GetDirectoryClient(dirName);
            var files = directory.GetFilesAndDirectories();

            foreach (var file in files)
            {
                Console.WriteLine(file.Name);
            }

            return files;
        }


        public bool DeleteFile(string fileName)
        {
            try
            {
                // Get a reference to a share and then create it
                ShareClient share = new ShareClient(connectionString, shareName);
                share.CreateIfNotExists();

                // Get a reference to a directory and create it
                ShareDirectoryClient directory = share.GetDirectoryClient(dirName);
                directory.CreateIfNotExists();

                // Get a reference to a file and upload it
                ShareFileClient file = directory.GetFileClient(fileName);
            
                return file.DeleteIfExists();
            }
            catch
            {
                return false;
            }
        }

        // DIR

        public IEnumerable<ShareFileItem> GetDirectories()
        {
            ShareClient share = new ShareClient(connectionString, shareName);
            share.CreateIfNotExists();

            return share.GetRootDirectoryClient().GetFilesAndDirectories();
        }

        public bool CreateDirectory(string dirName)
        {
            // Get a reference to a share and then create it
            ShareClient share = new ShareClient(connectionString, shareName);
            share.CreateIfNotExists();

            // Get a reference to a directory and create it
            ShareDirectoryClient directory = share.GetDirectoryClient(dirName);

            try
            {
                directory.CreateIfNotExists();
                return true;
            }
            catch
            {
                return false;
            }

        }

    }
}