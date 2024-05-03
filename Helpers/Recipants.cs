using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BulkEmailSender.Helpers
{
    public class Recipients
    {
        public List<string> GmailAddresses { get; } = new List<string>();

        public Recipients()
        {
            GmailAddresses.Add("navdeepmor.me@gmail.com");
            GmailAddresses.Add("prudhvicharan.nelloori@gmail.com");
        }
    }
}
