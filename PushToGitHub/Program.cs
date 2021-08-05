﻿using LibGit2Sharp;
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
                Repository repo = new Repository();

                string path = "https://github.com/milky5/push-to-github.git";

                // \\Mac\Home\Documents\test
                string folderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "test");

                CloneOptions options = new CloneOptions();
                options.BranchName = "master";
                options.CredentialsProvider = (_url, _user, _cred) => new UsernamePasswordCredentials
                {
                    Username = "", // access token
                    Password = string.Empty
                };

                Repository.Clone(path, folderPath, options);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"error: {ex}");
                throw;
            }

            Console.WriteLine("success");
            Console.Read();
        }
    }
}
