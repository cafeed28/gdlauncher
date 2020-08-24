using System.Threading.Tasks;
using System.Diagnostics;
using AngleSharp.Html.Parser;
using AngleSharp;
using AngleSharp.Dom;

namespace Launcher
{
    public partial class NewsPage
    {
        //public ObservableCollection<NewsItem> News { get; set; }

        public NewsPage()
        {
            InitializeComponent();

            Task.Run(() => GetNews());
        }

        private void SetNews(int length, IHtmlCollection<IElement> newsTitle, IHtmlCollection<IElement> newsContent)
        {
            Debug.WriteLine("SetNews()");
            Status.Dispatcher.Invoke(() => { Status.Text = ""; });
            for (int i = 0; i < length; i++)
            {
                string title = newsTitle[i].TextContent.Trim();
                string content = newsContent[i].TextContent.Trim();
                Debug.WriteLine("i = " + i);
                Debug.WriteLine("title = " + title);
                Debug.WriteLine("content = " + content);

                newsList.Dispatcher.Invoke(() =>
                {
                    newsList.Items.Add(
                        new NewsItem
                        {
                            Title = title,
                            Content = content
                        }
                    );
                });
            }
        }

        private async Task GetNews()
        {
            var parser = new HtmlParser();

            var config = Configuration.Default.WithDefaultLoader();
            var address = "https://gdasunos.simdif.com/page-3580645.html";
            var document = await BrowsingContext.New(config).OpenAsync(address);

            Debug.WriteLine(document);

            if (document == null)
            {
                Status.Dispatcher.Invoke(() => { Status.Text = "Ошибка. Код ошибки: 40"; });
                Debug.WriteLine("Error 40: Connection timed out");
                return;
            }

            if (document.Url == "https://gdasunos.simdif.com/index.html")
            {
                Status.Dispatcher.Invoke(() => { Status.Text = "Ошибка. Код ошибки: 41"; });
                Debug.WriteLine("Error 41: Redirected to index.html");
                return;
            }

            IHtmlCollection<IElement> newsTitle = document.QuerySelectorAll("div.sd-block-title-div");

            IHtmlCollection<IElement> newsContent = document.QuerySelectorAll("div.sd-block-text-desc");

            int length = newsTitle.Length;

            SetNews(length, newsTitle, newsContent);
        }
    }
}