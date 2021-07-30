using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace CfpGenerator.Models
{
    public class CallForProposalTemplate
    {
        public CallForProposalTemplate(string id, string textWithPlaceholders)
        {
            Id = id;
            Text = textWithPlaceholders;
            Placeholders = Initialize(textWithPlaceholders);
        }

        private static readonly Regex FieldRegex =
            new(@"\{(?<field>.*?)\}");

        public string Id { get; private set; }
        public string Text { get; }

        public IReadOnlyList<string> Placeholders { get; }
            = Array.Empty<string>();

        private static IReadOnlyList<string> Initialize(string text)
        {
            var matches = FieldRegex.Matches(text);
            var fields = new List<string>();
            foreach (Match match in matches) {
                fields.Add(match.Groups["field"].Value);
            }

            return fields.Distinct().ToList().AsReadOnly();
        }

        public string Process(params TemplateFieldValue[] fieldValues)
        {
            var sb = new StringBuilder(Text);

            foreach (var (name, value) in fieldValues) {
                sb.Replace($"{{{name}}}", value);
            }

            return sb.ToString();
        }

        public static readonly IReadOnlyList<CallForProposalTemplate> All =
            new List<CallForProposalTemplate>
            {
                new("The Matt", Resources.The_Matt),
                new("The Khalid", Resources.The_Khalid)
            };
    }

    public record TemplateFieldValue(string Name, string Value);
}