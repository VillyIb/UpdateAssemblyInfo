using System;
using System.Collections.Generic;
using System.IO;

namespace UpdateAssemblyInfo.Business
{
    public class AssemblyInfoApi
    {
        private Filehandling FileApi { get; set; }

        private PayloadHandler2 PayloadApi { get; set; }

        public String AssemblyCompany { get; set; }

        public String AssemblyCopyright { get; set; }

        /// <summary>
        /// AssemblyFileVersion should be specified as "#.#.#" and an additional ".0" will be added.
        /// Optionally specifify the full value "#.#.#.#".
        /// </summary>
        public String AssemblyFileVersion { get; set; }

        public String AssemblyProduct { get; set; }

        public String AssemblyTrademark { get; set; }

        public String AssemblyVersion { get; set; }

        public String NeutralResourcesLanguageAttribute { get; set; }


        /// <summary>
        /// Return the number of dots in a text.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private int CountDots(String value)
        {
            var result = -1;

            var index = -1;

            do
            {
                index = value.IndexOf('.', index + 1);
                result++;
            } while (index >= 0);

            return result;
        }


        private void AssemblyFileVersionProcess()
        {
            if (!(String.IsNullOrWhiteSpace(AssemblyFileVersion)))
            {
                if (PayloadApi.LocateValue("AssemblyFileVersion"))
                {
                    var value = PayloadApi.CurrentAttributeGet();

                    if (!(value.StartsWith(AssemblyFileVersion)))
                    {

                        value = (CountDots(AssemblyFileVersion) < 3)
                                    ? (AssemblyFileVersion + ".0")
                                    : AssemblyFileVersion;

                        PayloadApi.CurrentAttributeSet(value);
                    }
                }
            }
        }

        private void GenericFieldProcess(String key, String newValue)
        {
            if (!(String.IsNullOrWhiteSpace(newValue)))
            {
                if (PayloadApi.LocateValue(key))
                {
                    var value = PayloadApi.CurrentAttributeGet();

                    if ("null".Equals(newValue, StringComparison.OrdinalIgnoreCase) && !(String.IsNullOrWhiteSpace(value)))
                    {
                        PayloadApi.CurrentAttributeSet("");
                    }
                    else if (String.IsNullOrWhiteSpace(value) || !(value.StartsWith(newValue)))
                    {
                        PayloadApi.CurrentAttributeSet(newValue);
                    }
                }
            }
        }


        private bool BypassSpecial()
        {
            if (PayloadApi.LocateValue("AssemblyCompany"))
            {
                var value = PayloadApi.CurrentAttributeGet();

                return value.StartsWith("Villy", StringComparison.OrdinalIgnoreCase);
            }

            return false;
        }


        public void Execute()
        {
            FileApi = new Filehandling
                {
                    SolutionRoot = new DirectoryInfo(@"C:\Development\Backend")
                };
            FileApi.LocateFiles();

            PayloadApi = new PayloadHandler2();

            foreach (var file in FileApi.FileList)
            {
                List<String> t1;
                PayloadApi.Payload = Filehandling.PayloadRead(out t1, file) > 0 ? t1 : new List<string>(0);

                AssemblyFileVersionProcess();
                if (!(BypassSpecial()))
                {
                    GenericFieldProcess("AssemblyCompany", AssemblyCompany);
                    GenericFieldProcess("AssemblyTrademark", AssemblyTrademark);
                    GenericFieldProcess("AssemblyVersion", AssemblyVersion);
                    GenericFieldProcess("AssemblyCopyright", AssemblyCopyright);
                    GenericFieldProcess("AssemblyProduct", AssemblyProduct);
                    GenericFieldProcess("NeutralResourcesLanguageAttribute", NeutralResourcesLanguageAttribute);
                }

                if (PayloadApi.HasPayloadChanged)
                {
                    Filehandling.PayloadWrite(PayloadApi.Payload, file);
                }
            }


        }


    }
}
