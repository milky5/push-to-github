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

                Commit(folderPath, targetFilePath);
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

        public static void Commit(string folderPath, string filePath)
        {
            try
            {
                Repository repo = new Repository(folderPath);
                //repo.Index.Add()
                Commands.Stage(repo, filePath);

                Signature signature = new Signature("kakeibo-csv", "system@example.com", DateTimeOffset.Now);
                var commit = repo.Commit("append data", signature, signature);
                //Console.WriteLine(commit.Author.When.DateTime);
                //Console.WriteLine(commit.Sha);
                //Console.WriteLine("----------------------------------");

                PushOptions options = new PushOptions();
                options.CredentialsProvider = (_url, _user, _cred) => new UsernamePasswordCredentials
                {
                    Username = Environment.GetEnvironmentVariable("PAT"), // access token
                    Password = string.Empty
                };
                //options.OnPushStatusError = (e) => { 
                //    Console.WriteLine(e);
                //    Environment.Exit(1);
                //};
                //options.OnPushTransferProgress = (a, b, c) => {
                //    var msg = string.Format("Transfer status: {0} {1} {2}", a, b, c);
                //    Console.WriteLine(msg);
                //    return true;
                //};
                //options.OnPackBuilderProgress = (a, b, c) =>
                //{
                //    var msg = string.Format("Builder status: {0} {1} {2}", a, b, c);
                //    Console.WriteLine(msg);
                //    return true;
                //};

                //foreach (Commit c in repo.Head.TrackedBranch.Commits)
                //{
                //    Console.WriteLine(c.Author.When.DateTime);
                //    Console.WriteLine(c.Sha);
                //}

                //foreach (var item in repo.Branches)
                //{
                //    Console.WriteLine(item.FriendlyName);
                //}

                //Console.WriteLine(repo.Head.FriendlyName);


                repo.Network.Push(repo.Head, options);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
