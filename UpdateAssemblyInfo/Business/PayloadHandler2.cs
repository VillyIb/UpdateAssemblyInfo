using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UpdateAssemblyInfo.Business
{
    public class PayloadHandler2
    {

        private List<String> zPayload;

        /// <summary>
        /// Flag HasPayloadChanged is raised upon set operation.
        /// </summary>
        private String PayloadInternal
        {
            get { return zPayload[CurrentRow]; }
            set
            {
                zPayload[CurrentRow] = value;
                HasPayloadChanged = true;
            }
        }

        /// <summary>
        /// Flag HasPayloadChanged is cleared upon set operation.
        /// </summary>
        public List<String> Payload
        {
            get { return zPayload; }
            set
            {
                zPayload = value;
                HasPayloadChanged = false;
            }
        }

        protected int CurrentRow { get; set; }

        protected int CurrentValueStart { get; set; }

        protected int CurrentFirstAfterValue { get; set; }

        protected int CurrentValueLength { get; set; }

        public bool HasPayloadChanged { get; protected set; }

        // Syntax:
        // Key:       +++++++++++++++++++
        // Value:                          +++++++
        // [assembly: AssemblyFileVersion("1.0.0.0")]

        public bool LocateValue(String key)
        {
            var result = false;
            CurrentRow = -1;
            foreach (var row in Payload)
            {
                CurrentRow++;

                if (row.StartsWith("//"))
                {
                    continue;
                }

                var start = row.IndexOf(key, StringComparison.OrdinalIgnoreCase);
                if (start < 0)
                {
                    continue;
                }

                var start2 = row.IndexOf("\"", start + key.Length, StringComparison.Ordinal);
                if (start2 < 0)
                {
                    continue;
                }

                CurrentValueStart = start2 + 1;

                var end = row.IndexOf("\"", CurrentValueStart, StringComparison.Ordinal);
                if (end < 0)
                {
                    continue;
                }

                CurrentFirstAfterValue = end;
                CurrentValueLength = CurrentFirstAfterValue - CurrentValueStart;
                result = true;
                break;
            }
            return result;
        }

        public String CurrentAttributeGet()
        {
            return PayloadInternal.Substring(CurrentValueStart, CurrentValueLength);
        }

        public void CurrentAttributeSet(String value)
        {
            var t1 = PayloadInternal.Substring(0, CurrentValueStart);

            var t3 = PayloadInternal.Substring(CurrentFirstAfterValue);

            PayloadInternal = PayloadInternal.Substring(0, CurrentValueStart) + value + PayloadInternal.Substring(CurrentFirstAfterValue);
        }

    }
}
