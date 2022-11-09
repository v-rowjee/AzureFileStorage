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

namespace FileStorage.Services
{
    public class AzureFileStorage
    {
        private const string connectionString = "DefaultEndpointsProtocol=https;AccountName=jedifilestorageaccount;AccountKey=lLJPCZqidvkjfoOiGOA5WEkCdSsZdrQfjt021sGK5g0hq65OuoBm8gD0Rx1C9gcjrNwYzDYR9kCe+ASteLYwzw==;EndpointSuffix=core.windows.net";
        private const string shareName = "share";
        private const string dirName = "dir";


        public static void UploadFile(string fileName, string filePath)
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


        public static CloudFile DownloadFile(string name)
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
            CloudFile file = sampleDir.GetFileReference(name);

            Console.WriteLine("File Downloaded...");
            return file;
        }

        public static IEnumerable<ShareFileItem> ViewFiles()
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


        public static bool DeleteFile(string fileName)
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

        public static IEnumerable<ShareFileItem> GetDirectories()
        {
            ShareClient share = new ShareClient(connectionString, shareName);
            share.CreateIfNotExists();

            return share.GetRootDirectoryClient().GetFilesAndDirectories();
        }

        public static bool CreateDirectory(string _dirName)
        {
            // Get a reference to a share and then create it
            ShareClient share = new ShareClient(connectionString, shareName);
            share.CreateIfNotExists();

            // Get a reference to a directory and create it
            ShareDirectoryClient directory = share.GetDirectoryClient(_dirName);

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