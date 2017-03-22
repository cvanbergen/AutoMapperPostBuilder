using System;
using System.IO;
using System.Reflection;

namespace AutoMapperPostBuilder
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            if ((args.Length != 3) || (args[0].Length < 4) || (args[1].Length < 3) || (args[2].Length < 1))
            {
                Console.WriteLine(
                    "Invoke with: $ AutoMapperPostBuilder [FullAssemblyPath] [Class with full namespace] [method]");
                Environment.Exit(2);
            }

            var filePath = args[0];
            var classType = args[1];
            var invokeMethod = args[2];

            if ((filePath == null) || !File.Exists(filePath))
            {
                Console.WriteLine("Path is correct");
                Console.WriteLine(
                    "Invoke with: $ AutoMapperPostBuilder [FullAssemblyPath] [Class with full namespace] [method]");
                Environment.Exit(2);
            }

            Environment.CurrentDirectory = Path.GetDirectoryName(filePath);

            var currentDomain = AppDomain.CurrentDomain;
            currentDomain.AssemblyResolve += resolveEventHandler;
            var assembly = Assembly.LoadFrom(filePath);

            Type t = null;
            try
            {
                t = assembly.GetType(classType, true, true);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Environment.Exit(4);
            }

            try
            {
                t.InvokeMember(invokeMethod, BindingFlags.InvokeMethod, Type.DefaultBinder, null, null);
            }
            catch (Exception ex)
            {
                WriteExceptionMessagesRecursive(ex);
                Environment.Exit(1);
            }
            Console.WriteLine("Done...");
            Environment.Exit(0);
        }


        private static void WriteExceptionMessagesRecursive(Exception ex)
        {
            if (ex != null)
            {
                Console.WriteLine(ex.Message);
                if (ex.InnerException != null)
                    WriteExceptionMessagesRecursive(ex.InnerException);
            }
        }

        private static Assembly resolveEventHandler(object sender, ResolveEventArgs args)
        {
            Console.WriteLine("Resolving...");

            var assemblyPath = string.Empty;
            Assembly result = null;
            try
            {
                assemblyPath = Path.Combine(Environment.CurrentDirectory, args.Name.Split(',')[0] + " .dll");
                result = Assembly.LoadFrom(assemblyPath);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error while resolving assembly " + assemblyPath);
                Console.WriteLine(ex.Message);
                Environment.Exit(3);
            }
            return result;
        }
    }
}