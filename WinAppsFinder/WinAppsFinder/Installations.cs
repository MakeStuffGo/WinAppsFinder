using Microsoft.Win32;
using System;
using System.Collections.Generic;

namespace WinAppsFinder
{
    public class Installations
    {
        public static IEnumerable<Application> GetApplications()
        {
            string registry_key = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall";
            var HKLM32 = RegistryKey.OpenRemoteBaseKey(RegistryHive.LocalMachine, Environment.MachineName, RegistryView.Registry32);
            var HKLM64 = RegistryKey.OpenRemoteBaseKey(RegistryHive.LocalMachine, Environment.MachineName, RegistryView.Registry64);
            var key32 = HKLM32.OpenSubKey(registry_key);
            var key64 = HKLM64.OpenSubKey(registry_key);

            var list32 = GetApplications(key32);
            var list64 = GetApplications(key64);

            var list = new List<Application>();

            list.AddRange(list32);
            list.AddRange(list64);

            return list;
        }

        private static IEnumerable<Application> GetApplications(RegistryKey key)
        {
            var list = new List<Application>();

            foreach (string subkey_name in key.GetSubKeyNames())
            {
                using (RegistryKey subkey = key.OpenSubKey(subkey_name))
                {
                    if (subkey == null)
                        continue;

                    var name = subkey.GetValue("DisplayName") as string;

                    if (name == null)
                        continue;

                    var installPath = subkey.GetValue("InstallLocation") as string;

                    if (installPath == null)
                        continue;

                    var app = new Application
                    {
                        Name = name,
                        InstallPath = installPath
                    };

                    list.Add(app);
                }
            }

            return list;

        }
    }
}
