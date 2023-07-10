using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DLL_Injector
{
    internal sealed class Injector
    {
        private Process targetProcess;
        private string procName;
        private string DLL_PATH;

        public Injector(string procName, string dLL_PATH)
        {
            targetProcess = Process.GetProcessesByName(procName).FirstOrDefault();
            this.DLL_PATH = dLL_PATH;
        }

        public void InjectDll()
        {
            try
            {
                IntPtr hProcess = Imports.OpenProcess(0xFFFF, false, (uint)targetProcess.Id);

                if (hProcess == IntPtr.Zero)
                {
                    throw new Exception("Failed to open Process.");
                }

                IntPtr AllocPATH = Imports.VirtualAllocEx(hProcess, IntPtr.Zero, (IntPtr)(DLL_PATH.Length + 1), Imports.AllocationType.Commit | Imports.AllocationType.Reserve, Imports.MemoryProtection.ExecuteReadWrite);
                if (Imports.WriteProcessMemory(hProcess, AllocPATH, Encoding.ASCII.GetBytes(DLL_PATH), (IntPtr)(DLL_PATH.Length + 1), out _) == false)
                {
                    throw new Exception("Failed to write memory.");
                }

                IntPtr LoadModule = Imports.GetProcAddress(Imports.GetModuleHandle("Kernel32.dll"), "LoadLibraryA");
                if (LoadModule == IntPtr.Zero)
                {
                    throw new Exception("Failed to LoadModule.");
                }

                IntPtr hThread = Imports.CreateRemoteThread(hProcess, IntPtr.Zero, 0, LoadModule, AllocPATH, 0, IntPtr.Zero);
                if (hThread == IntPtr.Zero)
                {
                    throw new Exception("Failed to create Thread.");
                }

                MessageBox.Show("DLL successfully injected !", "Dll Injector", MessageBoxButtons.OK, MessageBoxIcon.Information);

                Imports.CloseHandle(hThread);
                Imports.CloseHandle(hProcess);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Dll Injector", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
