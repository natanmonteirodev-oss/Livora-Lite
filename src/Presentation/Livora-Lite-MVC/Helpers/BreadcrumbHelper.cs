using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace Livora_Lite.Helpers
{
    /// <summary>
    /// Helper para gerenciar breadcrumbs na aplicação
    /// </summary>
    public static class BreadcrumbHelper
    {
        public class BreadcrumbItem
        {
            public string Label { get; set; } = string.Empty;
            public string? Url { get; set; }
            public bool IsActive { get; set; }

            public BreadcrumbItem(string label, string? url = null, bool isActive = false)
            {
                Label = label;
                Url = url;
                IsActive = isActive;
            }
        }

        /// <summary>
        /// Adiciona item ao breadcrumb
        /// </summary>
        public static void AddBreadcrumb(ViewDataDictionary viewData, string label, string? url = null, bool isActive = false)
        {
            var breadcrumbs = GetBreadcrumbs(viewData);
            breadcrumbs.Add(new BreadcrumbItem(label, url, isActive));
            viewData["Breadcrumbs"] = breadcrumbs;
        }

        /// <summary>
        /// Obtém lista de breadcrumbs
        /// </summary>
        public static List<BreadcrumbItem> GetBreadcrumbs(ViewDataDictionary viewData)
        {
            if (viewData["Breadcrumbs"] is List<BreadcrumbItem> breadcrumbs)
            {
                return breadcrumbs;
            }

            var newBreadcrumbs = new List<BreadcrumbItem>();
            viewData["Breadcrumbs"] = newBreadcrumbs;
            return newBreadcrumbs;
        }

        /// <summary>
        /// Define breadcrumbs completos (sobrescreve os anteriores)
        /// </summary>
        public static void SetBreadcrumbs(ViewDataDictionary viewData, params BreadcrumbItem[] items)
        {
            viewData["Breadcrumbs"] = new List<BreadcrumbItem>(items);
        }

        /// <summary>
        /// Limpa breadcrumbs
        /// </summary>
        public static void ClearBreadcrumbs(ViewDataDictionary viewData)
        {
            viewData["Breadcrumbs"] = new List<BreadcrumbItem>();
        }
    }
}
