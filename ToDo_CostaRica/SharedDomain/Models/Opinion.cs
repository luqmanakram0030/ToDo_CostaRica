namespace ToDoCR.SharedDomain.Models
{
    public class Opinion
    {
        public string Date { get; set; }
        public string OpinionDescription { get; set; }
        public bool HasAdminReply { get; set; }
        public string AdminReply { get; set; }
        public string AdminReplyDate { get; set; }
    }
}
