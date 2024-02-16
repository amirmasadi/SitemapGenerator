using SitemapGenerator.Model;
using System.Globalization;
using System.Xml.Linq;

namespace SitemapGenerator
{
    // source : https://learn.microsoft.com/en-us/nuget/quickstart/create-and-publish-a-package-using-visual-studio?tabs=netcore-cli
    public static class SitemapXmlGenerator
    {
        public static bool SaveSitemapXml(string path, List<ProductModel> products)
        {
            try
            {
                var sitemapDocumentXml = GetSitemapDocument(GetNode(products));
                sitemapDocumentXml.Save(path);
                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }


        private static List<SitemapNode> GetNode(List<ProductModel> products)
        {
            List<SitemapNode> nodes = [];

            //string Domain = urlHelper.ActionContext.HttpContext.Request.Scheme + "://" + urlHelper.ActionContext.HttpContext.Request.Host;

            List<SitemapNode> sitemapNodes =
            [
                new SitemapNode()
                {
                    Url = "https://tbao.ir/",
                    Priority = 1,
                    Frequency = SitemapFrequency.Weekly,
                },

                new SitemapNode()
                {
                    Url = "https://tbao.ir/Company",
                    Priority = 1,
                    Frequency = SitemapFrequency.Weekly
                },
                new SitemapNode()
                {
                    Url = "https://tbao.ir/main",
                    Priority = 1,
                    Frequency = SitemapFrequency.Weekly
                }
            ];

            nodes.AddRange(sitemapNodes);

            foreach (var product in products)
            {
                nodes.Add(
                    new SitemapNode()
                    {
                        Url = $"https://tbao.ir/Product?pspId={product.ProductSalesPlanningId}",
                        Frequency = SitemapFrequency.Weekly,
                        Priority = 0.8,
                    });

            }
            return nodes;
        }

        private static XDocument GetSitemapDocument(List<SitemapNode> sitemapNodes)
        {
            XNamespace xmlns = "http://www.sitemaps.org/schemas/sitemap/0.9";
            XElement root = new XElement(xmlns + "urlset");
            foreach (SitemapNode sitemapNode in sitemapNodes)
            {
                XElement urlElement = new XElement(
                    xmlns + "url",
                    new XElement(xmlns + "loc", Uri.EscapeUriString(sitemapNode.Url)),
                    sitemapNode.LastModified == null ? null : new XElement(xmlns + "lastmod",
                        sitemapNode.LastModified.Value.ToLocalTime().ToString("yyyy-mm-ddthh:mm:sszzz")),
                    sitemapNode.Frequency == null ? null : new XElement(
                        xmlns + "changefreq",
                        sitemapNode.Frequency.Value.ToString().ToLowerInvariant()),
                    sitemapNode.Priority == null ? null : new XElement(
                        xmlns + "priority",
                        sitemapNode.Priority.Value.ToString("F1",
                            CultureInfo.InvariantCulture)));
                root.Add(urlElement);
            }
            return new XDocument(root);
        }


    }
}
