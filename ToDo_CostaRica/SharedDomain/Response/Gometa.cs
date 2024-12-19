using System;
using System.Collections.Generic;
using System.Text;
using J = Newtonsoft.Json.JsonPropertyAttribute;

namespace ToDoCR.SharedDomain.Response
{
    public partial class GometaCedula
    {
        [J("query")] public string Query { get; set; }
        [J("database_date")] public string DatabaseDate { get; set; }
        [J("ignored_words")] public List<string> IgnoredWords { get; set; }
        [J("results")] public List<Result> Results { get; set; }
        [J("overflow")] public long Overflow { get; set; }
        [J("resultcount")] public long Resultcount { get; set; }
        [J("license")] public Uri License { get; set; }
    }

    public partial class Result
    {
        [J("admin")] public string Admin { get; set; }
        [J("class")] public string Class { get; set; }
        [J("lastname1")] public string Lastname1 { get; set; }
        [J("rawcedula")] public string Rawcedula { get; set; }
        [J("temp")] public object Temp { get; set; }
        [J("lastname")] public string Lastname { get; set; }
        [J("firstname2")] public string Firstname2 { get; set; }
        [J("firstname")] public string Firstname { get; set; }
        [J("firstname1")] public string Firstname1 { get; set; }
        [J("fullname")] public string Fullname { get; set; }
        [J("type")] public string Type { get; set; }
        [J("lastname2")] public string Lastname2 { get; set; }
        [J("cedula")] public string Cedula { get; set; }
    }
}
