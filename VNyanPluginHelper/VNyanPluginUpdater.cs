using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Net;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.UI;

namespace JayoPoiyomiPlugin.VNyanPluginHelper
{
    class VNyanPluginUpdater
    {
        public event Action<string> OpenUrlRequested;

        private string currentVersion;
        private string latestVersion;
        private string repoName;
        private bool updateAvailable = false;

        public void CheckForUpdates(string version, string repo)
        {
            try
            {
                currentVersion = version;
                repoName = repo;
                HttpWebRequest Request = (HttpWebRequest)WebRequest.Create($"https://api.github.com/repos/{repoName}/releases");
                Request.UserAgent = "request";
                HttpWebResponse response = (HttpWebResponse)Request.GetResponse();
                StreamReader Reader = new StreamReader(response.GetResponseStream());
                string JsonResponse = Reader.ReadToEnd();
                JArray Releases = JArray.Parse(JsonResponse);
                latestVersion = Releases[0]["tag_name"].ToString();
                updateAvailable = currentVersion != latestVersion;
            }
            catch (Exception e)
            {
                Debug.Log($"Couldn't check for updates: {e.Message}");
            }
        }

        public void PrepareUpdateUI(GameObject versionText, GameObject updateText, GameObject updateButton)
        {

            versionText.GetComponent<Text>().text = currentVersion;
            updateText.GetComponent<Text>().text = $"New Update Available: {latestVersion}";
            updateButton.GetComponent<Button>().onClick.AddListener(() => { OpenUpdatePage(); });

            if (!updateAvailable)
            {
                updateText.SetActive(false);
                updateButton.SetActive(false);
            }
        }

        private void OpenUpdatePage()
        {
            OpenUrlRequested.Invoke($"https://github.com/{repoName}/releases/latest");

        }

    }
}
