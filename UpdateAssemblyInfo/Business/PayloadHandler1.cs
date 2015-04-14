using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UpdateAssemblyInfo.Business
{
    public class PayloadHandler1
    {

        private String zPayload;

        /// <summary>
        /// Flag HasPayloadChanged is raised upon set operation.
        /// </summary>
        private String PayloadInternal
        {
            get { return zPayload; }
            set
            {
                zPayload = value;
                HasPayloadChanged = true;
            }
        }

        /// <summary>
        /// Flag HasPayloadChanged is cleared upon set operation.
        /// </summary>
        public String Payload
        {
            get { return zPayload; }
            set
            {
                zPayload = value;
                HasPayloadChanged = false;
            }
        }

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
            var start = PayloadInternal.IndexOf(key, StringComparison.OrdinalIgnoreCase);
            if (start < 0) { return false; }

            var start2 = PayloadInternal.IndexOf("\"", start + key.Length, StringComparison.Ordinal);
            if (start2 < 0) { return false; }

            CurrentValueStart = start2 + 1;

            var end = PayloadInternal.IndexOf("\"", CurrentValueStart, StringComparison.Ordinal);
            if (end < 0) { return false; }

            CurrentFirstAfterValue = end;
            CurrentValueLength = CurrentFirstAfterValue - CurrentValueStart;

            return true;
        }

        public String CurrentAttributeGet()
        {
            return PayloadInternal.Substring(CurrentValueStart, CurrentValueLength);
        }

        public void CurrentAttributeSet(String value)
        {
            var t1 = PayloadInternal.Substring(0, CurrentValueStart);

            var t3 = Payload.Substring(CurrentFirstAfterValue);

            PayloadInternal = PayloadInternal.Substring(0, CurrentValueStart) + value + Payload.Substring(CurrentFirstAfterValue);
        }

    }
}
