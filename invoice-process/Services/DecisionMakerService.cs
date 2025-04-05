using invoice_process.DTOs;
using invoice_process.Interfaces;
using Microsoft.CodeAnalysis;
using Newtonsoft.Json;

namespace invoice_process.Services
{
    public class DecisionMakerService : IDecisionMaker
    {
        private readonly ILogger<DecisionMakerService> _logger;
        public string GetAction(InvoiceDto invoice)
        {
            var rules = LoadJson();
            if (rules == null || rules.Rules.Count == 0)
            {
                _logger.LogError("No rule table found, moving on without Rules");
                return string.Empty;
            }

            foreach (var rule in rules.Rules)
            {
                string[] splitConditions = rule.Condition.Split("AND");
                List<bool> isConditionMet = [];
                foreach (var condition in splitConditions)
                {
                    var res = EvaluateCondition(condition.Trim(), invoice);
                    isConditionMet.Add(res);
                }
                if (isConditionMet.All(x => x == true))
                {
                    return rule.Action;
                }
            }

            return "No Action found";
        }

        public RuleListDto LoadJson()
        {
            var rules = new RuleListDto();
            var path = Path.Combine(Directory.GetCurrentDirectory(), "Helper", "RuleTable.json");

            using (StreamReader r = new StreamReader(path))
            {
                string json = r.ReadToEnd();
                try
                {
                    // Deserialize the JSON string into a list of RuleDto objects
                    rules = JsonConvert.DeserializeObject<RuleListDto>(json);
                }
                catch (Exception ex)
                {
                    return rules;
                }
            }

            return rules;
        }

        public bool EvaluateCondition(string condition, InvoiceDto invoice)
        {
            //splitting the condition parts to fetch properies and also the operator
            string[] conditionParts = condition.Split(new String[] { "==", ">", "<", "!=" },
                         StringSplitOptions.RemoveEmptyEntries);

            string operation = condition.Contains("==") ? "==" :
                               condition.Contains(">") ? ">" :
                               condition.Contains("<") ? "<" :
                               condition.Contains("!=") ? "!=" : "";

            string propertyName = conditionParts[0].Trim();
            string expectedValue = conditionParts[1].Trim().Trim('\'');

            //get the property from invoice dto that matches with extracted property name
            var invoiceProperty = invoice.GetType().GetProperty(propertyName);
            if (invoiceProperty == null)
            {
                return false;
            }
            //get the property value invoice dto that matches with extracted property name
            var actualValue = invoiceProperty.GetValue(invoice)?.ToString();
            if (DetermineOperation(operation, expectedValue, actualValue))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool DetermineOperation(string operation, object expectedValue, object propertyValue)
        {
            //Determine the operation based on the operator
            switch (operation)
            {
                case "==":
                    return propertyValue.ToString() == expectedValue.ToString() ? true : false;
                case ">":
                    return Convert.ToDecimal(propertyValue) > Convert.ToDecimal(expectedValue) ? true : false;
                case "<":
                    return Convert.ToDecimal(propertyValue) < Convert.ToDecimal(expectedValue) ? true : false;
                case "!=":
                    return propertyValue.ToString() != expectedValue.ToString() ? true : false;
                default:
                    return false;
            }
        }
    }
}
