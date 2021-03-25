﻿using System;
using System.Collections.Generic;
using System.Text;

namespace SoftJail.DataProcessor.ImportDto
{
    public class PrisonerMailsInputModel
    {
        public PrisonerMailsInputModel()
        {
            this.Mails = new List<MailInputModel>();
        }

        public string FullName { get; set; }
        public string Nickname { get; set; }
        public int? Age { get; set; }
        public string IncarcerationDate { get; set; }
        public string ReleaseDate { get; set; }
        public decimal? Bail { get; set; }
        public int? CellId { get; set; }
        public ICollection<MailInputModel> Mails { get; set; }
    }

    public class MailInputModel
    {
        public string Description { get; set; }
        public string Sender { get; set; }
        public string Address { get; set; }
    }
}