using LibGit2Sharp;
using System;
using System.IO;

namespace PushToGitHub
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // H:\push-to-github\PushToGitHub\bin\Debug\netcoreapp3.1
                Console.WriteLine(Directory.GetCurrentDirectory());
                string folderPath = Path.Combine(Directory.GetCurrentDirectory(), "tmp");
                
                // Clone(folderPath);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }


            Console.WriteLine("success");
            Console.Read();
        }

        public static void Clone(string folderPath)
        {
            try
            {
                Repository repo = new Repository();

                string path = Environment.GetEnvironmentVariable("REPO_PATH");

                CloneOptions options = new CloneOptions();
                options.BranchName = "master";
                options.CredentialsProvider = (_url, _user, _cred) => new UsernamePasswordCredentials
                {
                    Username = Environment.GetEnvironmentVariable("PAT"), // access token
                    Password = string.Empty
                };

                Repository.Clone(path, folderPath, options);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"error: {ex}");
                throw;
            }
        }
    }
}
