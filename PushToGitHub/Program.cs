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
                string folderPath = Path.Combine(Directory.GetCurrentDirectory(), "tmp" + DateTime.Now.ToString("hhmmss"));

                Clone(folderPath);

                DateTime payDate = DateTime.Now;
                string targetFileName = payDate.ToString("yyyyMM") + ".csv";
                string targetFilePath = Path.Combine(folderPath, targetFileName);

                AppendFile(targetFilePath, "new data");

                CommitAndPush(folderPath, targetFileName);
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

        public static void AppendFile(string path, string text)
        {
            try
            {
                if (!File.Exists(path))
                {
                    File.Create(path).Dispose();
                }

                using (StreamWriter sw = File.AppendText(path))
                {
                    sw.WriteLine(text);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static void CommitAndPush(string folderPath, string targetFileName)
        {
            try
            {
                using (Repository repo = new Repository(folderPath))
                {
                    repo.Index.Add(targetFileName);

                    Signature signature = new Signature("kakeibo-csv", "system@example.com", DateTimeOffset.Now);
                    var commit = repo.Commit("append data", signature, signature);

                    PushOptions options = new PushOptions();
                    options.CredentialsProvider = (_url, _user, _cred) => new UsernamePasswordCredentials
                    {
                        Username = Environment.GetEnvironmentVariable("PAT"), // access token
                        Password = string.Empty
                    };
                    repo.Network.Push(repo.Head, options);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
