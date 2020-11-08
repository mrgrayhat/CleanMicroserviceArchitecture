using System;
using Newtonsoft.Json;

namespace SaeedRezayi.Api.Models.Api
{
    public class CspPost
    {
        [JsonProperty("csp-report")]
        public CspReport CspReport { get; set; }
    }

    public class CspReport
    {
        [JsonProperty("document-uri")]
        public string DocumentUri { get; set; }

        [JsonProperty("referrer")]
        public string Referrer { get; set; }

        [JsonProperty("violated-directive")]
        public string ViolatedDirective { get; set; }

        [JsonProperty("effective-directive")]
        public string EffectiveDirective { get; set; }

        [JsonProperty("original-policy")]
        public string OriginalPolicy { get; set; }

        [JsonProperty("disposition")]
        public string Disposition { get; set; }

        [JsonProperty("blocked-uri")]
        public string BlockedUri { get; set; }

        [JsonProperty("line-number")]
        public int LineNumber { get; set; }

        [JsonProperty("column-number")]
        public int ColumnNumber { get; set; }

        [JsonProperty("source-file")]
        public string SourceFile { get; set; }

        [JsonProperty("status-code")]
        public string StatusCode { get; set; }

        [JsonProperty("script-sample")]
        public string ScriptSample { get; set; }
    }
}
