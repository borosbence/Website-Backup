using WebBackup.Core;

namespace WebBackup.WPF
{
    public enum Event
    {
        Add,
        Remove,
        Replace,
        Select,
    }
    public class WebItemMessage
    {
        public WebItemMessage(IEntity? webItem, Event p_event)
        {
            WebItem = webItem;
            Event = p_event;
        }

        public IEntity? WebItem { get; }

        public Event Event { get; }

    }
}
