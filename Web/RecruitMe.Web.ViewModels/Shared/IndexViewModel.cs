namespace RecruitMe.Web.ViewModels.Shared
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class IndexViewModel
    {
        public bool IsProfileCreated { get; set; }

        public IEnumerable<IndexJobOffersModel> LastTenJobOffers { get; set; }

        public IEnumerable<IndexTopEmployersModel> TopEmployers { get; set; }
    }
}
