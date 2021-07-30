using System.Collections.Generic;
using System.Linq;
using System.Text.Encodings.Web;
using CfpGenerator.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CfpGenerator.Pages
{
    [Authorize]
    public class Index : PageModel
    {
        [BindProperty(SupportsGet = true)]
        public string SelectedTemplateId { get; set; }
        public CallForProposalTemplate Template { get; set; }
        public string Result { get; set; }

        public IList<SelectListItem> Templates =
            CallForProposalTemplate.All
                .Select(x => new SelectListItem(x.Id, x.Id))
                .ToList();

        public IActionResult OnGet()
        {
            // default
            SelectedTemplateId ??= Templates[0].Value;
            Template = CallForProposalTemplate.All.First(cfp => cfp.Id == SelectedTemplateId);
            return Request.IsHtmx()
                ? Partial("_Form", this)
                : Page();
        }

        public IActionResult OnPost()
        {
            Template = CallForProposalTemplate.All.First(cfp => cfp.Id == SelectedTemplateId);
            var fields = Request.Form
                .Select(k => new TemplateFieldValue(k.Key,$"<strong>{HtmlEncoder.Default.Encode(k.Value)}</strong>"))
                .ToArray();

            Result = Template.Process(fields);
            return Partial("_Result", this);
        }

    }
}