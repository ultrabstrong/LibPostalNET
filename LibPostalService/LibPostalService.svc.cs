using LibPostalNet;
using System;
using System.IO;
using System.Linq;

namespace LibPostalService
{
    public class LibPostalService : ILibPostalService
    {
        /// <summary>
        /// Parse address for address parts
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
        public LibPostalServiceResponse ParseAddress(string address)
        {
            LibPostalServiceResponse response = new LibPostalServiceResponse();
            try
            {
                // NOTE: This will only run in x64 applications because the postal.dll C++ wrapper library is compiled in x64. View the README.md in the LibPostalNet project for more information
                // Initialize components with data directory
                string dataPath = @"C:\LibPostalData\libpostal";
                if (!Directory.Exists(dataPath))
                {
                    response.SetWithStatus("LibPostal data does not exist at " + dataPath, System.Net.HttpStatusCode.NotFound);
                }
                else
                {
                    libpostal.LibpostalSetupDatadir(dataPath);
                    libpostal.LibpostalSetupParserDatadir(dataPath);
                    libpostal.LibpostalSetupLanguageClassifierDatadir(dataPath);

                    // Parse the address
                    LibpostalAddressParserResponse result = null;
                    try
                    {
                        LibpostalAddressParserOptions parseroptions = libpostal.LibpostalGetAddressParserDefaultOptions();
                        result = libpostal.LibpostalParseAddress(address, parseroptions);
                        if (result != null)
                        {
                            response.ParsedAddress = result.Results.Select(m => new AddressPart(m.Key, m.Value)).ToList();
                        }
                        else
                        {
                            response.SetWithStatus("Parse address returned empty results", System.Net.HttpStatusCode.NoContent);
                        }
                    }
                    catch (Exception ex)
                    {
                        response.SetWithException("Parse address failed", ex, System.Net.HttpStatusCode.InternalServerError);
                    }
                    finally
                    {
                        // Clean up lib postal parser
                        try { libpostal.LibpostalAddressParserResponseDestroy(result); } catch { }
                    }
                }
            }
            catch (Exception ex)
            {
                response.SetWithException("Failed to initialize LibPostal", ex, System.Net.HttpStatusCode.InternalServerError);
            }
            return response;
        }

    }
}
