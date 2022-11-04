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
        private const string connectionString = "DefaultEndpointsProtocol=https;AccountName=stproductiondemo;AccountKey=bt9lGVgcZ676Pu0z0dPp12o4ImLCxNjgw2GFfoX4LILAg76DslKIYEIiVTpguQ75Oro0eklIWQDy+AStOdG4NQ==;EndpointSuffix=core.windows.net";
        private const string shareName = "sample-share";
        private const string dirName = "sample-dir";


        public static void UploadFile(string name, string path)
        {
            // Name of the file we'll create
            string fileName = name;

            // Path to the local file to upload
            string localFilePath = path;

            // Get a reference to a share and then create it
            ShareClient share = new ShareClient(connectionString, shareName);
            share.CreateIfNotExists();

            // Get a reference to a directory and create it
            ShareDirectoryClient directory = share.GetDirectoryClient(dirName);
            directory.CreateIfNotExists();

            // Get a reference to a file and upload it
            ShareFileClient file = directory.GetFileClient(fileName);
            using (FileStream stream = File.OpenRead(localFilePath))
            {
                file.Create(stream.Length);
                file.UploadRange(
                    new HttpRange(0, stream.Length),
                    stream);
            }

            Console.WriteLine("File Uploaded...");
        }


        public static CloudFile GetFile(string name)
        {

            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(connectionString);
            // Create a CloudFileClient object for credentialed access to Azure Files.
            CloudFileClient fileClient = storageAccount.CreateCloudFileClient();

            // Get a reference to the file share.
            CloudFileShare share = fileClient.GetShareReference(shareName);

            // Get a reference to the root directory for the share.
            CloudFileDirectory rootDir = share.GetRootDirectoryReference();

            // Get a reference to the directory.
            CloudFileDirectory sampleDir = rootDir.GetDirectoryReference("sample-dir");

            // Get a reference to the file we created previously.
            CloudFile file = sampleDir.GetFileReference(name);

            Console.WriteLine("File Downloaded...");
            return file;
        }

        public static IEnumerable<ShareFileItem> ViewFiles()
        {
            string connectionString = "DefaultEndpointsProtocol=https;AccountName=stproductiondemo;AccountKey=bt9lGVgcZ676Pu0z0dPp12o4ImLCxNjgw2GFfoX4LILAg76DslKIYEIiVTpguQ75Oro0eklIWQDy+AStOdG4NQ==;EndpointSuffix=core.windows.net";

            // Name of the share, directory, and file we'll download from
            string shareName = "sample-share";
            string dirName = "sample-dir";

            ShareClient share = new ShareClient(connectionString, shareName);
            ShareDirectoryClient directory = share.GetDirectoryClient(dirName);
            var files = directory.GetFilesAndDirectories();

            for (int i = 0; i < files.Count(); i++)
            {
                Console.WriteLine($"{i + 1}. {files.ElementAt(i).Name}");
            }

            return files;
        }

    }
}