using System;
using System.Windows.Automation;
using ConditionType = System.Windows.Automation.Condition;

namespace PingPongTest
{
    public static class AutomationElementExtension
    {
        private static readonly TimeSpan s_MaxTimeout = TimeSpan.FromSeconds(100);

        public static AutomationElement WaitForFirst(this AutomationElement parent, TreeScope treeScope, ConditionType propertyCondition, TimeSpan timeout)
        {
            return parent.FindFirst(treeScope, propertyCondition);
        }

        public static AutomationElement WaitForFirst(this AutomationElement parent, TreeScope treeScope, ConditionType propertyCondition)
        {
            return parent.WaitForFirst(treeScope, propertyCondition, s_MaxTimeout);
        }
      
        public static AutomationElement WaitForFirstChild(this AutomationElement parent, string automationId)
        {
            return parent.WaitForFirst(TreeScope.Children, ConditionHelper.FromId(automationId, PropertyConditionFlags.IgnoreCase));
        }

        public static void Invoke(this AutomationElement element)
        {
            element.AsInvoke().Invoke();
        }

        private static InvokePattern AsInvoke(this AutomationElement element)
        {
            return (InvokePattern)element.GetCurrentPattern(InvokePattern.Pattern);
        }

        public static bool IsEnabled(this AutomationElement element)
        {
            return (bool)element.GetCurrentPropertyValue(AutomationElement.IsEnabledProperty);
        }
     }
}
