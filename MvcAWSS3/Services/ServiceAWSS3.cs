using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace MvcAWSS3.Services
{
    public class ServiceAWSS3
    {
        private string bucketName;
        private IAmazonS3 awsClient;

        public ServiceAWSS3 (IAmazonS3 awsclient, IConfiguration configuration)
        {
            this.awsClient = awsclient;
            this.bucketName = configuration["AWSS3:BucketName"];
        }

        public async Task<bool> UploadFileAsync(Stream stream, string fileName)
        {
            PutObjectRequest request = new PutObjectRequest()
            {
                InputStream = stream,
                Key = fileName,
                BucketName = this.bucketName
            };
            PutObjectResponse response = await this.awsClient.PutObjectAsync(request);
            if (response.HttpStatusCode == HttpStatusCode.OK)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<List<string>> GetS3FilesAsync()
        {
            ListVersionsResponse response = await this.awsClient.ListVersionsAsync(this.bucketName);
            return response.Versions.Select(x => x.Key).ToList();
        }

        public async Task<bool> DeleteFileAsync (string fileName)
        {
            DeleteObjectResponse response = await this.awsClient.DeleteObjectAsync(this.bucketName, fileName);
            if (response.HttpStatusCode == HttpStatusCode.OK)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<Stream> GetFileAsync (string fileName)
        {
            GetObjectResponse response = await this.awsClient.GetObjectAsync(this.bucketName, fileName);
            if (response.HttpStatusCode == HttpStatusCode.OK)
            {
                return response.ResponseStream;
            }
            else
            {
                return null;
            }
        }
    }
}
