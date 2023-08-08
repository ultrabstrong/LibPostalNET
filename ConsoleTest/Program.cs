using LibPostalClient;
using LibPostalNet;
using System;
using System.Collections.Generic;
using System.IO;

namespace ConsoleTest
{
    internal class Program
    {
        static void Main(string[] args)
        {
        }

        /// <summary>
        /// Test lib postal client
        /// </summary>
        static void TestLibPostalClient()
        {
            LibPostalServiceResponse response = LibPostalClient.LibPostalClient.ParseAddress("Av. Beira Mar 1647 - Salgueiros, 4400-382 Vila Nova de Gaia");
            if (response.Status == 200 && response.ParsedAddress != null)
            {
                foreach (var pair in response.ParsedAddress)
                {
                    Console.WriteLine($"{pair.Name}:{pair.Value}");
                }
            }
            else
            {
                Console.WriteLine($"{response.Status} : {response.Message}");
            }
        }

        /// <summary>
        /// Test lib postal net library
        /// </summary>
        static void TestLibPostalNet()
        {
            try
            {
                // NOTE: This will only run in x64 applications because the postal.dll C++ wrapper library is compiled in x64. View the README.md in the LibPostalNet project for more information
                // Initialize components with data directory
                string dataPath = @"C:\LibPostal\libpostal";
                if (!Directory.Exists(dataPath)) { throw new Exception("LibPostal data does not exist in directory " + dataPath); }
                libpostal.LibpostalSetupDatadir(dataPath);
                libpostal.LibpostalSetupParserDatadir(dataPath);
                libpostal.LibpostalSetupLanguageClassifierDatadir(dataPath);

                // Create query address
                string query = "Av. Beira Mar 1647 - Salgueiros, 4400-382 Vila Nova de Gaia";
                Console.WriteLine($"Parsing started for [{query}]");

                // Parse the address
                LibpostalAddressParserResponse response = null;
                try
                {
                    LibpostalAddressParserOptions parseroptions = libpostal.LibpostalGetAddressParserDefaultOptions();
                    response = libpostal.LibpostalParseAddress(query, parseroptions);
                    if (response != null)
                    {
                        foreach (KeyValuePair<string, string> result in response.Results)
                        {
                            Console.WriteLine(result.ToString());
                        }
                    }
                    else
                    {
                        Console.WriteLine("Parse address returned null");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Parse address failed{Environment.NewLine}{ex}");
                }
                finally
                {
                    libpostal.LibpostalAddressParserResponseDestroy(response);
                }

                // Normalize the address
                Console.WriteLine($"Expansion started for [{query}]");
                LibpostalNormalizeOptions normalizeoptions = libpostal.LibpostalGetDefaultOptions();
                normalizeoptions.AddressComponents = LibpostalNormalizeOptions.LIBPOSTAL_ADDRESS_ALL;
                LibpostalNormalizeResponse expansion = libpostal.LibpostalExpandAddress(query, normalizeoptions);
                if (expansion != null)
                {
                    foreach (string s in expansion.Expansions)
                    {
                        Console.WriteLine(s);
                    }
                }
                else
                {
                    Console.WriteLine("Expand address returned null");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"LibPostal Exceptions:{Environment.NewLine}{ex}");
            }
            finally
            {
                // Tear down the components
                try { libpostal.LibpostalTeardown(); } catch { Console.WriteLine("Failed to tear down lib postal"); }
                try { libpostal.LibpostalTeardownParser(); } catch { Console.WriteLine("Failed to tear down lib postal parser"); }
                try { libpostal.LibpostalTeardownLanguageClassifier(); } catch { Console.WriteLine("Failed to tear down lib postal language classifier"); }
            }
        }
    }
}
